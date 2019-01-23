/*
	Creates or updates a Namegiver database.
*/

CREATE TABLE [dbo].[Name] (
    [Id]        INT     IDENTITY (1, 1) NOT NULL,
    [Gender]    TINYINT NULL,
    [SuperType] TINYINT NULL,
    CONSTRAINT [PK_NameId] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [dbo].[NameInfo] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [NameId]        INT           NOT NULL,
    [Name]          NVARCHAR (20) NOT NULL,
    [Accepted]      BIT           CONSTRAINT [DF_Name_Accepted] DEFAULT ((0)) NOT NULL,
    [RejectedCount] INT           CONSTRAINT [DF_Name_RejectCount] DEFAULT ((0)) NOT NULL,
    [Language]      NVARCHAR (5)  NULL,
    CONSTRAINT [PK_NameInfo] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_NameInfo_NameId] FOREIGN KEY ([NameId]) REFERENCES [dbo].[Name] ([Id]) ON DELETE CASCADE
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Name_Unique]
    ON [dbo].[NameInfo]([Name] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_NameInfo_NameId]
    ON [dbo].[NameInfo]([NameId] ASC);
GO

CREATE TABLE [dbo].[NameProperty] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [NameId] INT            NOT NULL,
    [Key]    NVARCHAR (10)  NULL,
    [Value]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_NameProperty] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_NameProperty_Name] FOREIGN KEY ([NameId]) REFERENCES [dbo].[Name] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_NameProperty_Name]
    ON [dbo].[NameProperty]([NameId] ASC);
GO