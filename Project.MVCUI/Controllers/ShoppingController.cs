using PagedList;
using Project.BLL.RepositoryPattern.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using Project.MVCUI.Models.PageVMs;
using Project.MVCUI.Models.ShoppingTools;
using Project.VM.PureVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
                    CategoryID = x.CategoryID,
                    ImagePath = x.ImagePath,
                    UnitInStock = x.UnitInStock

                }).ToPagedList(page ?? 1, 9) : _pRep.Where(x => x.CategoryID == categoryID).Select(x => new ProductVM
                {
                    ID = x.ID,
                    ProductName = x.ProductName,
                    UnitPrice = x.UnitPrice,
                    CategoryID = x.CategoryID,
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

            Product eklenecekUrun = _pRep.Find(id);

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

        public ActionResult ConfirmOrder()
        {
            AppUserVM currentUser;

            if (Session["member"] != null)
            {
                currentUser = Session["member"] as AppUserVM;
            }
            
            return View();

        }
        // http://localhost:52640/api/Payment/RecievePayment

        // PaymentRequestModel ile bu adrese gönderilecek.


        [HttpPost]
        public ActionResult ConfirmOrder(OrderPaginationVM ovm)
        {
            bool sonuc;
            Cart sepet = Session["member"] as Cart;
            ovm.Order.TotalPrice = ovm.PaymentRM.ShoppingPrice = sepet.TotalPrice;

            #region APISection
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52640/api/");
                Task<HttpResponseMessage> postTask = client.PostAsJsonAsync("Payment/RecievePayment", ovm.PaymentRM);

                HttpResponseMessage result;

                try
                {
                    result = postTask.Result;
                }
                catch (Exception)
                {
                    TempData["baglantiRed"] = "Banka bağlantıyı reddetti";
                    return RedirectToAction("ShoppingList");

                }

                if (result.IsSuccessStatusCode)
                {
                    sonuc = true;
                }
                else
                {
                    sonuc = false;
                }

                if (sonuc)
                {
                    if (Session["member"] != null)
                    {
                        AppUserVM kullanici = Session["member"] as AppUserVM;
                        ovm.Order.AppUserID = kullanici.ID;
                    }


                    Order newOrder = new Order
                    {
                        ShippingAddress = ovm.Order.ShippingAddress,
                        TotalPrice = ovm.PaymentRM.ShoppingPrice,
                        AppUserID = ovm.Order.AppUserID,
                        NonMemberEmail = ovm.Order.NonMemberEmail,
                        NonMemberName = ovm.Order.NonMemberName
                    };

                    _oRep.Add(newOrder);

                    foreach (CartItem item in sepet.Sepetim)
                    {
                        OrderDetail od = new OrderDetail();
                        od.OrderID = newOrder.ID;
                        od.ProductID = item.ID;
                        od.TotalPrice = item.SubTotal;
                        od.Quantity = item.Amount;
                        _oDRep.Add(od);

                        //Stoktan düşürme

                        Product stoktanDusurulecek = _pRep.Find(item.ID);
                        stoktanDusurulecek.UnitInStock -= item.Amount;
                        _pRep.Update(stoktanDusurulecek);

                        //Eğer stoktan düşürüldüğünde stokta kalmayacak bir şekilde item varsa onun Amount'ı sepette aşılamayacak bir hale gelsin...
                    }

                    TempData["odeme"] = "Siparişiniz bize ulaşmıştır...Teşekkür ederiz.";
                    //if (Session["member"] != null)          ----------Refactor ETTTTTTTTTTTTTTTTT---------
                    //    MailService.Send(ovm.AppUserVM.Email, body: $"Siparişiniz başarıyla alındı {ovm.Order.TotalPrice}"); //Kullanıcıya Mail'de aldığı ürünleri de gönderin...
                    //else
                    MailService.Send(ovm.Order.NonMemberName, body: $"Siparişiniz başarıyla alındı {ovm.Order.TotalPrice}"); //Kullanıcıya Mail'de aldığı ürünleri de gönderin...
                 
                    Session.Remove("scart");
                    return RedirectToAction("ShoppingList");


                }
                else
                {
                    Task<string> s = result.Content.ReadAsStringAsync();
                    TempData["sorun"] = s;
                    return RedirectToAction("ShoppingList");
                }

            }

            #endregion
        }


    }

  
}