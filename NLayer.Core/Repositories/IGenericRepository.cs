using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int Id);

        IQueryable<T> GetAll();

        //IQueryable doğrudan db ye gitmez. yapılan işlemden sonra ToList() gibi methodlat çağrılmalıdır.
        //productRepository.Where(x=>x.Id > 5 ).OrderBy.ToListAsync();
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        Task<T> AddAsync(T entity);
        //Toplu Eklemek için
        Task AddRangeAsync(IEnumerable<T> entities);
           
        //EntityFramework tarafından sadece state i değiştiriliğ, uzun süren bir işlem olmadığı için async methodu iie kullanılmıyor
        void Update(T entity);
       
        void Remove(T entity);
        //Toplu Silmek İçin
        void RemoveRange(IEnumerable<T> entities);


    }
}
