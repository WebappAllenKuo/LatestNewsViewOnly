using System;
using System.Collections.Generic;
using Infra.Extensions;
using Infra.Models.Infra;
using Infra.Models.News;
using KueiExtensions.System.Text.Json;
using LatestNewsWeb.Infra;
using LatestNewsWeb.Infra.Validate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Account;
using Services.News;
using Services.Options;

namespace LatestNewsWeb.Controllers
{
    public class NewsController : BaseController
    {
        private readonly ValidatorFactory _validatorFactory;
        private readonly OptionService    _optionService;
        private readonly NewsService      _newsService;

        public NewsController(AccountService       accountService,
                              IHttpContextAccessor httpContextAccessor,
                              ValidatorFactory     validatorFactory,
                              OptionService        optionService,
                              NewsService          newsService)
            : base(accountService, httpContextAccessor)
        {
            _validatorFactory   = validatorFactory;
            _optionService = optionService;
            _newsService        = newsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.ColumnsJson = typeof(ListItemDto).ToTableColumnDtos().ToJson();

            return View();
        }

        [HttpPost]
        [Route("api/[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult GetList([FromBody]PageInfoDto pageInfo)
        {
            _validatorFactory.Validate(ModelState, pageInfo);

            var dto = new ListDto
                      {
                          PageInfo = pageInfo ?? new PageInfoDto(),
                          Items    = new ListItemDto[] {}
                      };

            SortColumn(dto.PageInfo);

            _newsService.GetPublishedSortByIsTop(dto);

            return ResponseDto(dto);
        }

        [HttpGet]
        public IActionResult Detail(Guid guid)
        {

            var dto = _newsService.Detail(guid);

            AssignAttachmentUrls(dto.AttachmentDtos);

            var optionsMap = new Dictionary<string, Option[]>
                             {
                                 [nameof(DetailDto.IsPublished)] = _optionService.GetBoolean(),
                                 [nameof(DetailDto.IsTop)]       = _optionService.GetBoolean()
                             };
            ViewBag.ColumnsJson = dto.GetType().ToTableColumnDtos(optionsMap).ToJson();

            return View(dto);
        }
    }
}
