using EFCoreNortwind.Data;
using EFCoreNortwind.Data.Dtos;
using EFCoreNortwind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

        public IActionResult ViewController()
        {
            var model = _db.MostOrderedFiveProducts.ToList();

            return View(model);
        }

        public IActionResult EFTransaction()
        {
            // aşağıdaki örnekteki tüm işlemler birbirleri ile bağlantılı olarak düşünülerek hepsi tek bir transaction bloğunda gönderilmiştir.

            var category1 = new Category
            {
                CategoryName = "Yeni 234"
            };

            category1.Products.Add(new Product
            {
                ProductName = "PYeni1",
                CategoryId = category1.CategoryId,
                Discontinued = false,
                UnitPrice = 10,
                UnitsInStock = 20,
                SupplierId = null
            });

            category1.Products.Add(new Product
            {
                ProductName = "PYeni2",
                CategoryId = category1.CategoryId,
                Discontinued = true,
                UnitPrice = 15,
                UnitsInStock = 26,
                SupplierId = null
            });


            _db.Categories.Add(category1); // Added

            var category2 = new Category
            {
                CategoryName = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like)."
            };

            _db.Categories.Add(category2);
            _db.SaveChanges();

            return View();

        }

        public IActionResult StoreProcedure()
        {

            var sqlParam = new PagedProductParams(
                searchText: "",
                sortColumn: "ProductName",
                sortOrder: SqlSortingTypes.Ascending,
                pageIndex: 8,
                limit: 10);

            var model = _db.PagedProducts.FromSqlRaw("PagedProducts @searchText, @limit, @pageIndex, @sortColumn, @sortOrder",sqlParam.Params).ToList();

            return View(model);
        }

        public IActionResult ChangeTrackerView()
        {

            //var qauery14 = _db.Orders.Include(x => x.Customer).Select(a => a.Customer.ContactName).ToList();
            //var qauery13 = _db.Orders.Select(a=> a.Customer.ContactName).ToList();

            // Include ve Select arasında yukarıdaki sorguda bir fark yoktur

            //var product = _db.Products.AsNoTracking().FirstOrDefault(x => x.ProductId == 1);

            /*_db.ChangeTracker.Entries();*/ // tüm entity ve state değişimlerini dizi olarak verir.

            //foreach (var item in _db.ChangeTracker.Entries())
            //{
            //    if(item.State == EntityState.Added)
            //    {

            //    }
            //}

            //var state1 = _db.Entry(product).State;

            //product.UnitPrice = 32;

            //var state2 = _db.Entry(product).State;

            //_db.Products.Update(product);
            //_db.SaveChanges();

            //var state3 = _db.Entry(product).State;


            // Disconnected Remove Operation

            // 1. yöntem (Attach ile silinecek olan nesneyi db Bağlar state değiştiririz)
            //Category cat3 = new Category();
            //cat3.CategoryId = 1009;

            //_db.Attach(cat3).State = EntityState.Deleted; // EF 6 da bu şekilde Attach etmek zorundaydık.
            //_db.SaveChanges();

            // 2. yöntem remove methodu ile silme
            //Category cat4 = new Category();
            //cat4.CategoryId = 1010;
            //_db.Remove(cat4);

            // ekleme için 1 yöntem
            //var category5 = new Category();
            //category5.CategoryName = "Yeni";

            //_db.Attach(category5).State = EntityState.Added;
            //_db.SaveChanges();

            // ekleme için 2 yöntem olan yöntem ile ekleme Add method connected gibi ekleyebiliriz.
            //var category6 = new Category();
            //category6.CategoryName = "Yeni kategori 6";

            //_db.Add(category6);
            //_db.SaveChanges();

            // Günceleme işleminde ise 1. yöntem

            //var category7 = new Category();
            //category7.CategoryId = 1011;
            //category7.CategoryName = "Yeni Güncel q4";
            //_db.Attach(category7).State = EntityState.Modified;
            //_db.SaveChanges();

            // 2. yöntem ise

            var category8 = new Category();
            category8.CategoryId = 1013;
            category8.CategoryName = "dsadsad34";

            _db.Update<Category>(category8);
            _db.SaveChanges();

            // EF Core da Update methodu disconnected state çalışır bu sebeple AsNoTracking işaretlenen bir nesnenin değerini değiştirebiliriz.

            return View();

        }

        public IActionResult Index()
        {

            // sorgu1
            // en çok sipariş verilen 5 adet ürünü çekelim ve kaç adet şipariş veridiğinide gösterelim

            //sorgu1
            // iki tablo birbiri ile çoka çok ilişkili ise ara tablo üzerinden diğer tabloya ulşamak için thenInclude yaparız.
            // 1'e  çok ilişki varsa yada 1'1 ilişki varsa Include yeterlidir.
            var pro1 = _db.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(p => p.Product)
                .SelectMany(u => u.OrderDetails)
                .GroupBy(c => c.ProductId)
                .Select(y => new
                {
                    ProductName = y.Key, // A
                    Quantity = y.Sum(z => z.Quantity) // 1200
                })
                .OrderByDescending(c => c.Quantity)
                .Take(5)
                .ToList();

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

            var q2 = _db.Categories
                .Include(x => x.Products)
                .Select(a => new
                {

                    CategoryName = a.CategoryName,
                    ProductCount = a.Products.Count()

                })
                .ToList();

            // sorgu3
            // hangi ülkede kaç adet çalışanım var

            var q3 = _db.Employees
                .GroupBy(x => x.Country)
                .Select(a => new
                {
                    Country = a.Key,
                    Count = a.Count()
                })
                .ToList();


            // sorgu4
            // tüm ürünlerin maliyeti ne kadar

            var q4 = _db.Products.Sum(x => x.UnitPrice * x.UnitsInStock);

            // sorgu5
            // şimdiye kadar ne kadar ciro yaptık

            var q5 = _db.Orders
                .Include(x => x.OrderDetails)
                .SelectMany(a => a.OrderDetails)
                .Sum(x => x.Quantity * x.UnitPrice * (decimal)(1 - x.Discount));


            // sorgu6
            // hangi müşteri hangi üründen kaç adet sipariş etti
            // çift alana göre group by işlemi

            var query6 = _db.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.Customer)
                .SelectMany(y => y.OrderDetails)
                .GroupBy(y => new { y.Order.CustomerId, y.Product.ProductName })
                .Select(a => new
            {

                Product = a.Key.ProductName,
                Customer = a.Key.CustomerId,
                TotalProductQuantity = a.Sum(x => x.Quantity)

            }).OrderByDescending(x => x.TotalProductQuantity).ToList();

            // hangi müşteri hangi kaç adet ürün sipariş etti
            var query61 = _db.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.Customer)
                .SelectMany(y => y.OrderDetails)
                .GroupBy(y => y.Order.CustomerId).Select(a => new
            {
                Customer = a.Key,
                TotalProductQuantity = a.Sum(x => x.Quantity)

            })
                .OrderByDescending(x => x.TotalProductQuantity)
                .ToList();

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

            var q7 = _db.Employees
                .Where(x => EF.Functions.Like(x.Notes, "%toast%"))
                .ToList();

            // sorgu 8
            // fiyatı 50 liranın üstünde olan ürünleri fiyata göre artandan azalana sıralayalım

            var q8 = _db.Products
                .OrderByDescending(x => x.UnitPrice >= 50)
                .ToList();

            // sorgu9 
            // rapor veren çalışanların listesi (yani bir müdürü bulunan çalışanlar)

            var q9 = _db.Employees
                .Where(x => x.ReportsTo != null)
                .ToList();

            // bütün müdürleri bul

            // select * from Employees e3 where e3.EmployeeID in (select distinct emp2.ReportsTo from Employees emp2)

            // Any ile çözüm bulamadık. 
            // Contains veya Any ile reporstto emplyeeId eşit olanlar sorgulandı
            var q10 = _db.Employees
            .Where(x => 
            _db.Employees
            .Select(y => y.ReportsTo)
            .Distinct()
            .Any(id => id == x.EmployeeId))
            .ToList();

            var q101 = _db.Employees
                .Where(x => _db.Employees
           .Select(y => y.ReportsTo)
           .Distinct()
           .Contains(x.EmployeeId))
                .ToList();

            // 1 can null 
            // 2 ali 1
            // mehmet 2


            // sorgu10
            // hangi tedarikçi kaç adet ürün tedarik ettik ?

            var q102 = _db.Products
                .Include(x => x.Supplier)
                .GroupBy(x => x.Supplier.ContactName)
                .Select(a => new
            {
                Supplier = a.Key,
                ProductStockCount = a.Sum(x => x.UnitsInStock)
            }).ToList();

            // hangi tedarikçi kaç çeşit ürün tedarik ediyor ?

            var q103 = _db.Products
                .Include(x => x.Supplier)
                .GroupBy(x => x.Supplier.ContactName).Select(a => new
            {
                Supplier = a.Key,
                ProductCount = a.Count()
            }).ToList();

            // sorgu 11
            // 50 yaş üzerindeki çalışanların listesi

            var q104 = _db.Employees
                .Where(x => (DateTime.Now.Year - x.BirthDate.Value.Year) > 66)
                .ToList();

            var q105 = _db.Employees
                .Where(x => EF.Functions.DateDiffYear(x.BirthDate, DateTime.Now) > 66)
                .ToList();

            // sorgu12
            // hangi çalışan kaç adet sipariş almış


            var query12 = _db.Orders
                .Include(x => x.Employee)
                .GroupBy(x => new { x.Employee.FirstName, x.Employee.LastName }).Select(a => new
                {
                    EmployeeName = $"{a.Key.FirstName} {a.Key.LastName}",
                    Count = a.Count()

                }).ToList();


            // sorgu13
            // Hangi ürün hangi kategoride hangi tedarikçi tarafından getirilmiştir. KategoryAdı,UrunAdı,Fiyatı,Stoğu,Tedarikçi bilgileri ekrana getirilecek sorgu

            var query13 = _db.Products
                .Include(x => x.Category)
                .Include(x => x.Supplier)
                .Select(a => new
            {
                ProductName = a.ProductName,
                CategoryName = a.Category.CategoryName,
                SupplierName = a.Supplier.ContactName,
                UnitPrice = a.UnitPrice,
                Stock = a.UnitsInStock

            }).AsNoTracking();

            // AsNoTracking toList veya firstOrDefault öncesinde kullanalım.
            // ToList ve firstOrDefault IQuerable olarak işaretlenmiş sorgunun Sql düşmesine yani execute edilmesine neden olur.

            // Not eğer sorgulama operasyonları yapıyorsak EF core bu sorgulananan nesnelerin her birini takip alıyor. buda performans kaybına yol açıyor. select işlemlerinde çok gereksiz bir durum. Sorgu performansını artırmak asNoTracking olarak işaretleyelim

            //var product = _db.Products.Find("1"); // ChangeTracker ile direk program tarafında attached oluyor. yani üzerinden değişiklik yapılınca direk dbye gönderilebilir. // Attached
            //product.UnitPrice = 2131; // Modified

            //_db.Products.Update(product);
            //_db.SaveChanges(); // detached

            query13.ToList();




            //sorgu14
            // en çok adet ürün sipariş edilen sipariş'in toplam tutarını bulalım

            // Sql 
            /*
             * select MaxPriceOrderTableRecord.OrderTotalPrice from (select 
                top 1
                od.OrderID as 'OrderNumber',
                SUM(od.Quantity) as 'OrderQuantity', 
                SUM(od.Quantity * od.UnitPrice * (1-od.Discount))  as 'OrderTotalPrice'
                from Orders o inner join [Order Details] od
                on o.OrderID = od.OrderID 
                group by od.OrderID
                order by OrderQuantity desc) as MaxPriceOrderTableRecord
             */
            var query14 = _db.Orders
                .Include(x => x.OrderDetails)
                .SelectMany(x => x.OrderDetails)
                .GroupBy(x => x.OrderId)
                
                .Select(a => new
            {
                OrderId = a.Key,
                OrderQuantity = a.Sum(x => x.Quantity),
                OrderTotalPrice = a.Sum(x => x.Quantity * x.UnitPrice * (decimal)(1 - x.Discount))

            })
                .OrderByDescending(x => x.OrderQuantity)
                .Take(1)
                .FirstOrDefault().OrderTotalPrice;


            //sorgu15
            // en çok sipariş edilen ilk 5 ürünün toplam tutarını hesaplayalım
            // Urun Adı, Toplam Tutar şeklinde ekranda listelenecek.

            var query15 = _db.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .SelectMany(x => x.OrderDetails)
                .GroupBy(x => x.Product.ProductName)
                .Select(a => new
            {
                ProductName = a.Key,
                ProductCount = a.Count(),
                ProductTotalPrice = 
                a.Sum(x=> x.Quantity * x.UnitPrice * (decimal)(1- x.Discount))

            })
                .OrderByDescending(x => x.ProductCount)
                .Take(5)
                .Sum(x=> x.ProductTotalPrice);


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
