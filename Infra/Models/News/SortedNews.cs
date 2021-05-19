namespace Infra.Models.News
{
    public class SortedNews
    {
        /// <summary>
        /// 一般新聞
        /// </summary>
        public SortNewsItem[] NormalNews { get; set; }

        /// <summary>
        /// 罝頂新聞
        /// </summary>
        public SortNewsItem[] TopNews    { get; set; }
    }
}
