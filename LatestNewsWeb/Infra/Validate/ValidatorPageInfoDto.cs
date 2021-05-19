using Infra.Models.Infra;

namespace LatestNewsWeb.Infra.Validate
{
    public class ValidatorPageInfoDto : Validator<PageInfoDto>
    {
        protected override void CustomValidate(PageInfoDto dto)
        {
            // 可能是 URL 輸入不正確的 Query String 導致 Model Binding 失敗， dto 就會是 null
            if (dto == null)
            {
                ValidateResult.Clear();
                return;
            }

            if (dto.PageNo < 1)
            {
                dto.PageNo = 1;
            }

            if (dto.OnePageCount < 1)
            {
                dto.OnePageCount = 10;
            }

            base.Validate(dto);
        }
    }
}
