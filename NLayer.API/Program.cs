using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using FluentValidation.AspNetCore;
using System.Reflection;
using NLayer.Service.Validations;
using NLayer.API.Filters;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Middlewares;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using NLayer.API.Modules;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//FluentValidation K�t�phanesinin implementesi i�in bu tarzda kullan�l�r
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ValidateFilterAttribute());
}).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());

//API nin otomatik d�nd��� response u kapat�yoruz ki �stteki method �al���p VAlidateFilterAttribute aktif hale gelsin
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Cache lemek i�in kullan�l�r
builder.Services.AddMemoryCache();

//NotFoundFilter class�m�z�n aktif hale gelmesi i�in kullan�l�r. Generic oldu�u i�in <> a��l�p b�rak�ld�
builder.Services.AddScoped(typeof(NotFoundFilter<>));

//Service projesinde AutoMapper k�t�phanesini kullan�lan MapProfile class tan�mlamas�
builder.Services.AddAutoMapper(typeof(MapProfile));

//Db ba�lant�s� i�in appsettingste ki ConnectionString verilir
builder.Services.AddDbContext<AppDbContext>(x =>
{
    //appsettingsten okuyaca�� k�s�m
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
     {
         //Dinamic olarak AppDbContext class�n� i�inde bar�nd�ran Repository projesinin ismini belirtmi� oluyoruz
         option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
     });
});

//Autofac k�t�phanesi ile Module klas�r� i�erisinde b�t�n Repository ve Service classlar� implement edilir
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(conteinerBuilder => conteinerBuilder.RegisterModule(new RepoServiceModule()));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Exception Middleware klas�r�nde bulunan UseCustomException extentionunu implement ediyoruz.
app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();
