# Survey Assessment Application - Architecture Documentation

## Overview

The Survey Assessment application follows a **Clean Architecture** pattern with clear separation of concerns, implementing a modern web application architecture using ASP.NET Core 9.0. The system is designed to handle survey submissions and provide statistical analysis through a RESTful API with a responsive frontend.

## Architectural Principles

### 1. Separation of Concerns
- **Presentation Layer**: Controllers and Frontend (HTML/CSS/JS)
- **Business Logic Layer**: Services
- **Data Access Layer**: Entity Framework Core with DbContext
- **Infrastructure Layer**: Configuration and Extensions

### 2. Dependency Inversion
- High-level modules don't depend on low-level modules
- Both depend on abstractions (interfaces)
- Implemented through ASP.NET Core's built-in Dependency Injection

### 3. Single Responsibility
- Each class and module has a single, well-defined responsibility
- Controllers handle HTTP requests/responses
- Services contain business logic
- DTOs handle data transfer
- Models represent domain entities

## System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                        │
├─────────────────────┬───────────────────────────────────────┤
│     Frontend        │            API Layer                 │
│   (HTML/CSS/JS)     │        (Controllers)                  │
│                     │                                       │
│  ┌─────────────┐   │  ┌─────────────────────────────────┐  │
│  │ index.html  │   │  │     SurveyController            │  │
│  │ script.js   │───┼──│  - POST /api/survey/submit      │  │
│  │ style.css   │   │  │  - GET /api/survey/statistics   │  │
│  │ results.html│   │  │                                 │  │
│  └─────────────┘   │  └─────────────────────────────────┘  │
└─────────────────────┴───────────────────────────────────────┤
│                    Business Logic Layer                      │
│  ┌─────────────────────────────────────────────────────────┐│
│  │                 SurveyService                           ││
│  │  - SubmitSurveyAsync()                                  ││
│  │  - GetAllSurveysAsync()                                 ││
│  │  - GetSurveyByIdAsync()                                 ││
│  │  - GetSurveyStatisticsAsync()                           ││
│  └─────────────────────────────────────────────────────────┘│
├──────────────────────────────────────────────────────────────┤
│                    Data Access Layer                         │
│  ┌─────────────────────────────────────────────────────────┐│
│  │                SurveyDbContext                          ││
│  │  - PersonalDetails DbSet                                ││
│  │  - Options DbSet                                        ││
│  │  - Entity Configurations                                ││
│  └─────────────────────────────────────────────────────────┘│
├──────────────────────────────────────────────────────────────┤
│                      Database Layer                          │
│  ┌─────────────────────────────────────────────────────────┐│
│  │                  SQL Server                             ││
│  │  - PersonalDetails Table                                ││
│  │  - Options Table                                        ││
│  └─────────────────────────────────────────────────────────┘│
└──────────────────────────────────────────────────────────────┘
```

## Component Architecture

### 1. Presentation Layer

#### Frontend Components
- **index.html**: Survey form interface
- **view-survey-results.html**: Statistics display interface
- **script.js**: Client-side logic and API communication
- **style.css**: Responsive styling and UI components

#### API Controllers
- **SurveyController**: RESTful endpoints for survey operations
  - Handles HTTP requests/responses
  - Input validation
  - Response formatting
  - Error handling

### 2. Business Logic Layer

#### Services
- **ISurveyService**: Service contract/interface
- **SurveyService**: Core business logic implementation
  - Survey submission processing
  - Data validation and transformation
  - Statistics calculation
  - Transaction management

### 3. Data Transfer Objects (DTOs)

```csharp
SurveySubmissionDto      // Input for survey submission
├── PersonalDetailsDto   // User information
└── SurveyOptionsDto     // Survey responses

SurveyResponseDto        // Complete survey data
├── PersonalDetails      // User details
└── Options[]           // All options selected

SurveyStatisticsDto      // Aggregated statistics
├── TotalResponses       // Count metrics
├── ColorStatistics      // Favorite color breakdown
├── FoodStatistics       // Favorite food breakdown
└── HobbyStatistics      // Hobby preferences
```

### 4. Domain Models

```csharp
PersonalDetails          // Core user entity
├── SurveyID (PK)       // Unique identifier
├── FirstName           // User's first name
├── LastName            // User's last name
├── Email               // Contact email
├── PhoneNumber         // Contact phone
├── DateOfBirth         // User's birth date
└── Options[]           // Navigation property

Options                  // Survey choices entity
├── SurveyID (FK)       // Foreign key to PersonalDetails
├── OptionValue (PK)    // Selected option value
└── PersonalDetails     // Navigation property
```

### 5. Data Access Layer

#### DbContext Configuration
- **Entity Framework Core 9.0**
- **Code-First approach**
- **Composite primary keys**
- **Cascade delete relationships**
- **Explicit table mapping**

## Data Flow Architecture

### Survey Submission Flow
```
User Form Input
     ↓
JavaScript Validation
     ↓
HTTP POST /api/survey/submit
     ↓
SurveyController.SubmitSurvey()
     ↓
SurveyService.SubmitSurveyAsync()
     ↓
Database Transaction
     ↓
Entity Framework Core
     ↓
SQL Server Database
     ↓
Return Survey ID
```

### Statistics Retrieval Flow
```
User Request
     ↓
HTTP GET /api/survey/statistics
     ↓
SurveyController.GetStatistics()
     ↓
SurveyService.GetSurveyStatisticsAsync()
     ↓
Entity Framework Queries
     ↓
SQL Server Aggregations
     ↓
Statistics DTO
     ↓
JSON Response
```

## Security Architecture

### Input Validation
- **Client-side**: JavaScript form validation
- **Server-side**: Model state validation
- **Database**: Entity Framework validation

### CORS Configuration
- Configured for development with `AllowAll` policy
- Should be restricted in production environments

### Data Protection
- SQL injection prevention through parameterized queries
- Input sanitization at multiple layers
- Error handling without information disclosure

## Configuration Architecture

### Environment-Based Configuration
```
appsettings.json              // Base configuration
appsettings.Development.json  // Development overrides
appsettings.Production.json   // Production settings
```

### Dependency Injection Configuration
- **ServiceCollectionExtensions**: Centralized service registration
- **Scoped lifetime**: For database-dependent services
- **Configuration binding**: For connection strings and settings

## Database Architecture

### Entity Relationships
```sql
PersonalDetails (1) ──────── (*) Options
     │                           │
     │ SurveyID                 │ SurveyID (FK)
     │ (Primary Key)            │ + OptionValue (Composite PK)
```

### Indexing Strategy
- Primary key on `PersonalDetails.SurveyID`
- Composite primary key on `Options(SurveyID, OptionValue)`
- Foreign key relationship with cascade delete

## Scalability Considerations

### Current Architecture Supports
- **Horizontal scaling**: Stateless API design
- **Database scaling**: Entity Framework optimization
- **Caching**: Ready for implementation with IMemoryCache
- **Load balancing**: Stateless controller design

### Future Enhancements
- **CQRS Pattern**: Separate read/write operations
- **Event Sourcing**: For audit trail requirements
- **Microservices**: Service decomposition possibilities
- **API Versioning**: For backward compatibility

## Performance Architecture

### Optimizations Implemented
- **Async/await**: Non-blocking operations
- **Connection pooling**: Entity Framework default
- **Lazy loading**: Disabled for predictable queries
- **Transaction scope**: Minimized for data consistency

### Monitoring Points
- **Database query performance**
- **API response times**
- **Memory usage patterns**
- **Error rates and types**

## Deployment Architecture

### Development Environment
- **Local SQL Server/LocalDB**
- **IIS Express hosting**
- **File-based configuration**

### Production Considerations
- **Azure SQL Database** or **SQL Server clustering**
- **Azure App Service** or **IIS hosting**
- **Environment variable configuration**
- **Application Insights** monitoring

## Error Handling Architecture

### Exception Handling Strategy
```
Client Error (400s)
├── Validation errors
├── Missing required fields
└── Invalid data formats

Server Error (500s)
├── Database connection issues
├── Transaction failures
└── Unexpected exceptions
```

### Logging Architecture
- **ASP.NET Core Logging**
- **Structured logging** ready for implementation
- **Log levels**: Information, Warning, Error
- **Performance logging** for slow queries

## Testing Architecture

### Test Structure (Recommended)
```
Tests/
├── Unit Tests
│   ├── Services/
│   ├── Controllers/
│   └── Models/
├── Integration Tests
│   ├── API endpoints
│   └── Database operations
└── End-to-End Tests
    └── Frontend workflows
```

## Technology Stack Architecture

### Backend Stack
- **.NET 9.0**: Runtime and framework
- **ASP.NET Core**: Web framework
- **Entity Framework Core 9.0**: ORM
- **SQL Server**: Database engine

### Frontend Stack
- **HTML5**: Semantic markup
- **CSS3**: Responsive design
- **Vanilla JavaScript**: Client-side logic
- **Fetch API**: HTTP communication

### Development Tools
- **Visual Studio 2022**: IDE
- **SQL Server Management Studio**: Database management
- **Swagger/OpenAPI**: API documentation

This architecture provides a solid foundation for the Survey Assessment application while maintaining flexibility for future enhancements and scaling requirements.
