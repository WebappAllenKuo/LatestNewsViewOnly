using System;
using System.Linq;
using Infra.Models;
using Infra.Models.News;
using Infra.Models.Vue;
using Infra.Parameters;
using Repositories.News;
using Services.Options;

namespace Services.News
{
    public class NewsService
    {
        private readonly NewsRepository _newsRepository;
        private readonly OptionService  _optionService;

        public NewsService(NewsRepository newsRepository,
                           OptionService  optionService)
        {
            _newsRepository = newsRepository;
            _optionService  = optionService;
        }

        public void GetList(ListDto listDto)
        {
            _newsRepository.GetList(listDto);

            // 當所在頁碼小於總頁數時，重新取得最後一頁資料
            if (listDto.PageInfo.PageCount > 0
             && listDto.PageInfo.PageNo    > listDto.PageInfo.PageCount)
            {
                listDto.PageInfo.PageNo = listDto.PageInfo.PageCount;
                _newsRepository.GetList(listDto);
            }
        }

        public void GetPublishedSortByIsTop(ListDto listDto)
        {
            _newsRepository.GetPublishedSortByIsTop(listDto);

            // 當所在頁碼小於總頁數時，重新取得最後一頁資料
            if (listDto.PageInfo.PageCount > 0
             && listDto.PageInfo.PageNo    > listDto.PageInfo.PageCount)
            {
                listDto.PageInfo.PageNo = listDto.PageInfo.PageCount;
                _newsRepository.GetPublishedSortByIsTop(listDto);
            }
        }

        private Dto Create(Dto dto)
        {
            dto.Guid       = Guid.NewGuid();
            dto.CreateTime = DateTime.Now;

            _newsRepository.Create(dto);

            return dto;
        }

        private Dto Edit(Dto dto)
        {
            dto.UpdateTime = DateTime.Now;
            _newsRepository.Edit(dto);

            return dto;
        }

        public void CreateOrEdit(Dto dto, Guid userGuid)
        {
            dto.CreatorGuid = userGuid;
            dto.UpdatorGuid = userGuid;

            dto = dto.Guid == null
                      ? Create(dto)
                      : Edit(dto);
        }

        public Dto Get(Guid guid)
        {
            var dto = _newsRepository.Get(guid);

            if (dto == null)
            {
                throw new ErrorCodeException(ErrorCode.E400010);
            }

            return dto;
        }

        public void Delete(Guid newsGuid)
        {
            _newsRepository.Delete(newsGuid);
        }

        public DetailDto Detail(Guid guid)
        {
            var dto = Get(guid);

            var result = new DetailDto
                         {
                             Guid         = dto.Guid,
                             Title        = dto.Title,
                             Content      = dto.Content,
                             IsPublished  = dto.IsPublished,
                             IsTop        = dto.IsTop,
                             Sort         = dto.Sort,
                             PublishDate  = dto.PublishDate,
                             PublishTime  = dto.PublishTime,
                             CreateTime   = dto.CreateTime,
                             CreatorGuid  = dto.CreatorGuid,
                             UpdateTime   = dto.UpdateTime,
                             UpdatorGuid  = dto.UpdatorGuid,
                             DataStatusId = dto.DataStatusId,

                             AttachmentDtos = dto.AttachmentDtos,
                         };

            var booleanOptions = _optionService.GetBoolean();

            result.IsPublishedName = booleanOptions.FirstOrDefault(b => b.Value.ToString() == result.IsPublished.ToString())
                                                  ?.Text;

            result.IsTopName = booleanOptions.FirstOrDefault(b => b.Value.ToString() == result.IsTop.ToString())
                                            ?.Text;

            return result;
        }

        public SortedNews GetSortedNews()
        {
            return _newsRepository.GetSortedNews();
        }

        public void PostSortedNews(SortableDto dto)
        {
            _newsRepository.PostSortedNews(dto);
        }
    }
}
