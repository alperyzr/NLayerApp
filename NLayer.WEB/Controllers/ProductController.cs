using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.Services;

namespace NLayer.WEB.Controllers
{
    public class ProductController : Controller
    {

        public async Task<IActionResult> Index()
        {
            return View();

        }
    }
}
