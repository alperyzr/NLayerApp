using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Entities;
using NLayer.Web;
using NLayer.Web.Services;

namespace NLayer.WEB.Controllers
{
    public class CategoryController:Controller
    {
       
        private readonly CategoryAPIService _CategoryAPIService;

        public CategoryController(CategoryAPIService CategoryAPIService)
        {
            _CategoryAPIService = CategoryAPIService;           
        }

        public async Task<IActionResult> Index()
        {
            return View(await _CategoryAPIService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto request)
        {
            var response = await _CategoryAPIService.Create(request);
            return RedirectToAction("Index");
        }

        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        public async Task<IActionResult> Update(int Id)
        {
            var category = await _CategoryAPIService.GetById(Id);
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryDto request)
        {
            if (ModelState.IsValid)
            {
                await _CategoryAPIService.UpdateAsync(request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        public async Task<IActionResult> Remove(int Id)
        {
            await _CategoryAPIService.Remove(Id);
            return RedirectToAction("Index");
        }
    }
}
