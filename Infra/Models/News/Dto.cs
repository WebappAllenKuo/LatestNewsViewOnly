using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Infra.Models.Vue;
using Infra.Parameters;
using KueiExtensions.System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Infra.Models.News
{
    public class Dto
    {
        /// <summary>
        /// Guid
        /// </summary>
        [Display(Name = "Guid")]
        public Guid? Guid { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        [Display(Name                   = "標題")]
        [Required(ErrorMessage          = "請填寫{0}")]
        [StringLength(500, ErrorMessage = "{0} 長度要介於 {2} 及 {1} 之間")]
        [TableColumn(InputType          = VueInputType.Text)]
        public string Title { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        [Display(Name                    = "內容")]
        [Required(ErrorMessage           = "請填寫{0}")]
        [StringLength(5000, ErrorMessage = "{0} 長度要介於 {2} 及 {1} 之間")]
        [TableColumn(InputType           = VueInputType.TinyMCE, TextAreaRows = 10)]
        public string Content { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        [Display(Name          = "是否上架")]
        [Required(ErrorMessage = "請選擇{0}")]
        [TableColumn(InputType = VueInputType.Radio)]
        public bool IsPublished { get; set; }

        /// <summary>
        /// 是否置頂
        /// </summary>
        [Display(Name          = "是否置頂")]
        [Required(ErrorMessage = "請填寫{0}")]
        [TableColumn(InputType = VueInputType.Radio)]
        public bool IsTop { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name          = "排序")]
        [Required(ErrorMessage = "請填寫{0}")]
        [TableColumn(InputType = VueInputType.Number)]
        public int Sort { get; set; } = 99;

        /// <summary>
        /// 發佈日期
        /// </summary>
        [Display(Name          = "發佈日期")]
        [TableColumn(InputType = VueInputType.Date)]
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// 發佈時間
        /// </summary>
        [Display(Name          = "發佈時間")]
        [TableColumn(InputType = VueInputType.Time)]
        [JsonConverter(typeof(StringNullableTimeSpanJsonConverter))]
        public TimeSpan? PublishTime { get; set; }

        /// <summary>
        /// 附件清單
        /// </summary>
        [Display(Name          = "已上傳附件")]
        [TableColumn(InputType = VueInputType.FilesListWithDeleteButton)]
        public AttachmentDto[] AttachmentDtos { get; set; }

        /// <summary>
        /// 正在上傳的附件s
        /// </summary>
        [Display(Name          = "附件")]
        [TableColumn(InputType = VueInputType.UploadFiles)]
        public AttachmentDto[] UploadFiles { get; set; } = Array.Empty<AttachmentDto>();

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
