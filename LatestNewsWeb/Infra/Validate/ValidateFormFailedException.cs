using System;
using System.Collections.Generic;
using Infra.Models.News;

namespace LatestNewsWeb.Infra.Validate
{
    public class ValidateFormFailedException : Exception
    {
        public Dictionary<string, List<string>> ValidateResult { get; set; }

        public object Dto { get; set; }
    }
}
