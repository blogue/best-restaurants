CREATE DATABASE [best_restaurants_test]
GO
USE [best_restaurants_test]
GO
/****** Object:  Table [dbo].[cuisines]    Script Date: 7/13/2016 9:03:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cuisines](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cuisine_type] [varchar](255) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
