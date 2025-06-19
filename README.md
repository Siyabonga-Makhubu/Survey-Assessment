# Survey Assessment Application

A modern web application built with ASP.NET Core (.NET 9) that allows users to submit survey responses and view survey statistics. The application features a RESTful API backend with Entity Framework Core for data persistence and a responsive frontend built with HTML, CSS, and JavaScript.

## 🚀 Features

- **Survey Submission**: Users can fill out surveys with personal details and multiple-choice questions
- **Survey Statistics**: View comprehensive statistics and results from submitted surveys
- **RESTful API**: Clean API endpoints for survey operations
- **Responsive Design**: Modern, mobile-friendly user interface
- **Entity Framework Core**: Robust data persistence with SQL Server
- **Real-time Validation**: Client-side and server-side validation

## 🛠️ Technology Stack

- **Backend**: ASP.NET Core 9.0
- **Database**: SQL Server with Entity Framework Core 9.0
- **Frontend**: HTML5, CSS3, JavaScript (ES6+)
- **API Documentation**: OpenAPI/Swagger
- **Architecture**: Clean Architecture with separation of concerns

## 📋 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or full version)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd Survey-Assessment
```

### 2. Database Setup

1. **Configure Connection String**: Update the connection string in `appsettings.json` and `appsettings.Development.json` to point to your SQL Server instance.

2. **Run Database Setup Script**: Execute the SQL script to create the database and tables:
   ```bash
   sqlcmd -S (localdb)\MSSQLLocalDB -i setup_sqlserver_database.sql
   ```
   
   Or follow the detailed instructions in `SQL_SERVER_SETUP.md`.

### 3. Build and Run

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

The application will be available at:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`

### 4. Access the Application

- **Survey Form**: Navigate to the root URL to fill out surveys
- **Survey Results**: Click "VIEW SURVEY RESULTS" to see statistics
- **API Documentation**: Visit `/swagger` (in development mode) to explore the API

## 📁 Project Structure

```
Survey-Assessment/
├── Controllers/           # API Controllers
│   └── SurveyController.cs
├── Data/                 # Database Context
│   └── SurveyDbContext.cs
├── DTOs/                 # Data Transfer Objects
│   ├── SurveyDto.cs
│   └── SurveyStatisticsDto.cs
├── Extensions/           # Service Configuration
│   └── ServiceCollectionExtensions.cs
├── Models/               # Entity Models
│   ├── Options.cs
│   └── PersonalDetails.cs
├── Services/             # Business Logic
│   └── SurveyService.cs
├── wwwroot/              # Static Web Files
│   ├── index.html        # Survey Form
│   ├── view-survey-results.html # Results View
│   ├── script.js         # JavaScript Logic
│   └── style.css         # Styling
├── docs/                 # Documentation
│   ├── Architecture.md
│   ├── CHECKBOX_RADIO_HANDLING.md
│   └── ProjectStructure.md
└── appsettings.json      # Configuration
```

## 🔧 API Endpoints

### Survey Controller (`/api/survey`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/submit` | Submit a new survey response |
| `GET` | `/statistics` | Get survey statistics and results |

### Example API Usage

**Submit Survey**:
```http
POST /api/survey/submit
Content-Type: application/json

{
  "personalDetails": {
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "phoneNumber": "1234567890",
    "dateOfBirth": "1990-01-01"
  },
  "options": {
    "favoriteColor": "Blue",
    "favoriteFood": "Pizza",
    "hobbies": ["Reading", "Gaming"]
  }
}
```

**Get Statistics**:
```http
GET /api/survey/statistics
```

## 🎨 Frontend Features

- **Responsive Design**: Adapts to different screen sizes
- **Form Validation**: Real-time validation with user feedback
- **Interactive UI**: Modern styling with hover effects and animations
- **Data Visualization**: Statistics displayed in an easy-to-read format
- **Navigation**: Simple navigation between survey form and results

## 🗄️ Database Schema

The application uses the following main entities:

- **PersonalDetails**: Stores user information (name, email, phone, date of birth)
- **Options**: Stores survey responses (favorite color, food, hobbies)

Refer to `setup_sqlserver_database.sql` for the complete database schema.

## 🔧 Configuration

Key configuration options in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SurveyDb;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## 🧪 Testing

The project includes HTTP test files for API testing:

- `SurveyApi.http`: Basic API tests
- `TestSurveyApi.http`: Extended test scenarios

Use these with Visual Studio or VS Code REST Client extension.

## 📚 Documentation

Additional documentation is available in the `docs/` folder:

- **Architecture.md**: System architecture overview
- **ProjectStructure.md**: Detailed project organization
- **CHECKBOX_RADIO_HANDLING.md**: Frontend form handling details

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

If you encounter any issues or have questions:

1. Check the documentation in the `docs/` folder
2. Review the SQL setup guide in `SQL_SERVER_SETUP.md`
3. Open an issue in the repository

## 🚀 Future Enhancements

- User authentication and authorization
- Advanced survey types and question formats
- Data export capabilities
- Real-time analytics dashboard
- Mobile application
- Survey templates and customization
