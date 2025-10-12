document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.getElementById('account') || document.getElementById('registerForm');
    const submitButton = document.getElementById('login-submit') || document.getElementById('registerSubmit');

    // Reusable function to set up a password toggle
    const setupPasswordToggle = (inputId, toggleId) => {
        const passwordInput = document.getElementById(inputId);
        const passwordToggle = document.getElementById(toggleId);

        if (passwordToggle && passwordInput) {
            passwordToggle.addEventListener('click', () => {
                const type = passwordInput.type === 'password' ? 'text' : 'password';
                passwordInput.type = type;
                passwordToggle.classList.toggle('show-password', type === 'text');
            });
        }
    };

    // Set up toggles for both login and register pages
    setupPasswordToggle('Input_Password', 'passwordToggle');
    setupPasswordToggle('Input_ConfirmPassword', 'confirmPasswordToggle');

    // Add CSS classes for validation errors
    const observer = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            if (mutation.target.hasAttribute('data-valmsg-for')) {
                const errorSpan = mutation.target;
                const inputId = errorSpan.getAttribute('data-valmsg-for').replace('.', '_');
                const inputElement = document.getElementById(inputId);
                if (inputElement) {
                    const formGroup = inputElement.closest('.form-group');
                    if (formGroup) {
                        if (errorSpan.textContent.trim() !== '') {
                            formGroup.classList.add('error');
                            errorSpan.classList.add('show');
                        } else {
                            formGroup.classList.remove('error');
                            errorSpan.classList.remove('show');
                        }
                    }
                }
            }
        });
    });

    document.querySelectorAll('[data-valmsg-for]').forEach(span => {
        observer.observe(span, { childList: true, characterData: true, subtree: true });
        // Initial check in case errors are already present
        if (span.textContent.trim() !== '') {
            const event = new Event('DOMNodeInserted');
            span.dispatchEvent(event);
        }
    });

    // Loading spinner on submit
    if (loginForm && submitButton) {
        loginForm.addEventListener('submit', (e) => {
            // A brief delay to allow client-side validation to run
            setTimeout(() => {
                const hasErrors = loginForm.querySelector('.field-validation-error');
                if (!hasErrors) {
                    submitButton.classList.add('loading');
                    submitButton.disabled = true;
                }
            }, 50);
        });
    }
});