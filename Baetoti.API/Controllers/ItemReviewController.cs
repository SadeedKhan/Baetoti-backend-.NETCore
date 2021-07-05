using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Interface.Repositories;

namespace Baetoti.API.Controllers
{
    public class ItemReviewController : ApiBaseController
    {

        public readonly IItemReviewRepository _itemReviewRepository;
        public readonly IMapper _mapper;

        public ItemReviewController(
            IItemReviewRepository itemReviewRepository,
            IMapper mapper
            )
        {
            _itemReviewRepository = itemReviewRepository;
            _mapper = mapper;
        }

    }
}
