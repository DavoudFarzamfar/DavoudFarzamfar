using Domain.Models;
using MediatR;

namespace Domain.ViewModels
{
    public class VmTransaction: Transactions, IRequest<VmCommandResult>
    {
        public string TransactionDate { get; set; }
    }
}
