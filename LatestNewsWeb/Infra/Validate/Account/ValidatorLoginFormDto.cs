using Infra.Models.Account;

namespace LatestNewsWeb.Infra.Validate.Account
{
    public class ValidatorLoginFormDto : Validator<LoginFormDto>
    {
        protected override void CustomValidate(LoginFormDto dto)
        {
            base.Validate(dto);
        }
    }
}
