 CREATE VIEW Data_Pub.DAS_Employer_AccountTransactions
    AS 
       -- Amounts from HMRC   
          SELECT 
               LD.DasAccountId
          ,    LD.CreatedDate
		  ,	   LD.SubmissionDate
		  ,    LD.PayrollMonth
		  ,    LD.PayrollYear
		  ,    NULL AS CollectionMonth
		  ,    NULL AS CollectionYear
		  ,	   NULL As CollectionPeriodName	
          ,    'IN Levy Money from HMRC' AS TransactionType
          ,    ROUND((LD.LevyDeclaredInMonth * LD.EnglishFraction),5) AS Amount
          FROM Data_Load.DAS_LevyDeclarations AS LD
          WHERE LD.IsLatest = 1
    UNION ALL
       -- Top Up Amount  
          SELECT 
                    LD.DasAccountId
               ,    LD.CreatedDate
  	   		   ,    LD.SubmissionDate
			   ,    LD.PayrollMonth
			   ,    LD.PayrollYear
			   ,    NULL AS CollectionMonth
			   ,    NULL AS CollectionYear
			   ,	NULL As CollectionPeriodName	
               ,    'IN Levy Top Up Amount' AS TransactionType
               ,    ROUND(LD.[TopupAmount],5) AS Amount
          FROM Data_Load.DAS_LevyDeclarations AS LD
		  WHERE LD.IsLatest = 1
    UNION ALL
      -- Payments 
          SELECT 
                    [EA].[DasAccountId] AS DasAccountId
               ,    PS.UpdateDateTime AS CreatedDate
			   ,    NULL as SubmissionDate
			   ,    NULL as PayrollMonth
			   ,	NULL as PayrollYear
			   ,    CollectionMonth
			   ,    CollectionYear
			   ,	CollectionPeriodName
               ,    'OUT '+[TransactionType]  AS TransactionType
               ,    ROUND((PS.Amount*-1),5) AS Amount -- Made negative as Payment
          FROM Data_Load.DAS_Payments AS PS
          -- DAS Account Name
               LEFT JOIN 
			   [Data_Load].[DAS_Employer_Accounts] EA 
			   ON EA.AccountId = [PS].[EmployerAccountId] AND 
			   EA.IsLatest = 1
          WHERE PS.EmployerAccountId IS NOT NULL -- removes non Levy Activity
               AND  PS.FundingSource = 'Levy'  -- Only Month coming from balances
               --AND  PS.IsLatest = 1

	 UNION ALL

---TRF Levy Transfer Sent

	SELECT 
                    [EA].[DasAccountId] AS DasAccountId
			   --,    EA.[EmployerAccountID]
               ,    PS.UpdateDateTime AS CreatedDate
			   ,    NULL as SubmissionDate
			   ,    NULL as PayrollMonth
			   ,	NULL as PayrollYear
			   ,    CollectionMonth
			   ,    CollectionYear
			   ,	CollectionPeriodName
               ,    'TRF '+'Levy Transfer Sent'  AS TransactionType
               ,    ROUND((PS.Amount*-1),5) AS Amount -- Made negative as Payment
          FROM Data_Load.DAS_Payments AS PS
          -- DAS Account Name
               LEFT JOIN 
			   [Data_Load].[DAS_Employer_Accounts] EA 
			   ON EA.AccountId = [PS].[EmployerAccountId] AND 
			   EA.IsLatest = 1
          WHERE PS.FundingAccountId IS NOT NULL AND PS.EmployerAccountId IS NOT NULL -- removes non Levy Activity
               AND  PS.FundingSource = 'LevyTransfer'  -- Only Month coming from balances
               --AND  PS.IsLatest = 1
	UNION ALL

---TRF Levy Transfer Recieved

	SELECT 
                    [EA].[DasAccountId] AS DasAccountId
               ,    PS.UpdateDateTime AS CreatedDate
			   ,    NULL AS SubmissionDate
			   ,    NULL AS PayrollMonth
			   ,	NULL AS PayrollYear
			   ,    CollectionMonth
			   ,    CollectionYear
			   ,	CollectionPeriodName
               ,    'TRF '+'Levy Transfer Recieved'  AS TransactionType
               ,    ROUND((PS.Amount*-1),5) AS Amount -- Made negative as Payment
          FROM Data_Load.DAS_Payments 
		  AS PS
          -- DAS Account Name
               LEFT JOIN 
			   [Data_Load].[DAS_Employer_Accounts] EA 
			   ON EA.AccountId = [PS].[EmployerAccountId] AND 
			   EA.IsLatest = 1
          WHERE PS.FundingAccountId IS NOT NULL and PS.EmployerAccountId IS NOT NULL -- removes non Levy Activity
               AND  PS.FundingSource = 'LevyTransfer'  -- Only Month coming from balances
               --AND  PS.IsLatest = 1

			    UNION ALL

---Out Learning Levy Transfer

			   SELECT 
                    [EA].[DasAccountId] AS DasAccountId
               ,    PS.UpdateDateTime AS CreatedDate
			   ,    NULL as SubmissionDate
			   ,    NULL as PayrollMonth
			   ,	NULL as PayrollYear
			   ,    CollectionMonth
			   ,    CollectionYear
			   ,	CollectionPeriodName
               ,    'OUT '+'Learning Levy Transfer'  AS TransactionType
               ,    ROUND((PS.Amount*-1),5) AS Amount -- Made negative as Payment
          FROM Data_Load.DAS_Payments 
		  AS PS
          ---- DAS Account Name
               LEFT JOIN 
			   [Data_Load].[DAS_Employer_Accounts] 
			   EA ON EA.AccountId = [PS].[EmployerAccountId] AND 
			   EA.IsLatest = 1
          WHERE PS.EmployerAccountId IS NOT NULL -- removes non Levy Activity
               AND  PS.FundingSource = 'LevyTransfer'  -- Only Month coming from balances
               --AND  PS.IsLatest = 1
