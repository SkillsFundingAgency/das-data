CREATE TABLE [HMRC].[Load_Control](
     SourceFile_ID BIGINT IDENTITY(1,1)
  ,  SourceFIle_Name VARCHAR(255)
  ,  SourceFile_Status VARCHAR(50) 
  ,  InsertDate DATETIME DEFAULT GETDATE()
  ,  CreatedBy VARCHAR(255) DEFAULT System_user
  ,  Flag_PastDataQaulityTests BIT DEFAULT 0
  ,  Flag_LoadedSuccessfullyintoLiveTable BIT DEFAULT 0
  ,  Flag_LoadedSuccessfullyintoHistoryTable BIT DEFAULT 0
) 