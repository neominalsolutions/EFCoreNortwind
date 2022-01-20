using Microsoft.EntityFrameworkCore.Migrations;
using NetCoreEFApp.Database.Utils;

namespace EFCoreNortwind.Migrations
{
    public partial class MostOrderedFiveProductsView1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql = MigrationUtil.ReadSql(typeof(EFCoreNortwind.Migrations.MostOrderedFiveProductsView1), "MostOrderedFiveProducts.sql");
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop view MostOrderedFiveProducts");
        }
    }
}
