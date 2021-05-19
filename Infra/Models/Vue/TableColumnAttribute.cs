using System;
using Infra.Models.Infra;
using Infra.Parameters;

namespace Infra.Models.Vue
{
    /// <summary>
    /// 用來標示需要被產生 TableColumnDto 的 Attribute
    /// </summary>
    public class TableColumnAttribute : Attribute
    {
        public VueInputType InputType { get; set; }

        public int TextAreaRows { get; set; }

        public bool Sortable { get; set; } = true;
    }
}
