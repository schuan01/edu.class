
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/27/2015 13:06:16
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


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Person'
CREATE TABLE [dbo].[Person] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Birthday] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [IdentificationCard] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Boards'
CREATE TABLE [dbo].[Boards] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Posts'
CREATE TABLE [dbo].[Posts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [PersonId] int  NOT NULL,
    [BoardId] int  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [PostType_Id] int  NOT NULL
);
GO

-- Creating table 'Groups'
CREATE TABLE [dbo].[Groups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [Teacher_Id] int  NOT NULL,
    [Board_Id] int  NOT NULL
);
GO

-- Creating table 'Tests'
CREATE TABLE [dbo].[Tests] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [StartDate] nvarchar(max)  NOT NULL,
    [EndDate] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [GroupId] int  NOT NULL
);
GO

-- Creating table 'Questions'
CREATE TABLE [dbo].[Questions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [TestId] int  NOT NULL,
    [QuestionType_Id] int  NOT NULL,
    [Response_Id] int  NOT NULL
);
GO

-- Creating table 'QuestionOptions'
CREATE TABLE [dbo].[QuestionOptions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [QuestionId] int  NOT NULL
);
GO

-- Creating table 'QuestionTypes'
CREATE TABLE [dbo].[QuestionTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdateAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Responses'
CREATE TABLE [dbo].[Responses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [QuestionOption_Id] int  NOT NULL
);
GO

-- Creating table 'Keys'
CREATE TABLE [dbo].[Keys] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HashKey] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [GroupId] int  NOT NULL
);
GO

-- Creating table 'Calendars'
CREATE TABLE [dbo].[Calendars] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [Group_Id] int  NOT NULL
);
GO

-- Creating table 'Events'
CREATE TABLE [dbo].[Events] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Date] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdateAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [CalendarId] int  NOT NULL,
    [EventType_Id] int  NOT NULL
);
GO

-- Creating table 'EventTypes'
CREATE TABLE [dbo].[EventTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Pages'
CREATE TABLE [dbo].[Pages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [GroupId] int  NOT NULL
);
GO

-- Creating table 'Alerts'
CREATE TABLE [dbo].[Alerts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [ReadAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [PersonId] int  NOT NULL
);
GO

-- Creating table 'Mails'
CREATE TABLE [dbo].[Mails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Subject] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreateAt] nvarchar(max)  NOT NULL,
    [ReadAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [PersonId] int  NOT NULL
);
GO

-- Creating table 'Avatars'
CREATE TABLE [dbo].[Avatars] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UrlPhoto] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [Person_Id] int  NOT NULL
);
GO

-- Creating table 'PostTypes'
CREATE TABLE [dbo].[PostTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Replays'
CREATE TABLE [dbo].[Replays] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedAt] nvarchar(max)  NOT NULL,
    [UpdatedAt] nvarchar(max)  NOT NULL,
    [Enabled] nvarchar(max)  NOT NULL,
    [PersonId] int  NOT NULL,
    [PostId] int  NOT NULL
);
GO

-- Creating table 'Person_Student'
CREATE TABLE [dbo].[Person_Student] (
    [GroupId] int  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Person_Teacher'
CREATE TABLE [dbo].[Person_Teacher] (
    [GroupId] int  NOT NULL,
    [Id] int  NOT NULL
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

-- Creating primary key on [Id] in table 'Replays'
ALTER TABLE [dbo].[Replays]
ADD CONSTRAINT [PK_Replays]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Person_Student'
ALTER TABLE [dbo].[Person_Student]
ADD CONSTRAINT [PK_Person_Student]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Person_Teacher'
ALTER TABLE [dbo].[Person_Teacher]
ADD CONSTRAINT [PK_Person_Teacher]
    PRIMARY KEY CLUSTERED ([Id] ASC);
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

-- Creating foreign key on [PersonId] in table 'Replays'
ALTER TABLE [dbo].[Replays]
ADD CONSTRAINT [FK_PersonReplay]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonReplay'
CREATE INDEX [IX_FK_PersonReplay]
ON [dbo].[Replays]
    ([PersonId]);
GO

-- Creating foreign key on [PostId] in table 'Replays'
ALTER TABLE [dbo].[Replays]
ADD CONSTRAINT [FK_PostReplay]
    FOREIGN KEY ([PostId])
    REFERENCES [dbo].[Posts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PostReplay'
CREATE INDEX [IX_FK_PostReplay]
ON [dbo].[Replays]
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

-- Creating foreign key on [GroupId] in table 'Person_Student'
ALTER TABLE [dbo].[Person_Student]
ADD CONSTRAINT [FK_GroupStudent]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupStudent'
CREATE INDEX [IX_FK_GroupStudent]
ON [dbo].[Person_Student]
    ([GroupId]);
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

-- Creating foreign key on [Id] in table 'Person_Student'
ALTER TABLE [dbo].[Person_Student]
ADD CONSTRAINT [FK_Student_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Person_Teacher'
ALTER TABLE [dbo].[Person_Teacher]
ADD CONSTRAINT [FK_Teacher_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------