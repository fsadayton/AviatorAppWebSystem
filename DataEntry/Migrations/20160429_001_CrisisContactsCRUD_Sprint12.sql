USE [ArmorApp]
GO

/****** Object:  Table [dbo].[CrisisContact]    Script Date: 5/18/2016 4:57:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CrisisContact](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ContactId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[CrisisContact]  WITH CHECK ADD  CONSTRAINT [fk_crisisContact] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contact] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CrisisContact] CHECK CONSTRAINT [fk_crisisContact]
GO


