using System.ComponentModel.DataAnnotations;

namespace Monitor.Domain.ViewModels.Endpoint
{
    public class IncluirEndpointViewModel
    {
        public long HandleSistema { get; set; }
        [Required]
        [StringLength(70)]
        public string Nome { get; set; }
        [Required]
        [StringLength(1000)]
        public string Url { get; set; }   
    }
}