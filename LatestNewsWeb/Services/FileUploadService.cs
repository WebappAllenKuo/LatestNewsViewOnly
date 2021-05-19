using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Infra.Models;
using KueiExtensions.Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Repositories;

namespace LatestNewsWeb.Services
{
    public class FileUploadService
    {
        private readonly AttachmentRepository _attachmentRepository;

        public FileUploadService(AttachmentRepository attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        /// <summary>
        /// 儲存檔案
        /// </summary>
        public async Task<AttachmentDto> SaveAsync(IFormFile fileDto, Guid? updatorGuid, string toFolder)
        {
            var attachmentDto = GenerateAttachmentDto(fileDto, updatorGuid, out var newFileName);

            await SaveFile(fileDto, newFileName, toFolder);

            _attachmentRepository.Add(attachmentDto);

            return attachmentDto;
        }

        /// <summary>
        /// 儲存檔案
        /// </summary>
        public async Task<AttachmentDto[]> SaveAsync(IFormFile[] fileDtos, Guid? updatorGuid, string toFolder)
        {
            var attachmentDtos = await Task.WhenAll(fileDtos.Select(async fileDto =>
                                                                    {
                                                                        var attachmentDto = GenerateAttachmentDto(fileDto, updatorGuid, out var newFileName);

                                                                        await SaveFile(fileDto, newFileName, toFolder);

                                                                        return attachmentDto;
                                                                    }));

            _attachmentRepository.Add(attachmentDtos);

            return attachmentDtos;
        }

        private static AttachmentDto GenerateAttachmentDto(IFormFile fileDto, Guid? updatorGuid, out string newFileName)
        {
            var attachmentDto = new AttachmentDto();
            attachmentDto.Guid     = Guid.NewGuid();
            attachmentDto.FileName = fileDto.FileName;

            newFileName                  = Guid.NewGuid() + new FileInfo(fileDto.FileName).Extension;
            attachmentDto.ContentType    = newFileName.GetContentType();
            attachmentDto.ActualFileName = newFileName;

            attachmentDto.UpdatorGuid = updatorGuid.GetValueOrDefault();
            return attachmentDto;
        }

        private async Task SaveFile(IFormFile fileDto, string newFileName, string imageFolder)
        {
            TryCreateFolder(imageFolder);

            var filePath = Path.Combine(imageFolder, newFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await fileDto.CopyToAsync(fileStream);
            }
        }

        /// <summary>
        /// 確認資料夾已建立，如未建立則會建立
        /// </summary>
        private static void TryCreateFolder(string newFileFolder)
        {
            if (Directory.Exists(newFileFolder))
            {
                return;
            }

            Directory.CreateDirectory(newFileFolder);
        }
    }
}
