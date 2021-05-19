using System;

namespace Infra.Models
{
    public class AttachmentDto
    {
        public Guid? Guid { get; set; }

        /// <summary>
        /// 上傳檔案之 FileName
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// response content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 檔案系統中之檔案名稱 (含副檔名)
        /// </summary>
        public string ActualFileName { get; set; }

        /// <summary>
        /// 下載檔案時，才會給定
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// 用於顯示 Url 檔案路徑，才會給定
        /// </summary>
        public string UrlPath { get; set; }

        public Guid? UpdatorGuid { get; set; }
    }
}
