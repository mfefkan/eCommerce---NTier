using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.VM.PureVMs
{
    public class AdminProductVM
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public short UnitInStock { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Status { get; set; }


    }
}
