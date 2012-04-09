USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[statistic_info]    Script Date: 04/05/2012 01:10:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[statistic_info](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[statistic_id] [tinyint] NOT NULL,
	[name] [nchar](25) NOT NULL,
	[format_string] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Statistic_Info] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

