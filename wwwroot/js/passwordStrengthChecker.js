const passwordInput = $("#passwordInput");
const strengthText = $("#passwordStrength");

// matches backend validation
function getPasswordScore() {
    const password = passwordInput.val();
    let score = 0;
    
    // min 12 characters
    if (password.length >= 12) {
        score++;

        // at least one digit
        if (password.match("[0-9]")) {
            score++;
        }
        
        // at least one lowercase and one uppercase
        if (password.match("(?=.*[a-z])(?=.*[A-Z]).+")) {
            score++;
        }

        // at least one special character
        if (password.match("[^0-9a-zA-Z ]")) {
            score++;
        }
    }
    
    return score;
}

passwordInput.on("input", function () {
    const score = getPasswordScore();
    
    let text;
    let colorClass; // danger, warning, success 
    switch (score) {
        case 1:
            text = "Very Weak";
            colorClass = "danger";
            break;
        case 2:
            text = "Weak";
            colorClass = "danger";
            break;
        case 3:
            text = "Medium";
            colorClass = "warning";
            break;
        case 4:
            text = "Strong";
            colorClass = "success";
            break;
    }
    
    if (!text) {
        strengthText.hide();
        return;
    }

    strengthText.removeClass();
    strengthText.addClass(`text-${colorClass}`);
    strengthText.text(`(${text})`);
    strengthText.show();
});