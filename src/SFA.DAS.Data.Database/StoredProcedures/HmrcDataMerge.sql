﻿-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [HMRC].[HMRC_Merge]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
UPDATE [HMRC].[DATA-Staging] SET [tax_year] = '0' WHERE [tax_year] = ''
UPDATE [HMRC].[DATA-Staging] SET [tax_month] = '0' WHERE [tax_month] = ''
UPDATE [HMRC].[DATA-Staging] SET [emp_ref] = '0' WHERE [emp_ref] = ''
UPDATE [HMRC].[DATA-Staging] SET [accounting_office_reference] = '0' WHERE [accounting_office_reference] = ''
UPDATE [HMRC].[DATA-Staging] SET [tax_ref_utr] = '0' WHERE [tax_ref_utr] = ''
UPDATE [HMRC].[DATA-Staging] SET [levy_due_ytd] = '0' WHERE [levy_due_ytd] = ''
UPDATE [HMRC].[DATA-Staging] SET [annual_levy_allowance_amount] = '0' WHERE [annual_levy_allowance_amount] = ''
UPDATE [HMRC].[DATA-Staging] SET [ef_by_pay_bill_till_date] = '0' WHERE [ef_by_pay_bill_till_date] = ''
UPDATE [HMRC].[DATA-Staging] SET [employer_name] = '0' WHERE [employer_name] = ''
UPDATE [HMRC].[DATA-Staging] SET [employer_address_line_1] = '0' WHERE [employer_address_line_1] = ''
UPDATE [HMRC].[DATA-Staging] SET [employer_address_line_2] = '0' WHERE [employer_address_line_2] = ''
UPDATE [HMRC].[DATA-Staging] SET [employer_address_line_3] = '0' WHERE [employer_address_line_3] = ''
UPDATE [HMRC].[DATA-Staging] SET [employer_address_line_4] = '0' WHERE [employer_address_line_4] = ''
UPDATE [HMRC].[DATA-Staging] SET [employer_address_line_5] = '0' WHERE [employer_address_line_5] = ''
UPDATE [HMRC].[DATA-Staging] SET [employer_post_code] = '0' WHERE [employer_post_code] = ''
UPDATE [HMRC].[DATA-Staging] SET [employer_foreign_country] = '0' WHERE [employer_foreign_country] = ''
UPDATE [HMRC].[DATA-Staging] SET [cessation_date] = '0' WHERE [cessation_date] = ''
UPDATE [HMRC].[DATA-Staging] SET [correspondance_name] = '0' WHERE [correspondance_name] = ''
UPDATE [HMRC].[DATA-Staging] SET [correspondance_address_line_1] = '0' WHERE [correspondance_address_line_1] = ''
UPDATE [HMRC].[DATA-Staging] SET [correspondance_address_line_2] = '0' WHERE [correspondance_address_line_2] = ''
UPDATE [HMRC].[DATA-Staging] SET [correspondance_address_line_3] = '0' WHERE [correspondance_address_line_3] = ''
UPDATE [HMRC].[DATA-Staging] SET [correspondance_address_line_4] = '0' WHERE [correspondance_address_line_4] = ''
UPDATE [HMRC].[DATA-Staging] SET [correspondance_address_line_5] = '0' WHERE [correspondance_address_line_5] = ''
UPDATE [HMRC].[DATA-Staging] SET [correspondance_post_code] = '0' WHERE [correspondance_post_code] = ''
UPDATE [HMRC].[DATA-Staging] SET [correspondance_foreign_country] = '0' WHERE [correspondance_foreign_country] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_due_tax_month1] = '0' WHERE [historical_levy_due_tax_month1] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_ef_tax_month1] = '0' WHERE [historical_ef_tax_month1] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_allowance_tax_month1] = '0' WHERE [historical_levy_allowance_tax_month1] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_due_tax_month2] = '0' WHERE [historical_levy_due_tax_month2] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_ef_tax_month2] = '0' WHERE [historical_ef_tax_month2] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_allowance_tax_month2] = '0' WHERE [historical_levy_allowance_tax_month2] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_due_tax_month3] = '0' WHERE [historical_levy_due_tax_month3] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_ef_tax_month3] = '0' WHERE [historical_ef_tax_month3] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_allowance_tax_month3] = '0' WHERE [historical_levy_allowance_tax_month3] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_due_tax_month4] = '0' WHERE [historical_levy_due_tax_month4] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_ef_tax_month4] = '0' WHERE [historical_ef_tax_month4] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_allowance_tax_month4] = '0' WHERE [historical_levy_allowance_tax_month4] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_due_tax_month5] = '0' WHERE [historical_levy_due_tax_month5] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_ef_tax_month5] = '0' WHERE [historical_ef_tax_month5] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_allowance_tax_month5] = '0' WHERE [historical_levy_allowance_tax_month5] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_due_tax_month6] = '0' WHERE [historical_levy_due_tax_month6] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_ef_tax_month6] = '0' WHERE [historical_ef_tax_month6] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_allowance_tax_month6] = '0' WHERE [historical_levy_allowance_tax_month6] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_due_tax_month7] = '0' WHERE [historical_levy_due_tax_month7] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_ef_tax_month7] = '0' WHERE [historical_ef_tax_month7] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_allowance_tax_month7] = '0' WHERE [historical_levy_allowance_tax_month7] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_due_tax_month8] = '0' WHERE [historical_levy_due_tax_month8] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_ef_tax_month8] = '0' WHERE [historical_ef_tax_month8] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_allowance_tax_month8] = '0' WHERE [historical_levy_allowance_tax_month8] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_due_tax_month9] = '0' WHERE [historical_levy_due_tax_month9] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_ef_tax_month9] = '0' WHERE [historical_ef_tax_month9] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_allowance_tax_month9] = '0' WHERE [historical_levy_allowance_tax_month9] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_due_tax_month10] = '0' WHERE [historical_levy_due_tax_month10] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_ef_tax_month10] = '0' WHERE [historical_ef_tax_month10] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_allowance_tax_month10] = '0' WHERE [historical_levy_allowance_tax_month10] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_due_tax_month11] = '0' WHERE [historical_levy_due_tax_month11] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_ef_tax_month11] = '0' WHERE [historical_ef_tax_month11] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_levy_allowance_tax_month11] = '0' WHERE [historical_levy_allowance_tax_month11] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_levy_due_previous_1] = '0' WHERE [historical_adjustment_levy_due_previous_1] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_ef_previous_1] = '0' WHERE [historical_adjustment_ef_previous_1] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_levy_allowance_previous_1] = '0' WHERE [historical_adjustment_levy_allowance_previous_1] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_levy_due_previous_2] = '0' WHERE [historical_adjustment_levy_due_previous_2] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_ef_previous_2] = '0' WHERE [historical_adjustment_ef_previous_2] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_levy_allowance_previous_2] = '0' WHERE [historical_adjustment_levy_allowance_previous_2] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_levy_due_previous_3] = '0' WHERE [historical_adjustment_levy_due_previous_3] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_ef_previous_3] = '0' WHERE [historical_adjustment_ef_previous_3] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_levy_allowance_previous_3] = '0' WHERE [historical_adjustment_levy_allowance_previous_3] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_levy_due_previous_4] = '0' WHERE [historical_adjustment_levy_due_previous_4] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_ef_previous_4] = '0' WHERE [historical_adjustment_ef_previous_4] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_levy_allowance_previous_4] = '0' WHERE [historical_adjustment_levy_allowance_previous_4] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_levy_due_previous_5] = '0' WHERE [historical_adjustment_levy_due_previous_5] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_ef_previous_5] = '0' WHERE [historical_adjustment_ef_previous_5] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_levy_allowance_previous_5] = '0' WHERE [historical_adjustment_levy_allowance_previous_5] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_levy_due_previous_6] = '0' WHERE [historical_adjustment_levy_due_previous_6] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_ef_previous_6] = '0' WHERE [historical_adjustment_ef_previous_6] = ''
UPDATE [HMRC].[DATA-Staging] SET [historical_adjustment_levy_allowance_previous_6] = '0' WHERE [historical_adjustment_levy_allowance_previous_6] = ''

   MERGE  [HMRC].[DATA-Live] AS Target
USING (SELECT  l.[tax_year]
      ,l.[tax_month]
      ,l.[emp_ref]
      ,l.[accounting_office_reference]
      ,l.[tax_ref_utr]
      ,l.[levy_due_ytd]
      ,l.[annual_levy_allowance_amount]
      ,l.[ef_by_pay_bill_till_date]
      ,l.[employer_name]
      ,l.[employer_address_line_1]
      ,l.[employer_address_line_2]
      ,l.[employer_address_line_3]
      ,l.[employer_address_line_4]
      ,l.[employer_address_line_5]
      ,l.[employer_post_code]
      ,l.[employer_foreign_country]
      ,l.[cessation_date]
      ,l.[correspondance_name]
      ,l.[correspondance_address_line_1]
      ,l.[correspondance_address_line_2]
      ,l.[correspondance_address_line_3]
      ,l.[correspondance_address_line_4]
      ,l.[correspondance_address_line_5]
      ,l.[correspondance_post_code]
      ,l.[correspondance_foreign_country]
      ,l.[historical_levy_due_tax_month1]
      ,l.[historical_ef_tax_month1]
      ,l.[historical_levy_allowance_tax_month1]
      ,l.[historical_levy_due_tax_month2]
      ,l.[historical_ef_tax_month2]
      ,l.[historical_levy_allowance_tax_month2]
      ,l.[historical_levy_due_tax_month3]
      ,l.[historical_ef_tax_month3]
      ,l.[historical_levy_allowance_tax_month3]
      ,l.[historical_levy_due_tax_month4]
      ,l.[historical_ef_tax_month4]
      ,l.[historical_levy_allowance_tax_month4]
      ,l.[historical_levy_due_tax_month5]
      ,l.[historical_ef_tax_month5]
      ,l.[historical_levy_allowance_tax_month5]
      ,l.[historical_levy_due_tax_month6]
      ,l.[historical_ef_tax_month6]
      ,l.[historical_levy_allowance_tax_month6]
      ,l.[historical_levy_due_tax_month7]
      ,l.[historical_ef_tax_month7]
      ,l.[historical_levy_allowance_tax_month7]
      ,l.[historical_levy_due_tax_month8]
      ,l.[historical_ef_tax_month8]
      ,l.[historical_levy_allowance_tax_month8]
      ,l.[historical_levy_due_tax_month9]
      ,l.[historical_ef_tax_month9]
      ,l.[historical_levy_allowance_tax_month9]
      ,l.[historical_levy_due_tax_month10]
      ,l.[historical_ef_tax_month10]
      ,l.[historical_levy_allowance_tax_month10]
      ,l.[historical_levy_due_tax_month11]
      ,l.[historical_ef_tax_month11]
      ,l.[historical_levy_allowance_tax_month11]
      ,l.[historical_adjustment_levy_due_previous_1]
      ,l.[historical_adjustment_ef_previous_1]
      ,l.[historical_adjustment_levy_allowance_previous_1]
      ,l.[historical_adjustment_levy_due_previous_2]
      ,l.[historical_adjustment_ef_previous_2]
      ,l.[historical_adjustment_levy_allowance_previous_2]
      ,l.[historical_adjustment_levy_due_previous_3]
      ,l.[historical_adjustment_ef_previous_3]
      ,l.[historical_adjustment_levy_allowance_previous_3]
      ,l.[historical_adjustment_levy_due_previous_4]
      ,l.[historical_adjustment_ef_previous_4]
      ,l.[historical_adjustment_levy_allowance_previous_4]
      ,l.[historical_adjustment_levy_due_previous_5]
      ,l.[historical_adjustment_ef_previous_5]
      ,l.[historical_adjustment_levy_allowance_previous_5]
      ,l.[historical_adjustment_levy_due_previous_6]
      ,l.[historical_adjustment_ef_previous_6]
      ,l.[historical_adjustment_levy_allowance_previous_6] from [HMRC].[DATA-Staging] as l) AS Source
ON (Target.[emp_ref] = Source.[emp_ref] and Target.[tax_month] = source.[tax_month] and Target.[tax_year] = source.[tax_year]  )
When matched THEN 
	  update set 
	   [levy_due_ytd]= source.[levy_due_ytd]
      ,[annual_levy_allowance_amount] = source.[annual_levy_allowance_amount]
      ,[ef_by_pay_bill_till_date]=source.[ef_by_pay_bill_till_date]
      ,[employer_name]=source.[employer_name]
      ,[employer_address_line_1]=source.[employer_address_line_1]
      ,[employer_address_line_2]=source.[employer_address_line_2]
      ,[employer_address_line_3]=source.[employer_address_line_3]
      ,[employer_address_line_4]=source.[employer_address_line_4]
      ,[employer_address_line_5]=source.[employer_address_line_5]
      ,[employer_post_code]=source.[employer_post_code]
      ,[employer_foreign_country]=source.[employer_foreign_country]
      ,[cessation_date]=source.[cessation_date]
      ,[correspondance_name]=source.[correspondance_name]
      ,[correspondance_address_line_1]=source.[correspondance_address_line_1]
      ,[correspondance_address_line_2]=source.[correspondance_address_line_2]
      ,[correspondance_address_line_3]=source.[correspondance_address_line_3]
      ,[correspondance_address_line_4]=source.[correspondance_address_line_4]
      ,[correspondance_address_line_5]=source.[correspondance_address_line_5]
      ,[correspondance_post_code]=source.[correspondance_post_code]
      ,[correspondance_foreign_country]=source.[correspondance_foreign_country]
      ,[historical_levy_due_tax_month1]=source.[historical_levy_due_tax_month1]
      ,[historical_ef_tax_month1]=source.[historical_ef_tax_month1]
      ,[historical_levy_allowance_tax_month1]=source.[historical_levy_allowance_tax_month1]
      ,[historical_levy_due_tax_month2]=source.[historical_levy_due_tax_month2]
      ,[historical_ef_tax_month2]=source.[historical_ef_tax_month2]
      ,[historical_levy_allowance_tax_month2]=source.[historical_levy_allowance_tax_month2]
      ,[historical_levy_due_tax_month3]=source.[historical_levy_due_tax_month3]
      ,[historical_ef_tax_month3]=source.[historical_ef_tax_month3]
      ,[historical_levy_allowance_tax_month3]=source.[historical_levy_allowance_tax_month3]
      ,[historical_levy_due_tax_month4]=source.[historical_levy_due_tax_month4]
      ,[historical_ef_tax_month4]=source.[historical_ef_tax_month4]
      ,[historical_levy_allowance_tax_month4]=source.[historical_levy_allowance_tax_month4]
      ,[historical_levy_due_tax_month5]=source.[historical_levy_due_tax_month5]
      ,[historical_ef_tax_month5]=source.[historical_ef_tax_month5]
      ,[historical_levy_allowance_tax_month5]=source.[historical_levy_allowance_tax_month5]
      ,[historical_levy_due_tax_month6]=source.[historical_levy_due_tax_month6]
      ,[historical_ef_tax_month6]=source.[historical_ef_tax_month6]
      ,[historical_levy_allowance_tax_month6]=source.[historical_levy_allowance_tax_month6]
      ,[historical_levy_due_tax_month7]=source.[historical_levy_due_tax_month7]
      ,[historical_ef_tax_month7]=source.[historical_ef_tax_month7]
      ,[historical_levy_allowance_tax_month7]=source.[historical_levy_allowance_tax_month7]
      ,[historical_levy_due_tax_month8]=source.[historical_levy_due_tax_month8]
      ,[historical_ef_tax_month8]=source.[historical_ef_tax_month8]
      ,[historical_levy_allowance_tax_month8]=source.[historical_levy_allowance_tax_month8]
      ,[historical_levy_due_tax_month9]=source.[historical_levy_due_tax_month9]
      ,[historical_ef_tax_month9]=source.[historical_ef_tax_month9]
      ,[historical_levy_allowance_tax_month9]=source.[historical_levy_allowance_tax_month9]
      ,[historical_levy_due_tax_month10]=source.[historical_levy_due_tax_month10]
      ,[historical_ef_tax_month10]=source.[historical_ef_tax_month10]
      ,[historical_levy_allowance_tax_month10]=source.[historical_levy_allowance_tax_month10]
      ,[historical_levy_due_tax_month11]=source.[historical_levy_due_tax_month11]
      ,[historical_ef_tax_month11]=source.[historical_ef_tax_month11]
      ,[historical_levy_allowance_tax_month11]=source.[historical_levy_allowance_tax_month11]
      ,[historical_adjustment_levy_due_previous_1]=source.[historical_adjustment_levy_due_previous_1]
      ,[historical_adjustment_ef_previous_1]=source.[historical_adjustment_ef_previous_1]
      ,[historical_adjustment_levy_allowance_previous_1]=source.[historical_adjustment_levy_allowance_previous_1]
      ,[historical_adjustment_levy_due_previous_2]=source.[historical_adjustment_levy_due_previous_2]
      ,[historical_adjustment_ef_previous_2]=source.[historical_adjustment_ef_previous_2]
      ,[historical_adjustment_levy_allowance_previous_2]=source.[historical_adjustment_levy_allowance_previous_2]
      ,[historical_adjustment_levy_due_previous_3]=source.[historical_adjustment_levy_due_previous_3]
      ,[historical_adjustment_ef_previous_3]=source.[historical_adjustment_ef_previous_3]
      ,[historical_adjustment_levy_allowance_previous_3]=source.[historical_adjustment_levy_allowance_previous_3]
      ,[historical_adjustment_levy_due_previous_4]=source.[historical_adjustment_levy_due_previous_4]
      ,[historical_adjustment_ef_previous_4]=source.[historical_adjustment_ef_previous_4]
      ,[historical_adjustment_levy_allowance_previous_4]=source.[historical_adjustment_levy_allowance_previous_4]
      ,[historical_adjustment_levy_due_previous_5]=source.[historical_adjustment_levy_due_previous_5]
      ,[historical_adjustment_ef_previous_5]=source.[historical_adjustment_ef_previous_5]
      ,[historical_adjustment_levy_allowance_previous_5]=source.[historical_adjustment_levy_allowance_previous_5]
      ,[historical_adjustment_levy_due_previous_6]=source.[historical_adjustment_levy_due_previous_6]
      ,[historical_adjustment_ef_previous_6]=source.[historical_adjustment_ef_previous_6]
      ,[historical_adjustment_levy_allowance_previous_6]=source.[historical_adjustment_levy_allowance_previous_6]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ( [tax_year]
      ,[tax_month]
      ,[emp_ref]
      ,[accounting_office_reference]
      ,[tax_ref_utr]
      ,[levy_due_ytd]
      ,[annual_levy_allowance_amount]
      ,[ef_by_pay_bill_till_date]
      ,[employer_name]
      ,[employer_address_line_1]
      ,[employer_address_line_2]
      ,[employer_address_line_3]
      ,[employer_address_line_4]
      ,[employer_address_line_5]
      ,[employer_post_code]
      ,[employer_foreign_country]
      ,[cessation_date]
      ,[correspondance_name]
      ,[correspondance_address_line_1]
      ,[correspondance_address_line_2]
      ,[correspondance_address_line_3]
      ,[correspondance_address_line_4]
      ,[correspondance_address_line_5]
      ,[correspondance_post_code]
      ,[correspondance_foreign_country]
      ,[historical_levy_due_tax_month1]
      ,[historical_ef_tax_month1]
      ,[historical_levy_allowance_tax_month1]
      ,[historical_levy_due_tax_month2]
      ,[historical_ef_tax_month2]
      ,[historical_levy_allowance_tax_month2]
      ,[historical_levy_due_tax_month3]
      ,[historical_ef_tax_month3]
      ,[historical_levy_allowance_tax_month3]
      ,[historical_levy_due_tax_month4]
      ,[historical_ef_tax_month4]
      ,[historical_levy_allowance_tax_month4]
      ,[historical_levy_due_tax_month5]
      ,[historical_ef_tax_month5]
      ,[historical_levy_allowance_tax_month5]
      ,[historical_levy_due_tax_month6]
      ,[historical_ef_tax_month6]
      ,[historical_levy_allowance_tax_month6]
      ,[historical_levy_due_tax_month7]
      ,[historical_ef_tax_month7]
      ,[historical_levy_allowance_tax_month7]
      ,[historical_levy_due_tax_month8]
      ,[historical_ef_tax_month8]
      ,[historical_levy_allowance_tax_month8]
      ,[historical_levy_due_tax_month9]
      ,[historical_ef_tax_month9]
      ,[historical_levy_allowance_tax_month9]
      ,[historical_levy_due_tax_month10]
      ,[historical_ef_tax_month10]
      ,[historical_levy_allowance_tax_month10]
      ,[historical_levy_due_tax_month11]
      ,[historical_ef_tax_month11]
      ,[historical_levy_allowance_tax_month11]
      ,[historical_adjustment_levy_due_previous_1]
      ,[historical_adjustment_ef_previous_1]
      ,[historical_adjustment_levy_allowance_previous_1]
      ,[historical_adjustment_levy_due_previous_2]
      ,[historical_adjustment_ef_previous_2]
      ,[historical_adjustment_levy_allowance_previous_2]
      ,[historical_adjustment_levy_due_previous_3]
      ,[historical_adjustment_ef_previous_3]
      ,[historical_adjustment_levy_allowance_previous_3]
      ,[historical_adjustment_levy_due_previous_4]
      ,[historical_adjustment_ef_previous_4]
      ,[historical_adjustment_levy_allowance_previous_4]
      ,[historical_adjustment_levy_due_previous_5]
      ,[historical_adjustment_ef_previous_5]
      ,[historical_adjustment_levy_allowance_previous_5]
      ,[historical_adjustment_levy_due_previous_6]
      ,[historical_adjustment_ef_previous_6]
      ,[historical_adjustment_levy_allowance_previous_6])
    VALUES ( source.[tax_year]
      ,source.[tax_month]
      ,source.[emp_ref]
      ,source.[accounting_office_reference]
      ,source.[tax_ref_utr]
      ,source.[levy_due_ytd]
      ,source.[annual_levy_allowance_amount]
      ,source.[ef_by_pay_bill_till_date]
      ,source.[employer_name]
      ,source.[employer_address_line_1]
      ,source.[employer_address_line_2]
      ,source.[employer_address_line_3]
      ,source.[employer_address_line_4]
      ,source.[employer_address_line_5]
      ,source.[employer_post_code]
      ,source.[employer_foreign_country]
      ,source.[cessation_date]
      ,source.[correspondance_name]
      ,source.[correspondance_address_line_1]
      ,source.[correspondance_address_line_2]
      ,source.[correspondance_address_line_3]
      ,source.[correspondance_address_line_4]
      ,source.[correspondance_address_line_5]
      ,source.[correspondance_post_code]
      ,source.[correspondance_foreign_country]
      ,source.[historical_levy_due_tax_month1]
      ,source.[historical_ef_tax_month1]
      ,source.[historical_levy_allowance_tax_month1]
      ,source.[historical_levy_due_tax_month2]
      ,source.[historical_ef_tax_month2]
      ,source.[historical_levy_allowance_tax_month2]
      ,source.[historical_levy_due_tax_month3]
      ,source.[historical_ef_tax_month3]
      ,source.[historical_levy_allowance_tax_month3]
      ,source.[historical_levy_due_tax_month4]
      ,source.[historical_ef_tax_month4]
      ,source.[historical_levy_allowance_tax_month4]
      ,source.[historical_levy_due_tax_month5]
      ,source.[historical_ef_tax_month5]
      ,source.[historical_levy_allowance_tax_month5]
      ,source.[historical_levy_due_tax_month6]
      ,source.[historical_ef_tax_month6]
      ,source.[historical_levy_allowance_tax_month6]
      ,source.[historical_levy_due_tax_month7]
      ,source.[historical_ef_tax_month7]
      ,source.[historical_levy_allowance_tax_month7]
      ,source.[historical_levy_due_tax_month8]
      ,source.[historical_ef_tax_month8]
      ,source.[historical_levy_allowance_tax_month8]
      ,source.[historical_levy_due_tax_month9]
      ,source.[historical_ef_tax_month9]
      ,source.[historical_levy_allowance_tax_month9]
      ,source.[historical_levy_due_tax_month10]
      ,source.[historical_ef_tax_month10]
      ,source.[historical_levy_allowance_tax_month10]
      ,source.[historical_levy_due_tax_month11]
      ,source.[historical_ef_tax_month11]
      ,source.[historical_levy_allowance_tax_month11]
      ,source.[historical_adjustment_levy_due_previous_1]
      ,source.[historical_adjustment_ef_previous_1]
      ,source.[historical_adjustment_levy_allowance_previous_1]
      ,source.[historical_adjustment_levy_due_previous_2]
      ,source.[historical_adjustment_ef_previous_2]
      ,source.[historical_adjustment_levy_allowance_previous_2]
      ,source.[historical_adjustment_levy_due_previous_3]
      ,source.[historical_adjustment_ef_previous_3]
      ,source.[historical_adjustment_levy_allowance_previous_3]
      ,source.[historical_adjustment_levy_due_previous_4]
      ,source.[historical_adjustment_ef_previous_4]
      ,source.[historical_adjustment_levy_allowance_previous_4]
      ,source.[historical_adjustment_levy_due_previous_5]
      ,source.[historical_adjustment_ef_previous_5]
      ,source.[historical_adjustment_levy_allowance_previous_5]
      ,source.[historical_adjustment_levy_due_previous_6]
      ,source.[historical_adjustment_ef_previous_6]
      ,source.[historical_adjustment_levy_allowance_previous_6]);
	  
	

END

GO


