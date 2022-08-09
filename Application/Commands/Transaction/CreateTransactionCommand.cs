using Dapper;
using Domain.Models;
using Domain.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Globalization;

namespace Application.Commands.Transaction
{
    public class CreateTransactionCommand
    {
        public class CreateTransactionCommandHandler : IRequestHandler<VmTransaction, VmCommandResult>
        {
            private readonly IConfiguration configuration;
            public CreateTransactionCommandHandler(IConfiguration configuration)
            {
                this.configuration = configuration;
            }
            public async Task<VmCommandResult> Handle(VmTransaction command, CancellationToken cancellationToken)
            {
                VmCommandResult ret;
                string sql = @"INSERT INTO [Transaction]
                         (TransactionId, PersonId, TransactionDate, Price)
VALUES        (@TransactionId,@PersonId,@TransactionDate,@Price);";
                Transactions transaction = new Transactions
                {
                    PersonId = command.PersonId,
                    Price = command.Price,
                    TransactionId = command.TransactionId,
                    TransactionDate = DateTime.ParseExact(command.TransactionDate, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture)
                };
                try
                {
                    using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        await connection.ExecuteAsync(sql, transaction);
                        ret = new VmCommandResult
                        {
                            Success = true,
                            Message = "ثبت با موفقیت انجام شد"
                        };
                    }
                }
                catch (Exception ex)
                {
                    ret = new VmCommandResult
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
                return ret;
            }
        }
    }
}
