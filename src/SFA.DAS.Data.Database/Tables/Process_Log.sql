CREATE TABLE [HMRC].[Process_Log](
     ProcessEvent_ID BIGINT IDENTITY(1,1)
  ,  ProcessDateTime DATETIME DEFAULT GETDATE()
  ,  ProcessEventName VARCHAR(255)
  ,  ProcessEventDescription VARCHAR(2000)
  ,  ProcessEventCreatedBy VARCHAR(255) DEFAULT System_user
  ,  SourceFile_ID BIGINT
) 