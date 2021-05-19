namespace Infra.Parameters
{
    /// <summary>
    /// 對應至 vue input type
    /// </summary>
    public enum VueInputType
    {
        None = 0,

        Date = 1,

        Time = 2,

        Number = 3,

        Text = 4,

        TinyMCE = 5,

        TextArea = 6,

        Radio = 7,

        DropDown = 8,

        /// <summary>
        /// 套用 jQuery UI Date Picker
        /// </summary>
        jQueryUiDatePicker = 9,

        /// <summary>
        /// 上傳檔案s
        /// </summary>
        UploadFiles = 10,

        /// <summary>
        /// 列出檔案清單，並提供刪除鈕，用於 新增/ 編輯
        /// </summary>
        FilesListWithDeleteButton = 11,

        DateTime = 12,

        /// <summary>
        /// 顯示 Html
        /// </summary>
        Html = 13,

        Password = 14,
    }
}
