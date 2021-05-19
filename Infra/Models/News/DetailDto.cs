using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Infra.Models.Vue;
using Infra.Parameters;
using KueiExtensions.System.Text.Json;

namespace Infra.Models.News
{
    public class DetailDto
    {
        /// <summary>
        /// Guid
        /// </summary>
        [Display(Name = "Guid")]
        public Guid? Guid { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        [Display(Name          = "標題")]
        [TableColumn(InputType = VueInputType.Text)]
        public string Title { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        [Display(Name          = "內容")]
        [TableColumn(InputType = VueInputType.Html)]
        public string Content { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        [Display(Name = "是否上架")]
        public bool IsPublished { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        [Display(Name          = "是否上架")]
        public string IsPublishedName { get; set; }

        /// <summary>
        /// 是否置頂
        /// </summary>
        [Display(Name = "是否置頂")]
        public bool IsTop { get; set; }

        /// <summary>
        /// 是否置頂
        /// </summary>
        [Display(Name          = "是否置頂")]
        public string IsTopName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        public int Sort { get; set; } = 99;

        /// <summary>
        /// 發佈日期
        /// </summary>
        [Display(Name = "發佈日期")]
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// 發佈時間
        /// </summary>
        [Display(Name = "發佈時間")]
        [JsonConverter(typeof(StringNullableTimeSpanJsonConverter))]
        public TimeSpan? PublishTime { get; set; }

        /// <summary>
        /// 發佈日期
        /// </summary>
        [Display(Name          = "發佈日期")]
        [TableColumn(InputType = VueInputType.Text)]
        public string PublishDateTimeFormatted
        {
            get
            {
                if (PublishDate.HasValue
                 && PublishTime.HasValue)
                {
                    return (PublishDate.Value + PublishTime.Value).ToString("yyyy/MM/dd HH:mm:ss");
                }

                if (PublishDate.HasValue)
                {
                    return PublishDate.Value.ToString("yyyy/MM/dd");
                }

                if (PublishTime.HasValue)
                {
                    return PublishTime.Value.ToString();
                }

                return string.Empty;
            }
        }


        /// <summary>
        /// 附件清單
        /// </summary>
        [Display(Name          = "已上傳附件")]
        [TableColumn(InputType = VueInputType.FilesListWithDeleteButton)]
        public AttachmentDto[] AttachmentDtos { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Display(Name = "建立時間")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 建立者 Guid
        /// </summary>
        [Display(Name = "建立者 Guid")]
        public Guid CreatorGuid { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        [Display(Name = "修改時間")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改者 Guid
        /// </summary>
        [Display(Name = "修改者 Guid")]
        public Guid? UpdatorGuid { get; set; }

        /// <summary>
        /// 資料狀態 Id
        /// </summary>
        [Display(Name = "資料狀態 Id")]
        public long DataStatusId { get; set; }
    }
}
