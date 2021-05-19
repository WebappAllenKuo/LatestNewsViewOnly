using System;
using System.Collections.Generic;
using Infra.Extensions;
using Infra.Models.Infra;
using Infra.Models.News;
using Infra.Models.Vue;
using Infra.Parameters;
using KueiExtensions.System.Text.Json;
using LatestNewsWeb.Infra;
using LatestNewsWeb.Infra.Validate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Account;
using Services.News;
using Services.Options;

namespace LatestNewsWeb.Areas.BackStage.Controllers
{
    [Area("BackStage")]
    [TypeFilter(typeof(UserInfoViewBagFilter))]
    [Authorize(Roles = RoleConst.Manager)]
    public class NewsController : BaseController
    {
        private readonly NewsService      _newsService;
        private readonly ValidatorFactory _validatorFactory;
        private readonly OptionService    _optionService;

        public NewsController(ILogger<HomeController> logger,
                              AccountService          accountService,
                              IHttpContextAccessor    httpContextAccessor,
                              NewsService             newsService,
                              ValidatorFactory        validatorFactory,
                              OptionService           optionService)
            : base(accountService, httpContextAccessor)
        {
            _newsService      = newsService;
            _validatorFactory = validatorFactory;
            _optionService    = optionService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var listDto = new ListDto
                          {
                              PageInfo = new PageInfoDto
                                         {
                                             // 預設排列順序
                                             SortColumn      = nameof(ListItemDto.Title),
                                             SortColumnOrder = SortColumnOrder.Asc,
                                         },
                          };

            _newsService.GetList(listDto);

            ViewBag.ColumnsJson = typeof(ListItemDto).ToTableColumnDtos().ToJson();

            ViewBag.NewsJson = listDto.ToJson();

            return View();
        }

        [HttpPost]
        [Route("api/[area]/[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult GetList([FromBody]PageInfoDto pageInfo)
        {
            var dto = new ListDto
                      {
                          PageInfo = pageInfo,
                          Items    = new ListItemDto[] {}
                      };

            SortColumn(pageInfo);

            _newsService.GetList(dto);

            return ResponseDto(dto);
        }

        [HttpPost]
        [Route("api/[area]/[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult GetSortedNews()
        {
            var result = _newsService.GetSortedNews();

            return ResponseDto(result);
        }

        [HttpPost]
        [Route("api/[area]/[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult PostSortedNews([FromBody]SortableDto dto)
        {
            // LeftItem：一般新聞
            // RightItem：置頂新聞

            _newsService.PostSortedNews(dto);

            return ResponseDto();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var optionsMap = new Dictionary<string, Option[]>
                             {
                                 [nameof(Dto.IsPublished)] = _optionService.GetBoolean(),
                                 [nameof(Dto.IsTop)]       = _optionService.GetBoolean()
                             };
            ViewBag.ColumnsJson        = typeof(Dto).ToTableColumnDtos(optionsMap).ToJson();
            ViewBag.ValidateResultJson = new Dictionary<string, List<string>>().ToJson();

            var viewModel = new Dto();

            return View("Edit", viewModel);
        }

        [HttpGet]
        public IActionResult Edit(Guid guid)
        {
            var viewModel = _newsService.Get(guid);

            AssignAttachmentUrls(viewModel.AttachmentDtos);

            var optionsMap = new Dictionary<string, Option[]>
                             {
                                 [nameof(Dto.IsPublished)] = _optionService.GetBoolean(),
                                 [nameof(Dto.IsTop)]       = _optionService.GetBoolean()
                             };
            ViewBag.ColumnsJson        = viewModel.GetType().ToTableColumnDtos(optionsMap).ToJson();
            ViewBag.ValidateResultJson = new Dictionary<string, List<string>>().ToJson();

            return View("Edit", viewModel);
        }

        /// <summary>
        /// 建立 / 編輯 共用
        /// </summary>
        [HttpPost, Route("api/[area]/[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult PostEdit([FromBody]Dto dto)
        {
            _validatorFactory.Validate(ModelState, dto);

            _newsService.CreateOrEdit(dto, new Guid("2F4BDE2B-A2DD-4BB8-AC28-A3EAC8F45186"));

            return ResponseDto(dto);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        [HttpPost, Route("api/[area]/[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromBody]Guid newsGuid)
        {
            _newsService.Delete(newsGuid);

            return ResponseDto();
        }
    }
}
