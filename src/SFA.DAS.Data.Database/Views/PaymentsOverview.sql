CREATE VIEW [Reporting].[PaymentsOverview]
AS
SELECT
CASE
       WHEN (C.[AgreementStatus] like'BothAgreed') THEN 'Agreed commitments to date'
       ELSE 'Non Agreed commitments to date'
END AS CommitmentStatus
,COUNT ((C.[CommitmentID])) AS Commitments
,Payment = SUM(Amount)
 
FROM [Data_pub].[Das_Commitments] AS C
               Join [Data_Pub].[DAS_Payments] AS P
                     On C.[DASAccountID] = P.[DASAccountID]
                           AND C.CommitmentID = P.CommitmentID
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
       ELSE 'Non agreed commitments to date with start date this month'
END AS CommitmentStatus
, COUNT ((C.[CommitmentID])) AS Commitments
, Payment =Sum(Amount)
 
FROM [Data_pub].[Das_Commitments] AS C
               Join [Data_Pub].[DAS_Payments] AS P
                     On C.[DASAccountID] = P.[DASAccountID]
                           AND C.CommitmentID = P.CommitmentID
WHERE C.StartDateInCurrentMonth = 'Yes' AND C.Flag_Latest = 1
GROUP BY
       CASE
       WHEN (C.[AgreementStatus]like'BothAgreed') THEN 'Agreed commitments with start date this month'
       ELSE 'Non agreed commitments to date with start date this month'
END