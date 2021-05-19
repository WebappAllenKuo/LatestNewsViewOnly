using System;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using Dapper;
using Infra.Models;
using Infra.Models.Account;
using Infra.Parameters;
using KueiExtensions.Dapper;
using Microsoft.Data.SqlClient;

namespace Repositories.Account
{
    public class AccountRepository : BaseRepository
    {
        private readonly IDbConnection _sqlConnection;

        public AccountRepository(IDbConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public UserInfoDto GetUserByGuid(Guid userGuid)
        {
            var sql = @"
SELECT [u].[Guid],
       [u].[Email],
       [u].[Name],
       [r].[Name]  AS [RoleName],
       [r].[Value] AS [Role]
FROM [dbo].[User] [u]
    JOIN [dbo].[Role] [r]
         ON [u].[RoleId] = [r].[Id]
WHERE [u].[DataStatusId] = @ActiveDataStatusId
  AND [u].[Guid] = @userGuid
";
            var param = new DynamicParameters();
            param.Add("userGuid",           userGuid,                DbType.Guid);
            param.Add("ActiveDataStatusId", (long)DataStatus.Active, DbType.Int64);

            var result = _sqlConnection.MultipleResult<UserInfoDto>(sql, param)
                                       .Read((ref UserInfoDto boxDto, SqlMapper.GridReader reader) => boxDto = reader.ReadFirstOrDefault<UserInfoDto>())
                                       .Query();

            return result;
        }

        public LoginInfoDto GetLoginInfo(string email)
        {
            var sql = @"
SELECT [u].[Guid],
       [u].[Email],
       [u].[Name],
       [u].[Password],
       [u].[PasswordSalt],
       [u].[Name],
       [r].[Name]  AS [RoleName],
       [r].[Value] AS [Role]
FROM [dbo].[User] [u]
    JOIN [dbo].[Role] [r]
         ON [u].[RoleId] = [r].[Id]
WHERE [u].[DataStatusId] = @ActiveDataStatusId
  AND [u].[Email] = @Email
";
            var param = new DynamicParameters();
            param.Add("Email",              email,                   DbType.AnsiString, size: 100);
            param.Add("ActiveDataStatusId", (long)DataStatus.Active, DbType.Int64);

            var result = _sqlConnection.MultipleResult<LoginInfoDto>(sql, param)
                                       .Read((ref LoginInfoDto boxDto, SqlMapper.GridReader reader) => boxDto = reader.ReadFirstOrDefault<LoginInfoDto>())
                                       .Query();

            return result;
        }
    }
}
