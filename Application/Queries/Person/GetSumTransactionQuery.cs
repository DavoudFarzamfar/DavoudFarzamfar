using Dapper;
using Domain.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Application.Queries.Person
{
    public class GetSumTransactionQuery : IRequest<IEnumerable<VmSumTransaction>>
    {
        public class GetSumTransactionQueryHandler : IRequestHandler<GetSumTransactionQuery, IEnumerable<VmSumTransaction>>
        {
            private readonly IConfiguration _configuration;
            public GetSumTransactionQueryHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }
            public async Task<IEnumerable<VmSumTransaction>> Handle(GetSumTransactionQuery query, CancellationToken cancellationToken)
            {
                var sql = @"SELECT        Person.Name + ' ' + Person.Family AS FullName, Person.PersonId, FORMAT([Transaction].TransactionDate,'yyyy/MM/dd') AS Date, SUM([Transaction].Price) AS Price
FROM            Person INNER JOIN
                         [Transaction] ON Person.PersonId = [Transaction].PersonId
GROUP BY FORMAT([Transaction].TransactionDate,'yyyy/MM/dd'), Person.PersonId, Person.Name + ' ' + Person.Family
ORDER BY Person.PersonId, Date";
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.QueryAsync<VmSumTransaction>(sql);
                    return result;
                }
            }
        }
    }
}
