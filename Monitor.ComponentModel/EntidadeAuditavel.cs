using System;

namespace Monitor.ComponentModel
{
    public abstract class EntidadeAuditavel : Entidade
    {
        protected DateTime createDateTime = DateTime.Now;
        public virtual DateTime CreateDateTime
        {
            get { return createDateTime; }
            set { createDateTime = value; }
        }

        public virtual DateTime? UpdateDateTime { get; set; }
    }
}
