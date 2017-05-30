CREATE VIEW [Reporting].[PaymentsOverview]
       AS
       SELECT
       CASE
              WHEN (C.[AgreementStatus] like'BothAgreed') THEN 'Agreed commitments to date'
              ELSE 'Non Agreed commitments to date'
       END AS CommitmentStatus
       ,COUNT ((C.[CommitmentID])) AS Commitments
       
       
       FROM [Data_pub].[Das_Commitments] AS C
                    
       WHERE  C.Flag_Latest = 1
       GROUP BY
            CASE
              WHEN (C.[AgreementStatus] like'BothAgreed') THEN 'Agreed commitments to date'
              ELSE 'Non Agreed commitments to date'
       END
       
        
       UNION
       
       SELECT
       CASE
              WHEN (C.[AgreementStatus]like'BothAgreed') THEN 'Agreed commitments with start date this month'
              ELSE 'Non agreed commitments with start date this month'
       END AS CommitmentStatus
       , COUNT ((C.[CommitmentID])) AS Commitments
       
       
       FROM [Data_pub].[Das_Commitments] AS C
                    
       WHERE C.StartDateInCurrentMonth = 'Yes' AND C.Flag_Latest = 1
       GROUP BY
              CASE
              WHEN (C.[AgreementStatus]like'BothAgreed') THEN 'Agreed commitments with start date this month'
              ELSE 'Non agreed commitments with start date this month'
       END
