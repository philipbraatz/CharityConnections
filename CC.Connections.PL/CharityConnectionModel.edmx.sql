
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/08/2019 07:23:54
-- Generated from EDMX file: C:\Users\philo\source\repos\CharityConnections\CC.Connections.PL\CharityConnectionModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [braatzdb];
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
IF OBJECT_ID(N'[dbo].[Members]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Members];
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

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Contact_Info'
CREATE TABLE [dbo].[Contact_Info] (
    [Contact_Info_ID] int  NOT NULL,
    [ContactInfo_FName] nvarchar(25)  NULL,
    [ContactInfo_LName] nvarchar(50)  NULL,
    [ContactInfo_Phone] nvarchar(12)  NULL,
    [ContactInfo_Email] nvarchar(50)  NULL,
    [DateOfBirth] datetime  NULL
);
GO

-- Creating table 'Event_Attendance'
CREATE TABLE [dbo].[Event_Attendance] (
    [EventAttendance_ID] int  NOT NULL,
    [Event_ID] int  NOT NULL,
    [Member_ID] int  NOT NULL,
    [Status] int  NULL
);
GO

-- Creating table 'Helping_Action'
CREATE TABLE [dbo].[Helping_Action] (
    [Helping_Action_ID] int  NOT NULL,
    [HelpingActionCategory_ID] int  NULL,
    [HelpingActionDescription] nvarchar(75)  NULL
);
GO

-- Creating table 'Locations'
CREATE TABLE [dbo].[Locations] (
    [Location_ID] int  NOT NULL,
    [ContactInfoAddress] nvarchar(50)  NULL,
    [ContactInfoCity] nvarchar(25)  NULL,
    [ContactInfoState] nvarchar(25)  NULL,
    [ContactInfoZip] nvarchar(10)  NULL
);
GO

-- Creating table 'Log_in'
CREATE TABLE [dbo].[Log_in] (
    [ContactInfoEmail] nvarchar(50)  NOT NULL,
    [MemberType] int  NULL,
    [LogInMember_ID] int  NULL,
    [LogInPassword] nvarchar(150)  NULL
);
GO

-- Creating table 'Member_Action'
CREATE TABLE [dbo].[Member_Action] (
    [MemberAction_ID] int  NOT NULL,
    [MemberActionMember_ID] int  NULL,
    [MemberActionAction_ID] int  NULL
);
GO

-- Creating table 'Members'
CREATE TABLE [dbo].[Members] (
    [Member_ID] int  NOT NULL,
    [MemberContact_ID] int  NULL,
    [MemberPreference_ID] int  NULL,
    [Location_ID] int  NULL
);
GO

-- Creating table 'Preferences'
CREATE TABLE [dbo].[Preferences] (
    [Preference_ID] int  NOT NULL,
    [Distance] decimal(18,2)  NULL
);
GO

-- Creating table 'Preferred_Category'
CREATE TABLE [dbo].[Preferred_Category] (
    [PreferredCategory_ID] int  NOT NULL,
    [MemberCat_Category_ID] int  NULL,
    [MemberCat_Member_ID] int  NULL
);
GO

-- Creating table 'Preferred_Charity'
CREATE TABLE [dbo].[Preferred_Charity] (
    [MemberCharity_ID] int  NOT NULL,
    [Member_ID] int  NULL,
    [Charity_ID] int  NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Category_ID] int  NOT NULL,
    [Category_Desc] nvarchar(70)  NULL,
    [Category_Color] nvarchar(6)  NULL,
    [Category_Image] nvarchar(20)  NULL
);
GO

-- Creating table 'Charities'
CREATE TABLE [dbo].[Charities] (
    [Charity_ID] int  NOT NULL,
    [Charity_Name] varchar(50)  NULL,
    [Charity_EIN] nvarchar(50)  NULL,
    [Charity_Deductibility] bit  NULL,
    [Charity_URL] nvarchar(75)  NULL,
    [Charity_Cause] nvarchar(255)  NULL,
    [Charity_Email] nvarchar(75)  NULL,
    [Charity_Category_ID] int  NULL,
    [Location_ID] int  NULL,
    [Charity_Requirements] nvarchar(50)  NULL
);
GO

-- Creating table 'Charity_Event'
CREATE TABLE [dbo].[Charity_Event] (
    [CharityEvent_ID] int  NOT NULL,
    [CharityEventName] nvarchar(75)  NULL,
    [CharityEventLocation_ID] int  NULL,
    [CharityEventCharity_ID] int  NULL,
    [CharityEventContactInfo_ID] int  NULL,
    [CharityEventStartDate] datetime  NULL,
    [CharityEventEndDate] datetime  NULL,
    [CharityEventRequirements] nvarchar(500)  NULL,
    [CharityEventDescription] nvarchar(1500)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Contact_Info_ID] in table 'Contact_Info'
ALTER TABLE [dbo].[Contact_Info]
ADD CONSTRAINT [PK_Contact_Info]
    PRIMARY KEY CLUSTERED ([Contact_Info_ID] ASC);
GO

-- Creating primary key on [EventAttendance_ID] in table 'Event_Attendance'
ALTER TABLE [dbo].[Event_Attendance]
ADD CONSTRAINT [PK_Event_Attendance]
    PRIMARY KEY CLUSTERED ([EventAttendance_ID] ASC);
GO

-- Creating primary key on [Helping_Action_ID] in table 'Helping_Action'
ALTER TABLE [dbo].[Helping_Action]
ADD CONSTRAINT [PK_Helping_Action]
    PRIMARY KEY CLUSTERED ([Helping_Action_ID] ASC);
GO

-- Creating primary key on [Location_ID] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [PK_Locations]
    PRIMARY KEY CLUSTERED ([Location_ID] ASC);
GO

-- Creating primary key on [ContactInfoEmail] in table 'Log_in'
ALTER TABLE [dbo].[Log_in]
ADD CONSTRAINT [PK_Log_in]
    PRIMARY KEY CLUSTERED ([ContactInfoEmail] ASC);
GO

-- Creating primary key on [MemberAction_ID] in table 'Member_Action'
ALTER TABLE [dbo].[Member_Action]
ADD CONSTRAINT [PK_Member_Action]
    PRIMARY KEY CLUSTERED ([MemberAction_ID] ASC);
GO

-- Creating primary key on [Member_ID] in table 'Members'
ALTER TABLE [dbo].[Members]
ADD CONSTRAINT [PK_Members]
    PRIMARY KEY CLUSTERED ([Member_ID] ASC);
GO

-- Creating primary key on [Preference_ID] in table 'Preferences'
ALTER TABLE [dbo].[Preferences]
ADD CONSTRAINT [PK_Preferences]
    PRIMARY KEY CLUSTERED ([Preference_ID] ASC);
GO

-- Creating primary key on [PreferredCategory_ID] in table 'Preferred_Category'
ALTER TABLE [dbo].[Preferred_Category]
ADD CONSTRAINT [PK_Preferred_Category]
    PRIMARY KEY CLUSTERED ([PreferredCategory_ID] ASC);
GO

-- Creating primary key on [MemberCharity_ID] in table 'Preferred_Charity'
ALTER TABLE [dbo].[Preferred_Charity]
ADD CONSTRAINT [PK_Preferred_Charity]
    PRIMARY KEY CLUSTERED ([MemberCharity_ID] ASC);
GO

-- Creating primary key on [Category_ID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Category_ID] ASC);
GO

-- Creating primary key on [Charity_ID] in table 'Charities'
ALTER TABLE [dbo].[Charities]
ADD CONSTRAINT [PK_Charities]
    PRIMARY KEY CLUSTERED ([Charity_ID] ASC);
GO

-- Creating primary key on [CharityEvent_ID] in table 'Charity_Event'
ALTER TABLE [dbo].[Charity_Event]
ADD CONSTRAINT [PK_Charity_Event]
    PRIMARY KEY CLUSTERED ([CharityEvent_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MemberActionAction_ID] in table 'Member_Action'
ALTER TABLE [dbo].[Member_Action]
ADD CONSTRAINT [FK__Member_Ac__Membe__38EE7070]
    FOREIGN KEY ([MemberActionAction_ID])
    REFERENCES [dbo].[Helping_Action]
        ([Helping_Action_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Member_Ac__Membe__38EE7070'
CREATE INDEX [IX_FK__Member_Ac__Membe__38EE7070]
ON [dbo].[Member_Action]
    ([MemberActionAction_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------