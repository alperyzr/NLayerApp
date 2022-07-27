using Microsoft.EntityFrameworkCore;
using NLayer.Core.Entities;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
       
        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<Category> GetSingleCategoryByIdWithProductsAsync(int categoryId)
        {
            //FirstOrDefault kullanıldığında where şartında kaç tane kayıt varsa ilkini verir
            //SingleOrDefault kullanıldığında İlgili Id li kayıt yoksa hata verir
            return await _context.Categories.Include(x => x.Products).Where(x=>x.Id == categoryId).SingleOrDefaultAsync();
        }
    }
}
