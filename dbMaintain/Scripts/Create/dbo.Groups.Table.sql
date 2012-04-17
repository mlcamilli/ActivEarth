USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[groups]    Script Date: 04/10/2012 14:01:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[groups](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[owner_id] [int] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[description] [varchar](max) NOT NULL,
	[green_score] [int] NOT NULL,
	[challenge_score] [int] NOT NULL,
	[contest_score] [int] NOT NULL,
	[badge_score] [int] NOT NULL,
 CONSTRAINT [PK__groups__3213E83F0AD2A005] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[groups]  WITH CHECK ADD  CONSTRAINT [FK_owner_id_groups] FOREIGN KEY([owner_id])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[groups] CHECK CONSTRAINT [FK_owner_id_groups]
GO

