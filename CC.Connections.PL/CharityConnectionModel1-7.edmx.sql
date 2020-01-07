
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/07/2020 08:45:26
-- Generated from EDMX file: D:\source\repos\CharityConnections\CC.Connections.PL\CharityConnectionModel.edmx
-- --------------------------------------------------

--Create Database dbCharityConnections;

--SET QUOTED_IDENTIFIER OFF;
--GO
--USE [dbCharityConnections];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Member_Ac__Membe__38EE7070]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Member_Action] DROP CONSTRAINT [FK__Member_Ac__Membe__38EE7070];
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
IF OBJECT_ID(N'[dbo].[Charity_Event]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Charity_Event];
GO
IF OBJECT_ID(N'[dbo].[Contact_Info]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contact_Info];
GO
IF OBJECT_ID(N'[dbo].[Event_Attendance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Event_Attendance];
GO
IF OBJECT_ID(N'[dbo].[Helping_Action]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Helping_Action];
GO
IF OBJECT_ID(N'[dbo].[Locations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Locations];
GO
IF OBJECT_ID(N'[dbo].[Log_in]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Log_in];
GO
IF OBJECT_ID(N'[dbo].[Member_Action]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Member_Action];
GO
IF OBJECT_ID(N'[dbo].[Preferences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Preferences];
GO
IF OBJECT_ID(N'[dbo].[Preferred_Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Preferred_Category];
GO
IF OBJECT_ID(N'[dbo].[Preferred_Charity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Preferred_Charity];
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

-- Creating table 'Charities'
CREATE TABLE [dbo].[Charities] (
    [Charity_Email] nvarchar(75)  NOT NULL,
    [Name] varchar(50)  NULL,
    [EIN] nvarchar(50)  NULL,
    [Deductibility] bit  NULL,
    [URL] nvarchar(75)  NULL,
    [Cause] nvarchar(500)  NULL,
    [Category_ID] uniqueidentifier  NULL,
    [Location_ID] uniqueidentifier  NULL
);
GO

-- Creating table 'Contact_Info'
CREATE TABLE [dbo].[Contact_Info] (
    [Member_Email] nvarchar(75)  NOT NULL,
    [FName] nvarchar(25)  NULL,
    [LName] nvarchar(50)  NULL,
    [Phone] nvarchar(12)  NULL,
    [DateOfBirth] datetime  NULL
);
GO

-- Creating table 'Event_Attendance'
CREATE TABLE [dbo].[Event_Attendance] (
    [ID] uniqueidentifier  NOT NULL,
    [Event_ID] uniqueidentifier  NOT NULL,
    [Volunteer_Email] nvarchar(75)  NOT NULL,
    [Volunteer_Status] int  NULL
);
GO

-- Creating table 'Helping_Action'
CREATE TABLE [dbo].[Helping_Action] (
    [ID] uniqueidentifier  NOT NULL,
    [Category_ID] uniqueidentifier  NULL,
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

-- Creating table 'Log_in'
CREATE TABLE [dbo].[Log_in] (
    [Member_Email] nvarchar(75)  NOT NULL,
    [MemberType] int  NULL,
    [Password] nvarchar(150)  NULL
);
GO

-- Creating table 'Member_Action'
CREATE TABLE [dbo].[Member_Action] (
    [ID] uniqueidentifier  NOT NULL,
    [Member_Email] nvarchar(75)  NULL,
    [Action_ID] uniqueidentifier  NULL
);
GO

-- Creating table 'Preferences'
CREATE TABLE [dbo].[Preferences] (
    [ID] uniqueidentifier  NOT NULL,
    [Distance] decimal(18,2)  NULL
);
GO

-- Creating table 'Preferred_Category'
CREATE TABLE [dbo].[Preferred_Category] (
    [ID] uniqueidentifier  NOT NULL,
    [Category_ID] uniqueidentifier  NULL,
    [Volunteer_Email] nvarchar(75)  NULL
);
GO

-- Creating table 'Preferred_Charity'
CREATE TABLE [dbo].[Preferred_Charity] (
    [ID] uniqueidentifier  NOT NULL,
    [Volunteer_Email] nvarchar(75)  NULL,
    [Charity_Email] nvarchar(75)  NULL
);
GO

-- Creating table 'Volunteers'
CREATE TABLE [dbo].[Volunteers] (
    [Volunteer_Email] nvarchar(75)  NOT NULL,
    [Preference_ID] uniqueidentifier  NULL,
    [Location_ID] uniqueidentifier  NULL
);
GO

-- Creating table 'Charity_Event'
CREATE TABLE [dbo].[Charity_Event] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(75)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Requirements] nvarchar(500)  NULL,
    [Description] nvarchar(1500)  NULL,
    [Location_ID] uniqueidentifier  NULL,
    [Charity_Email] nvarchar(75)  NULL
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

-- Creating primary key on [Charity_Email] in table 'Charities'
ALTER TABLE [dbo].[Charities]
ADD CONSTRAINT [PK_Charities]
    PRIMARY KEY CLUSTERED ([Charity_Email] ASC);
GO

-- Creating primary key on [Member_Email] in table 'Contact_Info'
ALTER TABLE [dbo].[Contact_Info]
ADD CONSTRAINT [PK_Contact_Info]
    PRIMARY KEY CLUSTERED ([Member_Email] ASC);
GO

-- Creating primary key on [ID] in table 'Event_Attendance'
ALTER TABLE [dbo].[Event_Attendance]
ADD CONSTRAINT [PK_Event_Attendance]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Helping_Action'
ALTER TABLE [dbo].[Helping_Action]
ADD CONSTRAINT [PK_Helping_Action]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [PK_Locations]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Member_Email] in table 'Log_in'
ALTER TABLE [dbo].[Log_in]
ADD CONSTRAINT [PK_Log_in]
    PRIMARY KEY CLUSTERED ([Member_Email] ASC);
GO

-- Creating primary key on [ID] in table 'Member_Action'
ALTER TABLE [dbo].[Member_Action]
ADD CONSTRAINT [PK_Member_Action]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Preferences'
ALTER TABLE [dbo].[Preferences]
ADD CONSTRAINT [PK_Preferences]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Preferred_Category'
ALTER TABLE [dbo].[Preferred_Category]
ADD CONSTRAINT [PK_Preferred_Category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Preferred_Charity'
ALTER TABLE [dbo].[Preferred_Charity]
ADD CONSTRAINT [PK_Preferred_Charity]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Volunteer_Email] in table 'Volunteers'
ALTER TABLE [dbo].[Volunteers]
ADD CONSTRAINT [PK_Volunteers]
    PRIMARY KEY CLUSTERED ([Volunteer_Email] ASC);
GO

-- Creating primary key on [ID] in table 'Charity_Event'
ALTER TABLE [dbo].[Charity_Event]
ADD CONSTRAINT [PK_Charity_Event]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Action_ID] in table 'Member_Action'
ALTER TABLE [dbo].[Member_Action]
ADD CONSTRAINT [FK__Member_Ac__Membe__38EE7070]
    FOREIGN KEY ([Action_ID])
    REFERENCES [dbo].[Helping_Action]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Member_Ac__Membe__38EE7070'
CREATE INDEX [IX_FK__Member_Ac__Membe__38EE7070]
ON [dbo].[Member_Action]
    ([Action_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------