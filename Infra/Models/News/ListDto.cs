using Infra.Models.Infra;

namespace Infra.Models.News
{
    public class ListDto
    {
        public PageInfoDto PageInfo { get; set; }

        public ListItemDto[] Items { get; set; }
    }
}
