ALTER DATABASE "SimManagementSystem" SET SINGLE_USER WITH ROLLBACK IMMEDIATE

-- change collation to Modern_Spanish_CI_AI_WS
ALTER DATABASE "SimManagementSystem" COLLATE Polish_CI_AS;

-- allow users back into the database
ALTER DATABASE "SimManagementSystem" SET MULTI_USER

SELECT name, collation_name FROM sys.databases WHERE name = 'SimManagementSystem';