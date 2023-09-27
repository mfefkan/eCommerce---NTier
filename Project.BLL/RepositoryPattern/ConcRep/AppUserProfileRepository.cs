using Project.BLL.RepositoryPattern.BaseRep;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.RepositoryPattern.ConcRep
{
    public class AppUserProfileRepository:BaseRepository<AppUserProfile>
    {
        //Burada BaseRepository den alınan miras da generic olarak AppUserProfile olarak verilmiştir. Bu da demek oluyor ki bu sınıf AppUserProfile ile çalışır harici bir model buraya erişemez.
        //Ayrıca burada BaseRepository'de ki IRepository Interface'i tarafından implement edilen metodlar dışında AppUserProfileRepository'de özel olarak kullanılabilecek metodlar da yazılabilir. 

        //Böylelikle Crud işlemleri, listeleme işlemleri vs gibi önemli olan sorgulama metodları miras yolu ile bütün alt repositorylerde ulaşılabilir bir şekilde bırakılmış olur. Bunun dışında da her bir alt repository kendi içinde kullanabileceği özel metodlar yazmaya müsait bırakılmıştır. 
    }
}
