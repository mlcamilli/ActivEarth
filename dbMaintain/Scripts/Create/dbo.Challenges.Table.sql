USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[challenges]    Script Date: 05/06/2012 14:51:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[challenges](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](30) NOT NULL,
	[description] [varchar](100) NOT NULL,
	[reward] [int] NOT NULL,
	[requirement] [float] NOT NULL,
	[persistent] [bit] NOT NULL,
	[end_time] [datetime] NOT NULL,
	[duration_days] [int] NOT NULL,
	[statistic] [tinyint] NOT NULL,
	[active] [bit] NOT NULL,
	[image_path] [varchar](75) NOT NULL,
 CONSTRAINT [PK_challenges] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

