USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[active_routes]    Script Date: 05/07/2012 00:30:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[active_routes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[gmt_offset] [int] NOT NULL,
	[distance] [float] NOT NULL,
	[end_latitude] [float] NOT NULL,
	[end_longitude] [float] NOT NULL,
	[end_time] [datetime] NOT NULL,
	[mode] [varchar](20) NOT NULL,
	[points] [text] NOT NULL,
	[start_latitude] [float] NOT NULL,
	[start_longitude] [float] NOT NULL,
	[start_time] [datetime] NOT NULL,
	[steps] [int] NOT NULL,
	[type] [varchar](20) NOT NULL,
 CONSTRAINT [PK_active_routes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[active_routes]  WITH CHECK ADD  CONSTRAINT [FK_active_routes_user_id] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO

ALTER TABLE [dbo].[active_routes] CHECK CONSTRAINT [FK_active_routes_user_id]
GO

