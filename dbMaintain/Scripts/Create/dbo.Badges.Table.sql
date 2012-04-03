USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[badges]    Script Date: 04/03/2012 16:34:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[badges](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[statistic] [tinyint] NOT NULL,
	[badge_level] [tinyint] NOT NULL,
	[progress] [tinyint] NOT NULL,
 CONSTRAINT [PK_badges] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[badges]  WITH CHECK ADD  CONSTRAINT [FK_badges_user_id] FOREIGN KEY([user_id])
REFERENCES [dbo].[profile] ([id])
GO

ALTER TABLE [dbo].[badges] CHECK CONSTRAINT [FK_badges_user_id]
GO

