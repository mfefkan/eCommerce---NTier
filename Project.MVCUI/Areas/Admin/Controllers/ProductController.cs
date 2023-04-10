using Project.BLL.RepositoryPattern.ConcRep;
using Project.ENTITIES.Models;
using Project.MVCUI.Areas.Admin.Data.AdminPageVMs;
using Project.VM.PureVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductRepository _pRep;
        CategoryRepository _cRep;

        public ProductController()
        {
            _pRep = new ProductRepository();
            _cRep = new CategoryRepository();
        }

        public ActionResult ListProducts(int? id) //buradaki id category id'sidir....
        {
            if (id == null)
            {
                List<AdminProductVM> products = GetProducts();


                AdminProductListPageVM alvm = new AdminProductListPageVM
                {
                    Products = products,
                };
                return View(alvm);
            }
            else
            {
                List<AdminProductVM> products = new List<AdminProductVM>();

                List<Product> selectedProducts = _pRep.Where(x=> x.CategoryID == id).ToList();

                foreach (Product item in selectedProducts)
                {
                    AdminProductVM alpvm = new AdminProductVM
                    {
                        ID = item.ID,
                        ProductName = item.ProductName,
                        UnitInStock = item.UnitInStock,
                        UnitPrice = item.UnitPrice,
                        Status = item.Status.ToString(),
                        CreatedDate = item.CreatedDate,
                        ModifiedDate = item.UpdatedDate,
                        DeletedDate = item.DeletedDate,

                    };

                    products.Add(alpvm);
                }

                AdminProductListPageVM aplpvm = new AdminProductListPageVM
                {
                    Products = products
                };
                return View(aplpvm);
            }
            


        }

        private List<AdminProductVM> GetProducts()
        {
            return _pRep.Select(x => new AdminProductVM
            {
                ID = x.ID,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                UnitInStock = x.UnitInStock,
                Status = x.Status.ToString(),
                CreatedDate =x.CreatedDate,
                ModifiedDate = x.UpdatedDate,
                DeletedDate = x.DeletedDate
                
            }).ToList();
       
        }

        private List<AdminCategoryVM> GetCategories()
        {
            return _cRep.Select(x => new AdminCategoryVM
            {
                ID= x.ID,
                CategoryName = x.CategoryName,
                Description = x.Description
            }).ToList();
        }


        public ActionResult AddProduct()
        {
            AdminProductListPageVM aplvm = new AdminProductListPageVM
            {
                Categories = GetCategories()
            };
            return View(aplvm);
        }

        [HttpPost]
        public ActionResult AddProduct(AdminProductVM product)
        {
            Product p = new Product
            {
                ProductName = product.ProductName,
                UnitInStock = product.UnitInStock,
                UnitPrice = product.UnitPrice,
                ID = product.ID
            };
            _pRep.Add(p);
            return RedirectToAction("ListProducts");
        }
 
           
        public ActionResult UpdateProduct(int id)
        {
            Product p = _pRep.Find(id);
            AdminProductVM productVM = new AdminProductVM
            {
                ProductName = p.ProductName,
                UnitInStock = p.UnitInStock,
                UnitPrice = p.UnitPrice,
                ID = p.ID,
                DeletedDate = p.DeletedDate,
                ModifiedDate = p.UpdatedDate,
                CreatedDate = p.CreatedDate,
                Status = p.Status.ToString()
            };
            return View(productVM);
        }

        [HttpPost]
        public ActionResult UpdateProduct(AdminProductVM product)
        {
            Product p = new Product
            {
                ProductName = product.ProductName,
                UnitInStock = product.UnitInStock,
                UnitPrice = product.UnitPrice,
            };
            _pRep.Update(p);

            return RedirectToAction("ListProducts");
            
        }
        

        public ActionResult DeleteProduct(int id)
        {
            _pRep.Delete(_pRep.Find(id));
            return RedirectToAction("ListProducts");
        }
    }
}