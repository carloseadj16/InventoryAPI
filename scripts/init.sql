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
        UpdatedAt   DATETIME2        NULL
    );
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
        UpdatedAt   DATETIME2        NULL
    );
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
