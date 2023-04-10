using Bogus.DataSets;
using Project.COMMON.Tools;
using Project.DAL.Context;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Init
{
    //Bogus kütüphanesini kurduk.
    
    public class MyInit:CreateDatabaseIfNotExists<MyContext>
    {
        protected override void Seed(MyContext context)
        {
            #region Admin

            AppUser au = new AppUser();
            au.UserName = "mfe";
            au.Password = DantexCrypt.Crypt("123");
            au.Email = "mfefkan@hotmail.com";
            au.Role = ENTITIES.Enums.UserRole.Admin;
            au.Active = true;
            context.AppUsers.Add(au);
            context.SaveChanges();

            AppUserProfile apAdminProfile = new AppUserProfile();
            apAdminProfile.FirstName = "Muhammed";
            apAdminProfile.LastName = "Efkan";
            apAdminProfile.ID = 1;

            #endregion


            #region NormalUser

            for (int i = 0; i < 10; i++)
            {
                AppUser nuser = new AppUser();
                nuser.UserName = new Internet("tr").UserName();
                nuser.Password = new Internet("tr").Password();
                nuser.Email = new Internet("tr").Email();
                context.AppUsers.Add(nuser);
                
            }
            context.SaveChanges();

            for (int i = 2; i < 12; i++)
            {
                AppUserProfile apu = new AppUserProfile();  
                apu.ID = i;
                apu.FirstName = new Name("tr").FirstName();
                apu.LastName = new Name("tr").LastName();

                context.AppUserProfiles.Add(apu);

            }
            context.SaveChanges();

            #endregion


            #region KategoriVeUrunBilgileri

            for (int i = 0; i < 10; i++)
            {
                Category c = new Category();
                c.CategoryName = new Commerce("tr").Categories(1)[0];
                c.Description = new Lorem("tr").Sentence(10);

                for (int j = 0; j < 30; j++)
                {
                    Product p = new Product();
                    p.ProductName = new Commerce("tr").ProductName();
                    p.UnitPrice = Convert.ToDecimal(new Commerce("tr").Price());
                    p.UnitInStock = 100;
                    p.ImagePath = new Images().Nightlife();
                    c.Products.Add(p);
                }
                context.Cateogries.Add(c);
                context.SaveChanges();
            }

            #endregion
        }
    }
}
