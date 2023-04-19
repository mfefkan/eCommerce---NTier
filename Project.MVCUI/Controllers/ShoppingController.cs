using PagedList;
using Project.BLL.RepositoryPattern.ConcRep;
using Project.ENTITIES.Models;
using Project.MVCUI.Models.ShoppingTools;
using Project.VM.PureVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class ShoppingController : Controller
    {
        OrderRepository _oRep;
        ProductRepository _pRep;
        OrderDetailRepository _oDRep;
        CategoryRepository _cRep;

        public ShoppingController()
        {
            _oRep = new OrderRepository();
            _pRep = new ProductRepository();
            _oDRep = new OrderDetailRepository();
            _cRep = new CategoryRepository();
        }
        public ActionResult ShoppingList(int? page,int? categoryID) //Nullable int olmasının sebebi kaçıncı sayfada olduğunu belirtmek için. İlk açılışta bu null olur daha sonrasında client isterse istenen sayfayı getirir...Aynı zamanda bu sayfa ürünleri kategorilerine göre de listeleyebileceği için bir diğer parametremiz de nullable CategoryID.
        {
            PagginationVM pavm = new PagginationVM
            {
                PagedProducts = categoryID == null ? _pRep.GetActives().Select(x => new ProductVM
                {
                    ID = x.ID,
                    ProductName = x.ProductName,
                    UnitPrice = x.UnitPrice,
                    CategoryID = categoryID,
                    ImagePath = x.ImagePath,
                    UnitInStock = x.UnitInStock

                }).ToPagedList(page ?? 1, 9) : _pRep.Where(x => x.CategoryID == categoryID).Select(x => new ProductVM
                {
                    ID = x.ID,
                    ProductName = x.ProductName,
                    UnitPrice = x.UnitPrice,
                    CategoryID = categoryID,
                    ImagePath = x.ImagePath,
                    UnitInStock = x.UnitInStock

                }).ToPagedList(page ?? 1, 9),
                Categories = _cRep.GetActives().Select(x => new CategoryVM
                {
                    ID = x.ID,
                    CategoryName = x.CategoryName,
                    Description = x.Description

                }).ToList()
            };
            if (categoryID != null) TempData["catID"] = categoryID;
            return View(pavm);
        }

        public ActionResult AddToCart(int id)
        {
            Cart c = Session["scart"] == null ? new Cart() : Session["scart"] as Cart;

            ProductVM eklenecekUrun = _pRep.Select(x => new ProductVM
            {
                ProductName = x.ProductName,
                UnitInStock = x.UnitInStock,
                UnitPrice = x.UnitPrice,
                CategoryID = x.CategoryID,
                ImagePath = x.ImagePath,

            }).FirstOrDefault(x=> x.ID == id);

            CartItem ci = new CartItem
            {
                ID = eklenecekUrun.ID,
                Name = eklenecekUrun.ProductName,
                Price = eklenecekUrun.UnitPrice,
                ImagePath = eklenecekUrun.ImagePath
            };

            c.SepeteEkle(ci);
            Session["scart"] = c;
            return RedirectToAction("ShoppingList");
        }

        public ActionResult CartPage()
        {
            if (Session["scart"] !=  null)
            {
                //Refactor edilecek.
                Cart c = Session["scart"] as Cart;
                return View(c);
            }
            TempData["bos"] = "Sepetinizde ürün bulunmamaktadır.";
            return RedirectToAction("ShoppingList");
        }


        public ActionResult DeleteFromCart(int id)
        {
            if (Session["scart"] != null)
            {
                Cart c = Session["scart"] as Cart;
                c.SepettenCikar(id);
                if(c.Sepetim.Count == 0)
                {
                    Session.Remove("scart");
                    TempData["sepetBos"] = "Sepetinizdeki tüm ürünler çıkarılmıştır";
                    return RedirectToAction("ShoppingList");
                }
            }
            return RedirectToAction("CartPage");
        }
    }

  
}