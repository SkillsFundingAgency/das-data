CREATE VIEW Reporting.LevyBalance
       AS
       SELECT
              [PayrollMonthShortNameYear] AS CalendarMonthShortNameYear
            , PayrollMonth
            , Payrollyear = cast (left(Payrollyear,2) AS INT)
            , 'Levy funds' AS ValueType
            , SUM([LevyDeclaredInMonthWithEnglishFractionApplied]) AS Value
            , ValueTypeSort = 1
         FROM [Data_Pub].[DAS_LevyDeclarations]
        WHERE Flag_latest = 1
     GROUP BY
              [PayrollMonthShortNameYear]
            , PayrollMonth
            , CAST(left(Payrollyear,2) AS INT)
UNION ALL
       SELECT
              [PayrollMonthShortNameYear] AS CalendarMonthShortNameYear
            , PayrollMonth
            , Payrollyear = CAST (left(Payrollyear,2) AS INT)
            , 'Levy top-up funds' AS ValueType
            , SUM([TopupAmount]) AS Value
            , ValueTypeSort = 2
         FROM [Data_Pub].[DAS_LevyDeclarations]
        WHERE Flag_latest = 1
     GROUP BY
              [PayrollMonthShortNameYear]
            , PayrollMonth
            , CAST(left(Payrollyear,2) AS INT)
UNION ALL
       SELECT
              [DeliveryMonthShortNameYear] AS CalendarMonthShortNameYear
            , DeliveryMonth
            , DeliveryYear =CAST(Right(DeliveryYear,2)AS INT)
            , 'Levy Payment' AS ValueType
            , SUM([Amount]) AS Value
            , ValueTypeSort = 3
         FROM [Data_Pub].[DAS_Payments]
        WHERE Flag_latest = 1
          AND FundingSource = 'Levy'
     GROUP BY
              [DeliveryMonthShortNameYear]
            , DeliveryMonth
            , CAST(Right(DeliveryYear,2)AS INT)
UNION ALL
       SELECT
              [DeliveryMonthShortNameYear] AS CalendarMonthShortNameYear
              , DeliveryMonth
              , DeliveryYear = CAST(Right(DeliveryYear,2)AS INT)
            , 'Fully funded SFA' AS ValueType
            , SUM([Amount]) AS Value
              ,ValueTypeSort = 6
         FROM [Data_Pub].[DAS_Payments]
        WHERE Flag_latest = 1
          AND FundingSource = 'FullyFundedSfa'
     GROUP BY
              [DeliveryMonthShortNameYear]
            , DeliveryMonth
            , CAST(Right(DeliveryYear,2)AS INT)
UNION ALL
       SELECT
              [DeliveryMonthShortNameYear] AS CalendarMonthShortNameYear
            , DeliveryMonth
            , DeliveryYear = CAST(Right(DeliveryYear,2)AS INT)
            , 'Co-invested Employer' AS ValueType
            , SUM([Amount]) AS Value
            , ValueTypeSort = 4
         FROM [Data_Pub].[DAS_Payments]
        WHERE Flag_latest = 1
          AND FundingSource = 'CoInvestedEmployer'
     GROUP BY
              [DeliveryMonthShortNameYear]
            , DeliveryMonth
            , CAST(Right(DeliveryYear,2)AS INT)
UNION ALL
       SELECT
              [DeliveryMonthShortNameYear] AS CalendarMonthShortNameYear
            , DeliveryMonth
            , DeliveryYear = CAST(Right(DeliveryYear,2)AS INT)
            , 'Co-invested SFA' AS ValueType
            , SUM([Amount]) AS Value
            , ValueTypeSort = 5
         FROM [Data_Pub].[DAS_Payments]
        WHERE Flag_latest = 1
          AND FundingSource = 'CoInvestedSfa'
     GROUP BY
              [DeliveryMonthShortNameYear]
            , DeliveryMonth
            , CAST(Right(DeliveryYear,2)AS INT);
GO
