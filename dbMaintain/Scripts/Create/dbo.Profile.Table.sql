USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[profile]    Script Date: 04/26/2012 02:54:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[profile](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[first_name] [varchar](50) NOT NULL,
	[last_name] [varchar](50) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[gender] [varchar](1) NOT NULL,
	[city] [varchar](50) NULL,
	[state] [varchar](2) NULL,
	[age] [int] NULL,
	[height] [int] NULL,
	[weight] [int] NULL,
	[green_score] [int] NOT NULL,
	[activity_score_total] [int] NOT NULL,
	[activity_score_badges] [int] NOT NULL,
	[activity_score_challenges] [int] NOT NULL,
	[activity_score_contests] [int] NOT NULL,
 CONSTRAINT [PK__profile__3213E83F060DEAE8] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[profile]  WITH CHECK ADD  CONSTRAINT [FK__profile__state__0CBAE877] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[profile] CHECK CONSTRAINT [FK__profile__state__0CBAE877]
GO

