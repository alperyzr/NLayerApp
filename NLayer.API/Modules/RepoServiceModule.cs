using Autofac;
using NLayer.Caching;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWork;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;
using Module = Autofac.Module;

namespace NLayer.API.Modules
{
    //Autofac kütüphanesinden instance alır
    //startup.cs te ve program.cs te eklenen interface ve classları tek bir yerde toplamak için kullanılır. Böylece program.cs dosyası temiz kalmış olur.
    public class RepoServiceModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Generic olan kısımlar için ekleme yöntemi bu şekilde kullanılır
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkService>();

            var apiAssembly = Assembly.GetExecutingAssembly();
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));


            //assembly lerde içinde "Repository" ve "Service" geçen kısımları ayıklar
            //InstancePerLifetimeScope => Scope methodu karşılığına gelir => request başladı, bitene kadar aynı instance ı kullansın
            //InstancePerDependecy => Transiet methoduna karşılık gelir => Herhangi bir classın consturacture ın da geçildiyse yeni instance açar
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<ProductServiceWithCaching>().As<IProductService>();
        }
    }
}
