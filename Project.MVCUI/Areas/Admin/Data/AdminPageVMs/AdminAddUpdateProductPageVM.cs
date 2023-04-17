using Project.VM.PureVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Areas.Admin.Data.AdminPageVMs
{
    public class AdminAddUpdateProductPageVM
    {
        public List<AdminCategoryVM> Categories { get; set; }
        public AdminProductVM Product { get; set; }
    }
}