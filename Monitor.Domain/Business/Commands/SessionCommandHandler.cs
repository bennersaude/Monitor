using System;
using System.Threading.Tasks;
using AutoMapper;
using Monitor.ComponentModel;
using Monitor.Data;
using NHibernate;

namespace Monitor.Domain.Business.Commands
{

    public abstract class SessionCommandHandler<C, E> 
        where C : Command
        where E : Entidade
    {
        private const string FALHA_REALIZAR_OPERACAO = "Falha ao realizar operação";
        protected readonly ISessionProvider sessionHelper;
        protected readonly IMapper mapper;

        public SessionCommandHandler(ISessionProvider sessionHelper,
            IMapper mapper)
        {
            this.sessionHelper = sessionHelper ?? throw new System.ArgumentNullException(nameof(sessionHelper));
            this.mapper = mapper;
        }

        public async Task<E> ExecutarERetornarAsync(C command, Func<C, ISession, E> operationDelegate)
        {
            if (command is null)
            {
                throw new System.ArgumentNullException(nameof(command));
            }
            using (var session = sessionHelper.OpenSession())
            {
                if (!command.IsValid(session))
                {
                    throw new FluentValidation.ValidationException(FALHA_REALIZAR_OPERACAO, command.ValidationResult.Errors);
                }
                session.BeginTransaction();
                var registro = await Task.FromResult(operationDelegate(command, session));
                session.GetCurrentTransaction().Commit();
                return registro;
            }
        }

        public async Task ExecutarAsync(C command, Action<C, ISession> operationDelegate)
        {
            if (command is null)
            {
                throw new System.ArgumentNullException(nameof(command));
            }
            using (var session = sessionHelper.OpenSession())
            {
                if (!command.IsValid(session))
                {
                    throw new FluentValidation.ValidationException(FALHA_REALIZAR_OPERACAO, command.ValidationResult.Errors);
                }
                session.BeginTransaction();
                await Task.Run(() => operationDelegate(command, session));
                session.GetCurrentTransaction().Commit();
            }
        }
    }
}