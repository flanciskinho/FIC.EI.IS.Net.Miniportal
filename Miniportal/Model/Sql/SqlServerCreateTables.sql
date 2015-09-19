/* 
 * SQL Server Script
 * 
 * This script can be directly executed to configure the test database from
 * PCs located at CECAFI Lab. The database and the corresponding users are 
 * already created in the sql server, so it will create the tables needed 
 * in the samples. 
 * 
 * In a local environment (for example, with the SQLServerExpress instance 
 * included in the VStudio installation) it will be necessary to create the 
 * database and the user required by the connection string. So, the following
 * steps are needed:
 *
 *      Configure within the CREATE DATABASE sql-sentence the path where 
 *      database and log files will be created  
 *
 * This script can be executed from MS Sql Server Management Studio Express,
 * but also it is possible to use a command Line syntax:
 *
 *    > sqlcmd.exe -U [user] -P [password] -I -i SqlServerCreateTables.sql
 *
 */

 
--USE [master]
--GO

--/****** Drop database if already exists  ******/
--IF  EXISTS (SELECT name FROM sys.databases WHERE name = 'PracticaIS')
--DROP DATABASE [PracticaIS]
--GO

--USE [master]
--GO

--/****** DataBase Creation ******/
								  
--CREATE DATABASE [PracticaIS] ON PRIMARY 
--( NAME = 'PracticaIS', FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\PracticaIS.mdf') 
--LOG ON 
--( NAME = 'PracticaIS_log', FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\PracticaIS_log.ldf')
--GO


--/******   Create LoginUser    ******/
--IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'user')
--CREATE LOGIN [user] 
--WITH   PASSWORD='password', 
--	   DEFAULT_DATABASE=[PracticaIS], 
--	   DEFAULT_LANGUAGE=[Español], 
--	   CHECK_EXPIRATION=OFF, 
--	   CHECK_POLICY=OFF
--GO


--/******   Set user as database dbo  ******/
--USE PracticaIS
--GO

--SP_CHANGEDBOWNER 'user'
--GO


USE [PracticaIS]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[LabelComment]') AND type in ('U'))
DROP TABLE [LabelComment]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Label]') AND type in ('U'))
DROP TABLE [Label]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Comment]') AND type in ('U'))
DROP TABLE [Comment]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Favourite]') AND type in ('U'))
DROP TABLE [Favourite]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Valuation]') AND type in ('U'))
DROP TABLE [Valuation]

/* ********** Drop Table UserProfile if already exists *********** */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[UserProfile]') AND type in ('U'))
DROP TABLE [UserProfile]

GO


/* ******************* UserProfile: Table Creation  ************** */

CREATE TABLE UserProfile (
	usrId bigint IDENTITY(1,1) NOT NULL,
	loginName varchar(30) NOT NULL,
	enPassword varchar(50) NOT NULL,
	firstName varchar(30) NOT NULL,
	lastName varchar(40) NOT NULL,
	email varchar(60) NOT NULL,
--	language varchar(2) NOT NULL,
--	country varchar(2) NOT NULL,

	CONSTRAINT [PK_UserProfile] PRIMARY KEY (usrId),
	CONSTRAINT [UniqueKey_Login] UNIQUE (loginName)
)


USE [PracticaIS]
GO


CREATE NONCLUSTERED INDEX [IX_UserProfileIndexByLoginName]
ON [UserProfile] ([loginName] ASC)

GO

/** favourite: table creation **/
CREATE TABLE Favourite (
	favouriteId bigint IDENTITY(1,1) NOT NULL,
	name varchar(30) NOT NULL,
	comment varchar(250),
	addDate datetime2 NOT NULL,

	productId bigint NOT NULL,
	userProfileId bigint NOT NULL,

	CONSTRAINT [U_Favourite] UNIQUE (userProfileId, productId),
	CONSTRAINT [PK_Favourite] PRIMARY KEY (favouriteId),
	CONSTRAINT [FK_Favourite_User] FOREIGN KEY (userProfileId) REFERENCES UserProfile(usrId)
)

USE [PracticaIS]
GO


CREATE NONCLUSTERED INDEX[IX_FavouriteIndexByUserProfileIdAndProductId]
ON [Favourite] ([userProfileId],[productId])

GO


/** valuation: table creation **/
CREATE TABLE Valuation (
	valuationId bigint IDENTITY(1,1) NOT NULL,
	score bigint NOT NULL,
	addDate datetime2 NOT NULL,
	txt varchar(250),

	sellerId varchar(250) NOT NULL,
	userProfileId bigint NOT NULL,
	productId bigint NOT NULL,

	CONSTRAINT [U_Valuation] UNIQUE (userProfileId, productId),
	CONSTRAINT [PK_Valuation] PRIMARY KEY (valuationId),
	CONSTRAINT [FK_Valuation_User] FOREIGN KEY (userProfileId) REFERENCES UserProfile(usrId)
)


USE [PracticaIS]
GO

CREATE NONCLUSTERED INDEX[IX_ValuationIndexByUserProfileIdAndProductId]
ON [Valuation] ([userProfileId],[productId])

GO

/** Comment: table creation **/
CREATE TABLE Comment (
	commentId bigint IDENTITY(1,1) NOT NULL,
	txt varchar(250) NOT NULL,
	addDate datetime2 NOT NULL,

	productId bigint NOT NULL,
	userProfileId bigint NOT NULL,


	CONSTRAINT [PK_Comment] PRIMARY KEY (commentId),
	CONSTRAINT [FK_Comment_User] FOREIGN KEY (userProfileId) REFERENCES UserProfile(usrId)
)


USE [PracticaIS]
GO

/** Label: table creation **/
CREATE TABLE Label (
	labelId bigint IDENTITY(1,1) NOT NULL,
	name varchar(60) NOT NULL,
	cnt bigint NOT NULL DEFAULT 0,

	CONSTRAINT [PK_Label] PRIMARY KEY (labelId),
	CONSTRAINT [UniqueKey_Label] UNIQUE (name)
)


USE [PracticaIS]
GO

CREATE NONCLUSTERED INDEX [IX_LabelIndexByName]
ON [Label] ([name] ASC)

GO

/** LabelComment: table creation **/
CREATE TABLE LabelComment (
	labelId bigint NOT NULL,
	commentId bigint NOT NULL,

	CONSTRAINT [FK_Label_LabelComment]   FOREIGN KEY (labelId)   REFERENCES Label  (labelId),
	CONSTRAINT [FK_Comment_LabelComment] FOREIGN KEY (commentId) REFERENCES Comment(commentId)
	ON DELETE CASCADE ON UPDATE CASCADE,

	CONSTRAINT [PK_LabelComment] PRIMARY KEY (labelId, commentId)
)


USE [PracticaIS]
GO

DECLARE @pepeId bigint, @teteId bigint, @memeId bigint, @yeyeId bigint, @leleId bigint;

-- La contrasena es pepe
INSERT INTO UserProfile (loginName, enPassword, firstName, lastName, email)
VALUES ( 'pepe', 'fJ58FJSyaEq3wZ1q/3N+Rg+p6Y1aI02hMQyX3fVpGDQ=', 'pepe', 'pepe', 'pepe@udc.es');
SET @pepeId = SCOPE_IDENTITY();
INSERT INTO UserProfile (loginName, enPassword, firstName, lastName, email)
VALUES ( 'tete', 'fJ58FJSyaEq3wZ1q/3N+Rg+p6Y1aI02hMQyX3fVpGDQ=', 'tete', 'tete', 'tete@udc.es');
SET @teteId = SCOPE_IDENTITY();
INSERT INTO UserProfile ( loginName, enPassword, firstName, lastName, email)
VALUES ( 'meme', 'fJ58FJSyaEq3wZ1q/3N+Rg+p6Y1aI02hMQyX3fVpGDQ=', 'meme', 'meme', 'meme@udc.es');
SET @memeId = SCOPE_IDENTITY();
INSERT INTO UserProfile ( loginName, enPassword, firstName, lastName, email)
VALUES ( 'yeye', 'fJ58FJSyaEq3wZ1q/3N+Rg+p6Y1aI02hMQyX3fVpGDQ=', 'yeye', 'yeye', 'yeye@udc.es');
SET @yeyeId = SCOPE_IDENTITY();
INSERT INTO UserProfile ( loginName, enPassword, firstName, lastName, email)
VALUES ( 'lele', 'fJ58FJSyaEq3wZ1q/3N+Rg+p6Y1aI02hMQyX3fVpGDQ=', 'lele', 'lele', 'pepe@udc.es');
SET @leleId = SCOPE_IDENTITY();

-- Anadir favoritos de pepe
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Nema17', 'Se necesitan 5 para la impresora', SYSDATETIMEOFFSET(), 1,  @pepeId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('MacBook', 'La bateria es nueva?', SYSDATETIMEOFFSET(), 2,  @pepeId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('GTA V (pc)', 'comprobar los requisitos', SYSDATETIMEOFFSET(), 3,  @pepeId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Bici Montana', 'Regalo para el primo', SYSDATETIMEOFFSET(), 4,  @pepeId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Sun glasses', 'Autenticas maximo dutti', SYSDATETIMEOFFSET(), 5,  @pepeId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Maleta de viaje', 'Comprar antes de ir a LA', SYSDATETIMEOFFSET(), 6,  @pepeId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Belt', 'Cinturon para el caballero', SYSDATETIMEOFFSET(), 7,  @pepeId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Zapatillas', 'Regalo para el primo', SYSDATETIMEOFFSET(), 8,  @pepeId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Cocina a gas', 'Regalo para la suegra', SYSDATETIMEOFFSET(), 9,  @pepeId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Cocina a gas', 'Regalo para la suegra', SYSDATETIMEOFFSET(), 10,  @pepeId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Adaptador', 'Para conectar aparatos en LA', SYSDATETIMEOFFSET(), 11,  @pepeId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Organizador de viaje', 'Era lo que queria Sara', SYSDATETIMEOFFSET(), 12,  @pepeId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Neceser', 'Para el viaje a LA', SYSDATETIMEOFFSET(), 13,  @pepeId);
-- Anadir favoritos de tete
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Nema17', '', SYSDATETIMEOFFSET(), 1,  @teteId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('MacBook', '', SYSDATETIMEOFFSET(), 2,  @teteId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('GTA V (pc)', '', SYSDATETIMEOFFSET(), 3,  @teteId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Bici Montana', '', SYSDATETIMEOFFSET(), 4,  @teteId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Sun glasses', '', SYSDATETIMEOFFSET(), 5,  @teteId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Maleta de viaje', '', SYSDATETIMEOFFSET(), 6,  @teteId);
INSERT INTO Favourite (name, comment, addDate, productId, userProfileId)
VALUES ('Belt', '', SYSDATETIMEOFFSET(), 7,  @teteId);