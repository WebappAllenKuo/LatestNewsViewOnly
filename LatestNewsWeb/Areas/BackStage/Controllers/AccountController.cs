using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Infra.Extensions;
using Infra.Models;
using Infra.Models.Account;
using KueiExtensions;
using KueiExtensions.System.Text.Json;
using LatestNewsWeb.Infra;
using LatestNewsWeb.Infra.Validate;
using LatestNewsWeb.Parameters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Account;

namespace LatestNewsWeb.Areas.BackStage.Controllers
{
    /// <summary>
    /// 個人帳號
    /// </summary>
    [Area("BackStage")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class AccountController : BaseController
    {
        public AccountController(AccountService             accountService,
                                 IHttpContextAccessor       httpContextAccessor,
                                 ILogger<AccountController> logger)
            : base(accountService, httpContextAccessor)
        {
            _logger           = logger;
        }

        private readonly ILogger<AccountController> _logger;

        [HttpGet]
        public IActionResult Login([FromQuery]string returnUrl)
        {
            if (IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new LoginFormDto
                            {
                                ReturnUrl = returnUrl.IsNullOrWhiteSpace()
                                                ? Url.Action("Index", "Home")
                                                : returnUrl
                            };
            ViewBag.ColumnsJson        = viewModel.GetType().ToTableColumnDtos().ToJson();
            ViewBag.ValidateResultJson = new Dictionary<string, List<string>>().ToJson();

            return View("Login", viewModel);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            var logMessage = $"UserGuid: {UserInfo?.Guid},UserName: {UserInfo?.Name}, RefererUrl:{Request.Headers["Referer"].ToString()}";
            _logger.LogInformation(logMessage);

            return View();
        }
    }
}
