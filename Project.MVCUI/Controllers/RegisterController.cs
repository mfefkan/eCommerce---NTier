using Project.BLL.RepositoryPattern.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using Project.MVCUI.Models.PageVMs;
using Project.VM.PureVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class RegisterController : Controller
    {
        AppUserRepository _apRep;
        AppUserProfileRepository _apUserProfile;

        public RegisterController()
        {
            _apRep = new AppUserRepository();
            _apUserProfile = new AppUserProfileRepository();
        }
        public ActionResult RegisterNow()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterNow(AppUserVM appUser, AppUserProfileVM profile)
        {
            if (_apRep.Any(x=> x.UserName ==  appUser.UserName))
            {
                ViewBag.ZatenVar = "Kullanıcı ismi daha önce alınmış.";
                return View();
            }
            else if (_apRep.Any(x=> x.Email == appUser.Email))
            {
                ViewBag.ZatenVar = "Email daha önce alınmış.";
                return View();
            }

            appUser.Password = DantexCrypt.Crypt(appUser.Password); //Şifreyi kriptoladık.

            AppUser domainUser = new AppUser
            {
                UserName = appUser.UserName,
                Password = appUser.Password,
                Email = appUser.Email,
            };

            _apRep.Add(domainUser);

            string gonderilecekMail = "Tebrikler Hesabınız oluşturulmuştur...Hesabınızı aktif etmek için http://localhost:55588/Register/Activation/"+domainUser.ActivationCode+"Linke tıklayabilirsiniz";

            MailService.Send(appUser.Email, gonderilecekMail,subject:"Hesap Aktivasyon!!");


            if(!string.IsNullOrEmpty(profile.FirstName.Trim()) || !string.IsNullOrEmpty(profile.LastName.Trim()))
            {
                AppUserProfile domainProfile = new AppUserProfile
                {
                    ID = domainUser.ID,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                };
            }
            return View("RegisterOK");
        }

        public ActionResult RegisterOK()
        {
            return View();
        }

        public ActionResult Activation(Guid id)
        {
            AppUser aktifEdilecek = _apRep.FirstOrDefault(x => x.ActivationCode == id);
            if(aktifEdilecek != null)
            {
                aktifEdilecek.Active = true;
                _apRep.Update(aktifEdilecek);
                TempData["HesapAktifMi"] = "Hesabınız aktif hale getirildi.";
                return RedirectToAction("Login", "Home");
            }
            TempData["HesapAktifMi"] = "Hesabınız bulunamadı";
            return RedirectToAction("Login", "Home");
        }

    }
}