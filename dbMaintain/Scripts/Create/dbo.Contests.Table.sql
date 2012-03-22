USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[contests]    Script Date: 03/21/2012 22:52:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[contests](
	[id] [int] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[description] [text] NOT NULL,
	[points] [int] NOT NULL,
	[end_mode] [tinyint] NOT NULL,
	[end_time] [datetime] NULL,
	[end_goal] [float] NULL,
	[start] [datetime] NOT NULL,
	[statistic] [tinyint] NOT NULL,
 CONSTRAINT [PK_contests] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

