CREATE TABLE Data_Load.DAS_CalendarMonth
(
    CalendarMonthID INT NOT NULL PRIMARY KEY
    , CalendarDate DATE NOT NULL
    , CalendarMonthNumber INT NOT NULL
    , CalendarMonthName VARCHAR(20) NOT NULL
    , CalendarMonthShortName VARCHAR(20) NOT NULL
    , CalendarYear INT NOT NULL
    , CalendarMonthNameYear VARCHAR(20) NOT NULL
    , CalendarMonthShortNameYear VARCHAR(20) NOT NULL
    , TaxMonthNumber VARCHAR(20) NOT NULL
    , TaxYear VARCHAR(20) NOT NULL
    , AcademicMonthNumber INT NOT NULL
    , AcademicYear VARCHAR(20) NOT NULL
)