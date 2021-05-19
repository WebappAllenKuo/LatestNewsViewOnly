using System;

namespace Infra.Models.News
{
    public class SortNewsItem
    {
        public Guid Guid { get; set; }

        public string Text { get; set; }

        public bool IsTop { get; set; }

        public int Sort { get; set; }
    }
}
