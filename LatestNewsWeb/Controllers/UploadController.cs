using System;
using System.Threading.Tasks;
using Infra.Models.Infra;
using LatestNewsWeb.Infra;
using LatestNewsWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Account;
using Services.Infra;

namespace LatestNewsWeb.Controllers
{
    public class UploadController : BaseController
    {
        private readonly PathService _pathService;

        private readonly FileUploadService _fileUploadService;

        public UploadController(AccountService       accountService,
                                IHttpContextAccessor httpContextAccessor,
                                PathService          pathService,
                                FileUploadService    fileUploadService)
            : base(accountService, httpContextAccessor)
        {
            _pathService       = pathService;
            _fileUploadService = fileUploadService;
        }

        /// <summary>
        /// 上傳圖檔
        /// </summary>
        [HttpPost("api/[controller]/[action]")]
        // TODO：[ValidateAntiForgeryToken]
        public async Task<IActionResult> TinyMceImage(IFormFile file)
        {
            if (file == null)
            {
                return Ok(new ResponseDto());
            }

            var toFolder = _pathService.GetImageFolderPath();

            var attachmentDto = await _fileUploadService.SaveAsync(file, new Guid("2F4BDE2B-A2DD-4BB8-AC28-A3EAC8F45186"), toFolder);

            return Ok(new
                      {
                          location = Url.Action("Image", "File", new { fileGuid = attachmentDto.Guid })
                      });
        }

        /// <summary>
        /// 上傳附件
        /// </summary>
        [HttpPost("api/[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Attachments(IFormFile[] files)
        {
            if (files?.Length > 0 == false)
            {
                return Ok(new ResponseDto());
            }

            var toFolder = _pathService.GetAttachmentFolderPath();

            var attachmentDto = await _fileUploadService.SaveAsync(files, new Guid("2F4BDE2B-A2DD-4BB8-AC28-A3EAC8F45186"), toFolder);

            return ResponseDto(attachmentDto);
        }
    }
}
