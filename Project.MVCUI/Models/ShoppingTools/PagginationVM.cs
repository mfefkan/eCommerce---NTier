using PagedList;
using Project.ENTITIES.Models;
using Project.VM.PureVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Models.ShoppingTools
{
    public class PagginationVM
    {

        //Refactor yaparken ProductVM olarak değiştirmeyi sakın unutmayın.
        public IPagedList<ProductVM> PagedProducts { get; set; }
        public List<CategoryVM> Categories { get; set; }
        public ProductVM Product { get; set; }

    }
}