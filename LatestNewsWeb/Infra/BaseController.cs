using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using Infra.Models;
using Infra.Models.Infra;
using KueiExtensions;
using LatestNewsWeb.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Account;

namespace LatestNewsWeb.Infra
{
    public abstract class BaseController : Controller
    {
        protected readonly AccountService AccountService;

        private readonly IHttpContextAccessor HttpContextAccessor;

        protected IPAddress IPAddress => HttpContextAccessor?.HttpContext?.Connection.RemoteIpAddress;

        protected ISession Session => HttpContextAccessor?.HttpContext?.Session;

        protected Guid? UserGuid => HttpContextAccessor?.HttpContext?.User.Claims
                                                        .FirstOrDefault(c => c.Type == CustomClaimTypes.UserGuid)
                                                       ?.Value.ToNullableGuid();

        protected bool IsAuthenticated => UserInfo != null
                                       && Identity.IsAuthenticated;


        private IIdentity Identity => HttpContextAccessor?.HttpContext?.User.Identity;

        protected BaseController(AccountService       accountService,
                                 IHttpContextAccessor httpContextAccessor)
        {
            AccountService      = accountService;
            HttpContextAccessor = httpContextAccessor;
        }

        public UserInfoDto UserInfo => AccountService.GetUserInfo(UserGuid);

        /// <summary>
        /// 套用 Web API 統一回傳格式
        /// </summary>
        protected IActionResult ResponseDto(bool isValid = true)
        {
            return Ok(new ResponseDto
                      {
                          IsValid = isValid
                      });
        }

        /// <summary>
        /// 套用 Web API 統一回傳格式
        /// </summary>
        protected IActionResult ResponseDto(object dto, bool isValid = true)
        {
            return Ok(new ResponseDto
                      {
                          Dto     = dto,
                          IsValid = isValid
                      });
        }

        /// <summary>
        /// 前端排序整理
        /// </summary>
        protected static void SortColumn(PageInfoDto pageInfo)
        {
            if (string.IsNullOrWhiteSpace(pageInfo.ClickSortColumn) == false)
            {
                if (pageInfo.SortColumn == pageInfo.ClickSortColumn)
                {
                    if (pageInfo.SortColumnOrder == SortColumnOrder.Asc)
                    {
                        pageInfo.SortColumnOrder = SortColumnOrder.Desc;
                    }
                    else
                    {
                        pageInfo.SortColumnOrder = SortColumnOrder.Asc;
                    }
                }
                else
                {
                    pageInfo.SortColumn      = pageInfo.ClickSortColumn;
                    pageInfo.SortColumnOrder = SortColumnOrder.Asc;
                }

                pageInfo.ClickSortColumn = string.Empty;
            }
        }

        protected void AssignAttachmentUrls(AttachmentDto[] attachmentDtos)
        {
            foreach (var attachmentDto in attachmentDtos)
            {
                attachmentDto.UrlPath = GenerateAttachUrl(attachmentDto);
            }
        }

        private string GenerateAttachUrl(AttachmentDto attachmentDto)
        {
            var result = Url.Action("Attachment", "File", new { fileGuid = attachmentDto.Guid });
            return result;
        }
    }
}
