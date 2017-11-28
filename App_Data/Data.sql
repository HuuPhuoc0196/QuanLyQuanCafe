CREATE DATABASE QuanLyQuanCafe
GO

USE QuanLyQuanCafe
GO

-- Bảng Tài khoản
-- Bảng Bàn ăn
-- Bảng Danh mục món ăn
-- Bảng Món ăn
-- Bảng Hóa Đơn
-- Bảng Chi tiết hóa đơn

CREATE TABLE Account 
(
	UserName NVARCHAR(100) PRIMARY KEY,
	DisplayName NVARCHAR(100) NOT NULL,
	PassWord NVARCHAR(1000) NOT NULL DEFAULT '123456',
	Type INT NOT NULL DEFAULT 0 -- 1: addmin && 0: tài khoản thường
)
GO

CREATE TABLE TableFood
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL,
	status NVARCHAR(100) NOT NULL DEFAULT N'Trống' -- Trống or Có người
)
GO

CREATE TABLE FoodCategory
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL
)
GO

CREATE TABLE Food
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL,
	IdCategory INT NOT NULL,
	price FLOAT NOT NULL DEFAULT 0

	FOREIGN KEY (IdCategory) REFERENCES dbo.FoodCategory(Id)
)
GO

CREATE TABLE Bill
(
	Id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE,
	IdTableFood INT NOT NULL,
	status INT NOT NULL DEFAULT 0 -- 1: đã thanh toán && 0: là chưa thanh toán

	FOREIGN KEY (IdTableFood) REFERENCES dbo.TableFood(Id)
)
GO

CREATE TABLE BillInfo
(
	Id INT IDENTITY PRIMARY KEY,
	IdBill INT NOT NULL,
	IdFood INT NOT NULL,
	Count INT NOT NULL DEFAULT 0

	FOREIGN KEY (IdBill) REFERENCES dbo.Bill(Id),
	FOREIGN KEY (IdFood) REFERENCES dbo.Food(Id)
)
GO

INSERT INTO Account VALUES('lhp0196', N'Lê Hữu Phước', '123456', 1)
INSERT INTO Account VALUES('account1', N'Account1', '123456', 0)



CREATE PROC usp_GetListAccountByUserName(@UserName nvarchar(100))
AS
	SELECT * FROM Account WHERE UserName = @UserName
GO

EXEC usp_GetListAccountByUserName 'lhp0196'
GO


CREATE PROC usp_Login(@UserName nvarchar(100), @PassWord nvarchar(100))
AS
BEGIN
	SELECT * FROM Account WHERE UserName = @UserName AND PassWord = @PassWord
END
GO

DECLARE @i INT = 16;
WHILE @i < 25
BEGIN
	INSERT INTO TableFood(Name, status) VALUES(N'Bàn ' + CAST(@i AS nvarchar(100)), N'Trống')
	SET @i = @i + 1
END

CREATE PROC usp_GetTableList
AS
	SELECT * FROM TableFood
GO

EXEC usp_GetTableList

