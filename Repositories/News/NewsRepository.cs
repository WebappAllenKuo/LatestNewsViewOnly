using System;
using System.Data;
using System.Linq;
using Dapper;
using Infra.Models;
using Infra.Models.News;
using Infra.Models.Vue;
using Infra.Parameters;
using Infra.Parameters.News;
using KueiExtensions;
using KueiExtensions.Dapper;
using Microsoft.Data.SqlClient;

namespace Repositories.News
{
    public class NewsRepository : BaseRepository
    {
        private readonly IDbConnection _sqlConnection;

        public NewsRepository(IDbConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public void GetList(ListDto listDto)
        {
            var sql = @"
;

SELECT COUNT(0)
FROM [dbo].[News] [n]
WHERE [n].[DataStatusId] = @ActiveDataStatusId
";
            var param = new DynamicParameters();
            param.Add("offset",       listDto.PageInfo.Skip,         DbType.Int32);
            param.Add("onePageCount", listDto.PageInfo.OnePageCount, DbType.Int32);

            param.Add("SortColumn", SortColumn(listDto.PageInfo.SortColumn, listDto.PageInfo.SortColumnOrder), DbType.String, size: 100);

            param.Add("ActiveDataStatusId", (long)DataStatus.Active, DbType.Int64);

            // param.Add("SearchVendorName",  listDto.SearchVendorName,  DbType.String, size: 500);
            // param.Add("SearchVendorGuid",  listDto.SearchVendorGuid,  DbType.Guid);
            // param.Add("SearchVendorTaxId", listDto.SearchVendorTaxId, DbType.String, size: 500);

            var reader = _sqlConnection.QueryMultiple(sql, param);

            listDto.Items              = reader.Read<ListItemDto>().ToArray();
            listDto.PageInfo.DataCount = reader.Read<int>().FirstOrDefault();
        }

        public void GetPublishedSortByIsTop(ListDto listDto)
        {
            var sql = @"

";
            var param = new DynamicParameters();
            param.Add("offset",       listDto.PageInfo.Skip,         DbType.Int32);
            param.Add("onePageCount", listDto.PageInfo.OnePageCount, DbType.Int32);

            param.Add("SortColumn", SortColumn(listDto.PageInfo.SortColumn, listDto.PageInfo.SortColumnOrder), DbType.String, size: 100);

            param.Add("ActiveDataStatusId", (long)DataStatus.Active, DbType.Int64);
            param.Add("IsPublished",        (long)IsPublish.True,    DbType.Int64);

            param.Add("SearchTitle", listDto.PageInfo.SearchFields.GetValueOrNull("SearchTitle"), DbType.String, size: 500);

            _sqlConnection.MultipleResult(sql, param)
                          .Read(reader => listDto.Items              = reader.Read<ListItemDto>().ToArray())
                          .Read(reader => listDto.PageInfo.DataCount = reader.Read<int>().FirstOrDefault())
                          .Query();
        }

        public SortedNews GetSortedNews()
        {
            var sql = @"
SELECT [n].[Guid],
       [n].[Title] AS [Text],
       [n].[IsTop],
       [n].[Sort]
FROM [dbo].[News] [n]
WHERE [n].[DataStatusId] = @ActiveDataStatusId
  AND [n].[IsTop] = @IsNotTop
ORDER BY [n].[Sort]

SELECT [n].[Guid],
       [n].[Title] AS [Text],
       [n].[IsTop],
       [n].[Sort]
FROM [dbo].[News] [n]
WHERE [n].[DataStatusId] = @ActiveDataStatusId
  AND [n].[IsTop] = @IsTop
ORDER BY [n].[Sort] 
";
            var param = new DynamicParameters();
            param.Add("ActiveDataStatusId", (long)DataStatus.Active, DbType.Int64);
            param.Add("IsTop",              (long)IsTop.True,        DbType.Int64);
            param.Add("IsNotTop",           (long)IsTop.False,       DbType.Int64);

            var reader = _sqlConnection.QueryMultiple(sql, param);

            var result = new SortedNews();
            result.NormalNews = reader.Read<SortNewsItem>().ToArray();
            result.TopNews    = reader.Read<SortNewsItem>().ToArray();

            return result;
        }

        public void Create(Dto dto)
        {
            _sqlConnection.Open();

            using (var trans = _sqlConnection.BeginTransaction())
            {
                try
                {
                    var sql = @"
INSERT INTO [dbo].[News]([Guid],
                         [Title],
                         [Content],
                         [IsPublished],
                         [IsTop],
                         [PublishDate],
                         [PublishTime],
                         [Sort],
                         [CreateTime],
                         [CreatorGuid])
VALUES (@Guid,
        @Title,
        @Content,
        @IsPublished,
        @IsTop,
        @PublishDate,
        @PublishTime,
        @Sort,
        @CreateTime,
        @CreatorGuid)
";
                    var param = new DynamicParameters();
                    param.Add("Guid",        dto.Guid);
                    param.Add("Title",       dto.Title);
                    param.Add("Content",     dto.Content);
                    param.Add("IsPublished", dto.IsPublished);
                    param.Add("IsTop",       dto.IsTop);
                    param.Add("PublishDate", dto.PublishDate);
                    param.Add("PublishTime", dto.PublishTime);
                    param.Add("Sort",        dto.Sort);
                    param.Add("CreateTime",  dto.CreateTime);
                    param.Add("CreatorGuid", dto.CreatorGuid);

                    _sqlConnection.Execute(sql, param, trans);

                    // 新上傳的附件

                    var insertAttachmentSql = @"
INSERT [dbo].[NewsAttachment] ([NewsGuid],
                               [AttachmentGuid],
                               [CreateTime],
                               [CreatorGuid],
                               [DataStatusId])
VALUES (@NewsGuid,
        @AttachmentGuid,
        @CreateTime,
        @CreatorGuid,
        @ActiveDataStatusId)
";
                    foreach (var attachmentDto in dto.UploadFiles)
                    {
                        var attachmentParam = new DynamicParameters();
                        attachmentParam.Add("NewsGuid",           dto.Guid);
                        attachmentParam.Add("AttachmentGuid",     attachmentDto.Guid);
                        attachmentParam.Add("CreateTime",         dto.CreateTime);
                        attachmentParam.Add("CreatorGuid",        dto.UpdatorGuid);
                        attachmentParam.Add("ActiveDataStatusId", (long)DataStatus.Active, DbType.Int64);

                        _sqlConnection.Execute(insertAttachmentSql, attachmentParam, trans);
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

        public Dto Get(Guid guid)
        {
            var sql = @"
SELECT [n].*
FROM [dbo].[News] [n]
WHERE [n].[Guid] = @NewsGuid

SELECT [a].*
FROM [dbo].[NewsAttachment] [na]
    JOIN [dbo].[Attachment] [a]
         ON [a].[Guid] = [na].[AttachmentGuid]
WHERE [na].[NewsGuid] = @NewsGuid
  AND [na].[DataStatusId] = @ActiveDataStatusId
";
            var param = new DynamicParameters();
            param.Add("NewsGuid",           guid);
            param.Add("ActiveDataStatusId", (long)DataStatus.Active, DbType.Int64);

            var result = _sqlConnection.MultipleResult<Dto>(sql, param)
                                       .Read((ref Dto boxDto, SqlMapper.GridReader reader) => boxDto = reader.ReadFirstOrDefault<Dto>())
                                       .Read((ref Dto boxDto, SqlMapper.GridReader reader) => boxDto.AttachmentDtos = reader.Read<AttachmentDto>().ToArray())
                                       .Query();

            return result;
        }

        public void Edit(Dto dto)
        {
            _sqlConnection.Open();

            using (var trans = _sqlConnection.BeginTransaction())
            {
                try
                {
                    var sql = @"
UPDATE [dbo].[News]
SET [Title]       = @Title,
    [Content]     = @Content,
    [IsPublished] = @IsPublished,
    [IsTop]       = @IsTop,
    [PublishDate] = @PublishDate,
    [PublishTime] = @PublishTime,
    [Sort]        = @Sort,
    [UpdateTime]  = @UpdateTime,
    [UpdatorGuid] = @UpdatorGuid
WHERE [Guid] = @Guid
";
                    var param = new DynamicParameters();
                    param.Add("Guid",        dto.Guid);
                    param.Add("Title",       dto.Title);
                    param.Add("Content",     dto.Content);
                    param.Add("IsPublished", dto.IsPublished);
                    param.Add("IsTop",       dto.IsTop);
                    param.Add("PublishDate", dto.PublishDate);
                    param.Add("PublishTime", dto.PublishTime);
                    param.Add("Sort",        dto.Sort);
                    param.Add("UpdateTime",  dto.UpdateTime);
                    param.Add("UpdatorGuid", dto.UpdatorGuid);

                    _sqlConnection.Execute(sql, param, trans);

                    #region 刪除附件

                    var deleteAttachmentSql = @"
UPDATE [na]
SET [DataStatusId] = @DeletedDataStatusId
FROM [dbo].[NewsAttachment] [na]
WHERE [na].[NewsGuid] = @NewsGuid
  AND [na].[AttachmentGuid] NOT IN @AttachmentGuids
                    ";
                    var deleteAttachmentParam = new DynamicParameters();
                    deleteAttachmentParam.Add("NewsGuid",            dto.Guid);
                    deleteAttachmentParam.Add("AttachmentGuids",     dto.AttachmentDtos.Select(a => a.Guid));
                    deleteAttachmentParam.Add("DeletedDataStatusId", DataStatus.Deleted);

                    _sqlConnection.Execute(deleteAttachmentSql, deleteAttachmentParam, trans);

                    #endregion

                    #region 新上傳的附件

                    var insertAttachmentSql = @"
INSERT [dbo].[NewsAttachment] ([NewsGuid],
                               [AttachmentGuid],
                               [CreateTime],
                               [CreatorGuid],
                               [DataStatusId])
VALUES (@NewsGuid,
        @AttachmentGuid,
        @UpdateTime,
        @UpdatorGuid,
        @ActiveDataStatusId)
";
                    foreach (var attachmentDto in dto.UploadFiles)
                    {
                        var insertAttachmentParam = new DynamicParameters();
                        insertAttachmentParam.Add("NewsGuid",           dto.Guid);
                        insertAttachmentParam.Add("AttachmentGuid",     attachmentDto.Guid);
                        insertAttachmentParam.Add("UpdateTime",         dto.UpdateTime);
                        insertAttachmentParam.Add("UpdatorGuid",        dto.UpdatorGuid);
                        insertAttachmentParam.Add("ActiveDataStatusId", (long)DataStatus.Active, DbType.Int64);

                        _sqlConnection.Execute(insertAttachmentSql, insertAttachmentParam, trans);
                    }

                    #endregion


                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public void Delete(Guid newsGuid)
        {
            var sql = @"
UPDATE [dbo].[News]
SET [DataStatusId] = @DeleteDataStatusId 
WHERE [Guid] = @Guid
";
            var param = new DynamicParameters();
            param.Add("Guid",               newsGuid);
            param.Add("DeleteDataStatusId", DataStatus.Deleted);

            _sqlConnection.Execute(sql, param);
        }

        public void PostSortedNews(SortableDto dto)
        {
            _sqlConnection.Open();
            using (var trans = _sqlConnection.BeginTransaction())
            {
                try
                {
                    var sql = @"
UPDATE [dbo].[News]
SET [Sort] = @Sort, 
    [IsTop] = @IsTopId
WHERE [Guid] = @Guid
";
                    UpdateSort(trans, dto.LeftItems,  sql, IsTop.False);
                    UpdateSort(trans, dto.RightItems, sql, IsTop.True);

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    _sqlConnection.Close();
                    throw;
                }
            }

            void UpdateSort(IDbTransaction trans,
                            Guid?[]        items,
                            string         sql,
                            IsTop          isTop)
            {
                for (var index = 0; index < items.Length; index++)
                {
                    var item = items[index];
                    if (item.HasValue == false)
                    {
                        continue;
                    }

                    var param = new DynamicParameters();
                    param.Add("Guid",    item,  DbType.Guid);
                    param.Add("Sort",    index, DbType.Int32);
                    param.Add("IsTopId", isTop);

                    _sqlConnection.Execute(sql, param, trans);
                }
            }
        }
    }
}
