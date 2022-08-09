using Domain.ViewModels;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Persons : IRequest<VmCommandResult>
    {
        [Key]
        [Required(ErrorMessage = "لطفا شناسه را وارد کنید")]
        public int PersonId { get; set; }
        [Required(ErrorMessage = "لطفا نام را وارد کنید")]
        [MaxLength(100, ErrorMessage = "نام نمیتواند بیش از 100 حرف باشد")]
        public string Name { get; set; }
        [Required(ErrorMessage = "لطفا نام خانوادگی را وارد کنید")]
        [MaxLength(100, ErrorMessage = "نام خانوادگی نمیتواند بیش از 100 حرف باشد")]
        public string Family { get; set; }
    }
}
