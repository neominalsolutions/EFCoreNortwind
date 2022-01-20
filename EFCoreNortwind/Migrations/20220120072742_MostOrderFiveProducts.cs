using Microsoft.EntityFrameworkCore.Migrations;
using NetCoreEFApp.Database.Utils;

namespace EFCoreNortwind.Migrations
{
    public partial class MostOrderFiveProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql = MigrationUtil.ReadSql(typeof(EFCoreNortwind.Migrations.MostOrderFiveProducts), "20220120070120_MostOrderedFiveProducts.sql");
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop view MostOrderFiveProducts");

        }
    }
}
