

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__profile__state__0CBAE877]') AND parent_object_id = OBJECT_ID(N'[dbo].[profile]'))
ALTER TABLE [dbo].[profile] DROP CONSTRAINT [FK__profile__state__0CBAE877]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[profile]') AND type in (N'U'))
DROP TABLE [dbo].[profile]
GO


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
	[gender] [char](1) NOT NULL,
	[city] [varchar](50) NULL,
	[state] [char](2) NULL,
	[age] [int] NULL,
	[height] [int] NULL,
	[weight] [int] NULL,
PRIMARY KEY CLUSTERED 
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


