CREATE VIEW [Data_Pub].[DAS_Psrs_SubmittedReports]
	AS 
SELECT  
       PSR.[Id] AS ID
       ,PSR.[DasAccountId]
       ,PSR.[OrganisationName]
	   ,EA.[AccountName] AS [DasOrganisationName]
       ,PSR.[ReportingPeriod]
	   --TODO: Do we want the label?
		--,'1 April 20' + SUBSTRING(CONVERT(char(4), PSR.[ReportingPeriod]),1,2) + ' to 31 March 20' + SUBSTRING(CONVERT(char(4), PSR.[ReportingPeriod]),3,2) AS ReportingPeriodLabel
       ,PSR.[FigureA]
       ,PSR.[FigureB]
       ,PSR.[FIgureE]
       ,PSR.[FigureC]
       ,PSR.[FigureD]
       ,PSR.[FigureF]
       ,PSR.[FigureG]
       ,PSR.[FigureH]
       ,PSR.[FigureI]
       ,PSR.[FullTimeEquivalent]
       ,PSR.[OutlineActions]
       ,PSR.[OutlineActionsWordCount]
       ,PSR.[Challenges]
       ,PSR.[ChallengesWordCount]
       ,PSR.[TargetPlans]
       ,PSR.[TargetPlansWordCount]
       ,PSR.[AnythingElse]
       ,PSR.[AnythingElseWordCount]
       ,PSR.[SubmittedAt]
       ,PSR.[SubmittedName]
       ,PSR.[SubmittedEmail]
FROM [Data_Load].[DAS_PublicSector_Reports] PSR
     LEFT JOIN [Data_Load].[DAS_Employer_Accounts] EA
     ON EA.[DasAccountId] = PSR.[DasAccountId]
     AND EA.[IsLatest] = 1
