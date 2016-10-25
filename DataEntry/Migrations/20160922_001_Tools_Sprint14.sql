USE [ArmorApp]
GO

/****** Object:  Table [dbo].[Tools]    Script Date: 9/23/2016 10:51:18 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tools](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Website] [nvarchar](100) NULL,
	[AppleStoreWebsite] [nvarchar](100) NULL,
	[GooglePlayWebsite] [nvarchar](100) NULL,
	[Description] [nvarchar](200) NULL,
	[Active] [bit] NOT NULL,
	[Name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Tools] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO



