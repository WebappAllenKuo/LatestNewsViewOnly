using System.Collections.Generic;

namespace Infra.Models.Infra
{
    public class ResponseDto
    {
        public int? ErrorCode { get; set; }

        public string Message { get; set; }

        public Dictionary<string, List<string>> ValidateResult { get; set; }

        public object Dto { get; set; }

        public bool IsValid { get; set; } = true;
    }
}
