using Dapper;
using Domain.Models;
using Domain.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Application.Commands.Person
{
    public class CreatePersonCommand
    {
        public class CreatePersonCommandHandler : IRequestHandler<Persons, VmCommandResult>
        {
            private readonly IConfiguration configuration;
            public CreatePersonCommandHandler(IConfiguration configuration)
            {
                this.configuration = configuration;
            }
            public async Task<VmCommandResult> Handle(Persons command, CancellationToken cancellationToken)
            {
                VmCommandResult ret;
                string sql = @"INSERT INTO Person
                         (PersonId, Name, Family)
VALUES        (@PersonId,@Name,@Family);";

                try
                {
                    using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        await connection.ExecuteAsync(sql, command);
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
