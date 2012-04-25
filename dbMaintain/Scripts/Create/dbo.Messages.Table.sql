USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[messages]    Script Date: 04/25/2012 15:42:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[messages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[group_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[title] [varchar](max) NOT NULL,
	[message] [varchar](max) NOT NULL,
	[date] [varchar](50) NOT NULL,
	[time] [varchar](50) NOT NULL,
 CONSTRAINT [PK_messages] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[messages]  WITH CHECK ADD  CONSTRAINT [FK_poster_id_messages] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[messages] CHECK CONSTRAINT [FK_poster_id_messages]
GO

