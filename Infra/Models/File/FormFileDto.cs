namespace Infra.Models.File
{
    public class FormFileDto
    {
        /// <summary>
        ///     上傳檔案之 FileName
        /// </summary>
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string ContentType { get; set; }
    }
}