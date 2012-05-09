USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[carpools]    Script Date: 05/08/2012 22:24:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[carpools](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[start] [varchar](100) NOT NULL,
	[destination] [varchar](100) NOT NULL,
	[time] [varchar](20) NOT NULL,
	[seats_available] [tinyint] NOT NULL,
	[comments] [text] NOT NULL,
 CONSTRAINT [PK_carpools] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[carpools]  WITH CHECK ADD  CONSTRAINT [FK_carpools_user_id] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[carpools] CHECK CONSTRAINT [FK_carpools_user_id]
GO

