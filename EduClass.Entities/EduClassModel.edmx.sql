
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/25/2015 21:18:38
-- Generated from EDMX file: D:\code\Edu.Class\EduClass.Entities\EduClassModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [dbEduclass];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BoardPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posts] DROP CONSTRAINT [FK_BoardPost];
GO
IF OBJECT_ID(N'[dbo].[FK_CalendarEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Events] DROP CONSTRAINT [FK_CalendarEvent];
GO
IF OBJECT_ID(N'[dbo].[FK_EventEventType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Events] DROP CONSTRAINT [FK_EventEventType];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupBoard]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Groups] DROP CONSTRAINT [FK_GroupBoard];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupCalendar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Calendars] DROP CONSTRAINT [FK_GroupCalendar];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupKey]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Keys] DROP CONSTRAINT [FK_GroupKey];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupPage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pages] DROP CONSTRAINT [FK_GroupPage];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupStudent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_Student] DROP CONSTRAINT [FK_GroupStudent];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupTeacher]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Groups] DROP CONSTRAINT [FK_GroupTeacher];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupTest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tests] DROP CONSTRAINT [FK_GroupTest];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonAlert]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Alerts] DROP CONSTRAINT [FK_PersonAlert];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonAvatar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Avatars] DROP CONSTRAINT [FK_PersonAvatar];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonMail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Mails] DROP CONSTRAINT [FK_PersonMail];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posts] DROP CONSTRAINT [FK_PersonPost];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonReplay]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Replies] DROP CONSTRAINT [FK_PersonReplay];
GO
IF OBJECT_ID(N'[dbo].[FK_PostPostType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posts] DROP CONSTRAINT [FK_PostPostType];
GO
IF OBJECT_ID(N'[dbo].[FK_PostReplay]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Replies] DROP CONSTRAINT [FK_PostReplay];
GO
IF OBJECT_ID(N'[dbo].[FK_QuestionOptionResponse]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Responses] DROP CONSTRAINT [FK_QuestionOptionResponse];
GO
IF OBJECT_ID(N'[dbo].[FK_QuestionQuestionOption]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QuestionOptions] DROP CONSTRAINT [FK_QuestionQuestionOption];
GO
IF OBJECT_ID(N'[dbo].[FK_QuestionQuestionType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Questions] DROP CONSTRAINT [FK_QuestionQuestionType];
GO
IF OBJECT_ID(N'[dbo].[FK_QuestionResponse]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Questions] DROP CONSTRAINT [FK_QuestionResponse];
GO
IF OBJECT_ID(N'[dbo].[FK_Student_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_Student] DROP CONSTRAINT [FK_Student_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_Teacher_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_Teacher] DROP CONSTRAINT [FK_Teacher_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_TestQuestion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Questions] DROP CONSTRAINT [FK_TestQuestion];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Alerts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Alerts];
GO
IF OBJECT_ID(N'[dbo].[Avatars]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Avatars];
GO
IF OBJECT_ID(N'[dbo].[Boards]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Boards];
GO
IF OBJECT_ID(N'[dbo].[Calendars]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Calendars];
GO
IF OBJECT_ID(N'[dbo].[Events]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Events];
GO
IF OBJECT_ID(N'[dbo].[EventTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventTypes];
GO
IF OBJECT_ID(N'[dbo].[Groups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Groups];
GO
IF OBJECT_ID(N'[dbo].[Keys]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Keys];
GO
IF OBJECT_ID(N'[dbo].[Mails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mails];
GO
IF OBJECT_ID(N'[dbo].[Pages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pages];
GO
IF OBJECT_ID(N'[dbo].[Person]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person];
GO
IF OBJECT_ID(N'[dbo].[Person_Student]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person_Student];
GO
IF OBJECT_ID(N'[dbo].[Person_Teacher]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person_Teacher];
GO
IF OBJECT_ID(N'[dbo].[Posts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Posts];
GO
IF OBJECT_ID(N'[dbo].[PostTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostTypes];
GO
IF OBJECT_ID(N'[dbo].[QuestionOptions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuestionOptions];
GO
IF OBJECT_ID(N'[dbo].[Questions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Questions];
GO
IF OBJECT_ID(N'[dbo].[QuestionTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuestionTypes];
GO
IF OBJECT_ID(N'[dbo].[Replies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Replies];
GO
IF OBJECT_ID(N'[dbo].[Responses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Responses];
GO
IF OBJECT_ID(N'[dbo].[Tests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tests];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Person'
CREATE TABLE [dbo].[Person] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(50)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [FirstName] nvarchar(75)  NOT NULL,
    [LastName] nvarchar(75)  NOT NULL,
    [Birthday] datetime  NULL,
    [Email] nvarchar(250)  NOT NULL,
    [IdentificationCard] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [Enabled] bit  NOT NULL
);
GO

-- Creating table 'Boards'
CREATE TABLE [dbo].[Boards] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [Enabled] bit  NOT NULL
);
GO

-- Creating table 'Posts'
CREATE TABLE [dbo].[Posts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(250)  NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [PersonId] int  NOT NULL,
    [BoardId] int  NOT NULL,
    [Enabled] bit  NOT NULL,
    [PostType_Id] int  NOT NULL
);
GO

-- Creating table 'Groups'
CREATE TABLE [dbo].[Groups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [Enabled] bit  NOT NULL,
    [Teacher_Id] int  NOT NULL,
    [Board_Id] int  NOT NULL
);
GO

-- Creating table 'Tests'
CREATE TABLE [dbo].[Tests] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [GroupId] int  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Questions'
CREATE TABLE [dbo].[Questions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [TestId] int  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [QuestionType_Id] int  NOT NULL,
    [Response_Id] int  NOT NULL
);
GO

-- Creating table 'QuestionOptions'
CREATE TABLE [dbo].[QuestionOptions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [QuestionId] int  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'QuestionTypes'
CREATE TABLE [dbo].[QuestionTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdateAt] datetime  NULL,
    [Enabled] bit  NOT NULL
);
GO

-- Creating table 'Responses'
CREATE TABLE [dbo].[Responses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [QuestionOption_Id] int  NOT NULL
);
GO

-- Creating table 'Keys'
CREATE TABLE [dbo].[Keys] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HashKey] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [GroupId] int  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Calendars'
CREATE TABLE [dbo].[Calendars] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(500)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [Enabled] bit  NOT NULL,
    [Group_Id] int  NOT NULL
);
GO

-- Creating table 'Events'
CREATE TABLE [dbo].[Events] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdateAt] datetime  NULL,
    [CalendarId] int  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [EventType_Id] int  NOT NULL
);
GO

-- Creating table 'EventTypes'
CREATE TABLE [dbo].[EventTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [Enabled] bit  NOT NULL
);
GO

-- Creating table 'Pages'
CREATE TABLE [dbo].[Pages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [GroupId] int  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Alerts'
CREATE TABLE [dbo].[Alerts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(250)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [ReadAt] datetime  NULL,
    [PersonId] int  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Mails'
CREATE TABLE [dbo].[Mails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Subject] nvarchar(250)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [ReadAt] datetime  NULL,
    [PersonId] int  NOT NULL,
    [Enabled] bit  NOT NULL
);
GO

-- Creating table 'Avatars'
CREATE TABLE [dbo].[Avatars] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UrlPhoto] nvarchar(max)  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [Enabled] bit  NOT NULL,
    [Person_Id] int  NOT NULL
);
GO

-- Creating table 'PostTypes'
CREATE TABLE [dbo].[PostTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [Enabled] bit  NOT NULL
);
GO

-- Creating table 'Replies'
CREATE TABLE [dbo].[Replies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(250)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NULL,
    [PersonId] int  NOT NULL,
    [PostId] int  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Person_Teacher'
CREATE TABLE [dbo].[Person_Teacher] (
    [GroupId] int  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Person_Student'
CREATE TABLE [dbo].[Person_Student] (
    [GroupId] int  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'GroupStudent'
CREATE TABLE [dbo].[GroupStudent] (
    [Groups_Id] int  NOT NULL,
    [Students_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Person'
ALTER TABLE [dbo].[Person]
ADD CONSTRAINT [PK_Person]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Boards'
ALTER TABLE [dbo].[Boards]
ADD CONSTRAINT [PK_Boards]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [PK_Posts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [PK_Groups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tests'
ALTER TABLE [dbo].[Tests]
ADD CONSTRAINT [PK_Tests]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [PK_Questions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'QuestionOptions'
ALTER TABLE [dbo].[QuestionOptions]
ADD CONSTRAINT [PK_QuestionOptions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'QuestionTypes'
ALTER TABLE [dbo].[QuestionTypes]
ADD CONSTRAINT [PK_QuestionTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Responses'
ALTER TABLE [dbo].[Responses]
ADD CONSTRAINT [PK_Responses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Keys'
ALTER TABLE [dbo].[Keys]
ADD CONSTRAINT [PK_Keys]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Calendars'
ALTER TABLE [dbo].[Calendars]
ADD CONSTRAINT [PK_Calendars]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [PK_Events]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EventTypes'
ALTER TABLE [dbo].[EventTypes]
ADD CONSTRAINT [PK_EventTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pages'
ALTER TABLE [dbo].[Pages]
ADD CONSTRAINT [PK_Pages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Alerts'
ALTER TABLE [dbo].[Alerts]
ADD CONSTRAINT [PK_Alerts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Mails'
ALTER TABLE [dbo].[Mails]
ADD CONSTRAINT [PK_Mails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Avatars'
ALTER TABLE [dbo].[Avatars]
ADD CONSTRAINT [PK_Avatars]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PostTypes'
ALTER TABLE [dbo].[PostTypes]
ADD CONSTRAINT [PK_PostTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Replies'
ALTER TABLE [dbo].[Replies]
ADD CONSTRAINT [PK_Replies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Person_Teacher'
ALTER TABLE [dbo].[Person_Teacher]
ADD CONSTRAINT [PK_Person_Teacher]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Person_Student'
ALTER TABLE [dbo].[Person_Student]
ADD CONSTRAINT [PK_Person_Student]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Groups_Id], [Students_Id] in table 'GroupStudent'
ALTER TABLE [dbo].[GroupStudent]
ADD CONSTRAINT [PK_GroupStudent]
    PRIMARY KEY CLUSTERED ([Groups_Id], [Students_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PersonId] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [FK_PersonPost]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonPost'
CREATE INDEX [IX_FK_PersonPost]
ON [dbo].[Posts]
    ([PersonId]);
GO

-- Creating foreign key on [BoardId] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [FK_BoardPost]
    FOREIGN KEY ([BoardId])
    REFERENCES [dbo].[Boards]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BoardPost'
CREATE INDEX [IX_FK_BoardPost]
ON [dbo].[Posts]
    ([BoardId]);
GO

-- Creating foreign key on [PostType_Id] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [FK_PostPostType]
    FOREIGN KEY ([PostType_Id])
    REFERENCES [dbo].[PostTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PostPostType'
CREATE INDEX [IX_FK_PostPostType]
ON [dbo].[Posts]
    ([PostType_Id]);
GO

-- Creating foreign key on [PersonId] in table 'Replies'
ALTER TABLE [dbo].[Replies]
ADD CONSTRAINT [FK_PersonReplay]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonReplay'
CREATE INDEX [IX_FK_PersonReplay]
ON [dbo].[Replies]
    ([PersonId]);
GO

-- Creating foreign key on [PostId] in table 'Replies'
ALTER TABLE [dbo].[Replies]
ADD CONSTRAINT [FK_PostReplay]
    FOREIGN KEY ([PostId])
    REFERENCES [dbo].[Posts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PostReplay'
CREATE INDEX [IX_FK_PostReplay]
ON [dbo].[Replies]
    ([PostId]);
GO

-- Creating foreign key on [Person_Id] in table 'Avatars'
ALTER TABLE [dbo].[Avatars]
ADD CONSTRAINT [FK_PersonAvatar]
    FOREIGN KEY ([Person_Id])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonAvatar'
CREATE INDEX [IX_FK_PersonAvatar]
ON [dbo].[Avatars]
    ([Person_Id]);
GO

-- Creating foreign key on [PersonId] in table 'Mails'
ALTER TABLE [dbo].[Mails]
ADD CONSTRAINT [FK_PersonMail]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonMail'
CREATE INDEX [IX_FK_PersonMail]
ON [dbo].[Mails]
    ([PersonId]);
GO

-- Creating foreign key on [PersonId] in table 'Alerts'
ALTER TABLE [dbo].[Alerts]
ADD CONSTRAINT [FK_PersonAlert]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonAlert'
CREATE INDEX [IX_FK_PersonAlert]
ON [dbo].[Alerts]
    ([PersonId]);
GO

-- Creating foreign key on [Group_Id] in table 'Calendars'
ALTER TABLE [dbo].[Calendars]
ADD CONSTRAINT [FK_GroupCalendar]
    FOREIGN KEY ([Group_Id])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupCalendar'
CREATE INDEX [IX_FK_GroupCalendar]
ON [dbo].[Calendars]
    ([Group_Id]);
GO

-- Creating foreign key on [CalendarId] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [FK_CalendarEvent]
    FOREIGN KEY ([CalendarId])
    REFERENCES [dbo].[Calendars]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CalendarEvent'
CREATE INDEX [IX_FK_CalendarEvent]
ON [dbo].[Events]
    ([CalendarId]);
GO

-- Creating foreign key on [EventType_Id] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [FK_EventEventType]
    FOREIGN KEY ([EventType_Id])
    REFERENCES [dbo].[EventTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventEventType'
CREATE INDEX [IX_FK_EventEventType]
ON [dbo].[Events]
    ([EventType_Id]);
GO

-- Creating foreign key on [Teacher_Id] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [FK_GroupTeacher]
    FOREIGN KEY ([Teacher_Id])
    REFERENCES [dbo].[Person_Teacher]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupTeacher'
CREATE INDEX [IX_FK_GroupTeacher]
ON [dbo].[Groups]
    ([Teacher_Id]);
GO

-- Creating foreign key on [GroupId] in table 'Pages'
ALTER TABLE [dbo].[Pages]
ADD CONSTRAINT [FK_GroupPage]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupPage'
CREATE INDEX [IX_FK_GroupPage]
ON [dbo].[Pages]
    ([GroupId]);
GO

-- Creating foreign key on [Board_Id] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [FK_GroupBoard]
    FOREIGN KEY ([Board_Id])
    REFERENCES [dbo].[Boards]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupBoard'
CREATE INDEX [IX_FK_GroupBoard]
ON [dbo].[Groups]
    ([Board_Id]);
GO

-- Creating foreign key on [GroupId] in table 'Keys'
ALTER TABLE [dbo].[Keys]
ADD CONSTRAINT [FK_GroupKey]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupKey'
CREATE INDEX [IX_FK_GroupKey]
ON [dbo].[Keys]
    ([GroupId]);
GO

-- Creating foreign key on [GroupId] in table 'Tests'
ALTER TABLE [dbo].[Tests]
ADD CONSTRAINT [FK_GroupTest]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupTest'
CREATE INDEX [IX_FK_GroupTest]
ON [dbo].[Tests]
    ([GroupId]);
GO

-- Creating foreign key on [TestId] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [FK_TestQuestion]
    FOREIGN KEY ([TestId])
    REFERENCES [dbo].[Tests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestQuestion'
CREATE INDEX [IX_FK_TestQuestion]
ON [dbo].[Questions]
    ([TestId]);
GO

-- Creating foreign key on [QuestionType_Id] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [FK_QuestionQuestionType]
    FOREIGN KEY ([QuestionType_Id])
    REFERENCES [dbo].[QuestionTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuestionQuestionType'
CREATE INDEX [IX_FK_QuestionQuestionType]
ON [dbo].[Questions]
    ([QuestionType_Id]);
GO

-- Creating foreign key on [Response_Id] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [FK_QuestionResponse]
    FOREIGN KEY ([Response_Id])
    REFERENCES [dbo].[Responses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuestionResponse'
CREATE INDEX [IX_FK_QuestionResponse]
ON [dbo].[Questions]
    ([Response_Id]);
GO

-- Creating foreign key on [QuestionId] in table 'QuestionOptions'
ALTER TABLE [dbo].[QuestionOptions]
ADD CONSTRAINT [FK_QuestionQuestionOption]
    FOREIGN KEY ([QuestionId])
    REFERENCES [dbo].[Questions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuestionQuestionOption'
CREATE INDEX [IX_FK_QuestionQuestionOption]
ON [dbo].[QuestionOptions]
    ([QuestionId]);
GO

-- Creating foreign key on [QuestionOption_Id] in table 'Responses'
ALTER TABLE [dbo].[Responses]
ADD CONSTRAINT [FK_QuestionOptionResponse]
    FOREIGN KEY ([QuestionOption_Id])
    REFERENCES [dbo].[QuestionOptions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuestionOptionResponse'
CREATE INDEX [IX_FK_QuestionOptionResponse]
ON [dbo].[Responses]
    ([QuestionOption_Id]);
GO

-- Creating foreign key on [Groups_Id] in table 'GroupStudent'
ALTER TABLE [dbo].[GroupStudent]
ADD CONSTRAINT [FK_GroupStudent_Group]
    FOREIGN KEY ([Groups_Id])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Students_Id] in table 'GroupStudent'
ALTER TABLE [dbo].[GroupStudent]
ADD CONSTRAINT [FK_GroupStudent_Student]
    FOREIGN KEY ([Students_Id])
    REFERENCES [dbo].[Person_Student]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupStudent_Student'
CREATE INDEX [IX_FK_GroupStudent_Student]
ON [dbo].[GroupStudent]
    ([Students_Id]);
GO

-- Creating foreign key on [Id] in table 'Person_Teacher'
ALTER TABLE [dbo].[Person_Teacher]
ADD CONSTRAINT [FK_Teacher_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Person_Student'
ALTER TABLE [dbo].[Person_Student]
ADD CONSTRAINT [FK_Student_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------