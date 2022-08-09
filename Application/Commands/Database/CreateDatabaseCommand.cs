using Dapper;
using Domain.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Application.Commands.Database
{
    public class CreateDatabaseCommand : IRequest<VmCommandResult>
    {
        public class CreateDatabaseCommandHandler : IRequestHandler<CreateDatabaseCommand, VmCommandResult>
        {
            private readonly IConfiguration configuration;
            public CreateDatabaseCommandHandler(IConfiguration configuration)
            {
                this.configuration = configuration;
            }
            public async Task<VmCommandResult> Handle(CreateDatabaseCommand command, CancellationToken cancellationToken)
            {
                VmCommandResult ret = new VmCommandResult
                {
                    Success = true,
                    Message = "ساخت پایگاه داده با موفقیت انجام شد موفقیت انجام شد"
                }; ;

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"));
                string DbName = builder.InitialCatalog;

                string sql = $@"declare @DefaultData nvarchar(512)
exec master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'DefaultData', @DefaultData output

declare @DefaultLog nvarchar(512)
exec master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'DefaultLog', @DefaultLog output

declare @MasterData nvarchar(512)
exec master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer\Parameters', N'SqlArg0', @MasterData output
select @MasterData=substring(@MasterData, 3, 255)
select @MasterData=substring(@MasterData, 1, len(@MasterData) - charindex('\', reverse(@MasterData)))

declare @MasterLog nvarchar(512)
exec master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer\Parameters', N'SqlArg2', @MasterLog output
select @MasterLog=substring(@MasterLog, 3, 255)
select @MasterLog=substring(@MasterLog, 1, len(@MasterLog) - charindex('\', reverse(@MasterLog)))

select @DefaultData = isnull(@DefaultData, @MasterData)+ N'\{DbName}.mdf'
select @DefaultLog = isnull(@DefaultLog, @MasterLog)+ N'\{DbName}_log.ldf'

declare @createdatabase nvarchar(4000)

SELECT @createdatabase = 'CREATE DATABASE {DbName} ON PRIMARY ( NAME = ''{DbName}'', 
 FILENAME = ' + quotename(@DefaultData) + ') LOG ON ( NAME = ''{DbName}_log'', 
FILENAME = ' + quotename(@DefaultLog) + ')'

EXEC (@createdatabase)";


                try
                {
                    using (var connection = new SqlConnection(configuration.GetConnectionString("masterConnection")))
                    {
                        connection.Open();
                        await connection.ExecuteAsync(sql, command);
                    }
                    sql = @$"SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Person](
	[PersonId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Family] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
/****** Object:  Table [dbo].[Transaction]    Script Date: 8/9/2022 10:17:21 AM ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Transaction](
	[TransactionId] [int] NOT NULL,
	[PersonId] [int] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[Price] [int] NOT NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Person] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Person] ([PersonId])
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Person]
USE [master]
ALTER DATABASE [DbFarzamfarTest] SET  READ_WRITE";
                    using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        await connection.ExecuteAsync(sql, command);
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
