CREATE TABLE [dbo].[DonationImages]
(
[DonationImageId] [int] NOT NULL,
[ItemId] [uniqueidentifier] NOT NULL,
[ImageUrl] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOn] [datetime] NOT NULL,
[ModifiedOn] [datetime] NULL,
[CreatedBy] [int] NOT NULL,
[ModifiedBy] [int] NULL
)
GO
ALTER TABLE [dbo].[DonationImages] ADD CONSTRAINT [PK_DonationDetailsImages] PRIMARY KEY CLUSTERED  ([DonationImageId])
GO
ALTER TABLE [dbo].[DonationImages] WITH NOCHECK ADD CONSTRAINT [FK_DonationImages_DonationDetails_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[DonationImages] WITH NOCHECK ADD CONSTRAINT [FK_DonationImages_Users_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
