using System;
using System.IO;
using LatestNewsWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace LatestNewsWeb.Controllers
{
    public class FileController : ControllerBase
    {
        private readonly FileService _fileService;

        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [Route("api/[controller]/[action]/{fileGuid:guid}")]
        public IActionResult Image(Guid fileGuid)
        {
            var dto      = _fileService.LoadFromImageFolder(fileGuid);
            var filePath = Path.Combine(dto.FolderPath, dto.ActualFileName);
            var fileInfo = new FileInfo(filePath);

            return File(fileInfo.OpenRead(), dto.ContentType, dto.FileName);
        }

        [HttpGet]
        [Route("api/[controller]/[action]/{fileGuid:guid}")]
        public IActionResult Attachment(Guid fileGuid)
        {
            var dto      = _fileService.LoadFromAttachmentFolder(fileGuid);
            var filePath = Path.Combine(dto.FolderPath, dto.ActualFileName);
            var fileInfo = new FileInfo(filePath);

            return File(fileInfo.OpenRead(), dto.ContentType, dto.FileName);
        }
    }
}
