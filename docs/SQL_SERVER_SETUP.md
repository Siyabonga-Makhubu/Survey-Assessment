# SQL Server Setup Guide for Survey Assessment Application

## Overview

This guide provides step-by-step instructions for setting up SQL Server for the Survey Assessment application. The application supports multiple SQL Server configurations including LocalDB, SQL Server Express, and full SQL Server instances.

## Prerequisites

### Required Software
- **SQL Server** (one of the following):
  - SQL Server LocalDB (recommended for development)
  - SQL Server Express (free edition)
  - SQL Server Developer Edition (free for development)
  - SQL Server Standard/Enterprise (for production)

### Optional Tools
- **SQL Server Management Studio (SSMS)** - GUI for database management
- **Azure Data Studio** - Cross-platform database tool
- **Visual Studio** - Has built-in SQL Server LocalDB support

## Installation Options

### Option 1: SQL Server LocalDB (Recommended for Development)

LocalDB is a lightweight version of SQL Server Express designed for developers.

#### 1.1 Install LocalDB

**Via Visual Studio Installer:**
1. Open Visual Studio Installer
2. Modify your Visual Studio installation
3. Go to "Individual components"
4. Check "SQL Server Express 2019 LocalDB" or newer
5. Click "Modify"

**Via Standalone Installer:**
1. Download from [Microsoft SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
2. Choose "Download now" under Express
3. Run the installer and select "Custom" installation
4. Select "LocalDB" feature
5. Complete the installation

#### 1.2 Verify LocalDB Installation

Open Command Prompt and run:
```cmd
sqllocaldb info
```

You should see available LocalDB instances. If not installed, you'll see:
```cmd
sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
```

#### 1.3 Connection String for LocalDB

The application is pre-configured for LocalDB:
```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=survey_dev;Trusted_Connection=true;TrustServerCertificate=true;"
```

### Option 2: SQL Server Express

Full SQL Server Express installation with SQL Server services.

#### 2.1 Download and Install

1. Go to [SQL Server Downloads](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
2. Download "Express" edition
3. Run the installer
4. Choose "Basic" installation for default settings
5. Note the server name (usually `localhost\SQLEXPRESS`)

#### 2.2 Connection String for SQL Server Express

Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=survey_dev;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### Option 3: Full SQL Server Instance

For production or when you have a full SQL Server installation.

#### 3.1 Connection String Examples

**Windows Authentication:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=survey_dev;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

**SQL Server Authentication:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=survey_dev;User Id=sa;Password=YourPassword;TrustServerCertificate=true;"
  }
}
```

**Remote SQL Server:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server-name;Database=survey_dev;User Id=username;Password=password;TrustServerCertificate=true;"
  }
}
```

## Database Setup

### Method 1: Using the Provided SQL Script (Recommended)

#### Step 1: Run the Setup Script

**Using SQL Server Management Studio (SSMS):**
1. Open SSMS
2. Connect to your SQL Server instance:
   - **Server name**: `(localdb)\MSSQLLocalDB` (for LocalDB)
   - **Authentication**: Windows Authentication
3. Open the file `setup_sqlserver_database.sql`
4. Click "Execute" or press F5

**Using Command Line (sqlcmd):**

For LocalDB:
```cmd
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "setup_sqlserver_database.sql"
```

For SQL Server Express:
```cmd
sqlcmd -S "localhost\SQLEXPRESS" -i "setup_sqlserver_database.sql"
```

For Full SQL Server:
```cmd
sqlcmd -S "localhost" -i "setup_sqlserver_database.sql"
```

**Using PowerShell:**
```powershell
# Navigate to project directory
cd "path\to\your\Survey-Assessment"

# Execute the script
Invoke-Sqlcmd -ServerInstance "(localdb)\MSSQLLocalDB" -InputFile "setup_sqlserver_database.sql"
```

#### Step 2: Verify Installation

The script will output success messages. You should see:
```
Database survey_dev created successfully.
Tables created successfully.
Sample data inserted successfully.
Database setup completed successfully!
```

### Method 2: Using Entity Framework Migrations

If you prefer to use EF Core migrations:

#### Step 1: Install EF Core Tools
```cmd
dotnet tool install --global dotnet-ef
```

#### Step 2: Create and Run Migrations
```cmd
# Navigate to project directory
cd "path\to\your\Survey-Assessment"

# Add migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

**Note**: This method won't include the sample data from the SQL script.

## Database Schema

The application creates the following database structure:

### Tables Created

#### 1. personaldetails
```sql
CREATE TABLE personaldetails (
    SurveyID int IDENTITY(1,1) PRIMARY KEY,
    FullName nvarchar(100) NOT NULL,
    Email nvarchar(100) NOT NULL,
    DateOfBirth date NOT NULL,
    ContactNumbers nvarchar(20) NOT NULL,
    SubmissionDate datetime2 NOT NULL DEFAULT GETDATE()
);
```

#### 2. options
```sql
CREATE TABLE options (
    SurveyID int NOT NULL,
    OptionValue nvarchar(100) NOT NULL,
    Rating int NULL,
    PRIMARY KEY (SurveyID, OptionValue),
    FOREIGN KEY (SurveyID) REFERENCES personaldetails(SurveyID) ON DELETE CASCADE
);
```

### Sample Data

The script includes sample data with 3 survey responses demonstrating:
- Multiple food preferences (checkbox behavior)
- Rating scales (radio button behavior)
- Various survey response patterns

## Troubleshooting

### Common Issues and Solutions

#### Issue 1: LocalDB Not Found
**Error**: `A network-related or instance-specific error occurred while establishing a connection to SQL Server`

**Solution**:
```cmd
# Check if LocalDB is installed
sqllocaldb info

# If not available, create and start instance
sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB

# Verify it's running
sqllocaldb info MSSQLLocalDB
```

#### Issue 2: Permission Denied
**Error**: `CREATE DATABASE permission denied`

**Solutions**:
- Ensure you're running as Administrator
- Use Windows Authentication (Trusted_Connection=true)
- For SQL Authentication, ensure the user has `dbcreator` role

#### Issue 3: Database Already Exists
**Error**: Database conflicts or schema mismatches

**Solution**:
```sql
-- Drop and recreate database
DROP DATABASE IF EXISTS survey_dev;
-- Then run the setup script again
```

#### Issue 4: Connection String Issues
**Error**: `Invalid connection string` or authentication failures

**Check**:
- Server name is correct
- Authentication method matches your setup
- Firewall allows SQL Server connections
- SQL Server Browser service is running (for named instances)

### Verification Steps

#### 1. Test Database Connection
```sql
-- Connect to the database
USE survey_dev;

-- Check tables exist
SELECT name FROM sys.tables;

-- Verify sample data
SELECT COUNT(*) as PersonalDetailsCount FROM personaldetails;
SELECT COUNT(*) as OptionsCount FROM options;
```

#### 2. Test Application Connection
```cmd
# Build and run the application
dotnet build
dotnet run

# The application should start without database errors
# Check the console output for any connection issues
```

#### 3. Test API Endpoints
Once the application is running, test the database connection:

```http
GET https://localhost:5001/api/survey/statistics
```

Should return survey statistics if the database is properly connected.

## Environment-Specific Configuration

### Development Environment
Use the provided `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=survey_dev;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### Production Environment
Update `appsettings.Production.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-production-server;Database=survey_prod;User Id=app_user;Password=secure_password;TrustServerCertificate=false;Encrypt=true;"
  }
}
```

### Azure SQL Database
For Azure deployment:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:your-server.database.windows.net,1433;Database=survey_prod;User Id=username@your-server;Password=password;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;"
  }
}
```

## Security Considerations

### Development
- Use Windows Authentication when possible
- LocalDB automatically uses current user credentials
- Avoid storing passwords in configuration files

### Production
- Use SQL Server Authentication with strong passwords
- Enable SSL/TLS encryption (`Encrypt=true`)
- Use Azure Key Vault or similar for connection string storage
- Implement connection string encryption
- Use least-privilege database accounts

### Connection String Security
Never commit production connection strings to source control. Use:
- Environment variables
- Azure App Configuration
- Key management services
- User secrets for development

## Performance Optimization

### Indexing Strategy
The application includes:
- Primary key on `personaldetails.SurveyID`
- Composite primary key on `options(SurveyID, OptionValue)`
- Foreign key relationship with cascade delete

### Additional Indexes (Optional)
For better performance with large datasets:
```sql
-- Index on submission date for time-based queries
CREATE INDEX IX_personaldetails_SubmissionDate 
ON personaldetails(SubmissionDate);

-- Index on email for lookup operations
CREATE INDEX IX_personaldetails_Email 
ON personaldetails(Email);
```

## Backup and Maintenance

### Regular Backups
```sql
-- Full backup
BACKUP DATABASE survey_dev 
TO DISK = 'C:\Backup\survey_dev.bak';

-- Differential backup
BACKUP DATABASE survey_dev 
TO DISK = 'C:\Backup\survey_dev_diff.bak'
WITH DIFFERENTIAL;
```

### Maintenance Tasks
- Regular index maintenance
- Statistics updates
- Database integrity checks
- Log file management

## Next Steps

After successful database setup:

1. **Run the Application**: Use `dotnet run` to start the Survey API
2. **Test the Frontend**: Navigate to `https://localhost:5001` to test the survey form
3. **Verify API**: Check `https://localhost:5001/swagger` for API documentation
4. **Submit Test Data**: Use the web interface to submit test surveys
5. **View Statistics**: Check the survey results page to verify data flow

## Support

If you encounter issues:

1. Check the application logs for specific error messages
2. Verify SQL Server is running: `services.msc` → look for SQL Server services
3. Test connection using SSMS or Azure Data Studio
4. Review firewall settings for SQL Server ports (default 1433)
5. Ensure your user account has appropriate SQL Server permissions

For additional help, refer to:
- [SQL Server Documentation](https://docs.microsoft.com/en-us/sql/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
