﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.VM.PureVMs
{
    public class OrderVM
    {
        public string ShippingAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public int? AppUserID { get; set; }
        public string NonMemberEmail { get; set; }
        public string NonMemberName { get; set; }
    }
}
