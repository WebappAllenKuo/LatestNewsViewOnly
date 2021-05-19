using System;
using System.ComponentModel.DataAnnotations;
using Infra.Extensions;
using Infra.Models.Vue;
using Infra.Parameters;
using KueiExtensions;

namespace Infra.Models.News
{
    public class ListItemDto
    {
        /// <summary>
        /// 項次
        /// </summary>
        [TableColumn(Sortable = false)]
        [Display(Name         = "項次")]
        public long No { get; set; }

        public Guid Guid { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        [TableColumn]
        [Display(Name = "標題")]
        public string Title { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        [Display(Name = "內容")]
        public string Content { get; set; }

        public bool IsPublished { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        [TableColumn]
        [Display(Name = "是否上架")]
        public string IsPublishedName => IsPublished.ToChineseNameYesNo();

        /// <summary>
        /// 是否置頂
        /// </summary>
        [Display(Name = "是否置頂")]
        public bool IsTop { get; set; }

        /// <summary>
        /// 是否置頂
        /// </summary>
        [TableColumn]
        [Display(Name = "是否置頂")]
        public string IsTopName => IsTop.ToChineseNameYesNo();

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        public int Sort { get; set; } = 99;

        /// <summary>
        /// 發佈時間
        /// </summary>
        [Display(Name = "發佈時間")]
        [TableColumn]
        public string PublishDateTime {
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
        /// 發佈日期
        /// </summary>
        [Display(Name = "發佈日期")]
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// 發佈時間
        /// </summary>
        [Display(Name = "發佈時間")]
        public TimeSpan? PublishTime { get; set; }
    }
}
