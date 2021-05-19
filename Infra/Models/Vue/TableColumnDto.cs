using System;
using System.Text.Json.Serialization;
using Infra.Models.Infra;
using Infra.Parameters;

namespace Infra.Models.Vue
{
    public class TableColumnDto
    {
        public string PropertyName { get; set; }

        public string DisplayName { get; set; }

        [JsonConverter(typeof(StringVueInputTypeJsonConverter))]
        public VueInputType InputType { get; set; }

        public Option[] Options { get; set; }

        /// <summary>
        /// 給 Text Area 跟 TinyMCE 用
        /// </summary>
        public int TextAreaRows { get; set; }

        /// <summary>
        /// 是否要支援排序
        /// </summary>
        public bool? Sortable { get; set; }
    }
}
