using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Entities;
using NLayer.Core.Services;
using NLayer.Web;
using NLayer.Web.Services;

namespace NLayer.WEB.Controllers
{
    public class ProductController : Controller
    {

        private readonly ProductAPIService _ProductAPIService;
        private readonly CategoryAPIService _CategoryAPIService;

        public ProductController(CategoryAPIService CategoryAPIService, ProductAPIService ProductAPIService)
        {
            _CategoryAPIService = CategoryAPIService;
            _ProductAPIService = ProductAPIService;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _ProductAPIService.GetProductWithCategoryAsync());
        }

        public async Task<IActionResult> Create()
        {
            var categoriesDto = await _CategoryAPIService.GetAllAsync();
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _ProductAPIService.SaveAsync(productDto);
                return RedirectToAction(nameof(Index));
            }
            var categoriesDto = await _CategoryAPIService.GetAllAsync();

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }


        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _ProductAPIService.GetByIdAsync(id);
            var categoriesDto = await _CategoryAPIService.GetAllAsync();
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", product.CategoryId);
            return View(product);

        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _ProductAPIService.UpdateAsync(productDto);
                return RedirectToAction(nameof(Index));

            }
            var categoriesDto = await _CategoryAPIService.GetAllAsync();
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", productDto.CategoryId);
            return View(productDto);

        }


        public async Task<IActionResult> Remove(int id)
        {
            await _ProductAPIService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
