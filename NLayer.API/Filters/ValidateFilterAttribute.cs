using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;

namespace NLayer.API.Filters
{
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        //Fluent Validation tarafında hata varsa, methoda girmeden o hataları customResponse claasından dönmemiz için kullanılan override method
        //Her API controllerın üstünde belirtmemiz gerekiyor
        // ve ya Global birşey olduğu için API projesinin program.cs kısmında hepsini etkileyecek şekilde belirtmek gerekmektedir.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                context.Result = new BadRequestObjectResult(CustomResponseDTO<NoContentDto>.Fail(400, errors));

            }
        }
    }
}
