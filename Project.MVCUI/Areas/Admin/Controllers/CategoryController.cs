﻿using Project.BLL.RepositoryPattern.ConcRep;
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
    public class CategoryController : Controller
    {
        CategoryRepository _cRep;

        public CategoryController()
        {
            _cRep = new CategoryRepository();
        }
        public ActionResult ListCategories(int? id)
        {
            List<AdminCategoryVM> categories;
            if(id == null)
            {
                categories = _cRep.Select(x => new AdminCategoryVM
                {
                    ID = x.ID,
                    CategoryName = x.CategoryName,
                    DeletedDate = x.DeletedDate,
                    CreatedDate = x.CreatedDate,
                    Description = x.Description,
                    ModifiedDate = x.UpdatedDate,
                    Status = x.Status.ToString()

                }).ToList();
            }
            else
            {
                categories = _cRep.Where(x => x.ID == id).Select(x => new AdminCategoryVM
                {
                    ID = x.ID,
                    CategoryName = x.CategoryName,
                    DeletedDate = x.DeletedDate,
                    CreatedDate = x.CreatedDate,
                    Description = x.Description,
                    ModifiedDate = x.UpdatedDate,
                    Status = x.Status.ToString()
                }).ToList() ;
            }

            AdminCategoryListPageVM alistVm = new AdminCategoryListPageVM
            {
                Categories = categories,
            };


            return View(alistVm);
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(CategoryVM category)
        {
            Category c = new Category
            {
                CategoryName = category.CategoryName,
                Description = category.Description

            };

            _cRep.Add(c);
            return RedirectToAction("ListCategories");
        }

        public ActionResult UpdateCategory(int id)
        {
            return View(_cRep.Find(id));
        }

        [HttpPost]
        public ActionResult UpdateCategory(CategoryVM category)
        {
            Category c = new Category
            {
                CategoryName = category.CategoryName,
                Description = category.Description
            };
            _cRep.Update(c);

            return RedirectToAction("ListCategories");
        }

        public ActionResult DeleteCategory(int id)
        {
            _cRep.Delete(_cRep.Find(id));
            return RedirectToAction("ListCategories");
        }

    }
}