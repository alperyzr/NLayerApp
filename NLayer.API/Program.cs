using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWork;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using FluentValidation.AspNetCore;
using System.Reflection;
using NLayer.Service.Validations;
using NLayer.API.Filters;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//FluentValidation Kütüphanesinin implementesi için bu tarzda kullanýlýr
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ValidateFilterAttribute());
}).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());

//API nin otomatik döndüðü response u kapatýyoruz ki üstteki method çalýþýp VAlidateFilterAttribute aktif hale gelsin
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//NotFoundFilter classýmýzýn aktif hale gelmesi için kullanýlýr. Generic olduðu için <> açýlýp býrakýldý
builder.Services.AddScoped(typeof(NotFoundFilter<>));

//======= INTERFACE IMPLEMANTASYONLARI
builder.Services.AddScoped<IUnitOfWorkService, UnitOfWork>();
//generic olduðu için typeof kullanarak bu þekilde yazdýk.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

//Service projesinde AutoMapper kütüphanesini kullanýlan MapProfile class tanýmlamasý
builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

//Db baðlantýsý için appsettingste ki ConnectionString verilir
builder.Services.AddDbContext<AppDbContext>(x =>
{
    //appsettingsten okuyacaðý kýsým
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
     {
         //Dinamic olarak AppDbContext classýný içinde barýndýran Repository projesinin ismini belirtmiþ oluyoruz
         option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
     });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Exception Middleware klasöründe bulunan UseCustomException extentionunu implement ediyoruz.
app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();
