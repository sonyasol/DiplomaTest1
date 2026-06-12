const API_URL = 'https://localhost:7248/api';

async function login() {
    const snils = document.getElementById('fieldSnils').value.trim();
    const password = document.getElementById('fieldPassword').value.trim();

    if (!snils) return showLoginError('Введите СНИЛС');
    if (!/^\d{11}$/.test(snils)) return showLoginError('СНИЛС должен содержать 11 цифр');
    if (!password) return showLoginError('Введите пароль');

    try {
        const response = await fetch(`${API_URL}/auth/login`, {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({snils, password})
        });

        if (response.ok) {
            const data = await response.json();

            localStorage.setItem('token', data.token);
            localStorage.setItem('userId', data.userId);
            localStorage.setItem('userRole', data.role);
            localStorage.setItem('userName', data.fullName);

            if (data.role === 'Admin' || data.role === 'Doctor') {
                window.location.href = 'patients.html';
            }  else {
                window.location.href = `patients-detail.html?id=${data.userId}`;
            }
        } else {
            showLoginError('Неверный СНИЛС или пароль');
        }
    } catch (err) {
        showLoginError('Не удалось подключиться к серверу');
    }
}

function showLoginError(message) {
    const block = document.getElementById('loginError');
    block.textContent = message;
    block.style.display = 'block';
}

document.getElementById('fieldPassword').addEventListener('keydown', function(e) {
    if (e.key === 'Enter') login();
});