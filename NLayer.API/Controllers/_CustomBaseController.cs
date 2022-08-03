using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class _CustomBaseController : ControllerBase
    {
        //API Controllerda rreturn OK veya BadRequest Demek yerine generic bir method yazıldı
        [NonAction]
        public IActionResult CreateActionResult<T>(CustomResponseDTO<T> response)
        {
            if (response.StatusCode == 204)
            {
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };
            }
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
