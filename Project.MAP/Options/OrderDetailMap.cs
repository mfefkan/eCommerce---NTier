using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Options
{
    public class OrderDetailMap:BaseMap<OrderDetail>
    {
        public OrderDetailMap()
        {
            Ignore(x=> x.ID);
            HasKey(x=> new
            {
                x.ProductID,
                x.OrderID
            });

            //Burada da sql tarafında OrderDetails tablosunda ID ignore edilerek yeni bir tip belirlenmiştir bu tip aynı anda hem ProductID hem OrderID alması gerekmektedir. Bu mantık da SQL'deki PrimaryKey mantığından yola çıkılarak oluşturulmuştur. OrderDetail tablosu Order ve Product tablolarının birleşmesinden oluşan sql de Junction Table dediğimiz bir tablodur....
        }
    }
}
