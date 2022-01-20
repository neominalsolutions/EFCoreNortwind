using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreNortwind.Data.Dtos
{
    /// <summary>
    /// En çok sipariş edilen 5 adet Ürünü raporlarız
    /// Views olarak kullanılan class'lar herhangi bir method barındırmaz key alanları Id alanları olmaz. sadece içerisinde property barındırır.
    /// Bu bir entity değil yani domain object değil dto yani data transfer objecttir.
    /// </summary>
    public class MostOrderedFiveProducts
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }

    }
}
