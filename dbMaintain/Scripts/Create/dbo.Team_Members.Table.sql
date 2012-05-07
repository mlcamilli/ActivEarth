USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[team_members]    Script Date: 04/18/2012 01:51:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[team_members](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[contest_id] [int] NOT NULL,
	[team_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
    [score] [float] NOT NULL,
	[initial_score] [float] NOT NULL,
	[initialized] [bit] NOT NULL,
 CONSTRAINT [PK__team_mem__3213E83F182C9B23] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[team_members]  WITH CHECK ADD  CONSTRAINT [FK_team_members_contest_id] FOREIGN KEY([contest_id])
REFERENCES [dbo].[contests] ([id])
GO

ALTER TABLE [dbo].[team_members] CHECK CONSTRAINT [FK_team_members_contest_id]
GO

ALTER TABLE [dbo].[team_members]  WITH CHECK ADD  CONSTRAINT [FK_team_members_team_id] FOREIGN KEY([team_id])
REFERENCES [dbo].[teams] ([id])
GO

ALTER TABLE [dbo].[team_members] CHECK CONSTRAINT [FK_team_members_team_id]
GO

ALTER TABLE [dbo].[team_members]  WITH CHECK ADD  CONSTRAINT [fk_team_members_user_id] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[team_members] CHECK CONSTRAINT [fk_team_members_user_id]
GO

