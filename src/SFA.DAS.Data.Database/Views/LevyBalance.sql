CREATE VIEW Reporting.LevyBalance
       AS
       SELECT
              [PayrollMonthShortNameYear] AS CalendarMonthShortNameYear
              , PayrollMonth
              , Payrollyear = cast (left(Payrollyear,2) AS INT)
            , 'LevyIn' AS ValueType
            , SUM([LevyDeclaredInMonthWithEnglishFractionApplied]) AS Value
         FROM [Data_Pub].[DAS_LevyDeclarations]
       WHERE Flag_latest = 1
       GROUP BY
              [PayrollMonthShortNameYear]  ,PayrollMonth,
                cast (left(Payrollyear,2) AS INT)
       UNION ALL
       SELECT
              [PayrollMonthShortNameYear] AS CalendarMonthShortNameYear
              , PayrollMonth
              , Payrollyear = CAST (left(Payrollyear,2) AS INT)
            , 'LevyTopUp' AS ValueType
            , SUM([TopupAmount]) AS Value
         FROM [Data_Pub].[DAS_LevyDeclarations]
       WHERE Flag_latest = 1
       GROUP BY
              [PayrollMonthShortNameYear], PayrollMonth
              , CAST (left(Payrollyear,2) AS INT)
       UNION ALL
       SELECT
              [DeliveryMonthShortNameYear] AS CalendarMonthShortNameYear
            , DeliveryMonth
              , DeliveryYear =CAST(Right(DeliveryYear,2)AS INT)
              , 'Levy Payment' AS ValueType
            , SUM([Amount]) AS Value
         FROM [Data_Pub].[DAS_Payments]
       WHERE Flag_latest = 1 and FundingSource = 'Levy'
       GROUP BY
              [DeliveryMonthShortNameYear], DeliveryMonth
              , CAST(Right(DeliveryYear,2)AS INT)
       UNION ALL
       SELECT
              [DeliveryMonthShortNameYear] AS CalendarMonthShortNameYear
              , DeliveryMonth
              , DeliveryYear = CAST(Right(DeliveryYear,2)AS INT)
            , 'FullyFundedSfa' AS ValueType
            , SUM([Amount]) AS Value
         FROM [Data_Pub].[DAS_Payments]
       WHERE Flag_latest = 1 and FundingSource = 'FullyFundedSfa'
       GROUP BY
              [DeliveryMonthShortNameYear], DeliveryMonth
              , CAST(Right(DeliveryYear,2)AS INT)
UNION ALL
SELECT
              [DeliveryMonthShortNameYear] AS CalendarMonthShortNameYear
              , DeliveryMonth
              , DeliveryYear = CAST(Right(DeliveryYear,2)AS INT)
            , 'CoInvestedEmployer' AS ValueType
            , SUM([Amount]) AS Value
         FROM [Data_Pub].[DAS_Payments]
       WHERE Flag_latest = 1 and FundingSource = 'CoInvestedEmployer'
       GROUP BY
              [DeliveryMonthShortNameYear], DeliveryMonth
              , CAST(Right(DeliveryYear,2)AS INT)
UNION ALL
SELECT
              [DeliveryMonthShortNameYear] AS CalendarMonthShortNameYear
              , DeliveryMonth
              , DeliveryYear = CAST(Right(DeliveryYear,2)AS INT)
            , 'CoInvestedSfa' AS ValueType
            , SUM([Amount]) AS Value
         FROM [Data_Pub].[DAS_Payments]
       WHERE Flag_latest = 1 and FundingSource = 'CoInvestedSfa'
       GROUP BY
              [DeliveryMonthShortNameYear], DeliveryMonth
              , CAST(Right(DeliveryYear,2)AS INT);
GO
