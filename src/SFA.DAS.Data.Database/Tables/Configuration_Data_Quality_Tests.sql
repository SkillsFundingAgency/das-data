CREATE TABLE HMRC.Configuration_Data_Quality_Tests
(
    ID BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1)
    , ColumnName VARCHAR(255)
    , ColumnNullable INT
    , ColumnType VARCHAR(255)
    , ColumnLength INT
    , ColumnPrecision INT
    , ColumnDefault VARCHAR(255)
    , RunColumnTests INT
    , ColumnPatternMatching VARCHAR(255)
    , ColumnMinValue VARCHAR(255)
    , ColumnMaxValue VARCHAR(255)
    , StopLoadIfTestTextLength BIT DEFAULT 0
    , StopLoadIfTestIsNumeric BIT DEFAULT 0
    , StopLoadIfTestPatternMatch BIT DEFAULT 0
    , StopLoadIfTestValueRange BIT DEFAULT 0
    , StopLoadIfTestDecimalPlaces BIT DEFAULT 0

    )