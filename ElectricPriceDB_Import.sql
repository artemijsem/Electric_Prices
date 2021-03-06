USE [electricpriceDB]
GO
/****** Object:  Table [dbo].[EP_API_TABLE]    Script Date: 1/7/2022 4:50:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EP_API_TABLE](
	[success] [bit] NULL,
	[data] [nvarchar](max) NULL,
	[timestamp] [int] NULL,
	[price] [float] NULL,
	[date] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[EP_API_TABLE] ADD  CONSTRAINT [DF__EP_API_TAB__date__4AB81AF0]  DEFAULT (getdate()) FOR [date]
GO
/****** Object:  StoredProcedure [dbo].[InsertAPIData]    Script Date: 1/7/2022 4:50:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertAPIData]

@json NVARCHAR(MAX)

AS

BEGIN

DECLARE @date AS DATETIME
SET @date = GETDATE()


INSERT INTO EP_API_TABLE
(
	success,
	data,
	timestamp,
	price
)

SELECT success,data,timestamp,price

FROM OPENJSON(@json)
WITH(
success BIT '$.success',
data NVARCHAR(MAX) '$.data' as JSON
)
OUTER APPLY OPENJSON(data)
WITH
(
timestamp INT '$.timestamp',
price FLOAT '$.price'
) as JsonValues

SELECT * FROM EP_API_TABLE
ORDER BY date DESC;






END 
GO
/****** Object:  StoredProcedure [dbo].[RetrieveAPIData]    Script Date: 1/7/2022 4:50:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RetrieveAPIData] 

AS
BEGIN
	
SELECT * FROM EP_API_TABLE
FOR JSON AUTO

END
GO
