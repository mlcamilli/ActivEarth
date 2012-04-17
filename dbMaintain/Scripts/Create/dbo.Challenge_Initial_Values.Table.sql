USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[challenge_initial_values]    Script Date: 04/17/2012 01:00:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[challenge_initial_values](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[challenge_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[value] [float] NOT NULL,
 CONSTRAINT [PK_challenge_initial_values] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[challenge_initial_values]  WITH CHECK ADD  CONSTRAINT [FK_challenge_initial_values_challenge_id] FOREIGN KEY([challenge_id])
REFERENCES [dbo].[challenges] ([id])
GO

ALTER TABLE [dbo].[challenge_initial_values] CHECK CONSTRAINT [FK_challenge_initial_values_challenge_id]
GO

ALTER TABLE [dbo].[challenge_initial_values]  WITH CHECK ADD  CONSTRAINT [FK_challenge_initial_values_user_id] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[challenge_initial_values] CHECK CONSTRAINT [FK_challenge_initial_values_user_id]
GO

