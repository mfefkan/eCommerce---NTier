using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Options
{
    public abstract class BaseMap<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        // Burada BaseMap'imiz EntityTypeConfiguration generic sınıfından miras almış. Bu sınıf database tablolarının içeriği ile değişikler yapabilmek adına özel metodlar içeren bir sınıftır.


        public BaseMap()
        {

        }
    }
}
