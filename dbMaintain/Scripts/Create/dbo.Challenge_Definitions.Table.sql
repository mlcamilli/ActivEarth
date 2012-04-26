USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[challenge_definitions]    Script Date: 04/25/2012 00:58:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[challenge_definitions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[challenge_type] [tinyint] NOT NULL,
	[persistent] [bit] NOT NULL,
	[statistic] [tinyint] NOT NULL,
	[requirement] [float] NOT NULL,
	[reward] [int] NOT NULL,
	[condition_text] [varchar](100) NOT NULL,
	[image_path] [varchar](75) NOT NULL,
	[name] [varchar](30) NULL,
 CONSTRAINT [PK_challenge_definitions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

