CREATE VIEW Reporting.LevyBalance
AS
SELECT
       [PayrollMonthShortNameYear] AS CalendarMonthShortNameYear
     , 'LevyIn' AS ValueType
     , SUM([LevyAvailableInMonth]) AS Value
  FROM [Data_Pub].[DAS_LevyDeclarations]
 WHERE Flag_latest = 1
 GROUP BY
       [PayrollMonthShortNameYear]
UNION ALL
SELECT
       [PayrollMonthShortNameYear] AS CalendarMonthShortNameYear
     , 'LevyTopUp' AS ValueType
     , SUM([TopupAmount]) AS Value
  FROM [Data_Pub].[DAS_LevyDeclarations]
 WHERE Flag_latest = 1
 GROUP BY
       [PayrollMonthShortNameYear]
UNION ALL
SELECT
       [DeliveryMonthShortNameYear] AS CalendarMonthShortNameYear
     , 'Spend' AS ValueType
     , SUM([Amount]) AS Value
  FROM [Data_Pub].[DAS_Payments]
 WHERE Flag_latest = 1
 GROUP BY
       [DeliveryMonthShortNameYear]
