using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Interface.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class ProviderController : ApiBaseController
    {

        public readonly IProviderRepository _providerRepository;
        public readonly IMapper _mapper;

        public ProviderController(
            IProviderRepository providerRepository,
            IMapper mapper
            )
        {
            _providerRepository = providerRepository;
            _mapper = mapper;
        }

    }
}
