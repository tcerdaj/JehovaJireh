CREATE TABLE [dbo].[Users]
(
[UserId] [int] NOT NULL,
[UserName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FirstName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LastName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Email] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PasswordHash] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SecurityStamp] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ImageUrl] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[City] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[State] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Zip] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Phone] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Gender] [nvarchar] (6) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ConfirmationToken] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IsConfirmed] [bit] NULL,
[IsChurchMember] [bit] NULL,
[Active] [bit] NULL,
[LockoutEnabled] [bit] NULL,
[TwoFactorEnabled] [bit] NULL,
[FailedCount] [int] NULL,
[ChurchName] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ChurchAddress] [nvarchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ChurchPhone] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ChurchPastor] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NeedToBeVisited] [bit] NULL,
[Comments] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOn] [datetime] NULL,
[ModifiedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[ModifiedBy] [int] NULL
)
GO
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED  ([UserId])
GO
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [UQ_Users_Email] UNIQUE NONCLUSTERED  ([Email])
GO
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [UQ_Users_UserName] UNIQUE NONCLUSTERED  ([UserName])
GO
ALTER TABLE [dbo].[Users] WITH NOCHECK ADD CONSTRAINT [FK_Users_Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Users] WITH NOCHECK ADD CONSTRAINT [FK_Users_Users_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
