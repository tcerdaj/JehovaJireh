CREATE TABLE [dbo].[Scheduler]
(
[SchedulerId] [int] NOT NULL,
[ItemId] [uniqueidentifier] NOT NULL,
[ItemName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Title] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ImageUrl] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [nvarchar] (180) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StartDate] [datetime] NOT NULL,
[EndDate] [datetime] NOT NULL,
[RecurrenceRule] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOn] [datetime] NOT NULL,
[ModifiedOn] [datetime] NULL,
[CreatedBy] [int] NOT NULL,
[ModifiedBy] [int] NULL
)
GO
ALTER TABLE [dbo].[Scheduler] ADD CONSTRAINT [PK_Scheduler] PRIMARY KEY CLUSTERED  ([SchedulerId])
GO
ALTER TABLE [dbo].[Scheduler] WITH NOCHECK ADD CONSTRAINT [FK_Scheduler_CreatedBy_Users_UserId] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Scheduler] WITH NOCHECK ADD CONSTRAINT [FK_Scheduler_ModifiedBy_Users_UserId] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
