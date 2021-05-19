using System.Net;
using LatestNewsWeb.Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Account;

namespace LatestNewsWeb.Controllers
{
    /// <summary>
    ///     Status Code Redirect 處理
    /// </summary>
    public class StatusCodeController : BaseController
    {
        public StatusCodeController(AccountService       accountService,
                                    IHttpContextAccessor httpContextAccessor)
            : base(accountService, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("[Controller]")]
        public IActionResult Index(HttpStatusCode code)
        {
            switch (code)
            {
                case HttpStatusCode.NotFound:
                    return View("NotFound");
                case HttpStatusCode.InternalServerError:
                default:
                    return View("InternalServerError");
            }
        }

        [Route("{*url}", Order = 999)]
        public IActionResult CatchAll()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        [HttpPost]
        [Route("Error")]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}
