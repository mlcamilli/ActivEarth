USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[group_hashtags]    Script Date: 04/10/2012 13:59:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[group_hashtags](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[group_id] [int] NOT NULL,
	[hashtag] [varchar](50) NOT NULL,
 CONSTRAINT [PK_group_hashtags] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[group_hashtags]  WITH CHECK ADD  CONSTRAINT [FK_group_id_group_hashtags] FOREIGN KEY([group_id])
REFERENCES [dbo].[groups] ([id])
GO

ALTER TABLE [dbo].[group_hashtags] CHECK CONSTRAINT [FK_group_id_group_hashtags]
GO

