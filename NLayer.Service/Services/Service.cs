using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IGenericRepository<T> _genericRepository;
        private readonly IUnitOfWorkService _unitOfWorkService;

        public Service(
            IGenericRepository<T> genericRepository,
            IUnitOfWorkService unitOfWorkService)
        {
            _genericRepository = genericRepository;
            _unitOfWorkService = unitOfWorkService;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _genericRepository.AddAsync(entity);
            await _unitOfWorkService.CommitAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _genericRepository.AddRangeAsync(entities);
            await _unitOfWorkService.CommitAsync();
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _genericRepository.AnyAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _genericRepository.GetAll().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            var hasProduct =  await _genericRepository.GetByIdAsync(Id);
            if (hasProduct == null)
            {
                //Generic gelen classa göre Classın Name ini alıp not found hatası dönüyoruz
                throw new NotFoundException($"{typeof(T).Name}({Id}) not found");
            }
            return hasProduct;
        }

        public async Task RemoveAync(T entity)
        {
            _genericRepository.Remove(entity);
            await _unitOfWorkService.CommitAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _genericRepository.RemoveRange(entities);
            await _unitOfWorkService.CommitAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _genericRepository.Update(entity);
            await _unitOfWorkService.CommitAsync();
            return entity;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _genericRepository.Where(expression);
        }
    }
}
