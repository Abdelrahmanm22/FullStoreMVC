using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.Contexts;
using Store.Helpers;
using Store.Models;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper _mapper;

        public ProductController(AppDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var Products = dbContext.Products.OrderByDescending(p=>p.Id).ToList();
            return View(Products);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductViewModel productVM)
        {
            
            if (ModelState.IsValid) {
                if (productVM.Image == null)
                    productVM.ImageFileName = null;
                else
                    productVM.ImageFileName = DocumentSettings.UplaodFile(productVM.Image, "Products");

                var MappedProduct = _mapper.Map<ProductViewModel, Product>(productVM);
                dbContext.Products.Add(MappedProduct);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(productVM);
        }
    }
}
