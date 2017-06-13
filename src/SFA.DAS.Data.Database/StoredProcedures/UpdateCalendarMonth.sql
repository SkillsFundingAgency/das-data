CREATE PROCEDURE [Data_Load].[UpdateCalendarMonth]
AS
BEGIN
--Insert Date into Table
DECLARE @IntNumberOfYears INT = 50
--DECLARE @IntYearsInPast INT = 10 --Default to 10
DECLARE @IntNumberofMonths INT
DECLARE @DateFirstMonth DATE
DECLARE @DateLastMonth DATE
SET @DateFirstMonth = DATEFROMPARTS(2000,1,1)
SET @DateLastMonth = DATEADD(m,(12*@IntNumberOfYears),DATEFROMPARTS(YEAR(GETDATE()),MONTH(GETDATE()),1))
SET @IntNumberofMonths = DATEDIFF(m,@DateFirstMonth,@DateLastMonth) +1

DECLARE @i INT = 1
--Temp Table
DECLARE @CalendarMonth TABLE
(
    CalendarMonthID INT NOT NULL PRIMARY KEY
    , CalendarDate DATE NOT NULL
    , CalendarMonthNumber INT NOT NULL
    , CalendarMonthName VARCHAR(20) NOT NULL
    , CalendarMonthShortName VARCHAR(20) NOT NULL
    , CalendarYear INT NOT NULL
    , CalendarMonthNameYear VARCHAR(20) NOT NULL
    , CalendarMonthShortNameYear VARCHAR(20) NOT NULL
    , TaxMonthNumber VARCHAR(20) NOT NULL
    , TaxYear VARCHAR(20) NOT NULL
    , AcademicMonthNumber INT NOT NULL
    , AcademicYear VARCHAR(20) NOT NULL
    , CalendarYearMonth_SortOrder INT NOT NULL
)
DECLARE @LoopDate DATE

SET @i = 1

WHILE @i <= @IntNumberofMonths
      BEGIN
		  SET @LoopDate =DATEADD(M,@i-1,@DateFirstMonth)
		  INSERT INTO @CalendarMonth
		  (
			 CalendarMonthID 
			 , CalendarDate 
			 , CalendarMonthNumber 
			 , CalendarMonthName 
			 , CalendarMonthShortName
			 , CalendarYear
			 , CalendarMonthNameYear
			 , CalendarMonthShortNameYear
			 , TaxMonthNumber
			 , TaxYear
			 , AcademicMonthNumber 
			 , AcademicYear 
                , CalendarYearMonth_SortOrder
		  )
		  SELECT 
			 CAST((CAST(YEAR(@LoopDate) AS VARCHAR(10)) + CAST(MONTH(@LoopDate) AS VARCHAR(2)) ) AS INT) AS CalendarMonthID 
			 , @LoopDate AS CalendarDate 
			 , MONTH(@LoopDate) AS CalendarMonthNumber 
			 , DATENAME(MONTH,@LoopDate) AS CalendarMonthName
			 , LEFT(DATENAME(MONTH,@LoopDate),3) AS CalendarMonthShortName 
			 , YEAR(@LoopDate) AS CalendarYear
			 , DATENAME(MONTH,@LoopDate) +' - '+ CAST(YEAR(@LoopDate) AS VARCHAR(20)) AS CalendarMonthNameYear
			 , LEFT(DATENAME(MONTH,@LoopDate),3) +' - '+ CAST(YEAR(@LoopDate) AS VARCHAR(20)) AS CalendarMonthShortNameYear
			 -- Tax Year
			 , CASE WHEN MONTH(@LoopDate) >= 4 THEN MONTH(@LoopDate) - 3
				  ELSE MONTH(@LoopDate) + 9 END TaxMonthNumber
			 , CASE WHEN MONTH(@LoopDate) >= 4 THEN CAST(RIGHT(YEAR(@LoopDate),2) AS VARCHAR(10)) + '-' + CAST(RIGHT(YEAR(@LoopDate)+1,2) AS VARCHAR(10)) 
				ELSE CAST(RIGHT(YEAR(@LoopDate),2) AS VARCHAR(10)) + '-' + CAST(RIGHT(YEAR(@LoopDate)+1,2) AS VARCHAR(10)) END  AS TaxYear --16-17
			 -- Academic Year
			 , CASE WHEN MONTH(@LoopDate) >= 8 THEN MONTH(@LoopDate) - 7
				  ELSE MONTH(@LoopDate) + 5 END AS AcademicMonthNumber 
			 , CASE WHEN YEAR(@LoopDate) >= 8 THEN CAST(YEAR(@LoopDate) AS VARCHAR(10)) + '/' + CAST(RIGHT(YEAR(@LoopDate)+1,2) AS VARCHAR(10)) 
				ELSE CAST(YEAR(@LoopDate)+1 AS VARCHAR(10)) + '/' + CAST(RIGHT(YEAR(@LoopDate),2) AS VARCHAR(10)) END AS AcademicYear 
                , CAST(YEAR(@LoopDate) AS VARCHAR(255)) +  CASE WHEN MONTH(@LoopDate) < 10 THEN '0' +  CAST(MONTH(@LoopDate) AS VARCHAR(255))  ELSE  CAST(MONTH(@LoopDate) AS VARCHAR(255)) END AS CalendarYearMonth_SortOrder

            SET @i = @i + 1

      END


-- Update rows

UPDATE CM
SET         [CalendarDate] = T_CM.[CalendarDate]
           ,[CalendarMonthNumber] = T_CM.[CalendarMonthNumber]
           ,[CalendarMonthName] = T_CM.[CalendarMonthName]
           ,[CalendarMonthShortName] = T_CM.[CalendarMonthShortName]
           ,[CalendarYear] = T_CM.[CalendarYear]
           ,[CalendarMonthNameYear] = T_CM.[CalendarMonthNameYear]
           ,[CalendarMonthShortNameYear] = T_CM.[CalendarMonthShortNameYear]
           ,[TaxMonthNumber] = T_CM.[TaxMonthNumber]
           ,[TaxYear] = T_CM.[TaxYear]
           ,[AcademicMonthNumber] = T_CM.[AcademicMonthNumber]
           ,[AcademicYear] = T_CM.[AcademicYear]
           ,[CalendarYearMonth_SortOrder] = T_CM.[CalendarYearMonth_SortOrder]
FROM [Data_Load].[DAS_CalendarMonth] AS CM
     INNER JOIN  @CalendarMonth AS T_CM ON CM.[CalendarMonthID] = T_CM.[CalendarMonthID]

-- Insert rows

INSERT INTO Data_Load.DAS_CalendarMonth
([CalendarMonthID]
           ,[CalendarDate]
           ,[CalendarMonthNumber]
           ,[CalendarMonthName]
           ,[CalendarMonthShortName]
           ,[CalendarYear]
           ,[CalendarMonthNameYear]
           ,[CalendarMonthShortNameYear]
           ,[TaxMonthNumber]
           ,[TaxYear]
           ,[AcademicMonthNumber]
           ,[AcademicYear]
           ,[CalendarYearMonth_SortOrder])
SELECT [CalendarMonthID]
           ,[CalendarDate]
           ,[CalendarMonthNumber]
           ,[CalendarMonthName]
           ,[CalendarMonthShortName]
           ,[CalendarYear]
           ,[CalendarMonthNameYear]
           ,[CalendarMonthShortNameYear]
           ,[TaxMonthNumber]
           ,[TaxYear]
           ,[AcademicMonthNumber]
           ,[AcademicYear]
           ,[CalendarYearMonth_SortOrder]
FROM @CalendarMonth
WHERE CalendarMonthID NOT IN (SELECT DISTINCT CalendarMonthID
						 FROM [Data_Load].[DAS_CalendarMonth])

END

GO
