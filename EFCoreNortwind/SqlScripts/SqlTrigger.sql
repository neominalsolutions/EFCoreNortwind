-- Sql Trigger

-- bir tabloya yap�lan insert,update,delete operasyonlar� sonras�
-- otomatik olarak tetiklenen database taraf�nda event olay g�d�ml� �al��an bir database yap�s�d�r. after update,delete,insert tiplerinde bir tabloya tan�mlanabilir. Bir tablo birden fazla trigger tan�mlamak, i�lem takibi a��s�ndan zor oldu�u ve bu triggerlar�n birbilerini kilitleme durumlar� oldu�u i�in tercih edilememektedir. Genelde database taraf�nda loglama ama�l� kullan�lan sistemledir.

alter trigger CategoryLog on Categories
after insert,delete,update
as
begin
-- sql de de�i�ken tan�mlama @de�i�ken isimlerine @i�aret ile ba�lat�r�z.
declare @cname nvarchar(50)
declare @oldcname nvarchar(50)

if Exists(select * from inserted) and Exists(select * from deleted)
begin
set @cname = (select CategoryName from inserted)
set @oldcname = (select CategoryName from deleted)
 print  'Tabloda g�ncellenen alan eski de�eri' + @oldcname + 'yeni de�eri' + @cname
end
else if Exists(select * from inserted)
begin
set @cname = (select CategoryName from inserted)
 print 'Tablodan Kay�t at�ld�' + @cname
end
else if Exists(select * from deleted)
begin
set @cname = (select CategoryName from deleted)
 print  'Tablodan Kay�t silindi' + @cname
end

--select CategoryName as 'IC',CategoryID as 'ICID' from inserted
--select CategoryName as 'IC',CategoryID as 'ICID' from inserted
--print  'Kategoriye kay�t girildi'
end

-- kendi kendine tetiklenir
-- tablo �zerinde yap�lan i�lemleri inserted ve deleted sanal tablolar�ndan okuyabiliriz.
insert into Categories(CategoryName) values('Kategori16545')

delete from Categories where CategoryID = 1014

update Categories set CategoryName='asdsadasda' where CategoryID = 1014


