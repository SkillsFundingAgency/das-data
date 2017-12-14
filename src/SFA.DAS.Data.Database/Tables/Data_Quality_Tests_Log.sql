CREATE TABLE HMRC.Data_Quality_Tests_Log
(
          Record_ID BIGINT
     ,    ColumnName  VARCHAR(255)
     ,    TestName    VARCHAR(255)
     ,    ErrorMessage  VARCHAR(255)
     ,    FlagStopLoad INT
     ,    SourceFile_ID BIGINT  NULL)