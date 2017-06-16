
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'XMLProcessor') DROP DATABASE XMLProcessor
CREATE DATABASE XMLProcessor
GO
USE XMLProcessor
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'XMLItems')	
CREATE TABLE XMLItems([ItemId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,[ItemName] [varchar](50) NOT  NULL,[GenerateXML] [bit] NOT  NULL );
GO
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Bag',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Pen',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Scale',0)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Mouse',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Keyboard',0)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Moniter',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Scale',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Eraser',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Watch',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Mobile',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Charger',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('I card',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Keys',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Values',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Shoe',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Box',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Paper',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Zews',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Wire',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Cable',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Ring',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Query',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Error',1)
INSERT INTO XMLItems (ItemName,GenerateXML) values ('Mouse',1)
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'XMLMessages')
CREATE TABLE XMLMessages([ItemId] [int] PRIMARY KEY NOT NULL,[ItemName] [varchar](50) NOT  NULL,
[Timestamp] [varchar](15) NOT  NULL,[Value] [int] NOT  NULL);
GO