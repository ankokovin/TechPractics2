
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/11/2018 10:43:28
-- Generated from EDMX file: C:\Users\user\Documents\GitHub\TechPractics2\TechPractics2\Models\UtilityModels\SiteUtilities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Site_Utilities];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_TokenTokenInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TokenSet] DROP CONSTRAINT [FK_TokenTokenInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_HashedTokenTokenInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HashedTokenSet] DROP CONSTRAINT [FK_HashedTokenTokenInfo];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[TokenSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TokenSet];
GO
IF OBJECT_ID(N'[dbo].[TokenInfoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TokenInfoSet];
GO
IF OBJECT_ID(N'[dbo].[HashedTokenSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HashedTokenSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'TokenSet'
CREATE TABLE [dbo].[TokenSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] nvarchar(max)  NOT NULL,
    [TokenInfo_Id] int  NOT NULL
);
GO

-- Creating table 'TokenInfoSet'
CREATE TABLE [dbo].[TokenInfoSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [dateTime] datetime  NOT NULL,
    [Salt] nvarchar(max)  NOT NULL,
    [Login] nvarchar(max)  NOT NULL,
    [Source] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'HashedTokenSet'
CREATE TABLE [dbo].[HashedTokenSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] nvarchar(max)  NOT NULL,
    [TokenInfo_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'TokenSet'
ALTER TABLE [dbo].[TokenSet]
ADD CONSTRAINT [PK_TokenSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TokenInfoSet'
ALTER TABLE [dbo].[TokenInfoSet]
ADD CONSTRAINT [PK_TokenInfoSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HashedTokenSet'
ALTER TABLE [dbo].[HashedTokenSet]
ADD CONSTRAINT [PK_HashedTokenSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [TokenInfo_Id] in table 'TokenSet'
ALTER TABLE [dbo].[TokenSet]
ADD CONSTRAINT [FK_TokenTokenInfo]
    FOREIGN KEY ([TokenInfo_Id])
    REFERENCES [dbo].[TokenInfoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TokenTokenInfo'
CREATE INDEX [IX_FK_TokenTokenInfo]
ON [dbo].[TokenSet]
    ([TokenInfo_Id]);
GO

-- Creating foreign key on [TokenInfo_Id] in table 'HashedTokenSet'
ALTER TABLE [dbo].[HashedTokenSet]
ADD CONSTRAINT [FK_HashedTokenTokenInfo]
    FOREIGN KEY ([TokenInfo_Id])
    REFERENCES [dbo].[TokenInfoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HashedTokenTokenInfo'
CREATE INDEX [IX_FK_HashedTokenTokenInfo]
ON [dbo].[HashedTokenSet]
    ([TokenInfo_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------