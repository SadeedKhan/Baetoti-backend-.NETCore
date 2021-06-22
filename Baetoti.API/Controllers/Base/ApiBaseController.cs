using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Baetoti.API.Controllers.Base
{
    [EnableCors("Trusted")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        protected string Identity
        {
            get
            {
                return User.Claims.Where(c => c.Type == ClaimsIdentity.DefaultNameClaimType)
                          .Select(c => c.Value).SingleOrDefault();
            }
        }

        protected string UserId
        {
            get
            {
                return User.Claims.Where(c => c.Type == ClaimsIdentity.DefaultNameClaimType)
                    .FirstOrDefault().Value;
            }
        }

    }
}
