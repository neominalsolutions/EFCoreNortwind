
-- verinin bir bütün halinde tutulamasý, tek bir iþlem olarak yapýlabilmesini saðlar. yani iþlemin atomik olmasý için kullanýrýz.

-- ürün sipariþ iþleminde veri tabanýna order Detail ve order kaydý düþer ayný zamanda ürün stok güncellenir.



begin transaction
transaction commit
-- tüm iþlemleri baþarýlý ise transaction commit edilir yani iþlemler kapatýlýr
transaction rollback
-- bir hata durumunda ise tüm iþlemler geri alýnýr
end 

-- genelde transaction yapýlarý store procedure ile kullanýlýr
-- store procedure kodlarý içerisinde eðer insert, update, delete gibi iþlemler varsa ve iþlemler biribiri ile alakalý iþlemler ise veri tabaný bütünlüðünü bozmamak için transaction kullanmalýyýz.


CREATE DATABASE BANKA
USE BANKA

CREATE TABLE HESAP
(
HESAP_ID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
AD       VARCHAR(30) NOT NULL,
SOYAD    VARCHAR(30) NOT NULL,
HESAP_NO INT         NOT NULL,
BAKIYE   INT         NOT NULL
)

INSERT INTO HESAP VALUES('ÝBRAHÝM','BAYRAKTAR',19265,7000)
INSERT INTO HESAP VALUES('SAMET','ULUTURK',19572,10000)
INSERT INTO HESAP VALUES('RAMAZAN','PINARBAÞI',19752,9500)
INSERT INTO HESAP VALUES('RAÞÝT','BAKIR',19912,17000)


UPDATE HESAP SET BAKIYE = BAKIYE - 1000 --1.Sorgu
WHERE AD='SAMET' AND SOYAD='ULUTURK'

UPDATE HESAP SET BAKIYE = BAKIYE + 1000 --2.Sorgu
WHERE AD='ÝBRAHÝM' AND SOYAD='BAYRAKTAR'

select * from HESAP

-- sadece CUD (Create, Update, Delete) iþlemlerinde ihtiyaç var.
-- select iþlemleri için ise gerek yoktur.
-- yazýlýmýcýnýn çalýþma zamanýnda tahmin edemediði hata durumlarýný tolere etmek için try catch bloðu arasýna kodumuzu sararak hata durumunun geri çekilmesini yani rollback yapýlmasýný saðlarýz

BEGIN TRANSACTION
BEGIN TRY

UPDATE HESAP SET BAKIYE = BAKIYE - 1000
WHERE AD='SAMET' AND SOYAD='ULUTURK'

--RAISERROR('ATM ARIZASI',16,1);

UPDATE HESAP SET BAKIYE = BAKIYE + 1000
WHERE AD='ÝBRAHÝM' AND SOYAD='BAYRAKTAR'

COMMIT
END TRY
BEGIN CATCH
print 'ATM ARIZASI'
ROLLBACK
END CATCH