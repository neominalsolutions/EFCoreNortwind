
-- verinin bir b�t�n halinde tutulamas�, tek bir i�lem olarak yap�labilmesini sa�lar. yani i�lemin atomik olmas� i�in kullan�r�z.

-- �r�n sipari� i�leminde veri taban�na order Detail ve order kayd� d��er ayn� zamanda �r�n stok g�ncellenir.



begin transaction
transaction commit
-- t�m i�lemleri ba�ar�l� ise transaction commit edilir yani i�lemler kapat�l�r
transaction rollback
-- bir hata durumunda ise t�m i�lemler geri al�n�r
end 

-- genelde transaction yap�lar� store procedure ile kullan�l�r
-- store procedure kodlar� i�erisinde e�er insert, update, delete gibi i�lemler varsa ve i�lemler biribiri ile alakal� i�lemler ise veri taban� b�t�nl���n� bozmamak i�in transaction kullanmal�y�z.


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

INSERT INTO HESAP VALUES('�BRAH�M','BAYRAKTAR',19265,7000)
INSERT INTO HESAP VALUES('SAMET','ULUTURK',19572,10000)
INSERT INTO HESAP VALUES('RAMAZAN','PINARBA�I',19752,9500)
INSERT INTO HESAP VALUES('RA��T','BAKIR',19912,17000)


UPDATE HESAP SET BAKIYE = BAKIYE - 1000 --1.Sorgu
WHERE AD='SAMET' AND SOYAD='ULUTURK'

UPDATE HESAP SET BAKIYE = BAKIYE + 1000 --2.Sorgu
WHERE AD='�BRAH�M' AND SOYAD='BAYRAKTAR'

select * from HESAP

-- sadece CUD (Create, Update, Delete) i�lemlerinde ihtiya� var.
-- select i�lemleri i�in ise gerek yoktur.
-- yaz�l�m�c�n�n �al��ma zaman�nda tahmin edemedi�i hata durumlar�n� tolere etmek i�in try catch blo�u aras�na kodumuzu sararak hata durumunun geri �ekilmesini yani rollback yap�lmas�n� sa�lar�z

BEGIN TRANSACTION
BEGIN TRY

UPDATE HESAP SET BAKIYE = BAKIYE - 1000
WHERE AD='SAMET' AND SOYAD='ULUTURK'

--RAISERROR('ATM ARIZASI',16,1);

UPDATE HESAP SET BAKIYE = BAKIYE + 1000
WHERE AD='�BRAH�M' AND SOYAD='BAYRAKTAR'

COMMIT
END TRY
BEGIN CATCH
print 'ATM ARIZASI'
ROLLBACK
END CATCH