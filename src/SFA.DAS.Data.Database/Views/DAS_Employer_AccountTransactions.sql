 CREATE VIEW Data_Pub.DAS_Employer_AccountTransactions
    AS 
       -- Amounts from HMRC   
          SELECT 
               LD.DasAccountId
          ,    LD.CreatedDate
          ,    'IN Levy Money from HMRC' AS TransactionType
          ,    ROUND((LD.LevyDeclaredInMonth * LD.EnglishFraction),2) AS Amount
          FROM Data_Load.DAS_LevyDeclarations AS LD
          WHERE LD.IsLatest = 1
    UNION ALL
       -- Top Up Amount  
          SELECT 
                    LD.DasAccountId
               ,    LD.CreatedDate
               ,    'IN Levy Top Up Amount' AS TransactionType
               ,    ROUND(LD.[TopupAmount],2) AS Amount
          FROM Data_Load.DAS_LevyDeclarations AS LD
          WHERE LD.IsLatest = 1
       UNION ALL
     --- Payments 
          SELECT 
                    [EA].[DasAccountId] AS DasAccountId
               ,    PS.UpdateDateTime AS CreatedDate
               ,    'OUT '+[TransactionType]  AS TransactionType
               ,    ROUND((PS.Amount*-1),2) AS Amount -- Made negative as Payment
          FROM Data_Load.DAS_Payments AS PS
          ---- DAS Account Name
               LEFT JOIN [Data_Load].[DAS_Employer_Accounts] EA ON EA.AccountId = [PS].[EmployerAccountId] AND EA.IsLatest = 1
          WHERE PS.EmployerAccountId IS NOT NULL -- removes non Levy Activity
               AND  PS.FundingSource = 'Levy'  -- Only Month coming from balances
               --AND  PS.IsLatest = 1
