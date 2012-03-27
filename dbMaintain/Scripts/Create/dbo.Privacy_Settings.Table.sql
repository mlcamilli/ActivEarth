USE [ActivEarth_Dev]
GO

/****** Object:  Table [dbo].[privacy_settings]    Script Date: 03/27/2012 03:22:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[privacy_settings](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[profile_visibility] [tinyint] NOT NULL,
	[email] [bit] NOT NULL,
	[gender] [bit] NOT NULL,
	[age] [bit] NOT NULL,
	[weight] [bit] NOT NULL,
	[height] [bit] NOT NULL,
	[groups] [bit] NOT NULL,
 CONSTRAINT [PK_privacy_settings] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[privacy_settings] ADD  CONSTRAINT [DF_privacy_settings_profile_visibility]  DEFAULT ((0)) FOR [profile_visibility]
GO

ALTER TABLE [dbo].[privacy_settings] ADD  CONSTRAINT [DF_privacy_settings_email]  DEFAULT ((0)) FOR [email]
GO

ALTER TABLE [dbo].[privacy_settings] ADD  CONSTRAINT [DF_privacy_settings_gender]  DEFAULT ((1)) FOR [gender]
GO

ALTER TABLE [dbo].[privacy_settings] ADD  CONSTRAINT [DF_privacy_settings_age]  DEFAULT ((1)) FOR [age]
GO

ALTER TABLE [dbo].[privacy_settings] ADD  CONSTRAINT [DF_privacy_settings_weight]  DEFAULT ((1)) FOR [weight]
GO

ALTER TABLE [dbo].[privacy_settings] ADD  CONSTRAINT [DF_privacy_settings_height]  DEFAULT ((1)) FOR [height]
GO

ALTER TABLE [dbo].[privacy_settings] ADD  CONSTRAINT [DF_privacy_settings_groups]  DEFAULT ((1)) FOR [groups]
GO

