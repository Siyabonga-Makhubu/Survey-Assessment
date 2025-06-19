# Survey API - Handling Checkboxes and Radio Buttons

This project demonstrates how to handle both **checkboxes** (multi-select) and **radio buttons** (single-select) in a C# Survey API using Clean Architecture principles.

## Overview

The survey form includes:
1. **Checkboxes for Food Preferences**: Users can select multiple favorite foods (Pizza, Pasta, Pap and Wors, Other)
2. **Radio Buttons for Agreement Ratings**: Users rate statements on a 1-5 scale (single selection per statement)

## Database Schema

### `personaldetails` table
Stores the user's personal information:
- `SurveyID` (Primary Key)
- `FullName` 
- `Email`
- `DateOfBirth`
- `ContactNumbers`
- `SubmissionDate`

### `options` table
Stores both checkbox and radio button responses using a flexible key-value approach:
- `SurveyID` (Foreign Key)
- `OptionValue` (Part of composite primary key)
- `Rating` (nullable integer)

## How Data is Stored

### Checkbox Data (Multi-select)
Food preferences are stored as separate rows with `OptionValue` like:
```sql
INSERT INTO options (SurveyID, OptionValue, Rating) VALUES 
(1, 'FavoriteFood:Pizza', NULL),
(1, 'FavoriteFood:Pasta', NULL);  -- Multiple rows for one survey
```

### Radio Button Data (Single-select)
Statement ratings are stored with the rating value:
```sql
INSERT INTO options (SurveyID, OptionValue, Rating) VALUES 
(1, 'MovieRating', 4),     -- Single row per statement
(1, 'RadioRating', 3),
(1, 'EatOutRating', 5),
(1, 'TVRating', 2);
```

## API Structure

### Models
- `PersonalDetails.cs`: Personal information
- `Options.cs`: Flexible response storage

### DTOs
- `SurveySubmissionDto`: Input for form submission
- `SurveyResponseDto`: Output for retrieving surveys
- `SurveyStatisticsDto`: Aggregated statistics

### Controllers
- `SurveyController.cs`: RESTful endpoints for survey operations

### Services
- `ISurveyService` & `SurveyService`: Business logic layer

## Frontend Implementation

### Form Structure
```html
<!-- Checkboxes (Multi-select) -->
<input type="checkbox" name="food" /> Pizza
<input type="checkbox" name="food" /> Pasta
<input type="checkbox" name="food" /> Pap and Wors
<input type="checkbox" name="food" /> Other

<!-- Radio Buttons (Single-select per group) -->
<input type="radio" name="statement1" value="1" /> Strongly Agree
<input type="radio" name="statement1" value="2" /> Agree
<input type="radio" name="statement1" value="3" /> Neutral
<!-- ... -->
```

### JavaScript Handling
```javascript
// Get multiple checkbox values
const favoriteFoods = [];
const foodCheckboxes = document.querySelectorAll('input[name="food"]:checked');
foodCheckboxes.forEach(checkbox => {
    const label = checkbox.closest('label').textContent.trim();
    favoriteFoods.push(label);
});

// Get single radio button value
function getRadioValue(name) {
    const radio = document.querySelector(`input[name="${name}"]:checked`);
    return radio ? radio.value : null;
}
```

## API Endpoints

### Submit Survey
```
POST /api/survey/submit
Content-Type: application/json

{
  "fullName": "John Doe",
  "email": "john@example.com",
  "dateOfBirth": "1990-05-15",
  "contactNumbers": "+1-555-123-4567",
  "favoriteFoods": ["Pizza", "Pasta"],  // Multiple selections allowed
  "movieRating": 4,                     // Single rating per statement
  "radioRating": 3,
  "eatOutRating": 5,
  "tvRating": 2
}
```

### Get All Surveys
```
GET /api/survey/all
```

### Get Survey Statistics
```
GET /api/survey/statistics
```

## Key Benefits of This Approach

1. **Flexible Schema**: The `options` table can handle both multi-select and single-select data
2. **Clean Separation**: DTOs clearly separate input/output from database models
3. **Type Safety**: Strong typing in C# with nullable ratings for optional data
4. **Scalability**: Easy to add new question types without schema changes
5. **Statistics**: Built-in support for calculating percentages and averages

## Running the Application

1. **Setup Database**: Execute `setup_sqlserver_database.sql` in SQL Server
2. **Restore Packages**: `dotnet restore SurveyApi.csproj`
3. **Run Application**: `dotnet run --project SurveyApi.csproj --urls "http://localhost:5001"`
4. **Access Survey**: Navigate to `http://localhost:5001` for the form
5. **View Results**: Navigate to `http://localhost:5001/view-survey-results.html`

## Example Usage Scenarios

### Multi-select Checkboxes
- Food preferences: Users can like both Pizza AND Pasta
- Hobbies: Users can enjoy multiple activities
- Skills: Users can have multiple programming languages

### Single-select Radio Buttons  
- Agreement scales: One rating per statement
- Gender selection: Only one option
- Priority ranking: Single choice per category

This approach provides maximum flexibility while maintaining data integrity and type safety.
