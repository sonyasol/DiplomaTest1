const token = localStorage.getItem('token');
const userRole = localStorage.getItem('userRole');
const userId = localStorage.getItem('userId');
const API_URL = 'https://localhost:7248/api'; 

if (!token) {
    window.location.href = 'login.html';
}

if (userRole === 'Patient') {
    const pageId = new URLSearchParams(window.location.search).get('id');

    if (pageId !== userId) {
        window.location.href = `patients-detail.html?id=${userId}`;
    }

    const backLink = document.querySelector('.nav-link-patients');
    if (backLink) backLink.style.display = 'none';

    const addBtn = document.querySelector('.btn-primary')
    if (addBtn) addBtn.style.display = 'none';
}

let currentPatientId = null;
let currentPatient = null;

function getPatientIdFromUrl() {
    const params = new URLSearchParams(window.location.search);
    return params.get('id');
}

async function loadPatient() {
    const patientId = getPatientIdFromUrl();
    if (!patientId) {
        showError('ID пациента не указан');
        return;
    }

    currentPatientId = patientId;

    try {
        const userResponse = await fetch(`${API_URL}/users/${patientId}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });
        if (!userResponse.ok) throw new Error('Пациент не найден');
        currentPatient = await userResponse.json();

        displayPatientInfo(currentPatient);

        await loadVaccines(patientId);
    } catch (err) {
        showError(err.message);
    }
}

function displayPatientInfo(patient) {
    const fullName = `${patient.lastName || ''} ${patient.firstName || ''} ${patient.middleName || ''}`.trim();
    document.getElementById('patientName').textContent = fullName || '-';
    document.getElementById('patientBirthDate').textContent = formatDate(patient.dateOfBirth);
    document.getElementById('patientSnils').textContent = formatSnils(patient.snils);
    document.getElementById('patientPhone').textContent = patient.phone || '-';
    document.getElementById('patientEmail').textContent = patient.email || '-';
}

async function loadVaccines(patientId) {
    try {
        const response = await fetch(`${API_URL}/Record/user/${patientId}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });
        if (!response.ok) {
            if (response.status === 404) {
                displayVaccines([]);
                return;
            }
            throw new Error('Ошибка загрузки прививок');
        }

        const vaccines = await response.json();
        displayVaccines(vaccines);
    } catch (err) {
        console.error('Ошибка: ', err);
        displayVaccines([]);
    }
}

function displayVaccines(vaccines) {
    const container = document.getElementById('vaccineList');
    const countSpan = document.getElementById('vaccineCount');

    countSpan.textContent = `${vaccines.length} ${getDeclension(vaccines.length)}`;

    if (!vaccines || vaccines.length === 0) {
        container.innerHTML= `
            <div class="empty-state">
                <div class="empty-icon">💉</div>
                <p>Нет данных о прививках</p>
                <button class="btn btn-primary" onclick="openAddVaccineModal()">
                    Добавить первую прививку
                </button>
            </div>
        `;
        return;
    }

    container.innerHTML = vaccines.map(vaccine => `
        <div class="vaccine-card">
            <div class="vaccine-header">
                <div class="vaccine-title">
                    <span class="vaccine-icon">💊</span>
                    <div>
                        <h3>${vaccine.vaccineName || '-'}</h3>
                        <p class="vaccine-infection">${vaccine.infectionName || 'Не указано'}</p>
                    </div>
                </div>
                <div class="vaccine-actions">
                    <button class="btn-icon" onclick="deleteVaccine(${vaccine.id})" title="Удалить">🗑️</button>
                </div>
            </div>

            <div class="vaccine-details">
                <div class="detail-item">
                    <label>Дата</label>
                    <span>${formatDate(vaccine.date) || '-'}</span>
                </div>
                <div class="detail-item">
                    <label>Серия</label>
                    <span>${vaccine.series || '-'}</span>
                </div>
                <div class="detail-item">
                    <label>Реакция</label>
                    <span class="reaction-badge ${vaccine.reaction ? 'reaction-yes' : 'reaction-no'}">
                        ${vaccine.reaction ? '⚠️ Была реакция' : '✅ Норма'}
                    </span>
                </div>
            </div>
        </div>
        `).join('');
}

async function addVaccine(event) {
    event.preventDefault();

    const vaccineId = document.getElementById('fieldVaccine').value;
    const date = document.getElementById('vaccineDate').value;
    const reaction = document.getElementById('hasReaction').checked;

    if (!vaccineId) return alert('Выберите вакцину');
    if (!date) return alert('Укажите дату');

    const recordData = {
        userId: parseInt(currentPatientId),
        vaccineId: parseInt(vaccineId),
        date: date,
        reaction: reaction
    };

    try {
        const response = await fetch(`${API_URL}/Record`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(recordData)
        });
        
        if (!response.ok) throw new Error('Ошибка при добавлении');

        closeVaccineModal();
        await loadVaccines(currentPatientId);
    } catch (err) {
        alert('Ошибка: ' + err.message);
    }
}

async function deleteVaccine(recordId) {
    if (!confirm('Удалить запись о прививке?')) return;

    try {
        const response = await fetch(`${API_URL}/Record/${recordId}`, {
            method: 'DELETE',
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) throw new Error('Ошибка при удалении');

        await loadVaccines(currentPatientId);
    } catch (err) {
        alert('Ошибка: ' + err.message);
    }
}

async function openAddVaccineModal() {
    document.getElementById('vaccineForm').reset();

    try {
        const response = await fetch(`${API_URL}/vaccines`, {
            headers: { 'Authorization': `Bearer ${token}`}
        });
        const vaccines = await response.json();

        const select = document.getElementById('fieldVaccine');
        select.innerHTML = '<option value="">- выберите вакцину -</option>' +
        vaccines.map(v => 
            `<option value="${v.id}">${v.name} (серия ${v.series})</option>`).join('');
    } catch(err) {
        console.error('Ощибка загрузки вакцин: ', err);
    }

    document.getElementById('vaccineModal').classList.add('open');
}

function closeVaccineModal() {
    document.getElementById('vaccineModal').classList.remove('open');
}

function formatDate(date) {
    if (!date) return '-';
    return new Date(date).toLocaleDateString('ru-Ru');
}

function formatSnils(snils) {
    if (!snils) return '-';
    const str = snils.toString().replace(/\D/g, '');
    if (str.length === 11) {
        return `${str.slice(0, 3)}-${str.slice(3, 6)}-${str.slice(6, 9)} ${str.slice(9)}`;
    }
    return snils;
}

function getDeclension(count) {
    if (count % 10 === 1 && count % 10 !== 11) return 'прививка';
    if ([2, 3, 4].includes(count % 10) && ![12,13,14].includes(count % 100)) return 'прививки';
    return 'прививок';
}

function showError(message) {
    document.getElementById('vaccineList').innerHTML = `
        <div class="error-state">
            <p>⚠️ ${message}</p>
            <button class="btn btn-secondary" onclick="location.reload()">Обновить</button>
        </div>
    `;
}

document.getElementById('vaccineForm').addEventListener('submit', addVaccine);
document.getElementById('vaccineModal').addEventListener('click', (e) => {
    if (e.target === document.getElementById('vaccineModal')) {
        closeVaccineModal();
    }
});

loadPatient()