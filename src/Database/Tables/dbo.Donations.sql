CREATE TABLE [dbo].[Donations]
(
[DonationId] [int] NOT NULL,
[RequestedBy] [int] NULL,
[Title] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (180) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DonatedOn] [datetime] NOT NULL,
[LastUpdate] [datetime] NULL,
[ExpireOn] [date] NULL,
[DonationStatus] [smallint] NOT NULL,
[CreatedOn] [datetime] NOT NULL,
[ModifiedOn] [datetime] NULL,
[CreatedBy] [int] NOT NULL,
[ModifiedBy] [int] NULL,
[Amount] [decimal] (10, 2) NULL
)
GO
ALTER TABLE [dbo].[Donations] ADD CONSTRAINT [PK_Donations] PRIMARY KEY CLUSTERED  ([DonationId])
GO
ALTER TABLE [dbo].[Donations] WITH NOCHECK ADD CONSTRAINT [FK_Donations_CreatedBy_Users_UserId] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Donations] WITH NOCHECK ADD CONSTRAINT [FK_Donations_ModifiedBy_Users_UserId] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Donations] WITH NOCHECK ADD CONSTRAINT [FK_Donations_RequestedBy_Users_UserId] FOREIGN KEY ([RequestedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
