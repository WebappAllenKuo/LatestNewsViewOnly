using System;
using System.Data;
using System.Linq;
using Dapper;
using Infra.Models;
using Microsoft.Data.SqlClient;

namespace Repositories
{
    public class AttachmentRepository : BaseRepository
    {
        private readonly IDbConnection _sqlConnection;

        public AttachmentRepository(IDbConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public AttachmentDto Get(Guid fileGuid)
        {
            var sql = @"
SELECT TOP 1 [a].[Guid],
             [a].[FileName],
             [a].[ContentType],
             [a].[ActualFileName]
FROM [dbo].[Attachment] [a]
WHERE [Guid] = @guid
";
            var param = new DynamicParameters();
            param.Add("Guid", fileGuid, DbType.Guid);

            return _sqlConnection.Query<AttachmentDto>(sql, param).FirstOrDefault();
        }

        public void Add(params AttachmentDto[] dtos)
        {
            _sqlConnection.Open();

            using (var trans = _sqlConnection.BeginTransaction())
            {
                var sql = @"
INSERT INTO [dbo].[Attachment]([Guid], [FileName], [ContentType], [ActualFileName], [CreateTime], [CreatorGuid], [UpdateTime], [UpdatorGuid])
VALUES (@Guid, @FileName, @ContentType, @ActualFileName, @CreateTime, @CreatorGuid, @UpdateTime, @UpdatorGuid)
";
                var now = DateTime.Now;

                try
                {
                    foreach (var dto in dtos)
                    {
                        var param = new DynamicParameters();
                        param.Add("Guid",           dto.Guid.Value,     DbType.Guid);
                        param.Add("FileName",       dto.FileName,       DbType.String, size: 100);
                        param.Add("ContentType",    dto.ContentType,    DbType.String, size: 100);
                        param.Add("ActualFileName", dto.ActualFileName, DbType.String, size: 100);
                        param.Add("CreateTime",     now,                DbType.DateTime2);
                        param.Add("CreatorGuid",    dto.UpdatorGuid,    DbType.Guid);
                        param.Add("UpdateTime",     now,                DbType.DateTime2);
                        param.Add("UpdatorGuid",    dto.UpdatorGuid,    DbType.Guid);

                        _sqlConnection.Execute(sql, param, trans);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
    }
}
