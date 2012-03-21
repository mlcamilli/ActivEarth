USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[team_members]    Script Date: 03/21/2012 15:41:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[team_members](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[team_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[team_members]  WITH CHECK ADD  CONSTRAINT [fk_team_id] FOREIGN KEY([team_id])
REFERENCES [dbo].[teams] ([id])
GO

ALTER TABLE [dbo].[team_members] CHECK CONSTRAINT [fk_team_id]
GO

ALTER TABLE [dbo].[team_members]  WITH CHECK ADD  CONSTRAINT [fk_user_id_teams] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[team_members] CHECK CONSTRAINT [fk_user_id_teams]
GO

