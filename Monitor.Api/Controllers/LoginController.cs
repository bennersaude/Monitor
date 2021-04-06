using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Monitor.Api.Attributes;
using Monitor.Api.Config;
using Monitor.Api.Models;
using Monitor.Domain.ViewModels.Login;
using NSwag.Annotations;

namespace Monitor.Api.Controllers
{
    [Authorize(Jwt.Bearer)]
    [ApiController]
    [Route("api/login")]
    public class LoginController: ControllerBase
    {
        private string adminUserName = Environment.GetEnvironmentVariable("ADMIN_USERNAME") ?? "admin";
        private string adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "1q2w#E";
        
        [AllowAnonymous]
        [HttpPost]
        [ValidateModelState]
        [OpenApiOperation(nameof(EfetuarLogin), "Efetuar Login")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(LoginResponse))]
        public async Task<LoginResponse> EfetuarLogin(
            [FromBody] LoginViewModel usuario,
            [FromServices] SigningConfigurations signingConfigurations,
            [FromServices] TokenConfigurations tokenConfigurations)
        {
            return await Task.Run(() => 
            {
                bool credenciaisValidas = false;
                if (usuario != null && !String.IsNullOrWhiteSpace(usuario.UserName))
                {
                    credenciaisValidas = usuario.UserName.ToLower() == adminUserName.ToLower() &&
                        usuario.Password == adminPassword;
                }

                if (credenciaisValidas)
                {
                    var roles = new[] { Roles.Administrador };
                    ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(usuario.UserName, "Login"),
                        new[]
                        {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserName),
                            new Claim("role", String.Join(',', roles))
                        }
                    );

                    DateTime dataCriacao = DateTime.Now;
                    DateTime dataExpiracao = dataCriacao +
                        TimeSpan.FromSeconds(tokenConfigurations.SecondsToExpire);

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                    {
                        Issuer = tokenConfigurations.Issuer,
                        Audience = tokenConfigurations.Audience,
                        SigningCredentials = signingConfigurations.SigningCredentials,
                        Subject = identity,
                        NotBefore = dataCriacao,
                        Expires = dataExpiracao
                    });
                    var token = handler.WriteToken(securityToken);

                    return new LoginResponse(
                        true,
                        dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                        dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                        token,
                        "OK"
                    );
                }
                else
                {
                    return new LoginResponse(
                        false,
                        "Falha ao autenticar");
                }
            });
        }
    }
}
