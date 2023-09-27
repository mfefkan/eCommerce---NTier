using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Options
{
    public class AppUserMap:BaseMap<AppUser>
    {
        public AppUserMap()
        {
            HasOptional(x=> x.AppUserProfile).WithRequired(x => x.AppUser);

			//Burada HasOptional metodu EntityTypeConfiguration sınıfından alınmış bir miras olarak kullanılabilmektedir. Böylelikle her bir AppUser'ın bir AppUserProfile'i olması opsiyonel bırakılmışken. AppUserProfile'ın AppUser'ı zorunlu kılınmıştır. Bu mantık SQL'tarafından gelmektedir. Burada bu mantığı uygulamamızı sağlayan BaseMap'imizden aldığımız onunda EntityTypeConfiguration'dan aldığı mirasdır.
		}
	}
}
