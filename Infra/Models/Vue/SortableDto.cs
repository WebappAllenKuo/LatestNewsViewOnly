using System;

namespace Infra.Models.Vue
{
    /// <summary>
    /// 對應 jquery-ui-drag-drop-dual-sortable
    /// </summary>
    public class SortableDto
    {
        public Guid?[] LeftItems { get; set; }

        public Guid?[] RightItems { get; set; }
    }
}
