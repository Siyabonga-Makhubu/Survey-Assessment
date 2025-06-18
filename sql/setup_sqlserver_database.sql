-- SQL Server Database Setup Script for Survey Application
-- Execute this script in SQL Server Management Studio (SSMS)

-- Step 1: Create the database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'survey_dev')
BEGIN
    CREATE DATABASE survey_dev;
    PRINT 'Database survey_dev created successfully.';
END
ELSE
BEGIN
    PRINT 'Database survey_dev already exists.';
END
GO

-- Step 2: Use the database
USE survey_dev;
GO

-- Step 3: Create tables

-- Drop tables if they exist (for clean setup)
IF OBJECT_ID('dbo.options', 'U') IS NOT NULL
    DROP TABLE dbo.options;

IF OBJECT_ID('dbo.personaldetails', 'U') IS NOT NULL
    DROP TABLE dbo.personaldetails;

-- Create PersonalDetails table
CREATE TABLE personaldetails (
    SurveyID int IDENTITY(1,1) PRIMARY KEY,
    FullName nvarchar(100) NOT NULL,
    Email nvarchar(100) NOT NULL,
    DateOfBirth date NOT NULL,
    ContactNumbers nvarchar(20) NOT NULL,
    SubmissionDate datetime2 NOT NULL DEFAULT GETDATE()
);

-- Create Options table
CREATE TABLE options (
    SurveyID int NOT NULL,
    OptionValue nvarchar(100) NOT NULL,
    Rating int NULL,
    PRIMARY KEY (SurveyID, OptionValue),
    FOREIGN KEY (SurveyID) REFERENCES personaldetails(SurveyID) ON DELETE CASCADE
);

PRINT 'Tables created successfully.';

-- Step 4: Insert sample data for testing (optional)
INSERT INTO personaldetails (FullName, Email, DateOfBirth, ContactNumbers, SubmissionDate)
VALUES 
    ('John Doe', 'john.doe@example.com', '1990-05-15', '+1-555-123-4567', GETDATE()),
    ('Jane Smith', 'jane.smith@example.com', '1985-08-22', '+1-555-987-6543', GETDATE()),
    ('Mike Johnson', 'mike.johnson@example.com', '1992-12-03', '+1-555-456-7890', GETDATE());

-- Insert corresponding options data to demonstrate multi-select and single-select
INSERT INTO options (SurveyID, OptionValue, Rating)
VALUES 
    -- Survey 1: John Doe - Multiple food choices (checkbox demo) + ratings (radio demo)
    (1, 'FavoriteFood:Pizza', NULL),
    (1, 'FavoriteFood:Pasta', NULL),      -- Multiple food selections allowed
    (1, 'MovieRating', 4),               -- Statement 1: I like to watch movies (1-5 scale)
    (1, 'RadioRating', 3),               -- Statement 2: I like to listen to radio
    (1, 'EatOutRating', 5),              -- Statement 3: I like to eat out
    (1, 'TVRating', 2),                  -- Statement 4: I like to watch TV
    
    -- Survey 2: Jane Smith - Single food choice + ratings
    (2, 'FavoriteFood:Pap and Wors', NULL),  -- Single food selection
    (2, 'MovieRating', 5),
    (2, 'RadioRating', 4),
    (2, 'EatOutRating', 3),
    (2, 'TVRating', 4),
    
    -- Survey 3: Mike Johnson - Multiple food choices + ratings
    (3, 'FavoriteFood:Pizza', NULL),
    (3, 'FavoriteFood:Other', NULL),     -- Multiple food selections
    (3, 'MovieRating', 3),
    (3, 'RadioRating', 2),
    (3, 'EatOutRating', 4),
    (3, 'TVRating', 5);

PRINT 'Sample data inserted successfully.';

-- Step 5: Verify the setup
SELECT 'PersonalDetails Count' as TableName, COUNT(*) as RecordCount FROM personaldetails
UNION ALL
SELECT 'Options Count' as TableName, COUNT(*) as RecordCount FROM options;

PRINT 'Database setup completed successfully!';
PRINT 'You can now run your Survey API application.';
