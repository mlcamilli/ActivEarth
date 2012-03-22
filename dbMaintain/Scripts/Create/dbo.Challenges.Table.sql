USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[challenges]    Script Date: 03/22/2012 00:47:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[challenges](
	[id] [int] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[description] [text] NOT NULL,
	[points] [int] NOT NULL,
	[requirement] [float] NOT NULL,
	[persistent] [bit] NOT NULL,
	[end_time] [datetime] NOT NULL,
	[duration_days] [int] NOT NULL,
	[statistic] [tinyint] NOT NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_challenges] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

