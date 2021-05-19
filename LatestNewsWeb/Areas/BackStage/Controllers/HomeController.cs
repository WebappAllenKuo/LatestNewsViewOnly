using Infra.Parameters;
using LatestNewsWeb.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Account;

namespace LatestNewsWeb.Areas.BackStage.Controllers
{
    [Area("BackStage")]
    [TypeFilter(typeof(UserInfoViewBagFilter))]
    [Authorize(Roles = RoleConst.Manager)]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(AccountService       accountService,
                              IHttpContextAccessor httpContextAccessor)
            : base(accountService, httpContextAccessor)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
