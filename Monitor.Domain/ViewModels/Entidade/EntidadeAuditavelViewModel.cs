using System;

namespace Monitor.Domain.ViewModels.Entidade
{
    public abstract class EntidadeAuditavelViewModel : EntidadeViewModel
    {
        public virtual DateTime CreateDateTime { get; set; }
        public virtual DateTime? UpdateDateTime { get; set; }
    }
}