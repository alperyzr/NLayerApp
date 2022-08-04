using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Entities;
using NLayer.Core.Services;

namespace NLayer.API.Filters
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : _BaseEntity
    {
        private readonly IService<T> _service;

        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //İlgili Methoda daha girmeden parametrelerinin ilkini genelde Id olacak şekilde alır.
            var idValue = context.ActionArguments.Values.FirstOrDefault();
            if (idValue == null) 
            { 
                await next.Invoke();
                return;
            }
            
            var id = (int)idValue;
            var anyEntity = await _service.AnyAsync(x=>x.Id == id);
            if (anyEntity)
            {
                await next.Invoke();
                return;
            }

            context.Result = new NotFoundObjectResult(CustomResponseDTO<NoContentDto>.Fail(404, $"{typeof(T).Name}({id}) is not found"));
        }
    }
}
