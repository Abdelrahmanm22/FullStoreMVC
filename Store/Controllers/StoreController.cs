using Microsoft.AspNetCore.Mvc;
using Store.Contexts;
using Store.Models;

namespace Store.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppDbContext dbContext;
        private readonly int pageSize = 8;

        public StoreController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index(int pageIndex)
        {
            IQueryable<Product> query = dbContext.Products;
            query = query.OrderByDescending(p => p.Id);

            ///pagination functionality
            if (pageIndex < 1) pageIndex = 1;

            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex-1)*pageSize).Take(pageSize);


            var Products = query.ToList();
            
            ViewBag.Products = Products;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageIndex = pageIndex;
            return View();
        }
    }
}
