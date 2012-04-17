USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[group_contests]    Script Date: 04/10/2012 14:00:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[group_contests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[contest_id] [int] NOT NULL,
	[group_id] [int] NOT NULL,
 CONSTRAINT [PK_group_contests] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[group_contests]  WITH CHECK ADD  CONSTRAINT [FK_contest_id_group_contests] FOREIGN KEY([contest_id])
REFERENCES [dbo].[contests] ([id])
GO

ALTER TABLE [dbo].[group_contests] CHECK CONSTRAINT [FK_contest_id_group_contests]
GO

ALTER TABLE [dbo].[group_contests]  WITH CHECK ADD  CONSTRAINT [FK_group_id_group_contests] FOREIGN KEY([group_id])
REFERENCES [dbo].[groups] ([id])
GO

ALTER TABLE [dbo].[group_contests] CHECK CONSTRAINT [FK_group_id_group_contests]
GO

