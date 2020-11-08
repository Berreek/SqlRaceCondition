CREATE TYPE [dbo].[AreaList] AS TABLE(
    [area] [varchar](max) NULL
    )
    GO;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SqlRaceCondition]
		@areaList AreaList READONLY
AS
BEGIN
SELECT 1;
END
