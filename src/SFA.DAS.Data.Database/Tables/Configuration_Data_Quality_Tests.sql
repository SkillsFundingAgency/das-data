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
    , FlagStopLoadIfTestTextLenght BIT DEFAULT 0
    , FlagStopLoadIfTestIsNumeric BIT DEFAULT 0
    , FlagStopLoadIfTestPatternMatch BIT DEFAULT 0
    , FlagStopLoadIfTestValueRange BIT DEFAULT 0
    , FlagStopLoadIfTestDecimalPlaces BIT DEFAULT 0

    )