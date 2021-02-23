using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProdsAndCats.Models;
using Microsoft.EntityFrameworkCore;

namespace ProdsAndCats.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("products")]
        public IActionResult Products()
        {
            ViewBag.AllProducts = _context.Products.ToList();
            return View();
        }
        [HttpPost("products/new")]
        public IActionResult NewProduct(Product newProduct)
        {
            if(ModelState.IsValid)
            {
                _context.Add(newProduct);
                _context.SaveChanges();               
                return RedirectToAction("Products");
            }
            else
            {
                ViewBag.AllProducts = _context.Products.ToList();
                return View("Products");
            }
        }
        [HttpPost("products/addcategory")]
        public IActionResult AddCategory(Association newAssociation)
        {
            _context.Add(newAssociation);
            _context.SaveChanges();
            return RedirectToAction("ShowProduct", new {num = newAssociation.ProductId});
        }
        [HttpGet("categories")]
        public IActionResult Categories()
        {
            ViewBag.AllCategories = _context.Category.ToList();
            return View();
        }
        [HttpPost("categories/new")]
        public IActionResult NewCategory(Category newCategory)
        {
            //create a new category
            if(ModelState.IsValid)
            {
                _context.Add(newCategory);
                _context.SaveChanges();
                return RedirectToAction("Categories");//for now
            }
            else 
            {
                ViewBag.AllCategories = _context.Category.ToList();
                return View("Categories");
            }
        }
        [HttpGet("categories/{num}")]
        public IActionResult ShowCategory(int num)
        {
            @ViewBag.ThisCategory = _context.Category
            .FirstOrDefault(c => c.CategoryId == num);

            List<Association> associations = _context.Associations
                .Include(a => a.Product)
                .Where(a => a.CategoryId == num)
                .ToList();
            ViewBag.Associations = associations;
            
            List<Product> RemainingProducts = _context.Products.ToList();
            foreach (Association item in associations)
            {
                if(RemainingProducts.Contains(item.Product))
                {
                    RemainingProducts.Remove(item.Product);
                }
            }
            @ViewBag.ProductsNotIncluded = RemainingProducts;
            return View();
        }
        [HttpGet("products/{num}")]
        public IActionResult ShowProduct(int num)
        {
            @ViewBag.ThisProduct = _context.Products
            .Include(p => p.Associations)
            .ThenInclude(p => p.Category)
            .FirstOrDefault(p => p.ProductID == num);

            List<Association> associations = _context.Associations
            .Include(a => a.Category)
            .Where(a => a.ProductId == num)
            .ToList();
            List<Category> CategoriesNotIncluded = _context.Category.ToList();

            foreach (Association item in associations)
            {
                if(CategoriesNotIncluded.Contains(item.Category))
                {
                    CategoriesNotIncluded.Remove(item.Category);
                }
            }
            ViewBag.CategoriesNotIncluded = CategoriesNotIncluded;
            ViewBag.Associations = associations;
            return View();
        }
        [HttpPost("/categories/addproduct")]
        public IActionResult AddProductToCategory(Association newAssociation)
        {
            //get the product 
            _context.Add(newAssociation);
            _context.SaveChanges();
            //retrieve the category
            // return Redirect($"/categories/{newAssociation.CategoryId}");
            return RedirectToAction("ShowCategory", new {num = newAssociation.CategoryId});
            //         [HttpGet("categories/{num}")]

        }
    }
}
