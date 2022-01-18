using EFCoreNortwind.Data;
using EFCoreNortwind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreNortwind.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NORTHWNDContext _db;

        public HomeController(ILogger<HomeController> logger, NORTHWNDContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            // sorgu1
            // en çok sipariş verilen 5 adet ürünü çekelim

            //sorgu1
           // iki tablo birbiri ile çoka çok ilişkili ise ara tablo üzerinden diğer tabloya ulşamak için thenInclude yaparız.
           // 1'e  çok ilişki varsa yada 1'1 ilişki varsa Include yeterlidir.
            var pro1 = _db.Orders.Include(x => x.OrderDetails).ThenInclude(p => p.Product).SelectMany(u => u.OrderDetails).GroupBy(c => c.Product.ProductName).Select(y => new
            {
                ProductName = y.Key,
                Quantity = y.Sum(z => z.Quantity)
            }).OrderByDescending(c => c.Quantity).Take(5).ToList();

            /*
             * SQL QUERY
             * select top 5 SUM(od.Quantity) as 'adet', p.ProductName from [Order Details] od inner join Orders o 
on od.OrderID = o.OrderID inner join Products p on
p.ProductID = od.ProductID
group by od.ProductID, p.ProductName 
order by adet desc

             * 
             * 
             */

            // sorgu2
            // hangi kategoride kaç adet ürün var

            // sorgu3
            // hangi ülkede kaç adet çalışanım var

            // sorgu4
            // tüm ürünlerin maliyeti ne kadar

            // sorgu5
            // şimdiye kadar ne kadar ciro yaptık

            // sorgu6
            // hangi müşteri kaç adet ürün sipariş etti

            //var pro6 = _db.Orders.Include(x => x.OrderDetails).Include(x => x.Customer).GroupBy(x => x.CustomerId).Select(a => new
            //{
            //    quantity = a.Sum(y=> y.OrderDetails.Sum(g=> g.Quantity)),  // adet 1 geliyor
            //    Customer = a.Key
            //}).ToList();

            /*
             * 
             * select SUM(od.Quantity) 'adet',od.ProductID , c.CustomerID
from Orders o inner join 
[Order Details] od on o.OrderID = od.OrderID 
inner join Products p on p.ProductID = od.ProductID 
inner  join Customers c on c.CustomerID = o.CustomerID
 group by  od.ProductID, c.CustomerID
             * 
             * 
             */

            // sorgu7
            // tost seven çalışanların sorgusu

            // sorgu 8
            // fiyatı 50 liranın üstünde olan ürünleri fiyata göre artandan azalana sıralayalım

            // sorgu9 
            // rapor veren çalışanların listesi (yani bir müdürü bulunan çalışanlar)

            // sorgu10
            // hangi tedarikçi kaç adet ürün tedarik ediyor ?

            // sorgu 11
            // 50 yaş üzerindeki çalışanların listesi

            // sorgu12
            // hangi çalışan kaç adet sipariş almış

            // sorgu13
            // Hangi ürün hangi kategoride hangi tedarikçi tarafından getirilmiştir. KategoryAdı,UrunAdı,Fiyatı,Stoğu,Tedarikçi bilgileri ekrana getirilecek sorgu

            //sorgu14
            // en çok adet ürün sipariş edilen sipariş'in toplam tutarını bulalım

            //sorgu15
            // en çok sipariş edilen ilk 5 ürünün toplam tutarını hesaplayalım
            // Urun Adı, Toplam Tutar şeklinde ekranda listelenecek.


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
