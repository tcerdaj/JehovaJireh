USE [jehovajirehDB]
GO
/****** Object:  User [IIS APPPOOL\DefaultAppPool]    Script Date: 1/31/2018 9:39:00 PM ******/
CREATE USER [IIS APPPOOL\DefaultAppPool] FOR LOGIN [IIS APPPOOL\DefaultAppPool] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [IIS APPPOOL\DefaultAppPool]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [IIS APPPOOL\DefaultAppPool]
GO
/****** Object:  Table [dbo].[DonationDetailImages]    Script Date: 1/31/2018 9:39:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonationDetailImages](
	[DonationImageId] [int] NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_DonationDetailsImages] PRIMARY KEY CLUSTERED 
(
	[DonationImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DonationDetails]    Script Date: 1/31/2018 9:39:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonationDetails](
	[ItemId] [uniqueidentifier] NOT NULL,
	[Line] [int] NOT NULL,
	[DonationId] [int] NOT NULL,
	[ItemType] [smallint] NOT NULL,
	[ItemName] [nvarchar](50) NULL,
	[RequestedBy] [int] NULL,
	[DonationStatus] [smallint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_DonationDetails] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DonationImages]    Script Date: 1/31/2018 9:39:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonationImages](
	[DonationImageId] [int] NOT NULL,
	[DonationId] [int] NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_DonationImages] PRIMARY KEY CLUSTERED 
(
	[DonationImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Donations]    Script Date: 1/31/2018 9:39:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Donations](
	[DonationId] [int] NOT NULL,
	[RequestedBy] [int] NULL,
	[RequestId] [int] NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Amount] [decimal](10, 2) NULL,
	[Description] [nvarchar](180) NULL,
	[DonatedOn] [datetime] NOT NULL,
	[ExpireOn] [date] NULL,
	[DonationStatus] [smallint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_Donations] PRIMARY KEY CLUSTERED 
(
	[DonationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Request]    Script Date: 1/31/2018 9:39:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Request](
	[RequestId] [int] NOT NULL,
	[ItemType] [smallint] NOT NULL,
	[ItemName] [nvarchar](50) NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[Title] [nvarchar](50) NULL,
	[Description] [nvarchar](180) NULL,
	[RequestStatus] [smallint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_Request] PRIMARY KEY CLUSTERED 
(
	[RequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Scheduler]    Script Date: 1/31/2018 9:39:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Scheduler](
	[SchedulerId] [int] NOT NULL,
	[DonationId] [int] NOT NULL,
	[ItemId] [uniqueidentifier] NULL,
	[Title] [nvarchar](50) NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[Description] [nvarchar](180) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[RecurrenceRule] [nvarchar](50) NULL,
	[RecurrenceId] [int] NULL,
	[RecurrenceException] [nvarchar](50) NULL,
	[StartTimezone] [nvarchar](80) NULL,
	[EndTimezone] [nvarchar](80) NULL,
	[IsAllDay] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_Scheduler] PRIMARY KEY CLUSTERED 
(
	[SchedulerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [FK_Scheduler_UQ_DonationId_ItemId] UNIQUE NONCLUSTERED 
(
	[DonationId] ASC,
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 1/31/2018 9:39:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](255) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](255) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[Address] [nvarchar](100) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](80) NULL,
	[Zip] [nvarchar](20) NULL,
	[Phone] [nvarchar](20) NULL,
	[Gender] [nvarchar](6) NULL,
	[ConfirmationToken] [nvarchar](30) NULL,
	[IsConfirmed] [bit] NULL,
	[IsChurchMember] [bit] NULL,
	[Active] [bit] NULL,
	[LockoutEnabled] [bit] NULL,
	[TwoFactorEnabled] [bit] NULL,
	[FailedCount] [int] NULL,
	[ChurchName] [nvarchar](80) NULL,
	[ChurchAddress] [nvarchar](120) NULL,
	[ChurchPhone] [nvarchar](20) NULL,
	[ChurchPastor] [nvarchar](80) NULL,
	[NeedToBeVisited] [bit] NULL,
	[Comments] [text] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Users_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Users_UserName] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  View [dbo].[vw_DonationRequested]    Script Date: 1/31/2018 9:39:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_DonationRequested]
AS 
SELECT T1.*
FROM
(SELECT ROW_NUMBER() OVER(ORDER BY D.DonationId, DD.ItemId) as ID, ISNULL(DD.ItemName,D.Title) as Title, D.[Description],
       ISNULL(DD.DonationStatus, D.DonationStatus) AS DonationStatus,ISNULL(DD.RequestedBy, D.RequestedBy) AS RequestedBy, D.DonationId,DD.ItemId,
	   U.FirstName, U.LastName, U.Email, U.Phone,U.ImageUrl AS UserImageUrl,
	   ISNULL((SELECT '['  + STUFF((SELECT ',{"ImageUrl":' +'"'+ ImageUrl +'"'+ '}'
		FROM DonationDetailImages
		WHERE ItemId = DD.ItemId
		FOR XML PATH(''), TYPE).value('.','NVARCHAR(MAX)'),1,1,'') + ']'),
	   (SELECT '['  + STUFF((SELECT ',{"ImageUrl":' +'"'+ ImageUrl +'"'+ '}'
		FROM DonationImages
		WHERE DonationId = D.DonationId
		FOR XML PATH(''), TYPE).value('.','NVARCHAR(MAX)'),1,1,'') + ']')) AS ImageUrl,
       ISNULL(DD.CreatedOn,D.CreatedOn) AS CreatedOn,
	   ISNULL(DD.CreatedBy,D.CreatedBy) AS CreatedBy
FROM Donations D
LEFT JOIN DonationDetails DD 
ON d.DonationId = dd.DonationId
LEFT JOIN
(
   SELECT UserId, FirstName, LastName, Email, ImageUrl, Phone
   FROM Users
) AS U ON ISNULL(DD.CreatedBy,D.CreatedBy) = U.UserId
LEFT JOIN
(SELECT DonationId, MAX(ImageUrl) AS ImageUrl
  FROM DonationImages
  GROUP BY DonationId 
) AS DI ON D.DonationId = DI.DonationId
LEFT JOIN
( SELECT ItemId, MAX(ImageUrl) AS ImageUrl
  FROM DonationDetailImages
  GROUP BY ItemId 
) AS DDI ON DDI.ItemId = DD.ItemId) AS T1 

WHERE CONCAT(T1.DonationId,T1.ItemId) NOT IN
(SELECT CONCAT(DonationId, ItemId) from Scheduler)





GO
ALTER TABLE [dbo].[Donations] ADD  CONSTRAINT [DF_Donations_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[DonationDetailImages]  WITH CHECK ADD  CONSTRAINT [FK_DonationDetailImages_DonationDetails] FOREIGN KEY([ItemId])
REFERENCES [dbo].[DonationDetails] ([ItemId])
GO
ALTER TABLE [dbo].[DonationDetailImages] CHECK CONSTRAINT [FK_DonationDetailImages_DonationDetails]
GO
ALTER TABLE [dbo].[DonationDetailImages]  WITH NOCHECK ADD  CONSTRAINT [FK_DonationDetailImages_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[DonationDetailImages] CHECK CONSTRAINT [FK_DonationDetailImages_Users_CreatedBy]
GO
ALTER TABLE [dbo].[DonationDetailImages]  WITH NOCHECK ADD  CONSTRAINT [FK_DonationDetailImages_Users_ModifiedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[DonationDetailImages] CHECK CONSTRAINT [FK_DonationDetailImages_Users_ModifiedBy]
GO
ALTER TABLE [dbo].[DonationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_DonationDetails_CreatedBy_Users_UserId] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[DonationDetails] CHECK CONSTRAINT [FK_DonationDetails_CreatedBy_Users_UserId]
GO
ALTER TABLE [dbo].[DonationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_DonationDetails_Donations] FOREIGN KEY([DonationId])
REFERENCES [dbo].[Donations] ([DonationId])
GO
ALTER TABLE [dbo].[DonationDetails] CHECK CONSTRAINT [FK_DonationDetails_Donations]
GO
ALTER TABLE [dbo].[DonationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_DonationDetails_ModifiedBy_Users_UserId] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[DonationDetails] CHECK CONSTRAINT [FK_DonationDetails_ModifiedBy_Users_UserId]
GO
ALTER TABLE [dbo].[DonationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_DonationDetails_RequestedBy_Users_UserId] FOREIGN KEY([RequestedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[DonationDetails] CHECK CONSTRAINT [FK_DonationDetails_RequestedBy_Users_UserId]
GO
ALTER TABLE [dbo].[DonationImages]  WITH NOCHECK ADD  CONSTRAINT [FK_DonationImages_Donations] FOREIGN KEY([DonationId])
REFERENCES [dbo].[Donations] ([DonationId])
GO
ALTER TABLE [dbo].[DonationImages] CHECK CONSTRAINT [FK_DonationImages_Donations]
GO
ALTER TABLE [dbo].[DonationImages]  WITH NOCHECK ADD  CONSTRAINT [FK_DonationImages_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[DonationImages] CHECK CONSTRAINT [FK_DonationImages_Users_CreatedBy]
GO
ALTER TABLE [dbo].[DonationImages]  WITH NOCHECK ADD  CONSTRAINT [FK_DonationImages_Users_ModifiedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[DonationImages] CHECK CONSTRAINT [FK_DonationImages_Users_ModifiedBy]
GO
ALTER TABLE [dbo].[Donations]  WITH NOCHECK ADD  CONSTRAINT [FK_Donations_CreatedBy_Users_UserId] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_CreatedBy_Users_UserId]
GO
ALTER TABLE [dbo].[Donations]  WITH NOCHECK ADD  CONSTRAINT [FK_Donations_ModifiedBy_Users_UserId] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_ModifiedBy_Users_UserId]
GO
ALTER TABLE [dbo].[Donations]  WITH NOCHECK ADD  CONSTRAINT [FK_Donations_Request] FOREIGN KEY([RequestId])
REFERENCES [dbo].[Request] ([RequestId])
GO
ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_Request]
GO
ALTER TABLE [dbo].[Donations]  WITH NOCHECK ADD  CONSTRAINT [FK_Donations_RequestedBy_Users_UserId] FOREIGN KEY([RequestedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_RequestedBy_Users_UserId]
GO
ALTER TABLE [dbo].[Request]  WITH NOCHECK ADD  CONSTRAINT [FK_Request_CreatedBy_Users_UserId] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_CreatedBy_Users_UserId]
GO
ALTER TABLE [dbo].[Request]  WITH NOCHECK ADD  CONSTRAINT [FK_Request_ModifiedBy_Users_UserId] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_ModifiedBy_Users_UserId]
GO
ALTER TABLE [dbo].[Scheduler]  WITH NOCHECK ADD  CONSTRAINT [FK_Scheduler_CreatedBy_Users_UserId] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Scheduler] CHECK CONSTRAINT [FK_Scheduler_CreatedBy_Users_UserId]
GO
ALTER TABLE [dbo].[Scheduler]  WITH NOCHECK ADD  CONSTRAINT [FK_Scheduler_DonationDetails] FOREIGN KEY([ItemId])
REFERENCES [dbo].[DonationDetails] ([ItemId])
GO
ALTER TABLE [dbo].[Scheduler] CHECK CONSTRAINT [FK_Scheduler_DonationDetails]
GO
ALTER TABLE [dbo].[Scheduler]  WITH NOCHECK ADD  CONSTRAINT [FK_Scheduler_Donations] FOREIGN KEY([DonationId])
REFERENCES [dbo].[Donations] ([DonationId])
GO
ALTER TABLE [dbo].[Scheduler] CHECK CONSTRAINT [FK_Scheduler_Donations]
GO
ALTER TABLE [dbo].[Scheduler]  WITH NOCHECK ADD  CONSTRAINT [FK_Scheduler_ModifiedBy_Users_UserId] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Scheduler] CHECK CONSTRAINT [FK_Scheduler_ModifiedBy_Users_UserId]
GO
ALTER TABLE [dbo].[Users]  WITH NOCHECK ADD  CONSTRAINT [FK_Users_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users_CreatedBy]
GO
ALTER TABLE [dbo].[Users]  WITH NOCHECK ADD  CONSTRAINT [FK_Users_Users_ModifiedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users_ModifiedBy]
GO
