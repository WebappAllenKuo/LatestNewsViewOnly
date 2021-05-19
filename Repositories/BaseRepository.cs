using Infra.Models.Infra;

namespace Repositories
{
    public class BaseRepository
    {
        protected string SortColumn(string sortColumn, SortColumnOrder sortColumnOrder)
        {
            return $"{sortColumn} {sortColumnOrder}";
        }
    }
}
