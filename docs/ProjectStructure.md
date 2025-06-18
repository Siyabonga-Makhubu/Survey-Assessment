# Survey Assessment Application - Project Structure

## Overview

This document provides a detailed breakdown of the Survey Assessment application's project structure, explaining the purpose and organization of each directory and file. The project follows Clean Architecture principles with clear separation of concerns and organized by feature and layer.

## Root Directory Structure

```
Survey-Assessment/
├── 📁 Configuration/           # Application configuration (currently empty)
├── 📁 Controllers/             # API Controllers (HTTP endpoints)
├── 📁 Data/                    # Database context and configurations
├── 📁 docs/                    # Project documentation
├── 📁 DTOs/                    # Data Transfer Objects
├── 📁 Extensions/              # Service configuration extensions
├── 📁 Models/                  # Domain/Entity models
├── 📁 Services/                # Business logic layer
├── 📁 wwwroot/                 # Static web files (HTML, CSS, JS)
├── 📁 bin/                     # Compiled binaries (auto-generated)
├── 📁 obj/                     # Build artifacts (auto-generated)
├── 📁 .git/                    # Git version control
├── 📁 .vs/                     # Visual Studio settings
├── 📄 Program.cs               # Application entry point
├── 📄 SurveyApi.csproj         # Project file with dependencies
├── 📄 Survey-Assessment.sln    # Visual Studio solution file
├── 📄 README.md                # Project overview and setup guide
└── 📄 appsettings*.json        # Configuration files
```

## Detailed Directory Analysis

### 🎯 Controllers/ - API Layer
**Purpose**: Handles HTTP requests and responses, defines API endpoints

```
Controllers/
└── SurveyController.cs         # RESTful API for survey operations
```

**Responsibilities**:
- HTTP request/response handling
- Input validation and model binding
- Route definitions (`/api/survey/`)
- Error handling and status codes
- Coordination with service layer

**Key Endpoints**:
- `POST /api/survey/submit` - Submit new survey
- `GET /api/survey/statistics` - Get survey statistics

### 💾 Data/ - Data Access Layer
**Purpose**: Database context and Entity Framework configurations

```
Data/
└── SurveyDbContext.cs          # Entity Framework DbContext
```

**Responsibilities**:
- Database connection management
- Entity configurations and relationships
- Table mappings and constraints
- Migration support
- Database transaction handling

### 🏗️ Models/ - Domain Layer
**Purpose**: Core business entities and domain models

```
Models/
├── PersonalDetails.cs          # User information entity
└── Options.cs                  # Survey options entity
```

**Entity Details**:
- **PersonalDetails**: User demographic and contact information
- **Options**: Survey responses with composite key structure
- **Relationships**: One-to-many (PersonalDetails → Options)

### 📦 DTOs/ - Data Transfer Objects
**Purpose**: Data contracts for API communication

```
DTOs/
├── SurveyDto.cs               # Survey submission and response DTOs
└── SurveyStatisticsDto.cs     # Statistics aggregation DTOs
```

**DTO Types**:
- **SurveySubmissionDto**: Input for survey submission
- **SurveyResponseDto**: Complete survey data output
- **SurveyStatisticsDto**: Aggregated statistics
- **PersonalDetailsDto**: User information transfer
- **SurveyOptionsDto**: Survey options transfer

### ⚙️ Services/ - Business Logic Layer
**Purpose**: Core business logic and operations

```
Services/
└── SurveyService.cs           # Survey business logic implementation
```

**Service Interface** (`ISurveyService`):
- `SubmitSurveyAsync()` - Process survey submissions
- `GetAllSurveysAsync()` - Retrieve all surveys
- `GetSurveyByIdAsync()` - Get specific survey
- `GetSurveyStatisticsAsync()` - Calculate statistics

### 🔧 Extensions/ - Configuration Layer
**Purpose**: Dependency injection and service configuration

```
Extensions/
└── ServiceCollectionExtensions.cs  # DI container configuration
```

**Configuration Methods**:
- `AddSurveyServices()` - Register application services
- Database context registration
- Service lifetime management

### 🌐 wwwroot/ - Frontend Layer
**Purpose**: Static web files served to browsers

```
wwwroot/
├── index.html                 # Survey form page
├── view-survey-results.html   # Results display page
├── script.js                  # Client-side JavaScript logic
└── style.css                  # Responsive styling
```

**Frontend Architecture**:
- **SPA-like behavior** with page navigation
- **Responsive design** for mobile and desktop
- **API integration** using Fetch API
- **Form validation** and user feedback

### 📚 docs/ - Documentation
**Purpose**: Project documentation and guides

```
docs/
├── Architecture.md            # System architecture overview
├── ProjectStructure.md        # This file - project organization
└── CHECKBOX_RADIO_HANDLING.md # Frontend form handling details
```

### 📁 Configuration/ - Application Settings
**Purpose**: Configuration classes and settings (currently empty)

```
Configuration/
└── (empty - reserved for future configuration classes)
```

**Future Use Cases**:
- Custom configuration models
- Settings validation classes
- Environment-specific configurations

## Configuration Files

### Application Settings
```
📄 appsettings.json            # Base configuration
📄 appsettings.Development.json # Development environment overrides
📄 appsettings.Production.json  # Production environment settings
```

**Configuration Sections**:
- **ConnectionStrings**: Database connection configuration
- **Logging**: Log level and provider settings
- **AllowedHosts**: Security configuration

### Project Configuration
```
📄 SurveyApi.csproj            # Project dependencies and build settings
📄 Survey-Assessment.sln       # Visual Studio solution configuration
```

### Database Setup
```
📄 setup_sqlserver_database.sql # Database creation script
📄 SQL_SERVER_SETUP.md         # Database setup instructions
📄 survey.sql                  # Additional SQL scripts
```

### Testing Files
```
📄 SurveyApi.http             # Basic API testing requests
📄 TestSurveyApi.http         # Extended API test scenarios
```

## Legacy Files (Root Level)

**Note**: These files exist in the root but should ideally be moved to `wwwroot/`:

```
📄 index.html                 # Should be in wwwroot/ (duplicate exists)
📄 script.js                  # Should be in wwwroot/ (duplicate exists)
📄 style.css                  # Should be in wwwroot/ (duplicate exists)
📄 view-survey-results.html   # Should be in wwwroot/ (duplicate exists)
```

## Auto-Generated Directories

### Build Artifacts
```
📁 bin/                       # Compiled assemblies and dependencies
├── Debug/net9.0/            # Debug build outputs
└── Release/net9.0/          # Release build outputs (when built)

📁 obj/                       # Temporary build files
├── project.assets.json      # NuGet package information
├── project.nuget.cache      # NuGet cache
└── Debug/net9.0/            # Intermediate build files
```

### Version Control
```
📁 .git/                     # Git repository data
📁 .vs/                      # Visual Studio user settings
```

## File Organization Principles

### 1. **Separation by Layer**
- Controllers → HTTP handling
- Services → Business logic
- Data → Database access
- Models → Domain entities

### 2. **Feature-Based Organization**
- All survey-related functionality grouped together
- DTOs match their corresponding operations
- Clear naming conventions

### 3. **Configuration Centralization**
- All settings in `appsettings*.json`
- Service registration in `Extensions/`
- Database configuration in `Data/`

### 4. **Documentation Co-location**
- Documentation in dedicated `docs/` folder
- API tests alongside main project
- Setup scripts at root level

## Recommended Improvements

### 1. **Clean Up Root Directory**
Remove duplicate static files from root:
```bash
# These files exist in both root and wwwroot/
rm index.html script.js style.css view-survey-results.html
```

### 2. **Add Missing Directories**
Consider adding these standard directories:
```
📁 Tests/                     # Unit and integration tests
📁 Middleware/                # Custom middleware components
📁 Validators/                # Input validation classes
📁 Exceptions/                # Custom exception classes
```

### 3. **Configuration Organization**
Move configuration classes to `Configuration/`:
```
Configuration/
├── DatabaseOptions.cs        # Database configuration model
├── ApiOptions.cs            # API-specific settings
└── SecurityOptions.cs       # Security configuration
```

### 4. **Add Health Checks**
```
📁 HealthChecks/             # Custom health check implementations
└── DatabaseHealthCheck.cs   # Database connectivity check
```

## Dependencies and References

### NuGet Packages (from SurveyApi.csproj)
```xml
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.6" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.6" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.6" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.6" />
```

### Framework Dependencies
- **.NET 9.0**: Target framework
- **ASP.NET Core**: Web framework
- **Entity Framework Core**: ORM
- **SQL Server**: Database provider

## File Naming Conventions

### C# Files
- **PascalCase** for all class names
- **Descriptive names** indicating purpose
- **Suffix conventions**:
  - `Controller.cs` for API controllers
  - `Service.cs` for business logic
  - `Dto.cs` for data transfer objects
  - `DbContext.cs` for database contexts

### Web Files
- **lowercase** with hyphens for HTML files
- **camelCase** for JavaScript files
- **lowercase** for CSS files

### Configuration Files
- **dot notation** for environment-specific configs
- **descriptive names** for SQL scripts

This project structure provides a solid foundation for a maintainable, scalable web application while following established .NET conventions and Clean Architecture principles.
