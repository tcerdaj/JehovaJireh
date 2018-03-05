CREATE TABLE [dbo].[Request]
(
[RequestId] [int] NOT NULL,
[ItemType] [smallint] NOT NULL,
[ItemName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ImageUrl] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Title] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [nvarchar] (180) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RequestStatus] [smallint] NOT NULL,
[CreatedOn] [datetime] NOT NULL,
[ModifiedOn] [datetime] NULL,
[CreatedBy] [int] NOT NULL,
[ModifiedBy] [int] NULL
)
GO
ALTER TABLE [dbo].[Request] ADD CONSTRAINT [PK_Request] PRIMARY KEY CLUSTERED  ([RequestId])
GO
ALTER TABLE [dbo].[Request] WITH NOCHECK ADD CONSTRAINT [FK_Request_CreatedBy_Users_UserId] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Request] WITH NOCHECK ADD CONSTRAINT [FK_Request_ModifiedBy_Users_UserId] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
