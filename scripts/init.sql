IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'InventoryDB')
BEGIN
    CREATE DATABASE InventoryDB;
END
GO

USE InventoryDB;
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categories' AND xtype='U')
BEGIN
    CREATE TABLE Categories (
        Id          INT NOT NULL PRIMARY KEY IDENTITY,
        Name        NVARCHAR(100)    NOT NULL,
        Description NVARCHAR(500)    NOT NULL DEFAULT '',
        CreatedAt   DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt   DATETIME2        NULL,
	Active      INT		     NOT NULL
    );
    CREATE UNIQUE INDEX UX_Categories_Id ON Categories(Id);
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
BEGIN
    CREATE TABLE Products (
        Id          INT NOT NULL PRIMARY KEY IDENTITY,
        Name        NVARCHAR(200)    NOT NULL,
        Description NVARCHAR(500)    NOT NULL DEFAULT '',
        Price       DECIMAL(18, 2)   NOT NULL,
        Stock       INT              NOT NULL DEFAULT 0,
        CategoryId  INT NOT NULL,
        CreatedAt   DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt   DATETIME2        NULL,
	Active	    INT		     NOT NULL
    );
    CREATE UNIQUE INDEX UX_Products_Id ON Products(Id);
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='InventoryMovements' AND xtype='U')
BEGIN
    CREATE TABLE InventoryMovements (
        Id           INT NOT NULL PRIMARY KEY IDENTITY,
        ProductId    INT NOT NULL,
        Quantity     INT              NOT NULL,
        MovementType NVARCHAR(10)     NOT NULL,
        Reason       NVARCHAR(300)    NOT NULL DEFAULT '',
        CreatedAt    DATETIME2        NOT NULL DEFAULT GETUTCDATE()
    );
    
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='IdempotencyKeys' AND xtype='U')
BEGIN
   CREATE TABLE IdempotencyKeys (
        Id          UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        RequestId   NVARCHAR(100)    NOT NULL,
        Response    NVARCHAR(MAX)    NOT NULL,
        CreatedAt   DATETIME2        NOT NULL DEFAULT GETUTCDATE()
   );

   CREATE UNIQUE INDEX UX_IdempotencyKeys_RequestId ON IdempotencyKeys(RequestId);
END
GO