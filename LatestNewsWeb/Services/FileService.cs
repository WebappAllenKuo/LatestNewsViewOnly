using System;
using System.IO;
using System.Threading.Tasks;
using Infra.Models;
using Infra.Models.File;
using Repositories;
using Services.Infra;

namespace LatestNewsWeb.Services
{
    public class FileService
    {
        public FileService(AttachmentRepository attachmentRepository,
                           PathService          pathService)
        {
            _attachmentRepository = attachmentRepository;
            _pathService          = pathService;
        }

        private readonly AttachmentRepository _attachmentRepository;
        private readonly PathService          _pathService;

        public AttachmentDto LoadFromImageFolder(Guid fileGuid)
        {
            var result = _attachmentRepository.Get(fileGuid);
            result.FolderPath = _pathService.GetImageFolderPath();
            return result;
        }

        public AttachmentDto LoadFromAttachmentFolder(Guid fileGuid)
        {
            var result = _attachmentRepository.Get(fileGuid);
            result.FolderPath = _pathService.GetAttachmentFolderPath();
            return result;
        }
    }
}
