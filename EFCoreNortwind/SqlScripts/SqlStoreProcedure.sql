USE [NORTHWND]
GO
/****** Object:  StoredProcedure [dbo].[PagedProducts]    Script Date: 20.01.2022 12:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[PagedProducts]
(
  @searchText nvarchar(50),
  @limit int,
  @pageIndex int,
  @sortColumn nvarchar(50),
  @sortOrder nvarchar(4)
)
as
begin

	if(@pageIndex = 0)
	begin
	   set @pageIndex =1
	end

  select * from dbo.ProductCategorySupplierView p
   where p.ProductName like  '%'+@searchText+'%' or p.CategoryName like '%'+@searchText+'%' or p.SupplierCompany like '%' +@searchText+ '%' or p.UnitPrice like '%'+@searchText+'%' or p.Stock like '%'+@searchText+'%'
   order by  
    
	CASE WHEN @sortColumn = 'UnitPrice' and  @sortOrder = 'ASC' THEN p.UnitPrice END ASC,

	CASE WHEN @sortColumn = 'UnitPrice' and  @sortOrder = 'DESC' THEN p.UnitPrice END DESC, 

    CASE WHEN @sortColumn = 'Stock' and  @sortOrder = 'ASC' THEN p.Stock  END ASC,

	CASE WHEN @sortColumn = 'Stock' and  @sortOrder = 'DESC' THEN p.Stock  END DESC,

	CASE WHEN @sortColumn = 'ProductName' and  @sortOrder = 'ASC' THEN p.ProductName END ASC,

	CASE WHEN @sortColumn = 'ProductName' and  @sortOrder = 'DESC' THEN p.ProductName END DESC,

	CASE WHEN @sortColumn = 'CategoryName' and  @sortOrder = 'ASC' THEN p.CategoryName END ASC,

	CASE WHEN @sortColumn = 'CategoryName' and  @sortOrder = 'DESC' THEN p.CategoryName END DESC,

	CASE WHEN @sortColumn = 'SupplierCompany' and  @sortOrder = 'ASC' THEN p.SupplierCompany END ASC,

	CASE WHEN @sortColumn = 'SupplierCompany' and  @sortOrder = 'DESC' THEN p.SupplierCompany END DESC

    offset (@pageIndex-1) * @limit ROWS --  @offset * @limit kadar kayýt atlat
	FETCH NEXT @limit ROWS ONLY; -- kayýt atlatma islemi yaptýktan sonra @limit kadar kaydý select yap.
   
end

exec PagedProducts  
@searchText='',
@limit=10,
@pageIndex=8,
@sortColumn='ProductName',
@sortOrder='Asc'