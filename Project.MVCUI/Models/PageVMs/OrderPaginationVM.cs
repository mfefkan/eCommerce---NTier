using Project.MVCUI.Models.OuterRequestModels;
using Project.VM.PureVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Models.PageVMs
{
    public class OrderPaginationVM
    {
        public OrderVM Order{ get; set; }
        public List<OrderVM> Orders { get; set; }
        public PaymentRequestModel  PaymentRM { get; set; }
    }
}