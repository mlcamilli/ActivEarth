USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[recycling_centers]    Script Date: 05/08/2012 22:24:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[recycling_centers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[location] [varchar](160) NOT NULL,
	[comments] [text] NOT NULL,
	[automotive] [bit] NOT NULL,
	[electronics] [bit] NOT NULL,
	[construction] [bit] NOT NULL,
	[batteries] [bit] NOT NULL,
	[garden] [bit] NOT NULL,
	[glass] [bit] NOT NULL,
	[hazardous] [bit] NOT NULL,
	[household] [bit] NOT NULL,
	[metal] [bit] NOT NULL,
	[paint] [bit] NOT NULL,
	[paper] [bit] NOT NULL,
	[plastic] [bit] NOT NULL,
 CONSTRAINT [PK_recycling_centers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[recycling_centers]  WITH CHECK ADD  CONSTRAINT [FK_recycling_centers_user_id] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[recycling_centers] CHECK CONSTRAINT [FK_recycling_centers_user_id]
GO

