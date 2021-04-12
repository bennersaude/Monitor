using System;
using System.ComponentModel.DataAnnotations;

namespace Monitor.Domain.ViewModels.Entidade
{
    public abstract class EntidadeAuditavelViewModel : EntidadeViewModel
    {
        [Display(Name = "Criação")]
        public virtual DateTime CreateDateTime { get; set; }
        [Display(Name = "Alteração")]
        public virtual DateTime? UpdateDateTime { get; set; }
    }
}