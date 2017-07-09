USE [Paradify]
GO
/****** Object:  Table [dbo].[User]    Script Date: 07/06/2017 18:25:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Birthdate] [datetime] NULL,
	[Country] [varchar](10) NULL,
	[DisplayName] [varchar](255) NULL,
	[Email] [varchar](255) NULL,
	[Href] [varchar](255) NULL,
	[UserId] [varchar](50) NULL,
	[Images] [varchar](255) NULL,
	[Product] [varchar](255) NULL,
	[Type] [varchar](255) NULL,
	[Uri] [varchar](255) NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[History]    Script Date: 07/06/2017 18:25:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[History](
	[Query] [varchar](255) NOT NULL,
	[TrackId] [varchar](50) NULL,
	[UserId] [varchar](50) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[source] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[CountOfSearchGroupByUser]    Script Date: 07/06/2017 18:25:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CountOfSearchGroupByUser] 
AS
BEGIN
	select 
		U.UserId, 
		Count(1) as CountOfSearch 
	from [User] as U 
				inner join History AS H on U.UserId = H.UserId
	group by U.UserId,U.CreatedDate
	order by Count(1) desc 
	
END
GO
/****** Object:  Default [CreatedDateDefaultValue]    Script Date: 07/06/2017 18:25:24 ******/
ALTER TABLE [dbo].[History] ADD  CONSTRAINT [CreatedDateDefaultValue]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
/****** Object:  Default [DF__History__source__21B6055D]    Script Date: 07/06/2017 18:25:24 ******/
ALTER TABLE [dbo].[History] ADD  DEFAULT ((1)) FOR [source]
GO
/****** Object:  Default [DF__User__CreatedDat__1273C1CD]    Script Date: 07/06/2017 18:25:24 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF__User__CreatedDat__1273C1CD]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
