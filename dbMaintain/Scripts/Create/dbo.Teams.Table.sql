USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[teams]    Script Date: 04/17/2012 21:49:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[teams](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[contest_id] [int] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[score] [float] NOT NULL,
    [locked] [bit] NOT NULL,
    [bracket] [tinyint] NOT NULL,
	[group_id] [int] NULL,
 CONSTRAINT [PK__teams__3213E83F145C0A3F] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[teams]  WITH CHECK ADD  CONSTRAINT [fk_contest_id] FOREIGN KEY([contest_id])
REFERENCES [dbo].[contests] ([id])
GO

ALTER TABLE [dbo].[teams] CHECK CONSTRAINT [fk_contest_id]
GO

ALTER TABLE [dbo].[teams]  WITH CHECK ADD  CONSTRAINT [FK_teams_group_id] FOREIGN KEY([group_id])
REFERENCES [dbo].[groups] ([id])
GO

ALTER TABLE [dbo].[teams] CHECK CONSTRAINT [FK_teams_group_id]
GO

