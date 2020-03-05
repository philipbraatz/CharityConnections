
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/30/2019 12:00:13
-- Generated from EDMX file: D:\source\repos\CharityConnections\CC.Connections.PL\CharityConnectionModel.edmx
-- --------------------------------------------------

--Create Database dbCharityConnections; --Uncomment to create database before running the first time

SET QUOTED_IDENTIFIER OFF;
GO
USE [dbCharityConnections];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Member_Ac__Membe__38EE7070]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MemberAction] DROP CONSTRAINT [FK__Member_Ac__Membe__38EE7070];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ContactInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContactInfo];
GO
IF OBJECT_ID(N'[dbo].[EventAttendance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventAttendance];
GO
IF OBJECT_ID(N'[dbo].[HelpingAction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HelpingAction];
GO
IF OBJECT_ID(N'[dbo].[Locations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Locations];
GO
IF OBJECT_ID(N'[dbo].[LogIn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LogIn];
GO
IF OBJECT_ID(N'[dbo].[MemberAction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MemberAction];
GO
IF OBJECT_ID(N'[dbo].[Volunteer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Volunteer];
GO
IF OBJECT_ID(N'[dbo].[Preferences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Preferences];
GO
IF OBJECT_ID(N'[dbo].[PreferredCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PreferredCategory];
GO
IF OBJECT_ID(N'[dbo].[Preferred_Charity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Preferred_Charity];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Charities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Charities];
GO
IF OBJECT_ID(N'[dbo].[CharityEvent]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CharityEvent];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ContactInfo'
CREATE TABLE [dbo].[ContactInfo] (
    [MemberEmail] nvarchar(75) NOT NULL,
    [FName] nvarchar(25)  NULL,
    [LName] nvarchar(50)  NULL,
    [Phone] nvarchar(12)  NULL,
    [DateOfBirth] datetime  NULL
);
GO

-- Creating table 'EventAttendance'
CREATE TABLE [dbo].[EventAttendance] (
    [ID] UNIQUEIDENTIFIER  NOT NULL,
    [EventID] UNIQUEIDENTIFIER  NOT NULL,
    [VolunteerEmail] nvarchar(50)  NOT NULL,
    [VolunteerStatus] int  NULL
);
GO

-- Creating table 'HelpingAction'
CREATE TABLE [dbo].[HelpingAction] (
    [ID] UNIQUEIDENTIFIER  NOT NULL,
    [CategoryID] UNIQUEIDENTIFIER  NULL,
    [Description] nvarchar(75)  NULL
);
GO

-- Creating table 'Locations'
CREATE TABLE [dbo].[Locations] (
    [ID] UNIQUEIDENTIFIER  NOT NULL,
    [Address] nvarchar(50)  NULL,
    [City] nvarchar(25)  NULL,
    [State] nvarchar(25)  NULL,
    [Zip] nvarchar(10)  NULL
);
GO

-- Creating table 'LogIn'
CREATE TABLE [dbo].[LogIn] (
    [MemberEmail] nvarchar(50)  NOT NULL,
    [MemberType] int  NULL,
    [Password] nvarchar(150)  NULL
);
GO

-- Creating table 'MemberAction'
CREATE TABLE [dbo].[MemberAction] (
    [ID] UNIQUEIDENTIFIER  NOT NULL,
    [MemberEmail] nvarchar(50)  NULL,
    [ActionID] UNIQUEIDENTIFIER  NULL
);
GO

-- Creating table 'Volunteer'
CREATE TABLE [dbo].[Volunteer] (
    [VolunteerEmail] UNIQUEIDENTIFIER  NOT NULL,
    [Contact_ID] UNIQUEIDENTIFIER  NULL,
    [PreferenceID] UNIQUEIDENTIFIER  NULL,
    [LocationID] UNIQUEIDENTIFIER  NULL
);
GO

-- Creating table 'Preferences'
CREATE TABLE [dbo].[Preferences] (
    [ID] UNIQUEIDENTIFIER  NOT NULL,
    [Distance] decimal(18,2)  NULL
);
GO

-- Creating table 'PreferredCategory'
CREATE TABLE [dbo].[PreferredCategory] (
    [ID] UNIQUEIDENTIFIER  NOT NULL,
    [CategoryID] UNIQUEIDENTIFIER  NULL,
    [VolunteerEmail] UNIQUEIDENTIFIER  NULL
);
GO

-- Creating table 'Preferred_Charity'
CREATE TABLE [dbo].[Preferred_Charity] (
    [ID] UNIQUEIDENTIFIER  NOT NULL,
    [VolunteerEmail] UNIQUEIDENTIFIER  NULL,
    [CharityEmail] UNIQUEIDENTIFIER  NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [ID] UNIQUEIDENTIFIER  NOT NULL,
    [Desc] nvarchar(70)  NULL,
    [Color] nvarchar(6)  NULL,
    [Image] nvarchar(20)  NULL
);
GO

-- Creating table 'Charities'
CREATE TABLE [dbo].[Charities] (
    [CharityEmail] nvarchar(75)  NOT NULL,
    [Name] varchar(50)  NULL,
    [EIN] nvarchar(50)  NULL,
    [Deductibility] bit  NULL,
    [URL] nvarchar(75)  NULL,
    [Cause] nvarchar(255)  NULL,
    [CategoryID] UNIQUEIDENTIFIER  NULL,
    [LocationID] UNIQUEIDENTIFIER  NULL
);
GO

-- Creating table 'CharityEvent'
CREATE TABLE [dbo].[CharityEvent] (
    [ID] UNIQUEIDENTIFIER  NOT NULL,
    [Name] nvarchar(75)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Requirements] nvarchar(500)  NULL,
    [Description] nvarchar(1500)  NULL,
    [LocationID] UNIQUEIDENTIFIER  NULL,
    [Charity_ID] UNIQUEIDENTIFIER  NULL,
    [ContactInfo_ID] UNIQUEIDENTIFIER  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ContactInfo_ID] in table 'ContactInfo'
ALTER TABLE [dbo].[ContactInfo]
ADD CONSTRAINT [PK_ContactInfo]
    PRIMARY KEY CLUSTERED ([MemberEmail] ASC);
GO

-- Creating primary key on [EventAttendance_ID] in table 'EventAttendance'
ALTER TABLE [dbo].[EventAttendance]
ADD CONSTRAINT [PK_EventAttendance]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [HelpingActionID] in table 'HelpingAction'
ALTER TABLE [dbo].[HelpingAction]
ADD CONSTRAINT [PK_HelpingAction]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [LocationID] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [PK_Locations]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ContactInfoEmail] in table 'LogIn'
ALTER TABLE [dbo].[LogIn]
ADD CONSTRAINT [PK_LogIn]
    PRIMARY KEY CLUSTERED ([MemberEmail] ASC);
GO

-- Creating primary key on [MemberActionID] in table 'MemberAction'
ALTER TABLE [dbo].[MemberAction]
ADD CONSTRAINT [PK_MemberAction]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Member_ID] in table 'Members'
ALTER TABLE [dbo].[Volunteer]
ADD CONSTRAINT [PK_Volunteer]
    PRIMARY KEY CLUSTERED ([VolunteerEmail] ASC);
GO

-- Creating primary key on [PreferenceID] in table 'Preferences'
ALTER TABLE [dbo].[Preferences]
ADD CONSTRAINT [PK_Preferences]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [PreferredCategoryID] in table 'PreferredCategory'
ALTER TABLE [dbo].[PreferredCategory]
ADD CONSTRAINT [PK_PreferredCategory]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [MemberCharity_ID] in table 'Preferred_Charity'
ALTER TABLE [dbo].[Preferred_Charity]
ADD CONSTRAINT [PK_Preferred_Charity]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [CategoryID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Charity_ID] in table 'Charities'
ALTER TABLE [dbo].[Charities]
ADD CONSTRAINT [PK_Charities]
    PRIMARY KEY CLUSTERED ([CharityEmail] ASC);
GO

-- Creating primary key on [CharityEventID] in table 'CharityEvent'
ALTER TABLE [dbo].[CharityEvent]
ADD CONSTRAINT [PK_CharityEvent]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MemberActionActionID] in table 'MemberAction'
ALTER TABLE [dbo].[MemberAction]
ADD CONSTRAINT [FK__Member_Ac__Membe__38EE7070]
    FOREIGN KEY ([ActionID])
    REFERENCES [dbo].[HelpingAction]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Member_Ac__Membe__38EE7070'
CREATE INDEX [IX_FK__Member_Ac__Membe__38EE7070]
ON [dbo].[MemberAction]
    ([ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------