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
    public class DriverController : ApiBaseController
    {

        public readonly IDriverRepository _driverRepository;
        public readonly IMapper _mapper;

        public DriverController(
            IDriverRepository driverRepository,
            IMapper mapper
            )
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
        }

    }
}
