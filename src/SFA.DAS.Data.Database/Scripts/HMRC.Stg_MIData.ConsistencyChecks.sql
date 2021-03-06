/*--------------------------------------------------------------------------------------
HMRC.Stg_MIData.ConsistencyChecks.sql

Jira: DATAMANAGE-482 : Data Consistnecy Checks for HMRC

Compare data on HMRC.Stg_MIData (stg) to Data_Load.DAS_LevyDeclarations (das) 
using source minus target and target minus source queries to ensure there are no
missing or surplus rows. Any rows returned from the queries are therefore exceptions 
that need to be investigated. 

Note this will need to be revisited once delta process is created as not all rows on 
LevyDeclarations will be on staging. Only rows since the last cut off. In future these 
will also wneed to become an SP package to run as pars of the monthly load routines. 
For now this is MVP to validate the full DB extract as part of UAT.
--------------------------------------------------------------------------------------*/

/*****************************
-- 1. submission_id
*************************** */
WITH cte_submission_id AS 
( SELECT 
    CASE WHEN submission_id IS NULL OR submission_id= '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( submission_id AS INT ) IS NULL THEN '' -- 'Not Integer' 
      WHEN submission_id < 1 THEN '' --'Low Value' -- need to find what min id is currently 
      ELSE submission_id
    END AS submission_id
    FROM HMRC.Stg_MIData
)
SELECT 'submission_id: on HMRC.Stg_MIData not on Data_Load.DAS_LevyDeclarations'
, submission_id
FROM cte_submission_id
WHERE submission_id <> '' 
EXCEPT 
SELECT 'submission_id: on HMRC.Stg_MIData not on Data_Load.DAS_LevyDeclarations'
, CONVERT( varchar, SubmissionId )
FROM Data_Load.DAS_LevyDeclarations
WHERE IsLatest = 1;


WITH cte_submission_id AS 
( SELECT 
   CASE WHEN submission_id IS NULL OR submission_id= '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( submission_id AS INT ) IS NULL THEN '' -- 'Not Integer' 
      WHEN submission_id < 1 THEN '' --'Low Value' -- need to find what min id is currently 
      ELSE submission_id
    END AS submission_id
    FROM HMRC.Stg_MIData
)
SELECT 'submission_id: on Data_Load.DAS_LevyDeclarations not on HMRC.Stg_MIData'
, CONVERT( varchar, SubmissionId ) AS SubmissionId
FROM Data_Load.DAS_LevyDeclarations
WHERE IsLatest = 1
EXCEPT 
SELECT 'submission_id: on Data_Load.DAS_LevyDeclarations not on HMRC.Stg_MIData'
, submission_id
FROM cte_submission_id
WHERE submission_id <> '' 
;

/* ***************************
-- 2. pay_scheme_reference
*************************** */

WITH cte_submission_id AS 
( SELECT 
    CASE WHEN submission_id IS NULL OR submission_id= '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( submission_id AS INT ) IS NULL THEN  '' -- 'Not Integer' 
      WHEN submission_id < 1 THEN '' -- 'Low Value' -- need to find what min id is currently 
      ELSE submission_id
    END AS submission_id
    , CASE WHEN paye_scheme_reference Is Null OR paye_scheme_reference = '' THEN '' -- 'Missing' 
        WHEN LEN(paye_scheme_reference) > 11 THEN '' -- '> 11 chars'
        WHEN LEN(paye_scheme_Reference) < 6 THEN  '' -- '< 6 chars'
        WHEN  paye_scheme_reference NOT LIKE '[0-9][0-9][0-9]/[A-Z0-9]%' THEN '' -- 'Format NOT 999/XXXXXXX'
        ELSE paye_scheme_reference 
      END AS paye_scheme_reference
    FROM HMRC.Stg_MIData
   WHERE submission_id IS NOT NULL AND submission_id <> ''
)
SELECT 'pay_scheme_reference: on HMRC.Stg_MIData not on Data_Load.DAS_LevyDeclarations'
, submission_id
, Paye_Scheme_Reference
FROM cte_submission_id
WHERE submission_id <> '' 
EXCEPT 
SELECT 'pay_scheme_reference: on HMRC.Stg_MIData not on Data_Load.DAS_LevyDeclarations'
, CONVERT( varchar, SubmissionId ) AS submission_id
, PayeSchemeReference
FROM Data_Load.DAS_LevyDeclarations
WHERE IsLatest = 1;

WITH cte_submission_id AS 
( SELECT 
    CASE WHEN submission_id IS NULL OR submission_id= '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( submission_id AS INT ) IS NULL THEN  '' -- 'Not Integer' 
      WHEN submission_id < 1 THEN '' -- 'Low Value' -- need to find what min id is currently 
      ELSE submission_id
    END AS submission_id
    , CASE WHEN paye_scheme_reference Is Null OR paye_scheme_reference = '' THEN '' -- 'Missing' 
        WHEN LEN(paye_scheme_reference) > 11 THEN '' -- '> 11 chars'
        WHEN LEN(paye_scheme_Reference) < 6 THEN  '' -- '< 6 chars'
        WHEN  paye_scheme_reference NOT LIKE '[0-9][0-9][0-9]/[A-Z0-9]%' THEN '' -- 'Format NOT 999/XXXXXXX'
        ELSE paye_scheme_reference 
      END AS paye_scheme_reference
    FROM HMRC.Stg_MIData
   WHERE submission_id IS NOT NULL AND submission_id <> ''
)
SELECT 'pay_scheme_reference: on Data_Load.DAS_LevyDeclarations not on HMRC.Stg_MIData'
, CONVERT( varchar, SubmissionId ) AS submission_id
, PayeSchemeReference
FROM Data_Load.DAS_LevyDeclarations
WHERE IsLatest = 1
EXCEPT 
SELECT 'pay_scheme_reference: on Data_Load.DAS_LevyDeclarations not on HMRC.Stg_MIData'
, submission_id
, Paye_Scheme_Reference
FROM cte_submission_id
WHERE submission_id <> '' 
;

/* ***************************
-- 3. submission_date
*************************** */

WITH cte_submission_date AS 
( SELECT 
    CASE WHEN submission_id IS NULL OR submission_id= '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( submission_id AS INT ) IS NULL THEN  '' -- 'Not Integer' 
      WHEN submission_id < 1 THEN '' -- 'Low Value' -- need to find what min id is currently 
      ELSE submission_id
    END AS submission_id
    , CASE WHEN paye_scheme_reference Is Null OR paye_scheme_reference = '' THEN '' -- 'Missing' 
        WHEN LEN(paye_scheme_reference) > 11 THEN '' -- '> 11 chars'
        WHEN LEN(paye_scheme_Reference) < 6 THEN  '' -- '< 6 chars'
        WHEN  paye_scheme_reference NOT LIKE '[0-9][0-9][0-9]/[A-Z0-9]%' THEN '' -- 'Format NOT 999/XXXXXXX'
        ELSE paye_scheme_reference 
      END AS paye_scheme_reference
    , CASE WHEN (submission_date) IS NULL OR submission_date = '' THEN '' -- 'NULL'
      WHEN ISDATE( submission_date ) = 0 THEN '' -- 'Invalid Date' 
      WHEN submission_date NOT LIKE '[2][0][0-9][0-9]-[0-9][0-9]-[0-9][0-9]'  THEN '' -- 'Invalid Date yyyy-mm-dd'  
      ELSE submission_date
    END AS submission_date
    FROM HMRC.Stg_MIData
   WHERE submission_id IS NOT NULL AND submission_id <> ''
)
SELECT 'submission_date: on HMRC.Stg_MIData not on Data_Load.DAS_LevyDeclarations'
, submission_id
, Paye_Scheme_Reference
, submission_date
FROM cte_submission_date
WHERE submission_id <> '' 
EXCEPT 
SELECT 'submission_date: on HMRC.Stg_MIData not on Data_Load.DAS_LevyDeclarations'
, CONVERT( varchar, SubmissionId ) AS SubmissionId
, PayeSchemeReference
, SUBSTRING( CONVERT( varchar, SubmissionDate, 120 ), 1, 10) as SubmissionDate
FROM Data_Load.DAS_LevyDeclarations
WHERE IsLatest = 1;

WITH cte_submission_date AS 
( SELECT 
    CASE WHEN submission_id IS NULL OR submission_id= '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( submission_id AS INT ) IS NULL THEN  '' -- 'Not Integer' 
      WHEN submission_id < 1 THEN '' -- 'Low Value' -- need to find what min id is currently 
      ELSE submission_id
    END AS submission_id
    , CASE WHEN paye_scheme_reference Is Null OR paye_scheme_reference = '' THEN '' -- 'Missing' 
        WHEN LEN(paye_scheme_reference) > 11 THEN '' -- '> 11 chars'
        WHEN LEN(paye_scheme_Reference) < 6 THEN  '' -- '< 6 chars'
        WHEN  paye_scheme_reference NOT LIKE '[0-9][0-9][0-9]/[A-Z0-9]%' THEN '' -- 'Format NOT 999/XXXXXXX'
        ELSE paye_scheme_reference 
      END AS paye_scheme_reference
    , CASE WHEN (submission_date) IS NULL OR submission_date = '' THEN '' -- 'NULL'
      WHEN ISDATE( submission_date ) = 0 THEN '' -- 'Invalid Date' 
      WHEN submission_date NOT LIKE '[2][0][0-9][0-9]-[0-9][0-9]-[0-9][0-9]'  THEN '' -- 'Invalid Date yyyy-mm-dd'  
      ELSE submission_date
    END AS submission_date
    FROM HMRC.Stg_MIData
   WHERE submission_id IS NOT NULL AND submission_id <> ''
)
SELECT 'submission_date: on Data_Load.DAS_LevyDeclarations not on HMRC.Stg_MIData'
, CONVERT( varchar, SubmissionId ) AS SubmissionId
, PayeSchemeReference
, SUBSTRING( CONVERT( varchar, SubmissionDate, 120 ), 1, 10) as SubmissionDate
FROM Data_Load.DAS_LevyDeclarations
WHERE IsLatest = 1
EXCEPT
SELECT 'submission_date: on Data_Load.DAS_LevyDeclarations not on HMRC.Stg_MIData'
, submission_id
, Paye_Scheme_Reference
, submission_date
FROM cte_submission_date
WHERE submission_id <> '' 
;

/* *************************************************
-- 4/5. payroll_year/ payroll_month - done together.
***************************************************/*


WITH cte_submission_yrmth AS 
( SELECT
    CASE WHEN submission_id IS NULL OR submission_id= '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( submission_id AS INT ) IS NULL THEN  '' -- 'Not Integer' 
      WHEN submission_id < 1 THEN '' -- 'Low Value' -- need to find what min id is currently 
      ELSE submission_id
    END AS submission_id
    , CASE WHEN paye_scheme_reference Is Null OR paye_scheme_reference = '' THEN '' -- 'Missing' 
        WHEN LEN(paye_scheme_reference) > 11 THEN '' -- '> 11 chars'
        WHEN LEN(paye_scheme_Reference) < 6 THEN  '' -- '< 6 chars'
        WHEN  paye_scheme_reference NOT LIKE '[0-9][0-9][0-9]/[A-Z0-9]%' THEN '' -- 'Format NOT 999/XXXXXXX'
        ELSE paye_scheme_reference 
      END AS paye_scheme_reference
  , CASE WHEN payroll_year IS NULL OR payroll_year = '' THEN '' -- 'Missing' 
      WHEN LEN(payroll_year) > 5 THEN '' -- '> 5 chars yy-yy'
      WHEN LEN(payroll_year) < 5 THEN '' -- '< 5 chars yy-yy'
  -- next section gets the year from and to and checks only 1 year - checking they are numbers first - isnumeric returns 0 if not to stop errors.
      WHEN CONVERT(int, SUBSTRING(payroll_year, 4, 2) ) - CONVERT(int, SUBSTRING(payroll_year, 1, 2) ) <> 1 THEN '' -- 'Year Range <> 1 yy-yy'
      ELSE Payroll_Year  
    END AS payroll_year
  , CASE WHEN payroll_month Is Null OR payroll_month = '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( payroll_month AS INT ) IS NULL THEN '' -- 'Not Integer' 
      WHEN payroll_month  NOT BETWEEN 1 AND 12 THEN '' -- 'Invalid Month (1-12)' 
      ELSE Payroll_Month
    END AS payroll_month
    FROM HMRC.Stg_MIData
    WHERE submission_id IS NOT NULL AND submission_id <> ''
)
SELECT 'Payroll_Year/Payroll_month: on HMRC.Stg_MIData not on Data_Load.DAS_LevyDeclarations'
, Submission_id
, Paye_Scheme_Reference
, Payroll_Year
, payroll_month
FROM cte_submission_yrmth
WHERE submission_id <> '' AND submission_id <> ''
EXCEPT 
SELECT 'Payroll_Year/Payroll_month: on HMRC.Stg_MIData not on Data_Load.DAS_LevyDeclarations'
, SubmissionId
, PayeSchemeReference
, CONVERT( varchar, PayrollYear) AS PayrollYear
, CONVERT(varchar, PayrollMonth) AS PayrollMonth
FROM Data_Load.DAS_LevyDeclarations
WHERE IsLatest = 1;

WITH cte_submission_yrmth AS 
( SELECT
    CASE WHEN submission_id IS NULL OR submission_id= '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( submission_id AS INT ) IS NULL THEN  '' -- 'Not Integer' 
      WHEN submission_id < 1 THEN '' -- 'Low Value' -- need to find what min id is currently 
      ELSE submission_id
    END AS submission_id
    , CASE WHEN paye_scheme_reference Is Null OR paye_scheme_reference = '' THEN '' -- 'Missing' 
        WHEN LEN(paye_scheme_reference) > 11 THEN '' -- '> 11 chars'
        WHEN LEN(paye_scheme_Reference) < 6 THEN  '' -- '< 6 chars'
        WHEN  paye_scheme_reference NOT LIKE '[0-9][0-9][0-9]/[A-Z0-9]%' THEN '' -- 'Format NOT 999/XXXXXXX'
        ELSE paye_scheme_reference 
      END AS paye_scheme_reference
  , CASE WHEN payroll_year IS NULL OR payroll_year = '' THEN '' -- 'Missing' 
      WHEN LEN(payroll_year) > 5 THEN '' -- '> 5 chars yy-yy'
      WHEN LEN(payroll_year) < 5 THEN '' -- '< 5 chars yy-yy'
  -- next section gets the year from and to and checks only 1 year - checking they are numbers first - isnumeric returns 0 if not to stop errors.
      WHEN CONVERT(int, SUBSTRING(payroll_year, 4, 2) ) - CONVERT(int, SUBSTRING(payroll_year, 1, 2) ) <> 1 THEN '' -- 'Year Range <> 1 yy-yy'
      ELSE Payroll_Year  
    END AS payroll_year
  , CASE WHEN payroll_month Is Null OR payroll_month = '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( payroll_month AS INT ) IS NULL THEN '' -- 'Not Integer' 
      WHEN payroll_month  NOT BETWEEN 1 AND 12 THEN '' -- 'Invalid Month (1-12)' 
      ELSE Payroll_Month
    END AS payroll_month
    FROM HMRC.Stg_MIData
    WHERE submission_id IS NOT NULL AND submission_id <> ''
)
SELECT 'Payroll_Year/Payroll_month: on Data_Load.DAS_LevyDeclarations not on HMRC.Stg_MIData'
, PayeSchemeReference
, CONVERT( varchar, PayrollYear) AS PayrollYear
, CONVERT(varchar, PayrollMonth) AS PayrollMonth
FROM Data_Load.DAS_LevyDeclarations
WHERE IsLatest = 1
EXCEPT 
SELECT 'Payroll_Year/Payroll_month: on Data_Load.DAS_LevyDeclarations not on HMRC.Stg_MIData'
, Paye_Scheme_Reference
, Payroll_Year
, payroll_month
FROM cte_submission_yrmth
WHERE submission_id <> '' AND submission_id <> ''
;

/********************************
-- 6. levy_due_year_to_date
********************************/

WITH cte_submission_levydue AS 
( SELECT
    CASE WHEN submission_id IS NULL OR submission_id= '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( submission_id AS INT ) IS NULL THEN  '' -- 'Not Integer' 
      WHEN submission_id < 1 THEN '' -- 'Low Value' -- need to find what min id is currently 
      ELSE submission_id
    END AS submission_id
    , CASE WHEN paye_scheme_reference Is Null OR paye_scheme_reference = '' THEN '' -- 'Missing' 
        WHEN LEN(paye_scheme_reference) > 11 THEN '' -- '> 11 chars'
        WHEN LEN(paye_scheme_Reference) < 6 THEN  '' -- '< 6 chars'
        WHEN  paye_scheme_reference NOT LIKE '[0-9][0-9][0-9]/[A-Z0-9]%' THEN '' -- 'Format NOT 999/XXXXXXX'
        ELSE paye_scheme_reference 
      END AS paye_scheme_reference
  , CASE WHEN payroll_year IS NULL OR payroll_year = '' THEN '' -- 'Missing' 
      WHEN LEN(payroll_year) > 5 THEN '' -- '> 5 chars yy-yy'
      WHEN LEN(payroll_year) < 5 THEN '' -- '< 5 chars yy-yy'
  -- next section gets the year from and to and checks only 1 year - checking they are numbers first - isnumeric returns 0 if not to stop errors.
      WHEN CONVERT(int, SUBSTRING(payroll_year, 4, 2) ) - CONVERT(int, SUBSTRING(payroll_year, 1, 2) ) <> 1 THEN '' -- 'Year Range <> 1 yy-yy'
      ELSE Payroll_Year  
    END AS payroll_year
  , CASE WHEN payroll_month Is Null OR payroll_month = '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( payroll_month AS INT ) IS NULL THEN '' -- 'Not Integer' 
      WHEN payroll_month  NOT BETWEEN 1 AND 12 THEN '' -- 'Invalid Month (1-12)' 
      ELSE Payroll_Month
    END AS payroll_month
  , CASE WHEN levy_due_year_to_date IS NULL OR levy_due_year_to_date = '' THEN '' -- 'Missing' 
      WHEN TRY_CAST(levy_due_year_to_date AS DECIMAL(12,2)) IS NULL THEN '' -- 'Not Decimal 1234567890.12' 
      WHEN CONVERT( decimal(12,2), levy_due_year_to_date ) < 0 THEN '' -- 'Negative' 
      -- try to get decimal check working - whole pence? 
      --WHEN PATINDEX('%.%', levy_due_year_to_Date) > 0 THEN  -- found . so extrat to end to check chars after . 
      --  CASE WHEN LEN(SUBSTRING( levy_due_year_to_Date, PATINDEX('%.%', levy_due_year_to_Date), LEN(levy_due_year_to_Date)))
      --    > 2 THEN 'Too many decmials'
      --  ELSE ''
      --  END
      WHEN LEN(levy_due_year_to_date) > 12 THEN '' -- 'Too long 123456789.12'
      ELSE levy_due_year_to_date
    END AS levy_due_year_to_date
    FROM HMRC.Stg_MIData
    WHERE submission_id IS NOT NULL AND submission_id <> ''
)
SELECT 'levy_due_year_to_date: on HMRC.Stg_MIData not on Data_Load.DAS_LevyDeclarations'
, submission_id
, Paye_Scheme_Reference
, Payroll_Year
, payroll_month
, levy_due_year_to_date
FROM cte_submission_levydue
WHERE submission_id <> '' AND submission_id <> ''
EXCEPT 
SELECT 'levy_due_year_to_date: on HMRC.Stg_MIData not on Data_Load.DAS_LevyDeclarations'
, SubmissionId
, PayeSchemeReference
, CONVERT( varchar, PayrollYear) AS PayrollYear
, CONVERT(varchar, PayrollMonth) AS PayrollMonth
, CONVERT(varchar, LevyDueYearToDate) AS LevyDueYearToDate
FROM Data_Load.DAS_LevyDeclarations
WHERE IsLatest = 1
;

WITH cte_submission_levydue AS 
( SELECT
    CASE WHEN submission_id IS NULL OR submission_id= '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( submission_id AS INT ) IS NULL THEN  '' -- 'Not Integer' 
      WHEN submission_id < 1 THEN '' -- 'Low Value' -- need to find what min id is currently 
      ELSE submission_id
    END AS submission_id
    , CASE WHEN paye_scheme_reference Is Null OR paye_scheme_reference = '' THEN '' -- 'Missing' 
        WHEN LEN(paye_scheme_reference) > 11 THEN '' -- '> 11 chars'
        WHEN LEN(paye_scheme_Reference) < 6 THEN  '' -- '< 6 chars'
        WHEN  paye_scheme_reference NOT LIKE '[0-9][0-9][0-9]/[A-Z0-9]%' THEN '' -- 'Format NOT 999/XXXXXXX'
        ELSE paye_scheme_reference 
      END AS paye_scheme_reference
  , CASE WHEN payroll_year IS NULL OR payroll_year = '' THEN '' -- 'Missing' 
      WHEN LEN(payroll_year) > 5 THEN '' -- '> 5 chars yy-yy'
      WHEN LEN(payroll_year) < 5 THEN '' -- '< 5 chars yy-yy'
  -- next section gets the year from and to and checks only 1 year - checking they are numbers first - isnumeric returns 0 if not to stop errors.
      WHEN CONVERT(int, SUBSTRING(payroll_year, 4, 2) ) - CONVERT(int, SUBSTRING(payroll_year, 1, 2) ) <> 1 THEN '' -- 'Year Range <> 1 yy-yy'
      ELSE Payroll_Year  
    END AS payroll_year
  , CASE WHEN payroll_month Is Null OR payroll_month = '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( payroll_month AS INT ) IS NULL THEN '' -- 'Not Integer' 
      WHEN payroll_month  NOT BETWEEN 1 AND 12 THEN '' -- 'Invalid Month (1-12)' 
      ELSE Payroll_Month
    END AS payroll_month
  , CASE WHEN levy_due_year_to_date IS NULL OR levy_due_year_to_date = '' THEN '' -- 'Missing' 
      WHEN TRY_CAST(levy_due_year_to_date AS DECIMAL(12,2)) IS NULL THEN '' -- 'Not Decimal 1234567890.12' 
      WHEN CONVERT( decimal(12,2), levy_due_year_to_date ) < 0 THEN '' -- 'Negative' 
      -- try to get decimal check working - whole pence? 
      --WHEN PATINDEX('%.%', levy_due_year_to_Date) > 0 THEN  -- found . so extrat to end to check chars after . 
      --  CASE WHEN LEN(SUBSTRING( levy_due_year_to_Date, PATINDEX('%.%', levy_due_year_to_Date), LEN(levy_due_year_to_Date)))
      --    > 2 THEN 'Too many decmials'
      --  ELSE ''
      --  END
      WHEN LEN(levy_due_year_to_date) > 12 THEN '' -- 'Too long 123456789.12'
      ELSE levy_due_year_to_date
    END AS levy_due_year_to_date
    FROM HMRC.Stg_MIData
    WHERE submission_id IS NOT NULL AND submission_id <> ''
)
SELECT 'levy_due_year_to_date: on Data_Load.DAS_LevyDeclarations not on HMRC.Stg_MIData'
, submission_id
, Paye_Scheme_Reference
, Payroll_Year
, payroll_month
, levy_due_year_to_date
FROM cte_submission_levydue
WHERE submission_id <> '' AND submission_id <> ''
EXCEPT 
SELECT 'levy_due_year_to_date: on Data_Load.DAS_LevyDeclarations not on HMRC.Stg_MIData'
, SubmissionId
, PayeSchemeReference
, CONVERT( varchar, PayrollYear) AS PayrollYear
, CONVERT(varchar, PayrollMonth) AS PayrollMonth
, CONVERT(varchar, LevyDueYearToDate) AS LevyDueYearToDate
FROM Data_Load.DAS_LevyDeclarations
WHERE IsLatest = 1
;

/*************************** 
-- 7. ef (English fraction)
*************************** */

WITH cte_submission_levydue AS 
( SELECT
    CASE WHEN submission_id IS NULL OR submission_id= '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( submission_id AS INT ) IS NULL THEN  '' -- 'Not Integer' 
      WHEN submission_id < 1 THEN '' -- 'Low Value' -- need to find what min id is currently 
      ELSE submission_id
    END AS submission_id
    , CASE WHEN paye_scheme_reference Is Null OR paye_scheme_reference = '' THEN '' -- 'Missing' 
        WHEN LEN(paye_scheme_reference) > 11 THEN '' -- '> 11 chars'
        WHEN LEN(paye_scheme_Reference) < 6 THEN  '' -- '< 6 chars'
        WHEN  paye_scheme_reference NOT LIKE '[0-9][0-9][0-9]/[A-Z0-9]%' THEN '' -- 'Format NOT 999/XXXXXXX'
        ELSE paye_scheme_reference 
      END AS paye_scheme_reference
  , CASE WHEN payroll_year IS NULL OR payroll_year = '' THEN '' -- 'Missing' 
      WHEN LEN(payroll_year) > 5 THEN '' -- '> 5 chars yy-yy'
      WHEN LEN(payroll_year) < 5 THEN '' -- '< 5 chars yy-yy'
  -- next section gets the year from and to and checks only 1 year - checking they are numbers first - isnumeric returns 0 if not to stop errors.
      WHEN CONVERT(int, SUBSTRING(payroll_year, 4, 2) ) - CONVERT(int, SUBSTRING(payroll_year, 1, 2) ) <> 1 THEN '' -- 'Year Range <> 1 yy-yy'
      ELSE Payroll_Year  
    END AS payroll_year
  , CASE WHEN payroll_month Is Null OR payroll_month = '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( payroll_month AS INT ) IS NULL THEN '' -- 'Not Integer' 
      WHEN payroll_month  NOT BETWEEN 1 AND 12 THEN '' -- 'Invalid Month (1-12)' 
      ELSE Payroll_Month
    END AS payroll_month
  , CASE WHEN levy_due_year_to_date IS NULL OR levy_due_year_to_date = '' THEN '' -- 'Missing' 
      WHEN TRY_CAST(levy_due_year_to_date AS DECIMAL(12,2)) IS NULL THEN '' -- 'Not Decimal 1234567890.12' 
      WHEN CONVERT( decimal(12,2), levy_due_year_to_date ) < 0 THEN '' -- 'Negative' 
      -- try to get decimal check working - whole pence? 
      --WHEN PATINDEX('%.%', levy_due_year_to_Date) > 0 THEN  -- found . so extrat to end to check chars after . 
      --  CASE WHEN LEN(SUBSTRING( levy_due_year_to_Date, PATINDEX('%.%', levy_due_year_to_Date), LEN(levy_due_year_to_Date)))
      --    > 2 THEN 'Too many decmials'
      --  ELSE ''
      --  END
      WHEN LEN(levy_due_year_to_date) > 12 THEN '' -- 'Too long 123456789.12'
      ELSE levy_due_year_to_date
    END AS levy_due_year_to_date
  , CASE WHEN EnglishFraction IS NULL OR EnglishFraction = '' THEN '' -- 'Missing' 
      WHEN TRY_CAST(EnglishFraction AS DECIMAL(6,5)) IS NULL THEN '' -- 'Not Decimal 1.23456'
      WHEN CONVERT( decimal(6,5), EnglishFraction ) < 0 THEN '' -- 'Negative' 
      WHEN CONVERT( decimal(6,5), EnglishFraction ) > 1 THEN '' -- '> 1 Not A Fraction' 
      WHEN LEN(EnglishFraction) > 7 THEN '' -- 'Length > 7'
      ELSE EnglishFraction
    END AS EnglishFraction
    FROM HMRC.Stg_MIData
    WHERE submission_id IS NOT NULL AND submission_id <> ''
)
SELECT 'EnglishFraction: on HMRC.Stg_MIData not on Data_Load.DAS_LevyDeclarations'
, submission_id
, Paye_Scheme_Reference
, Payroll_Year
, payroll_month
--, levy_due_year_to_date
, EnglishFraction
FROM cte_submission_levydue
WHERE submission_id <> '' AND submission_id <> ''
EXCEPT 
SELECT 'EnglishFraction: on HMRC.Stg_MIData not on Data_Load.DAS_LevyDeclarations'
, SubmissionId
, PayeSchemeReference
, CONVERT( varchar, PayrollYear) AS PayrollYear
, CONVERT(varchar, PayrollMonth) AS PayrollMonth
--, CONVERT(varchar, LevyDueYearToDate) AS LevyDueYearToDate
, CONVERT(varchar, EnglishFraction) AS EnglishFraction
FROM Data_Load.DAS_LevyDeclarations
WHERE IsLatest = 1
;

WITH cte_submission_levydue AS 
( SELECT
    CASE WHEN submission_id IS NULL OR submission_id= '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( submission_id AS INT ) IS NULL THEN  '' -- 'Not Integer' 
      WHEN submission_id < 1 THEN '' -- 'Low Value' -- need to find what min id is currently 
      ELSE submission_id
    END AS submission_id
    , CASE WHEN paye_scheme_reference Is Null OR paye_scheme_reference = '' THEN '' -- 'Missing' 
        WHEN LEN(paye_scheme_reference) > 11 THEN '' -- '> 11 chars'
        WHEN LEN(paye_scheme_Reference) < 6 THEN  '' -- '< 6 chars'
        WHEN  paye_scheme_reference NOT LIKE '[0-9][0-9][0-9]/[A-Z0-9]%' THEN '' -- 'Format NOT 999/XXXXXXX'
        ELSE paye_scheme_reference 
      END AS paye_scheme_reference
  , CASE WHEN payroll_year IS NULL OR payroll_year = '' THEN '' -- 'Missing' 
      WHEN LEN(payroll_year) > 5 THEN '' -- '> 5 chars yy-yy'
      WHEN LEN(payroll_year) < 5 THEN '' -- '< 5 chars yy-yy'
  -- next section gets the year from and to and checks only 1 year - checking they are numbers first - isnumeric returns 0 if not to stop errors.
      WHEN CONVERT(int, SUBSTRING(payroll_year, 4, 2) ) - CONVERT(int, SUBSTRING(payroll_year, 1, 2) ) <> 1 THEN '' -- 'Year Range <> 1 yy-yy'
      ELSE Payroll_Year  
    END AS payroll_year
  , CASE WHEN payroll_month Is Null OR payroll_month = '' THEN '' -- 'Missing' 
      WHEN TRY_CAST( payroll_month AS INT ) IS NULL THEN '' -- 'Not Integer' 
      WHEN payroll_month  NOT BETWEEN 1 AND 12 THEN '' -- 'Invalid Month (1-12)' 
      ELSE Payroll_Month
    END AS payroll_month
  , CASE WHEN levy_due_year_to_date IS NULL OR levy_due_year_to_date = '' THEN '' -- 'Missing' 
      WHEN TRY_CAST(levy_due_year_to_date AS DECIMAL(12,2)) IS NULL THEN '' -- 'Not Decimal 1234567890.12' 
      WHEN CONVERT( decimal(12,2), levy_due_year_to_date ) < 0 THEN '' -- 'Negative' 
      -- try to get decimal check working - whole pence? 
      --WHEN PATINDEX('%.%', levy_due_year_to_Date) > 0 THEN  -- found . so extrat to end to check chars after . 
      --  CASE WHEN LEN(SUBSTRING( levy_due_year_to_Date, PATINDEX('%.%', levy_due_year_to_Date), LEN(levy_due_year_to_Date)))
      --    > 2 THEN 'Too many decmials'
      --  ELSE ''
      --  END
      WHEN LEN(levy_due_year_to_date) > 12 THEN '' -- 'Too long 123456789.12'
      ELSE levy_due_year_to_date
    END AS levy_due_year_to_date
  , CASE WHEN EnglishFraction IS NULL OR EnglishFraction = '' THEN '' -- 'Missing' 
      WHEN TRY_CAST(EnglishFraction AS DECIMAL(6,5)) IS NULL THEN '' -- 'Not Decimal 1.23456'
      WHEN CONVERT( decimal(6,5), EnglishFraction ) < 0 THEN '' -- 'Negative' 
      WHEN CONVERT( decimal(6,5), EnglishFraction ) > 1 THEN '' -- '> 1 Not A Fraction' 
      WHEN LEN(EnglishFraction) > 7 THEN '' -- 'Length > 7'
      ELSE EnglishFraction
    END AS EnglishFraction
    FROM HMRC.Stg_MIData
    WHERE submission_id IS NOT NULL AND submission_id <> ''
)
SELECT 'EnglishFraction: on  Data_Load.DAS_LevyDeclarations not on HMRC.Stg_MIData'
, SubmissionId
, PayeSchemeReference
, CONVERT( varchar, PayrollYear) AS PayrollYear
, CONVERT(varchar, PayrollMonth) AS PayrollMonth
, CONVERT(varchar, EnglishFraction) AS EnglishFraction
FROM Data_Load.DAS_LevyDeclarations
WHERE IsLatest = 1
EXCEPT 
SELECT 'EnglishFraction: on  Data_Load.DAS_LevyDeclarations not on HMRC.Stg_MIData'
, submission_id
, Paye_Scheme_Reference
, Payroll_Year
, payroll_month
, EnglishFraction
FROM cte_submission_levydue
WHERE submission_id <> '' AND submission_id <> ''
;

-- The End