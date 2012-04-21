USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[contests]    Script Date: 04/20/2012 22:06:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[contests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[description] [text] NOT NULL,
	[points] [int] NOT NULL,
	[end_mode] [tinyint] NOT NULL,
	[end_time] [datetime] NULL,
	[end_goal] [float] NULL,
	[start] [datetime] NOT NULL,
	[statistic] [tinyint] NOT NULL,
	[type] [tinyint] NOT NULL,
	[searchable] [bit] NOT NULL,
	[active] [bit] NOT NULL,
	[deactivated] [datetime] NULL,
	[creator_id] [int] NOT NULL,
 CONSTRAINT [PK_contests] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[contests]  WITH CHECK ADD  CONSTRAINT [FK_contests_creator_id] FOREIGN KEY([creator_id])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[contests] CHECK CONSTRAINT [FK_contests_creator_id]
GO

