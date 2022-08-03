using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs;
using NLayer.Service.Exceptions;
using System.Text.Json;

namespace NLayer.API.Middlewares
{
    //Middleware classlarının başında Use kullanılması best practies için önemlidir.
    //Extention method yazmak için mutlaka static method olmalıdır.
    //IApplicationBuilder interfaceini implement etmiş bütün classlar için kullanılabilir
    public static class UseCustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                //Sonlandırıcı bir middlewaredir. Request buraya girdiği andan itibaren daha ileriye gitmez ve geriye döner
                config.Run(async context =>
                {
                    //Middleware tarafından fırlatılacak hatanın tipi belirtilir. JSON tipinde belirttik
                    context.Response.ContentType = "application/json";

                    //Uygulamada fırlatılan hatayı yakalar.
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    //Status Code, Servis katmanındaki ClientSideException dan dönüyorsa 400, Başka bir yerden dönüüyorsa 500 hata kodu dönmesi için kullanılır
                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        NotFoundException => 404,
                        //switch-case yapısının default durumu (- ile lampta kullanılmaldıır)
                        _ => 500
                    };
                    context.Response.StatusCode = statusCode;

                    var response = CustomResponseDTO<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);

                    //FrameWork otomatik olarak JSON a çeviriyor, ancek Custom bir middleware olduğu için, elde edilen response JSON a serilaze edilir.
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));

                });
            });
        }
    }
}
