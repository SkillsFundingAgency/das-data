CREATE VIEW Reporting.LevyBalance
       AS
       SELECT
              [PayrollMonthShortNameYear] AS CalendarMonthShortNameYear
            , CalendarMonthNumber
            , CalendarYear
            , DateSort = CalendarMonthNumber*10000 + CalendarYear
            , 'Levy funds' AS ValueType
            , SUM([LevyDeclaredInMonthWithEnglishFractionApplied]) AS Value
            , ValueTypeSort = 1
         FROM [Data_Pub].[DAS_LevyDeclarations] Dec
              JOIN [Data_Pub].DAS_CalendarMonth Cal
              ON Cal.TaxMonthNumber = Dec.PayrollMonth
              AND Cal.TaxYear = Dec.PayrollYear
        WHERE Flag_Latest = 1
     GROUP BY
              [PayrollMonthShortNameYear]
            , CalendarMonthNumber
            , CalendarYear
            , CalendarMonthNumber*10000 + CalendarYear
UNION ALL
       SELECT
              [PayrollMonthShortNameYear] AS CalendarMonthShortNameYear
            , CalendarMonthNumber
            , CalendarYear
            , DateSort = CalendarMonthNumber*10000 + CalendarYear
            , 'Levy top-up funds' AS ValueType
            , SUM([TopupAmount]) AS Value
            , ValueTypeSort = 2
         FROM [Data_Pub].[DAS_LevyDeclarations] Dec
              JOIN [Data_Pub].DAS_CalendarMonth Cal
              ON Cal.TaxMonthNumber = Dec.PayrollMonth
              AND Cal.TaxYear = Dec.PayrollYear
        WHERE Flag_Latest = 1
     GROUP BY
              [PayrollMonthShortNameYear]
            , CalendarMonthNumber
            , CalendarYear
            , CalendarMonthNumber*10000 + CalendarYear
UNION ALL
       SELECT
              [DeliveryMonthShortNameYear] AS CalendarMonthShortNameYear
            , DeliveryMonth
            , DeliveryYear
            , DateSort = DeliveryMonth*10000 + DeliveryYear
            , 'Levy Payment' AS ValueType
            , SUM([Amount]) AS Value
            , ValueTypeSort = 3
         FROM [Data_Pub].[DAS_Payments]
        WHERE Flag_Latest = 1
          AND FundingSource = 'Levy'
     GROUP BY
              [DeliveryMonthShortNameYear]
            , DeliveryMonth
            , DeliveryYear
            , DeliveryMonth*10000 + DeliveryYear
UNION ALL
       SELECT
              [DeliveryMonthShortNameYear] AS CalendarMonthShortNameYear
            , DeliveryMonth
            , DeliveryYear
            , DateSort = DeliveryMonth*10000 + DeliveryYear
            , 'Fully funded SFA' AS ValueType
            , SUM([Amount]) AS Value
            , ValueTypeSort = 6
         FROM [Data_Pub].[DAS_Payments]
        WHERE Flag_latest = 1
          AND FundingSource = 'FullyFundedSfa'
     GROUP BY
              [DeliveryMonthShortNameYear]
            , DeliveryMonth
            , DeliveryYear
            , DeliveryMonth*10000 + DeliveryYear
UNION ALL
       SELECT
              [DeliveryMonthShortNameYear] AS CalendarMonthShortNameYear
            , DeliveryMonth
            , DeliveryYear
            , DateSort = DeliveryMonth*10000 + DeliveryYear
            , 'Co-invested Employer' AS ValueType
            , SUM([Amount]) AS Value
            , ValueTypeSort = 4
         FROM [Data_Pub].[DAS_Payments]
        WHERE Flag_latest = 1
          AND FundingSource = 'CoInvestedEmployer'
     GROUP BY
              [DeliveryMonthShortNameYear]
            , DeliveryMonth
            , DeliveryYear
            , DeliveryMonth*10000 + DeliveryYear
UNION ALL
       SELECT
              [DeliveryMonthShortNameYear] AS CalendarMonthShortNameYear
            , DeliveryMonth
            , DeliveryYear
            , DateSort = DeliveryMonth*10000 + DeliveryYear
            , 'Co-invested SFA' AS ValueType
            , SUM([Amount]) AS Value
            , ValueTypeSort = 5
         FROM [Data_Pub].[DAS_Payments]
        WHERE Flag_latest = 1
          AND FundingSource = 'CoInvestedSfa'
     GROUP BY
              [DeliveryMonthShortNameYear]
            , DeliveryMonth
            , DeliveryYear
            , DeliveryMonth*10000 + DeliveryYear;
GO
