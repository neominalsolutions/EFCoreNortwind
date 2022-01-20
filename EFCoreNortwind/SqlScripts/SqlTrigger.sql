-- Sql Trigger

-- bir tabloya yapýlan insert,update,delete operasyonlarý sonrasý
-- otomatik olarak tetiklenen database tarafýnda event olay güdümlü çalýþan bir database yapýsýdýr. after update,delete,insert tiplerinde bir tabloya tanýmlanabilir. Bir tablo birden fazla trigger tanýmlamak, iþlem takibi açýsýndan zor olduðu ve bu triggerlarýn birbilerini kilitleme durumlarý olduðu için tercih edilememektedir. Genelde database tarafýnda loglama amaçlý kullanýlan sistemledir.

alter trigger CategoryLog on Categories
after insert,delete,update
as
begin
-- sql de deðiþken tanýmlama @deðiþken isimlerine @iþaret ile baþlatýrýz.
declare @cname nvarchar(50)
declare @oldcname nvarchar(50)

if Exists(select * from inserted) and Exists(select * from deleted)
begin
set @cname = (select CategoryName from inserted)
set @oldcname = (select CategoryName from deleted)
 print  'Tabloda güncellenen alan eski deðeri' + @oldcname + 'yeni deðeri' + @cname
end
else if Exists(select * from inserted)
begin
set @cname = (select CategoryName from inserted)
 print 'Tablodan Kayýt atýldý' + @cname
end
else if Exists(select * from deleted)
begin
set @cname = (select CategoryName from deleted)
 print  'Tablodan Kayýt silindi' + @cname
end

--select CategoryName as 'IC',CategoryID as 'ICID' from inserted
--select CategoryName as 'IC',CategoryID as 'ICID' from inserted
--print  'Kategoriye kayýt girildi'
end

-- kendi kendine tetiklenir
-- tablo üzerinde yapýlan iþlemleri inserted ve deleted sanal tablolarýndan okuyabiliriz.
insert into Categories(CategoryName) values('Kategori16545')

delete from Categories where CategoryID = 1014

update Categories set CategoryName='asdsadasda' where CategoryID = 1014


