using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreNortwind.Data.Dtos
{
    /// <summary>
    /// Store Procedure ile çalışırken tablodaki sütün ismi ve tabloda sütüunun sql karşlığını program tarafına verilmesi önemli yoksa çalışmaz.
    /// _db.PagedProducts.FromSqlRaw("sql_query") // FromSqlRaw ile store procedure içerisinde select işlemlerini dinamik parametre bazlı çalıştırabiliriz.
    /// </summary>
    public class PagedProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string SupplierCompany { get; set; }
        public decimal UnitPrice { get; set; }
        public short Stock { get; set; }

    }

    public static class SqlSortingTypes
    {
        public const string Ascending = "Asc";
        public const string Descending = "Desc";
    }

    public class PagedProductParams
    {
        public object[] Params => @params.ToArray();
        private List<object> @params = new List<object>();

        public PagedProductParams(string searchText, string sortColumn, string sortOrder = SqlSortingTypes.Ascending, int limit = 10, int pageIndex=1)
        {
            var _searchText = new SqlParameter("@searchText", System.Data.SqlDbType.NVarChar, 50);
            _searchText.Value = searchText.Trim();

            var _limit = new SqlParameter("@limit", System.Data.SqlDbType.Int);
            _limit.Value = limit;

            var _pageIndex = new SqlParameter("@pageIndex", System.Data.SqlDbType.Int);
            _pageIndex.Value = pageIndex;

            var _sortColumn = new SqlParameter("@sortColumn", System.Data.SqlDbType.NVarChar, 50);
            _sortColumn.Value = sortColumn;

            var _sortOrder = new SqlParameter("@sortOrder", System.Data.SqlDbType.NVarChar, 4);
            _sortOrder.Value = sortOrder;

            @params.Add(_searchText);
            @params.Add(_limit);
            @params.Add(_pageIndex);
            @params.Add(_sortColumn);
            @params.Add(_sortOrder);


        }
    }
}
