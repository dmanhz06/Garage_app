CREATE DATABASE GarageManagement;
go
USE GarageManagement;
go

DROP TABLE IF EXISTS Invoices;
DROP TABLE IF EXISTS RepairOrderParts;
DROP TABLE IF EXISTS RepairOrderServices;
DROP TABLE IF EXISTS RepairOrders;
-- Xóa các bảng danh mục sau
DROP TABLE IF EXISTS Vehicles;
DROP TABLE IF EXISTS Customers;
DROP TABLE IF EXISTS Services;
DROP TABLE IF EXISTS Parts;
DROP TABLE IF EXISTS Users;

CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    Address NVARCHAR(MAX),
    Email VARCHAR(255)
);

CREATE TABLE Services (
    ServiceID INT PRIMARY KEY IDENTITY(1,1),
    ServiceName NVARCHAR(255),
    Price DECIMAL(18, 2)
);

CREATE TABLE Parts (
    PartID INT PRIMARY KEY IDENTITY(1,1),
    PartName NVARCHAR(255),
    Price DECIMAL(18, 2),
    StockQuantity INT
);

CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(50) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Role NVARCHAR(20) -- 'Admin' hoặc 'Staff'
);


CREATE TABLE Vehicles (
    VehicleID INT PRIMARY KEY IDENTITY(1,1),
    LicensePlate VARCHAR(20) UNIQUE NOT NULL,
    Brand NVARCHAR(100),
    Model NVARCHAR(100),
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID)
);


CREATE TABLE RepairOrders (
    RepairOrderID INT PRIMARY KEY IDENTITY(1,1),
    VehicleID INT FOREIGN KEY REFERENCES Vehicles(VehicleID),
    EntryDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50), -- Đang chờ, Đang sửa, Hoàn thành
    Note NVARCHAR(MAX)
);

CREATE TABLE RepairOrderServices (
    RepairOrderID INT FOREIGN KEY REFERENCES RepairOrders(RepairOrderID),
    ServiceID INT FOREIGN KEY REFERENCES Services(ServiceID),
    Quantity INT DEFAULT 1,
    PriceAtTime DECIMAL(18,2), 
    PRIMARY KEY (RepairOrderID, ServiceID)
);

CREATE TABLE RepairOrderParts (
    RepairOrderID INT FOREIGN KEY REFERENCES RepairOrders(RepairOrderID),
    PartID INT FOREIGN KEY REFERENCES Parts(PartID),
    Quantity INT NOT NULL,
    PriceAtTime DECIMAL(18,2),
    PRIMARY KEY (RepairOrderID, PartID)
);

CREATE TABLE Invoices (
    InvoiceID INT PRIMARY KEY IDENTITY(1,1),
    RepairOrderID INT NOT NULL, 
    InvoiceDate DATETIME DEFAULT GETDATE(),
    LaborCost DECIMAL(18, 2) DEFAULT 0,
    PartsTotal DECIMAL(18, 2) DEFAULT 0,
    TotalAmount AS (LaborCost + PartsTotal), 
    Status NVARCHAR(50) DEFAULT N'Đã thanh toán',
    CONSTRAINT FK_Invoice_RepairOrder FOREIGN KEY (RepairOrderID) 
    REFERENCES RepairOrders(RepairOrderID)
);
GO

-- Xóa dữ liệu các bảng liên kết trước để không bị lỗi Foreign Key
TRUNCATE TABLE Invoices;
TRUNCATE TABLE RepairOrderParts;
TRUNCATE TABLE RepairOrderServices;

-- Xóa dữ liệu bảng cha (Dùng DELETE vì TRUNCATE không cho xóa bảng có Foreign Key tham chiếu)
DELETE FROM RepairOrders;
DELETE FROM Vehicles;
DELETE FROM Customers;

-- Reset lại số tự động (ID) về 1 cho các bảng đã dùng DELETE
DBCC CHECKIDENT ('RepairOrders', RESEED, 0);
DBCC CHECKIDENT ('Vehicles', RESEED, 0);
DBCC CHECKIDENT ('Customers', RESEED, 0);
GO

select *from Invoices

