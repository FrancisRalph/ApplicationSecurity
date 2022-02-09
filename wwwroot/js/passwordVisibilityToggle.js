const toggles = document.querySelectorAll("[data-password-toggle]");
for (let toggle of toggles) {
    const inputId = toggle.getAttribute("data-password-toggle");
    const input = document.getElementById(inputId);
    
    const icon = document.createElement("i");
    toggle.appendChild(icon);
    
    function updateToggle(_isToggled) {
        input.setAttribute(
            "type",
            _isToggled ? "text" : "password"
        );
        icon.className = `fas ${_isToggled ? "fa-eye-slash" : "fa-eye"}`
    }
    
    let isToggled = false;
    updateToggle(isToggled)
    
    toggle.addEventListener("click", function () {
        isToggled = !isToggled;
        updateToggle(isToggled);
    })
}
