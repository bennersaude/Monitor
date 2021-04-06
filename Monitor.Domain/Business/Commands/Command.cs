using System;
using FluentValidation.Results;
using NHibernate;

namespace Monitor.Domain.Business.Commands
{
    public abstract class Command
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public abstract bool IsValid(ISession session = null);
    }
}