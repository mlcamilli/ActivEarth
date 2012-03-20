IF EXISTS(SELECT * FROM msdb.sys.syslogins WHERE UPPER([name]) = 'ROB') 
  BEGIN
	DROP LOGIN [Rob]
	PRINT '<<< REMOVED INSTANCE LOGIN FOR Rob >>>'
  END
GO

CREATE LOGIN [Rob] WITH PASSWORD=N'RHr0x0r!', DEFAULT_DATABASE=[{{DatabaseName}}], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
PRINT '<<< CREATED INSTANCE LOGIN FOR Rob >>>'
GO

PRINT '<<- Switching to {{DatabaseName}} for remainder of permissions ->>'
GO
USE [{{DatabaseName}}]
GO

IF EXISTS(SELECT * FROM sys.sysusers WHERE UPPER([name]) = 'ROB') 
  BEGIN
	DROP USER [Rob]
	PRINT '<<< REMOVED {{DatabaseName}} Security Login for Rob >>>'
  END
GO

CREATE USER [Rob] FOR LOGIN [Rob]
GO
PRINT '<<< CREATED {{DatabaseName}} Security Login for Rob >>>'
GO

EXEC sp_addrolemember N'db_datareader', N'Rob'
GO
PRINT '<<< Added Rob to the db_datareader group for {{DatabaseName}} >>>'
GO
EXEC sp_addrolemember N'db_datawriter', N'Rob'
GO
PRINT '<<< Added Rob to the db_datawriter group for {{DatabaseName}} >>>'
GO
