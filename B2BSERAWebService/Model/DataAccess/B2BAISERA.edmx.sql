
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 08/28/2013 13:14:27
-- Generated from EDMX file: E:\Kantor\Project\SERA\Backup\2013Agustus28 - Local Ws\B2BAISERA - Upload S02001\B2BSERAWebService\Model\DataAccess\B2BAISERA.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [B2BAISERA];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Transaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionData] DROP CONSTRAINT [FK_Transaction];
GO
IF OBJECT_ID(N'[dbo].[FK_TransactionData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionDataDetail] DROP CONSTRAINT [FK_TransactionData];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DocumentFileType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentFileType];
GO
IF OBJECT_ID(N'[dbo].[DocumentIPAddress]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentIPAddress];
GO
IF OBJECT_ID(N'[dbo].[DocumentStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentStatus];
GO
IF OBJECT_ID(N'[dbo].[Response]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Response];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[Transaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transaction];
GO
IF OBJECT_ID(N'[dbo].[TransactionData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionData];
GO
IF OBJECT_ID(N'[dbo].[TransactionDataDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionDataDetail];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DocumentFileTypes'
CREATE TABLE [dbo].[DocumentFileTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [FileTypeName] varchar(20)  NULL,
    [CreatedWho] varchar(50)  NULL,
    [CreatedWhen] datetime  NULL,
    [ChangedWho] varchar(50)  NULL,
    [ChangedWhen] datetime  NULL
);
GO

-- Creating table 'DocumentIPAddresses'
CREATE TABLE [dbo].[DocumentIPAddresses] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [IPAddress] varchar(15)  NOT NULL,
    [CreatedWho] varchar(50)  NULL,
    [CreatedWhen] datetime  NULL,
    [ChangedWho] varchar(50)  NULL,
    [ChangedWhen] datetime  NULL
);
GO

-- Creating table 'DocumentStatus'
CREATE TABLE [dbo].[DocumentStatus] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [StatusName] varchar(20)  NULL,
    [CreatedWho] varchar(50)  NULL,
    [CreatedWhen] datetime  NULL,
    [ChangedWho] varchar(50)  NULL,
    [ChangedWhen] datetime  NULL
);
GO

-- Creating table 'Responses'
CREATE TABLE [dbo].[Responses] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [WebServiceName] varchar(50)  NOT NULL,
    [MethodName] varchar(50)  NOT NULL,
    [Acknowledge] bit  NOT NULL,
    [TicketNo] varchar(500)  NULL,
    [Message] varchar(500)  NULL,
    [CreatedWho] varchar(50)  NOT NULL,
    [CreatedWhen] datetime  NOT NULL,
    [ChangedWho] varchar(50)  NOT NULL,
    [ChangedWhen] datetime  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserName] varchar(200)  NULL,
    [Password] varchar(200)  NULL,
    [ClientTag] varchar(50)  NULL,
    [CreatedWho] varchar(50)  NULL,
    [CreatedWhen] datetime  NULL,
    [ChangedWho] varchar(50)  NULL,
    [ChangedWhen] datetime  NULL
);
GO

-- Creating table 'Transactions'
CREATE TABLE [dbo].[Transactions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TicketNo] varchar(200)  NOT NULL,
    [ClientTag] varchar(50)  NOT NULL,
    [CreatedWho] varchar(50)  NOT NULL,
    [CreatedWhen] datetime  NOT NULL,
    [ChangedWho] varchar(50)  NOT NULL,
    [ChangedWhen] datetime  NOT NULL
);
GO

-- Creating table 'TransactionDatas'
CREATE TABLE [dbo].[TransactionDatas] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TransGUID] varchar(40)  NOT NULL,
    [DocumentNumber] varchar(30)  NOT NULL,
    [FileType] varchar(20)  NOT NULL,
    [IPAddress] varchar(15)  NOT NULL,
    [DestinationUser] varchar(10)  NOT NULL,
    [Key1] varchar(20)  NOT NULL,
    [Key2] varchar(20)  NOT NULL,
    [Key3] varchar(20)  NOT NULL,
    [DataLength] int  NOT NULL,
    [RowStatus] varchar(50)  NOT NULL,
    [CreatedWho] varchar(50)  NOT NULL,
    [CreatedWhen] datetime  NOT NULL,
    [ChangedWho] varchar(50)  NOT NULL,
    [ChangedWhen] datetime  NOT NULL,
    [Transaction_ID] int  NOT NULL
);
GO

-- Creating table 'TransactionDataDetails'
CREATE TABLE [dbo].[TransactionDataDetails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Data] varchar(400)  NULL,
    [TransactionData_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'DocumentFileTypes'
ALTER TABLE [dbo].[DocumentFileTypes]
ADD CONSTRAINT [PK_DocumentFileTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DocumentIPAddresses'
ALTER TABLE [dbo].[DocumentIPAddresses]
ADD CONSTRAINT [PK_DocumentIPAddresses]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DocumentStatus'
ALTER TABLE [dbo].[DocumentStatus]
ADD CONSTRAINT [PK_DocumentStatus]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Responses'
ALTER TABLE [dbo].[Responses]
ADD CONSTRAINT [PK_Responses]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Transactions'
ALTER TABLE [dbo].[Transactions]
ADD CONSTRAINT [PK_Transactions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TransactionDatas'
ALTER TABLE [dbo].[TransactionDatas]
ADD CONSTRAINT [PK_TransactionDatas]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TransactionDataDetails'
ALTER TABLE [dbo].[TransactionDataDetails]
ADD CONSTRAINT [PK_TransactionDataDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Transaction_ID] in table 'TransactionDatas'
ALTER TABLE [dbo].[TransactionDatas]
ADD CONSTRAINT [FK_Transaction]
    FOREIGN KEY ([Transaction_ID])
    REFERENCES [dbo].[Transactions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Transaction'
CREATE INDEX [IX_FK_Transaction]
ON [dbo].[TransactionDatas]
    ([Transaction_ID]);
GO

-- Creating foreign key on [TransactionData_ID] in table 'TransactionDataDetails'
ALTER TABLE [dbo].[TransactionDataDetails]
ADD CONSTRAINT [FK_TransactionData]
    FOREIGN KEY ([TransactionData_ID])
    REFERENCES [dbo].[TransactionDatas]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TransactionData'
CREATE INDEX [IX_FK_TransactionData]
ON [dbo].[TransactionDataDetails]
    ([TransactionData_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------