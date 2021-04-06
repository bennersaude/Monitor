using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHibernate;
using NHibernate.Cfg;
using Environment = System.Environment;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using NSwag;
using NSwag.Generation.Processors.Security;
using NSwag.AspNetCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;
using log4net;
using Microsoft.IdentityModel.Logging;
using Monitor.Data;
using Monitor.Api.Filters;
using Monitor.Data.Types;
using Monitor.Api.Config;
using Monitor.Data.Infra.Helpers;
using Monitor.Api.Controllers;
using Monitor.Api.HealthCheck;
using Monitor.Api.Settings;

namespace Monitor.Api
{
    public class Startup
    {
        private const string PROXY_IP = "PROXY_IP";
        private const string MEMORY = "memory";
        private const string SITE_PREFIX = "SITE_PREFIX";
        private readonly IWebHostEnvironment environment;
        public IConfiguration Configuration { get; }
        private string serviceName = "Monitor de Ambientes Benner - Service";
        private string informationalVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "0.0.1";
        private string sitePrefix
        {
            get
            {
                var prefix = Environment.GetEnvironmentVariable(SITE_PREFIX)
                     ?? Configuration.GetSection("AppSettings:sitePrefix")?.Value
                     ?? String.Empty;
                prefix = prefix.Replace("/", String.Empty);
                return prefix;
            }
        }

        private SessionProvider sessionProvider;
        public IServiceProvider serviceProvider { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.environment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(x => x.ClearProviders().AddLog4Net());
            ConfigureSignin(services);
            services.AddControllers(c => c.Filters.Add(typeof(ExceptionFilter)))
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
            services.AddCors();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable(PROXY_IP)))
                    options.KnownProxies.Add(IPAddress.Parse(Environment.GetEnvironmentVariable(PROXY_IP)));
            });
            var connectionString = Environment.GetEnvironmentVariable("BENNERMONITOR_CONNECTIONSTRING")
                ?? Configuration.GetConnectionString("BennerMonitor");
            if (String.IsNullOrEmpty(connectionString))
                throw new ConfigurationErrorsException($"Não encontrada string de conexão 'BennerMonitor'!");
            sessionProvider = new SessionProvider(TipoAplicacao.StandAlone,
                connectionString,
                assembliesWithMappingsPatterns: new[] { "Monitor.Data", "Monitor.Domain" },
                false);
            var path = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                    "hibernate.config"
                    );
            sessionProvider.Configure(services, path);
            services.AddSingleton<ISessionProvider>(sessionProvider);
            services.AddSingleton<IConfiguration>(Configuration);
            var settingsHelper = new SettingsHelper(Configuration);
            services.AddSingleton<ISettingsHelper>(settingsHelper);
            services.AddHealthChecks().AddCheck<MemoryHealthCheck>(MEMORY, tags: new[] { MEMORY });
            Domain.DependencyInjectionHelper.RegisterTypes(services);

            services.AddOpenApiDocument(config =>
            {
                config.OperationProcessors.Add(new OperationSecurityScopeProcessor(Jwt.Bearer));
                config.AddSecurity(Jwt.Bearer, Enumerable.Empty<string>(),
                    new NSwag.OpenApiSecurityScheme()
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = nameof(Authorization),
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Copy this into the value field: Bearer {token}"
                    }
                );
                config.PostProcess = d =>
                {
                    d.Info.Title = serviceName;
                    d.Info.Version = informationalVersion;
                    d.Info.Description = serviceName;
                };
            });

            var automapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(DomainHelper.GetAssembly("Monitor.Domain"));
            });

            services.AddSingleton(automapperConfig.CreateMapper());
        }

        private void ConfigureSignin(IServiceCollection services)
        {
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);
            var tokenConfigurations = GetTokenConfigurations();
            services.AddSingleton(tokenConfigurations);            
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(Jwt.Bearer, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
            IdentityModelEventSource.ShowPII = true;
        }

        private TokenConfigurations GetTokenConfigurations()
        {
            var tokenConfigurations = new TokenConfigurations();
            tokenConfigurations.Audience = Environment.GetEnvironmentVariable("API_AUDIENCE") ?? "ConectaMonitorAudience";
            tokenConfigurations.Issuer = Environment.GetEnvironmentVariable("API_ISSUER") ?? "ConectaMonitorIssuer";
            tokenConfigurations.SecondsToExpire = int.Parse(Environment.GetEnvironmentVariable("JWT_TOKEN_SECONDSTOEXPIRE") ?? "3600");
            return tokenConfigurations;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var logger = LogManager.GetLogger(typeof(Startup));
            const string PR_BR = "pt-BR";
            var cultureInfo = new CultureInfo(PR_BR);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            var supportedCultures = new[]
            {
                new CultureInfo(PR_BR)
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(PR_BR),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            if (!String.IsNullOrEmpty(sitePrefix))
            {
                app.UsePathBase(new PathString($"/{sitePrefix}"));
            }
            app.Use((context, next) =>
            {
                context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                context.Response.Headers["Access-Control-Allow-Headers"] = "Origin, X-Requested-With, Content-Type, Accept";
                return next();
            });
            app.Use((context, next) =>
            {
                const string PATHBASE = nameof(context.Request.PathBase);
                const string PATH = nameof(context.Request.Path);
                logger.Debug($"{PATHBASE}: {context.Request.PathBase}");
                logger.Debug($"{PATH}: {context.Request.Path}");
                if (context.Request.Path.HasValue && !context.Request.Path.Value.Contains("swagger") && context.Request.PathBase.HasValue)
                {
                    context.Request.PathBase = String.Empty;
                    logger.Debug($"{PATHBASE} changed");
                    logger.Debug($"{PATHBASE}: {context.Request.PathBase}");
                    logger.Debug($"{PATH}: {context.Request.Path}");
                }
                return next();
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                ServeUnknownFileTypes = true
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDefaultFiles();

            app.UseOpenApi();
            app.UseSwaggerUi3(config =>
            {
                config.DocumentTitle = $"Service Documentation - {serviceName}";
            });

            app.UseHealthChecks($"/ready", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("service"),
                ResponseWriter = WriteResponse
            });

            app.UseHealthChecks($"/live", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains(MEMORY),
                ResponseWriter = WriteResponse
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context =>
                {
                    var prefix = String.IsNullOrEmpty(sitePrefix) ? String.Empty : $"/{sitePrefix}";
                    context.Response.Redirect($"{prefix}/swagger");
                    return context.Response.WriteAsync("ok");
                });
                /*endpoints.MapGet($"{BUILDVERSIONSTXT}", context =>
                {
                    return context.Response.WriteAsync(informationalVersion);
                });*/
                endpoints.MapControllers();
            });
            app.UseCors(
                options =>
                    {
                        options.AllowAnyOrigin();
                        options.AllowAnyMethod();
                        options.AllowAnyHeader();
                    }
                );
        }

        private static System.Threading.Tasks.Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }
    }
}
