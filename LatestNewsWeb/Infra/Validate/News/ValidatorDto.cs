using System;
using Infra.Models.News;
using KueiExtensions;

namespace LatestNewsWeb.Infra.Validate.News
{
    public class ValidatorDto : Validator<Dto>
    {
        protected override void CustomValidate(Dto dto)
        {
            base.Validate(dto);
        }
    }
}
