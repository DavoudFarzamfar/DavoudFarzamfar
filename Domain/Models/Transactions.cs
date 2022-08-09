using Domain.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Transactions
    {
        [Key]
        [Required(ErrorMessage = "لطفا شناسه را وارد کنید")]
        public int TransactionId { get; set; }
        [Required(ErrorMessage = "لطفا شناسه شخص را وارد کنید")]
        public int PersonId { get; set; }
        [Required(ErrorMessage = "لطفا تاریخ تراکنش را وارد کنید")]
        public DateTime TransactionDate { get; set; }
        [Required(ErrorMessage = "لطفا مبلغ را وارد کنید")]
        public int Price { get; set; }
    }
}
