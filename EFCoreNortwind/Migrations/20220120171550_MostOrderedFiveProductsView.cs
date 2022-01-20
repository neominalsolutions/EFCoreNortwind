using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreNortwind.Migrations
{
    public partial class MostOrderedFiveProductsView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("create view MostOrderedFiveProducts as select top(5) p.ProductName as 'ProductName', SUM(od.Quantity) as 'Quantity' from[Order Details] od inner join Orders o on od.OrderID = o.OrderID inner join Products p on p.ProductID = od.ProductID group by od.ProductID, p.ProductName order by Quantity desc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop view MostOrderedFiveProducts");
        }
    }
}
