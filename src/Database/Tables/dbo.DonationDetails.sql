CREATE TABLE [dbo].[DonationDetails]
(
[ItemId] [uniqueidentifier] NOT NULL,
[Line] [int] NOT NULL,
[DonationId] [int] NOT NULL,
[ItemType] [smallint] NOT NULL,
[ItemName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RequestedBy] [int] NULL,
[DonationStatus] [smallint] NOT NULL,
[CreatedOn] [datetime] NOT NULL,
[ModifiedOn] [datetime] NULL,
[CreatedBy] [int] NOT NULL,
[ModifiedBy] [int] NULL
)
GO
ALTER TABLE [dbo].[DonationDetails] ADD CONSTRAINT [PK_DonationDetails] PRIMARY KEY CLUSTERED  ([ItemId])
GO
ALTER TABLE [dbo].[DonationDetails] WITH NOCHECK ADD CONSTRAINT [FK_DonationDetails_CreatedBy_Users_UserId] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[DonationDetails] WITH NOCHECK ADD CONSTRAINT [FK_DonationDetails_ModifiedBy_Users_UserId] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[DonationDetails] WITH NOCHECK ADD CONSTRAINT [FK_DonationDetails_RequestedBy_Users_UserId] FOREIGN KEY ([RequestedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
