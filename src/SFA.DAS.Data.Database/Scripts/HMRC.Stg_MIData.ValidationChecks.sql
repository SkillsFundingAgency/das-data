-- Validation checks on HRMC.Stg_MIData. 
-- Check for null when required, length, precision, format 

SET LANGUAGE british; -- stop dates like 13/01/2019 being rejected as invalid due to default USA format
SET DATEFORMAT ymd; -- set default date expection to yyyy-mm-dd

WITH cte_data_quality_checks AS
(
  -- create a row number to allow outer query to limit rows to check.
  SELECT 
    ROW_NUMBER () OVER ( ORDER BY submission_id, submission_date, paye_scheme_reference, accounting_office_reference ) AS RowNum

  -- Test 1: pay_scheme_reference STRING 999/XXXXXXX  
  , paye_scheme_reference 
  , CASE WHEN paye_scheme_reference Is Null OR paye_scheme_reference = '' THEN 'Missing' 
      WHEN LEN(paye_scheme_reference) > 11 THEN '> 11 chars'
      WHEN LEN(paye_scheme_Reference) < 6 THEN  '< 6 chars'
      WHEN  paye_scheme_reference NOT LIKE '[0-9][0-9][0-9]/[A-Z0-9]%' THEN 'Format NOT 999/XXXXXXX'
      ELSE ''
    END AS paye_scheme_reference_error

  -- Test 2: accounting_office_reference STRING xxxxxxxxxxxx
  , accounting_office_reference 
  , CASE WHEN accounting_office_reference Is Null OR accounting_office_reference = '' THEN 'Missing' 
      WHEN LEN(accounting_office_reference) > 12 THEN '> 12 Chars' 
      WHEN LEN(accounting_office_reference) < 6 THEN '< 6 Chars' 
      WHEN  accounting_office_reference NOT LIKE '[A-Z0-9]%' THEN 'NOT Alphanumeric [A-Z0-9]' 
      WHEN  PATINDEX('%-%', accounting_office_reference ) > 0 THEN 'NOT Alphanumeric [A-Z0-9]' -- extra check for - as SQLserver treats them as a range in like
      ELSE ''
    END AS accounting_office_reference_error

  -- Test 3: scheme_cessation_date STRING yyyy-MM-dd
  , scheme_cessation_date
  , CASE WHEN scheme_cessation_date IS NULL OR scheme_cessation_date = '' THEN '' -- Allowed to be empty
      WHEN ISDATE( scheme_cessation_date ) = 0 THEN 'Invalid Date yyyy-mm-mm' 
      WHEN scheme_cessation_date NOT LIKE '[2][0][0-9][0-9]-[0-9][0-9]-[0-9][0-9]'  THEN 'Invalid Date yyyy-mm-dd'  
      ELSE ''
    END AS scheme_cessation_date_error

 -- Test 4: submission_date STRING yyyy-MM-dd
  , submission_date
  , CASE WHEN (submission_date) IS NULL OR submission_date = '' THEN 'Missing'
      WHEN ISDATE( submission_date ) = 0 THEN 'Invalid Date yyyy-mm-dd' 
      WHEN submission_date NOT LIKE '[2][0][0-9][0-9]-[0-9][0-9]-[0-9][0-9]'  THEN 'Invalid Date yyyy-mm-dd'  
      ELSE ''
    END AS submission_date_error
 
   -- Test 5: submission_id INT 0
  , submission_id
   , CASE WHEN submission_id IS NULL OR submission_id= '' THEN 'Missing' 
      WHEN TRY_CAST( submission_id AS INT ) IS NULL THEN 'Not Integer' 
      WHEN submission_id < 10000 THEN 'Low Value' -- need to find what min id is currently 
      ELSE ''
    END AS submission_id_error

   -- Test 6: payroll_year STRING yy-yy
  , payroll_year
  , CASE WHEN payroll_year IS NULL OR payroll_year = '' THEN 'Missing' 
      WHEN LEN(payroll_year) > 5 THEN '> 5 chars yy-yy'
      WHEN LEN(payroll_year) < 5 THEN '< 5 chars yy-yy'
  -- next section gets the year from and to and checks only 1 year - checking they are numbers first - isnumeric returns 0 if not to stop errors.
      WHEN CONVERT(int, SUBSTRING(payroll_year, 4, 2) ) - CONVERT(int, SUBSTRING(payroll_year, 1, 2) ) <> 1 THEN 'Year Range <> 1 yy-yy'
      ELSE '' 
    END AS payroll_year_errors

  -- Test 7: payroll_month INT 0
  , payroll_month
    , CASE WHEN payroll_month Is Null OR payroll_month = '' THEN 'Missing' 
      WHEN TRY_CAST( payroll_month AS INT ) IS NULL THEN 'Not Integer' 
      WHEN payroll_month  NOT BETWEEN 1 AND 12 THEN 'Invalid Month (1-12)' 
      ELSE ''
    END AS payroll_month_error

  -- Test 8: levy_due_year_to_date NUMBER 0.00
  , levy_due_year_to_date
  , CASE WHEN levy_due_year_to_date IS NULL OR levy_due_year_to_date = '' THEN 'Missing' 
      WHEN TRY_CAST(levy_due_year_to_date AS DECIMAL(12,2)) IS NULL THEN 'Not Decimal 1234567890.12' 
      WHEN CONVERT( decimal(12,2), levy_due_year_to_date ) < 0 THEN 'Negative' 
      -- try to get decimal check working - whole pence? 
      --WHEN PATINDEX('%.%', levy_due_year_to_Date) > 0 THEN  -- found . so extrat to end to check chars after . 
      --  CASE WHEN LEN(SUBSTRING( levy_due_year_to_Date, PATINDEX('%.%', levy_due_year_to_Date), LEN(levy_due_year_to_Date)))
      --    > 2 THEN 'Too many decmials'
      --  ELSE ''
      --  END
      WHEN LEN(levy_due_year_to_date) > 12 THEN 'Too long 123456789.12'
      ELSE ''
    END AS levy_due_year_to_date_error
    
  -- Test 9: EnglishFraction NUMBER 0.00000
  , EnglishFraction
  , CASE WHEN EnglishFraction IS NULL OR EnglishFraction = '' THEN 'Missing' 
    WHEN TRY_CAST(EnglishFraction AS DECIMAL(6,5)) IS NULL THEN 'Not Decimal 1.23456'
    WHEN CONVERT( decimal(6,5), EnglishFraction ) < 0 THEN 'Negative' 
    WHEN CONVERT( decimal(6,5), EnglishFraction ) > 1 THEN '> 1 Not A Fraction' 
    WHEN LEN(EnglishFraction) > 7 THEN 'Length > 7'
    ELSE ''
  END AS EnglishFraction_Error

  -- Test 10: ef_date_calculated STRING yyyy-MM-dd
  , ef_date_calculated
  , CASE WHEN (ef_date_calculated) IS NULL OR ef_date_calculated = '' THEN 'Missing'
      WHEN ISDATE( ef_date_calculated ) = 0 THEN 'Invalid Date yyyy-mm-mm' 
      WHEN ef_date_calculated NOT LIKE '[2][0][0-9][0-9]-[0-9][0-9]-[0-9][0-9]' THEN 'Invalid Date yyyy-mm-dd'  
      ELSE ''
    END AS ef_date_calculated_error

  
  -- Test 11: Latest_EnglishFraction  NUMBER 0.00000
  , Latest_EnglishFraction
  , CASE WHEN Latest_EnglishFraction IS NULL OR Latest_EnglishFraction = '' THEN '' -- Missing is allowed.
      WHEN TRY_CAST(Latest_EnglishFraction AS DECIMAL(6,5)) IS NULL THEN 'Not Decimal 1.23456'
      WHEN CONVERT( decimal(6,5), Latest_EnglishFraction ) < 0 THEN 'Negative' 
      WHEN CONVERT( decimal(6,5), Latest_EnglishFraction ) > 1 THEN '>1 Not A Fraction' 
      WHEN LEN(Latest_EnglishFraction) > 7 THEN 'Length > 7'
      ELSE ''
    END AS Latest_EnglishFraction_error
  

  -- Test 12: latest_ef_date_calculated STRING yyyy-MM-dd
  , latest_ef_date_calculated
  , CASE WHEN (latest_ef_date_calculated) IS NULL OR latest_ef_date_calculated = '' THEN '' -- Missing is allowed.
      WHEN ISDATE( latest_ef_date_calculated ) = 0 THEN 'Invalid Date yyyy-mm-mm' 
      WHEN latest_ef_date_calculated NOT LIKE '[2][0][0-9][0-9]-[0-9][0-9]-[0-9][0-9]' THEN 'Invalid Date yyyy-mm-dd'
      ELSE ''
    END AS latest_ef_date_calculated_error
 FROM  HMRC.Stg_MIData
 
)
-- pull back all rows from CTE where there are errors.
SELECT * FROM cte_data_quality_checks dqc
WHERE   
--  RowNum BETWEEN 1 AND 1000 AND  -- this can be added in to limit rows to check
  ( paye_scheme_reference_error > '' OR 
    accounting_office_reference_error > '' OR 
    scheme_cessation_date_error > '' OR 
    submission_date_error > '' OR 
    submission_id_error > '' OR 
    payroll_year_errors > '' OR 
    payroll_month_error > '' OR 
    levy_due_year_to_date_error > '' OR 
    EnglishFraction_error > '' OR 
    ef_date_calculated_error > '' OR 
    Latest_EnglishFraction_error > '' OR 
    latest_ef_date_calculated_error> '' 
  ) 
  ;