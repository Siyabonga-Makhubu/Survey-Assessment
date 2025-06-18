// Survey form handling with support for both checkboxes and radio buttons

function SubmitEvent(event) {
    event.preventDefault();
    
    // Get form data
    const formData = new FormData(event.target);
    
    // Personal details
    const fullName = formData.get('fullname')?.trim();
    const email = formData.get('email')?.trim();
    const dateOfBirth = formData.get('Date-of-Birth');
    const contactNumbers = formData.get('phone')?.trim();
    
    // Validation for personal details
    if (!fullName) {
        alert('Please enter your full name.');
        document.getElementById('fullname').focus();
        return;
    }
    
    if (!email) {
        alert('Please enter your email address.');
        document.getElementById('email').focus();
        return;
    }
      if (!dateOfBirth) {
        alert('Please select your date of birth.');
        document.getElementById('Date-of-Birth').focus();
        return;
    }
    
    // Calculate age and validate age range (5-120)
    const birthDate = new Date(dateOfBirth);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    
    // Adjust age if birthday hasn't occurred this year
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }
    
    if (age < 5) {
        alert('Age must be at least 5 years old.');
        document.getElementById('Date-of-Birth').focus();
        return;
    }
    
    if (age > 120) {
        alert('Age cannot be more than 120 years old.');
        document.getElementById('Date-of-Birth').focus();
        return;
    }
    
    if (!contactNumbers) {
        alert('Please enter your contact number.');
        document.getElementById('phone').focus();
        return;
    }
    
    // Validate phone number (basic validation)
    if (contactNumbers.length < 10) {
        alert('Please enter a valid contact number (at least 10 digits).');
        document.getElementById('phone').focus();
        return;
    }
    
    // Get favorite foods (checkboxes - multiple selections allowed)
    const favoriteFoods = [];
    const foodCheckboxes = document.querySelectorAll('input[name="food"]:checked');
    foodCheckboxes.forEach(checkbox => {
        favoriteFoods.push(checkbox.value);
    });
    
    // Validate that at least one food is selected
    if (favoriteFoods.length === 0) {
        alert('Please select at least one favorite food.');
        return;
    }
    
    // Get statement ratings (radio buttons - single selection per statement)
    const movieRating = getRadioValue('statement1');
    const radioRating = getRadioValue('statement2');
    const eatOutRating = getRadioValue('statement3');
    const tvRating = getRadioValue('statement4');
    
    // Validation for ratings with specific messages
    if (!movieRating) {
        alert('Please rate the statement: "I like to watch movies"');
        return;
    }
    
    if (!radioRating) {
        alert('Please rate the statement: "I like to listen to radio"');
        return;
    }
    
    if (!eatOutRating) {
        alert('Please rate the statement: "I like to eat out"');
        return;
    }
    
    if (!tvRating) {
        alert('Please rate the statement: "I like to watch TV"');
        return;
    }
    
    // Create submission object with proper casing to match API DTOs
    const submission = {
        fullName: fullName,
        email: email,
        dateOfBirth: dateOfBirth,
        contactNumbers: contactNumbers,
        favoriteFoods: favoriteFoods,
        movieRating: parseInt(movieRating),
        radioRating: parseInt(radioRating),
        eatOutRating: parseInt(eatOutRating),
        tvRating: parseInt(tvRating)
    };
    
    console.log('Submitting survey:', submission);
    
    // Disable submit button to prevent double submission
    const submitButton = event.target.querySelector('button[type="submit"]');
    const originalButtonText = submitButton.textContent;
    submitButton.disabled = true;
    submitButton.textContent = 'Submitting...';
    
    // Submit to API
    fetch('/api/survey/submit', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(submission)
    })
    .then(response => {
        console.log('Response status:', response.status);
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`HTTP error! status: ${response.status}, message: ${text}`);
            });
        }
        return response.json();
    })
    .then(data => {
        console.log('Success:', data);
        alert(`Survey submitted successfully! Survey ID: ${data.surveyId || data.SurveyId}`);
        
        // Reset form
        event.target.reset();
        
        // Optionally redirect to results page
        // window.location.href = 'view-survey-results.html';
    })
    .catch(error => {
        console.error('Error:', error);
        alert(`Error submitting survey: ${error.message}. Please check the console for more details and try again.`);
    })
    .finally(() => {
        // Re-enable submit button
        submitButton.disabled = false;
        submitButton.textContent = originalButtonText;
    });
}

// Helper function to get radio button value
function getRadioValue(name) {
    const radio = document.querySelector(`input[name="${name}"]:checked`);
    return radio ? radio.value : null;
}

// Load survey results for the results page
function loadSurveyResults() {
    fetch('/api/survey/all')
        .then(response => response.json())
        .then(data => {
            displaySurveyResults(data);
        })
        .catch(error => {
            console.error('Error loading survey results:', error);
        });
}

// Load survey statistics
function loadSurveyStatistics() {
    fetch('/api/survey/statistics')
        .then(response => response.json())
        .then(data => {
            displaySurveyStatistics(data);
        })
        .catch(error => {
            console.error('Error loading survey statistics:', error);
        });
}

// Display survey results in a table
function displaySurveyResults(surveys) {
    const container = document.getElementById('survey-results');
    if (!container) return;
    
    if (surveys.length === 0) {
        container.innerHTML = '<p>No survey results found.</p>';
        return;
    }
    
    let html = '<table class="results-table">';
    html += '<thead><tr>';
    html += '<th>Survey ID</th>';
    html += '<th>Full Name</th>';
    html += '<th>Email</th>';
    html += '<th>Date of Birth</th>';
    html += '<th>Contact</th>';
    html += '<th>Favorite Foods</th>';
    html += '<th>Movie Rating</th>';
    html += '<th>Radio Rating</th>';
    html += '<th>Eat Out Rating</th>';
    html += '<th>TV Rating</th>';
    html += '<th>Submission Date</th>';
    html += '</tr></thead><tbody>';
    
    surveys.forEach(survey => {
        html += '<tr>';
        html += `<td>${survey.surveyID}</td>`;
        html += `<td>${survey.fullName}</td>`;
        html += `<td>${survey.email}</td>`;
        html += `<td>${new Date(survey.dateOfBirth).toLocaleDateString()}</td>`;
        html += `<td>${survey.contactNumbers}</td>`;
        html += `<td>${survey.favoriteFoods.join(', ')}</td>`;
        html += `<td>${survey.movieRating || 'N/A'}</td>`;
        html += `<td>${survey.radioRating || 'N/A'}</td>`;
        html += `<td>${survey.eatOutRating || 'N/A'}</td>`;
        html += `<td>${survey.tvRating || 'N/A'}</td>`;
        html += `<td>${new Date(survey.submissionDate).toLocaleDateString()}</td>`;
        html += '</tr>';
    });
    
    html += '</tbody></table>';
    container.innerHTML = html;
}

// Display survey statistics
function displaySurveyStatistics(stats) {
    console.log('Displaying statistics:', stats);
    const container = document.getElementById('survey-statistics');
    if (!container) {
        console.error('Statistics container not found!');
        return;
    }
    
    if (!stats || stats.totalSurveys === undefined) {
        container.innerHTML = '<p style="color: red;">Invalid statistics data received</p>';
        return;
    }

    let html = `
        <div class="stats-grid">
            <div class="stat-row">
                <span class="stat-label">Total number of surveys :</span>
                <span class="stat-value">${stats.totalSurveys}</span>
            </div>
            <div class="stat-row">
                <span class="stat-label">Average Age :</span>
                <span class="stat-value">${stats.averageAge}</span>
            </div>
            <div class="stat-row">
                <span class="stat-label">Oldest person who participated in survey :</span>
                <span class="stat-value">${stats.oldestAge}</span>
            </div>
            <div class="stat-row">
                <span class="stat-label">Youngest person who participated in survey :</span>
                <span class="stat-value">${stats.youngestAge}</span>
            </div>
            
            <div class="stat-row">
                <span class="stat-label">Percentage of people who like Pizza :</span>
                <span class="stat-value">${stats.pizzaPercentage} % Pizza</span>
            </div>
            <div class="stat-row">
                <span class="stat-label">Percentage of people who like Pasta :</span>
                <span class="stat-value">${stats.pastaPercentage} % Pasta</span>
            </div>
            <div class="stat-row">
                <span class="stat-label">Percentage of people who like Pap and Wors :</span>
                <span class="stat-value">${stats.papAndWorsPercentage} % Pap and Wors</span>
            </div>
            
            <div class="stat-row">
                <span class="stat-label">People who like to watch movies :</span>
                <span class="stat-value">${stats.movieAverageRating} average of rating</span>
            </div>
            <div class="stat-row">
                <span class="stat-label">People who like to listen to radio :</span>
                <span class="stat-value">${stats.radioAverageRating} average of rating</span>
            </div>
            <div class="stat-row">
                <span class="stat-label">People who like to eat out :</span>
                <span class="stat-value">${stats.eatOutAverageRating} average of rating</span>
            </div>
            <div class="stat-row">
                <span class="stat-label">People who like to watch TV :</span>
                <span class="stat-value">${stats.tvAverageRating} average of rating</span>
            </div>
        </div>
    `;
    
    container.innerHTML = html;
    console.log('Statistics displayed successfully');
}

// Initialize page-specific functionality
document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM loaded, current path:', window.location.pathname);
    console.log('Full URL:', window.location.href);
    
    // Check if we have the survey statistics container on this page
    const statisticsContainer = document.getElementById('survey-statistics');
    
    if (statisticsContainer) {
        console.log('Found survey statistics container, loading data...');
        loadSurveyStatistics();
    } else {
        console.log('Survey statistics container not found, this is not the results page');
    }
});
