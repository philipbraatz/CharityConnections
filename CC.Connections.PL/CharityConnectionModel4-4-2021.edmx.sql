
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/04/2021 19:00:46
-- Generated from EDMX file: C:\Users\philo\source\repos\CharityConnections\CC.Connections.PL\CharityConnectionModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CC.Connections.DB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__MemberAc__Membe__38EE7070]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MemberActions] DROP CONSTRAINT [FK__MemberAc__Membe__38EE7070];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Charities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Charities];
GO
IF OBJECT_ID(N'[dbo].[CharityEvents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CharityEvents];
GO
IF OBJECT_ID(N'[dbo].[ContactInfoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContactInfoes];
GO
IF OBJECT_ID(N'[dbo].[EventAttendances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventAttendances];
GO
IF OBJECT_ID(N'[dbo].[HelpingActions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HelpingActions];
GO
IF OBJECT_ID(N'[dbo].[Locations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Locations];
GO
IF OBJECT_ID(N'[dbo].[LogIns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LogIns];
GO
IF OBJECT_ID(N'[dbo].[MemberActions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MemberActions];
GO
IF OBJECT_ID(N'[dbo].[Preferences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Preferences];
GO
IF OBJECT_ID(N'[dbo].[PreferredCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PreferredCategories];
GO
IF OBJECT_ID(N'[dbo].[PreferredCharities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PreferredCharities];
GO
IF OBJECT_ID(N'[dbo].[Volunteers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Volunteers];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [ID] uniqueidentifier  NOT NULL,
    [Desc] nvarchar(70)  NULL,
    [Color] nvarchar(6)  NULL,
    [Image] nvarchar(20)  NULL
);
GO

-- Creating table 'CharityEvents'
CREATE TABLE [dbo].[CharityEvents] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(75)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Requirements] nvarchar(500)  NULL,
    [Description] nvarchar(1500)  NULL,
    [LocationID] uniqueidentifier  NULL,
    [CharityEmail] nvarchar(75)  NULL
);
GO

-- Creating table 'ContactInfoes'
CREATE TABLE [dbo].[ContactInfoes] (
    [MemberEmail] nvarchar(75)  NOT NULL,
    [FName] nvarchar(25)  NULL,
    [LName] nvarchar(50)  NULL,
    [Phone] nvarchar(12)  NULL,
    [DateOfBirth] datetime  NULL
);
GO

-- Creating table 'EventAttendances'
CREATE TABLE [dbo].[EventAttendances] (
    [ID] uniqueidentifier  NOT NULL,
    [EventID] uniqueidentifier  NOT NULL,
    [VolunteerEmail] nvarchar(75)  NOT NULL,
    [VolunteerStatus] int  NULL
);
GO

-- Creating table 'HelpingActions'
CREATE TABLE [dbo].[HelpingActions] (
    [ID] uniqueidentifier  NOT NULL,
    [CategoryID] uniqueidentifier  NULL,
    [Description] nvarchar(75)  NULL
);
GO

-- Creating table 'Locations'
CREATE TABLE [dbo].[Locations] (
    [ID] uniqueidentifier  NOT NULL,
    [Address] nvarchar(50)  NULL,
    [City] nvarchar(25)  NULL,
    [State] nvarchar(25)  NULL,
    [Zip] nvarchar(10)  NULL
);
GO

-- Creating table 'MemberActions'
CREATE TABLE [dbo].[MemberActions] (
    [ID] uniqueidentifier  NOT NULL,
    [MemberEmail] nvarchar(75)  NULL,
    [ActionID] uniqueidentifier  NULL
);
GO

-- Creating table 'Preferences'
CREATE TABLE [dbo].[Preferences] (
    [ID] uniqueidentifier  NOT NULL,
    [Distance] decimal(18,2)  NULL
);
GO

-- Creating table 'PreferredCategories'
CREATE TABLE [dbo].[PreferredCategories] (
    [ID] uniqueidentifier  NOT NULL,
    [CategoryID] uniqueidentifier  NULL,
    [VolunteerEmail] nvarchar(75)  NULL
);
GO

-- Creating table 'PreferredCharities'
CREATE TABLE [dbo].[PreferredCharities] (
    [ID] uniqueidentifier  NOT NULL,
    [VolunteerEmail] nvarchar(75)  NULL,
    [CharityEmail] nvarchar(75)  NULL
);
GO

-- Creating table 'Charities'
CREATE TABLE [dbo].[Charities] (
    [CharityEmail] nvarchar(75)  NOT NULL,
    [Name] varchar(50)  NULL,
    [EIN] nvarchar(50)  NULL,
    [Deductibility] bit  NULL,
    [URL] nvarchar(75)  NULL,
    [Cause] nvarchar(500)  NULL,
    [CategoryID] uniqueidentifier  NULL,
    [LocationID] uniqueidentifier  NULL
);
GO

-- Creating table 'Volunteers'
CREATE TABLE [dbo].[Volunteers] (
    [VolunteerEmail] nvarchar(75)  NOT NULL,
    [PreferenceID] uniqueidentifier  NULL,
    [LocationID] uniqueidentifier  NULL
);
GO

-- Creating table 'LogIns'
CREATE TABLE [dbo].[LogIns] (
    [MemberEmail] nvarchar(75)  NOT NULL,
    [MemberType] int  NULL,
    [Password] nvarchar(150)  NULL,
    [Key] nchar(40)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CharityEvents'
ALTER TABLE [dbo].[CharityEvents]
ADD CONSTRAINT [PK_CharityEvents]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [MemberEmail] in table 'ContactInfoes'
ALTER TABLE [dbo].[ContactInfoes]
ADD CONSTRAINT [PK_ContactInfoes]
    PRIMARY KEY CLUSTERED ([MemberEmail] ASC);
GO

-- Creating primary key on [ID] in table 'EventAttendances'
ALTER TABLE [dbo].[EventAttendances]
ADD CONSTRAINT [PK_EventAttendances]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HelpingActions'
ALTER TABLE [dbo].[HelpingActions]
ADD CONSTRAINT [PK_HelpingActions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [PK_Locations]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'MemberActions'
ALTER TABLE [dbo].[MemberActions]
ADD CONSTRAINT [PK_MemberActions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Preferences'
ALTER TABLE [dbo].[Preferences]
ADD CONSTRAINT [PK_Preferences]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'PreferredCategories'
ALTER TABLE [dbo].[PreferredCategories]
ADD CONSTRAINT [PK_PreferredCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'PreferredCharities'
ALTER TABLE [dbo].[PreferredCharities]
ADD CONSTRAINT [PK_PreferredCharities]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [CharityEmail] in table 'Charities'
ALTER TABLE [dbo].[Charities]
ADD CONSTRAINT [PK_Charities]
    PRIMARY KEY CLUSTERED ([CharityEmail] ASC);
GO

-- Creating primary key on [VolunteerEmail] in table 'Volunteers'
ALTER TABLE [dbo].[Volunteers]
ADD CONSTRAINT [PK_Volunteers]
    PRIMARY KEY CLUSTERED ([VolunteerEmail] ASC);
GO

-- Creating primary key on [MemberEmail] in table 'LogIns'
ALTER TABLE [dbo].[LogIns]
ADD CONSTRAINT [PK_LogIns]
    PRIMARY KEY CLUSTERED ([MemberEmail] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ActionID] in table 'MemberActions'
ALTER TABLE [dbo].[MemberActions]
ADD CONSTRAINT [FK__MemberAc__Membe__38EE7070]
    FOREIGN KEY ([ActionID])
    REFERENCES [dbo].[HelpingActions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__MemberAc__Membe__38EE7070'
CREATE INDEX [IX_FK__MemberAc__Membe__38EE7070]
ON [dbo].[MemberActions]
    ([ActionID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------