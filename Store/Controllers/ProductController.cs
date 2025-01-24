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
        private readonly int pageSize = 5;

        public ProductController(AppDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }
        public IActionResult Index(int pageIndex)
        {
            IQueryable<Product> query = dbContext.Products;
            query = query.OrderByDescending(p => p.Id);

            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);


            var Products = query.ToList();

            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = totalPages;
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
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest();

            var Employee = dbContext.Products.Find(id);

            if (Employee is null) return NotFound();

            var MappedProduct = _mapper.Map<Product,ProductViewModel>(Employee);
            return View(MappedProduct);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel productVM)
        {
            if (ModelState.IsValid) {
                try
                {
                    if (productVM.Image is not null)
                        productVM.ImageFileName = DocumentSettings.UplaodFile(productVM.Image,"Products");
                    var MappedProduct = _mapper.Map<ProductViewModel,Product>(productVM);
                    dbContext.Products.Update(MappedProduct);
                    dbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex) { 
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(productVM);
        }

        public IActionResult Delete(int? id) {
            if (id is null) return BadRequest();
            var Employee = dbContext.Products.Find(id);
            if (Employee is null) return NotFound();

            try
            {
                dbContext.Products.Remove(Employee);
                int result = dbContext.SaveChanges();
                if (result > 0 && Employee.ImageFileName is not null)
                {
                    DocumentSettings.DeleteFile(Employee.ImageFileName, "Products");
                }
                
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
