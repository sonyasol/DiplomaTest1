const token = localStorage.getItem('token');
const userRole = localStorage.getItem('userRole');
const userId = localStorage.getItem('userId');
const API_URL = 'https://localhost:7248/api'; 

if (!token) {
    window.location.href = 'login.html'
}

if (userRole === 'Patient') {
    window.location.href = `patients-detail.html?id=${userId}`;
}

let allUsers = [];

function goToPatientPage(patientId) 
{
    window.location.href = `patients-detail.html?id=${patientId}`;
}

async function loadUsers() {
    showLoading();
    try {
        const response = await fetch(`${API_URL}/users`, {
            headers: {'Authorization': `Bearer ${token}` }
        });
        if (!response.ok) throw new Error(`������ ${response.status}`);
        allUsers = await response.json();
        renderTable(allUsers);
        updateStats(allUsers.length, allUsers.length);
    } catch (err) {
        showError(err.message);
    }
}

function renderTable(users) {
    const tbody = document.getElementById('tableBody');
    const state = document.getElementById('tableState');

    // если нет пользователей
    if (users.length === 0) {
        tbody.innerHTML = '';
        state.innerHTML = `
            <div class="state-box">
                <div class="state-title">Ничего не найдено</div>
            </div>`;
        return;
    }

    // если есть пользователи
    state.innerHTML = '';
    tbody.innerHTML = users.map(u => `
        <tr onclick="goToPatientPage(${u.id})">
            <td>${u.fullName || '-'}</td>
            <td>${formatDate(u.dateOfBirth)}</td>
            <td>${formatSnils(u.snils)}</td>
            <td>${u.snils || '-'}</td>
            <td><button class="btn-detail">Подробнее</button></td>
        </tr>
    `).join('');
}

function updateStats(total, filtered) {
            document.getElementById('statTotal').textContent = total;
            document.getElementById('statFiltered').textContent = filtered;
        }

// ========================
        // ПОИСК
        // ========================
        document.getElementById('searchInput').addEventListener('input', function() {
            const query = this.value.toLowerCase().trim();
            const filtered = allUsers.filter(u =>
                (u.fullName || '').toLowerCase().includes(query) ||
                (u.snils || '').includes(query)
            );
            renderTable(filtered);
            updateStats(allUsers.length, filtered.length);
        });


        // Форматирование даты: 2000-05-23 → 23.05.2000
        function formatDate(d) {
            if (!d) return '—';
            return new Date(d).toLocaleDateString('ru-RU');
        }

        // Форматирование СНИЛС: 12345678901 → 123-456-789 01
        function formatSnils(s) {
            if (!s) return '—';
            const n = s.toString().replace(/\D/g, '');
            if (n.length === 11)
                return `${n.slice(0,3)}-${n.slice(3,6)}-${n.slice(6,9)} ${n.slice(9)}`;
            return s;
        }

function showLoading() {
    document.getElementById('tableBody').innerHTML = '';
    document.getElementById('tableState').innerHTML = `
        <div class="state-box">
            <div class="state-title">Загрузка данных...</div>
        </div>`;    
}

function showError(msg) {
            document.getElementById('tableBody').innerHTML = '';
            document.getElementById('tableState').innerHTML = `
                <div class="state-box">
                    <div class="state-icon">⚠️</div>
                    <div class="state-title">Не удалось загрузить данные</div>
                    <div class="state-sub">${msg}</div>
                </div>`;
        }

console.log('JS работает');

function openAddModal() {
    document.getElementById('fieldLastName').value = '';
    document.getElementById('fieldFirstName').value = '';
    document.getElementById('fieldMiddleName').value = '';
    document.getElementById('fieldDateOfBirth').value = '';
    document.getElementById('fieldSnils').value = '';
    document.getElementById('fieldPhone').value = '';
    document.getElementById('fieldEmail').value = '';
    document.getElementById('fieldPassword').value = '';
    document.getElementById('fieldRole').value = '';

    hideError();
    document.getElementById('snilsHint').textContent = '';

    document.getElementById('addModalOverlay').classList.add('open');
}

function closeAddModal() {
    document.getElementById('addModalOverlay').classList.remove('open');
}

document.getElementById('addModalClose').addEventListener('click', closeAddModal);

document.getElementById('addModalCancel').addEventListener('click', closeAddModal);

document.getElementById('addModalOverlay').addEventListener('click', function(e) {
    if (e.target === this) closeAddModal();
});

document.getElementById('fieldSnils').addEventListener('input', async function() {
    const snils = this.value.trim();
    const hint = document.getElementById('snilsHint');

    if (snils.length < 11) {
        hint.textContent = '';
        hint.className = 'field-hint';
        return;
    }

    if (!/^\d{11}$/.test(snils)) {
        hint.textContent = 'СНИЛС должен содержать только цифры';
        hint.className = 'field-hint hint-error';
        return;
    }

    hint.textContent = 'Проверка...';
    hint.className = 'field-hint';

    try {
        const response = await fetch(`${API_URL}/users/snils/${snils}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (response.ok) {
            hint.textContent = 'Пользователь с таким СНИЛС уже существует';
            hint.className = 'field-hint hint-error'
        } else if (response.status === 404) {
            hint.textContent = 'СНИЛС свободен';
            hint.className = 'field-hint hint-ok'
        }
    } catch (err) {
        hint.textContent = '';
    }
});

async function saveUser() {
    const lastName = document.getElementById('fieldLastName').value.trim();
    const firstName = document.getElementById('fieldFirstName').value.trim();
    const middleName = document.getElementById('fieldMiddleName').value.trim();
    const dateOfBirth = document.getElementById('fieldDateOfBirth').value.trim();
    const snils = document.getElementById('fieldSnils').value.trim();
    const phone = document.getElementById('fieldPhone').value.trim();
    const email = document.getElementById('fieldEmail').value.trim();
    const password = document.getElementById('fieldPassword').value.trim();
    const roleId = document.getElementById('fieldRole').value.trim();

    if (!lastName) return showError('Введите фамилию');
    if (!firstName) return showError('Введите имя');
    if (!dateOfBirth) return showError('Введите дату рождения');
    if (!snils) return showError('Введите СНИЛС');
    if (!/^\d{11}$/.test(snils)) return showError('СНИЛС должен содержать 11 цифр');
    if (!password) return showError('Введите пароль');
    if (password.length < 6) return showError('Пароль минимум 6 символов');
    if (!roleId) return showError('Выберите роль');

    const newUser = {
        lastName,
        firstName, 
        middleName,
        dateOfBirth,
        snils, 
        phone, 
        email,
        password,
        roleId: parseInt(roleId)
    };

    try {
        const response = await fetch(`${API_URL}/users`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(newUser)
        });

        if (response.ok) {
            closeAddModal();
            await loadUsers();
        } else {
            const text = await response.text();
            showError(text || `Ошибка ${response.status}`);
        }
    } catch (err) {
        showError('Не удалось подключиться к серверу');
    }
}

function showError(message) {
    const block = document.getElementById('addModalError');
    block.textContent = message;
    block.style.display = 'block';
}

function hideError() {
    const block = document.getElementById('addModalError');
    block.textContent = '';
    block.style.display = 'none';
}



loadUsers();



