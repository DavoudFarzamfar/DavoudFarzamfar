using Dapper;
using Domain.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;


namespace Application.Queries.Person
{
    public class GetSumTransactionWithTotalQuery : IRequest<IEnumerable<VmSumTransactionWithTotal>>
    {
        public class GetSumTransactionWithTotalQueryHandler : IRequestHandler<GetSumTransactionWithTotalQuery, IEnumerable<VmSumTransactionWithTotal>>
        {
            private readonly IConfiguration _configuration;
            public GetSumTransactionWithTotalQueryHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }
            public async Task<IEnumerable<VmSumTransactionWithTotal>> Handle(GetSumTransactionWithTotalQuery query, CancellationToken cancellationToken)
            {
                var sql = @"SELECT        Person.Name + ' ' + Person.Family AS FullName, [Transaction].PersonId, FORMAT([Transaction].TransactionDate, 'yyyy/MM/dd') AS Date, SUM([Transaction].Price) AS Price,
                             (SELECT        SUM(Price) AS Total
                               FROM            [Transaction] AS Tr
                               WHERE        (PersonId = [Transaction].PersonId) AND (FORMAT(TransactionDate, 'yyyy/MM/dd') <= FORMAT([Transaction].TransactionDate, 'yyyy/MM/dd'))) AS Total
FROM            Person INNER JOIN
                         [Transaction] ON Person.PersonId = [Transaction].PersonId
GROUP BY [Transaction].PersonId, FORMAT([Transaction].TransactionDate, 'yyyy/MM/dd'), Person.Name + ' ' + Person.Family
ORDER BY [Transaction].PersonId, Date";
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.QueryAsync<VmSumTransactionWithTotal>(sql);
                    return result;
                }
            }
        }
    }
}
