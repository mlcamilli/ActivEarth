USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[badge_level_info]    Script Date: 04/05/2012 01:03:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[badge_level_info](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[statistic] [tinyint] NOT NULL,
	[level] [tinyint] NOT NULL,
	[requirement] [float] NOT NULL,
	[reward] [int] NOT NULL,
	[image_path] [text] NOT NULL,
 CONSTRAINT [PK_badge_level_info] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

