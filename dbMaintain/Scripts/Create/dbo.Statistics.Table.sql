USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[statistics]    Script Date: 04/14/2012 23:50:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[statistics](
	[id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[statistic_type] [tinyint] NOT NULL,
	[value] [float] NOT NULL,
 CONSTRAINT [PK_statistics] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[statistics]  WITH CHECK ADD  CONSTRAINT [FK_statistics_users] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[statistics] CHECK CONSTRAINT [FK_statistics_users]
GO

