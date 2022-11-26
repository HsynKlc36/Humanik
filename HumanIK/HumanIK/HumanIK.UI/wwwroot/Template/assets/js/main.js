const eyes = document.querySelectorAll(".show-hide-icon");

eyes.forEach(eye => {
    const input = eye.previousElementSibling;
    eye.addEventListener("click", (e) => {
        const type = input.getAttribute("type") === "password" ? "text" : "password";
        input.setAttribute("type", type);
        eye.classList.toggle("fa-eye-slash")
    })
})