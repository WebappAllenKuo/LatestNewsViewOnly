using Microsoft.AspNetCore.Mvc.Filters;

namespace LatestNewsWeb.Infra
{
    /// <summary>
    /// 給定 UserInfo 的資料至 ViewBag.UserInfo 中
    /// </summary>
    public class UserInfoViewBagFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var baseController = (context.Controller as BaseController);

            if (baseController == null)
            {
                return;
            }

            baseController.ViewBag.UserInfo = baseController.UserInfo;
        }
    }
}