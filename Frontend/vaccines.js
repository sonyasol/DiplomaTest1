const token = localStorage.getItem('token');
const userRole = localStorage.getItem('userRole');
const userId = localStorage.getItem('userId');
const API_URL = 'https://localhost:7248/api'; 

if (!token) {
    window.location.href = 'login.html';
}

let allVaccines = [];
 
async function loadVaccines() {
    showLoading();
    try {
        const response = await fetch(`${API_URL}/Vaccine`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });
        if (!response.ok) throw new Error(`������ ${response.status}`);
        allVaccines = await response.json();
        renderTable(allVaccines);
        updateStats(allVaccines.length, allVaccines.length);
    } catch (err) {
        showError(err.message);
    }
}

function renderTable(vaccine) {
    const tbody = document.getElementById('tableBody');
    const state = document.getElementById('tableState');

    if (vaccine.length === 0) {
        tbody.innerHTML = '';
        state.innerHTML = `
            <div class="state-box">
                <div class="state-title">Ничего не найдено</div>
            </div>`;
        return;
    }

    state.innerHTML = '';
    tbody.innerHTML = vaccine.map(v => `
        <tr onclick="openModal(${v.id})">
            <td class="td-name">${v.name || '-'}</td>
            <td class="td-series">${v.series}</td>
            <td class="td-expDate">${v.expiryDate || '-'}</td>
            <td class="td-infectionName">${v.infectionName}</td>
        </tr>
    `).join('');
}

async function openModal(id) {
    document.getElementById('modalName').textContent = 'Загрузка...';
    document.getElementById('modalGrid').innerHTML = '';
    document.getElementById('modalRecords').innerHTML = '';
    document.getElementById('modalOverlay').classList.add('open');

    try {
        const response = await fetch(`${API_URL}/vaccines/${id}`);
        if (!response.ok) throw new Error(`Ошибка ${response.status}`);
        const user = await response.json();
        renderModal(user);
    } catch (err) {
        document.getElementById('modalName').textContent = 'Ошибка загрузки';
    }
}

function updateStats(total, filtered) {
            document.getElementById('statTotal').textContent = total;
            document.getElementById('statFiltered').textContent = filtered;
        }

        document.getElementById('searchInput').addEventListener('input', function() {
            const query = this.value.toLowerCase().trim();
            const filtered = allVaccines.filter(v =>
                (v.name || '').toLowerCase().includes(query)
            );
            renderTable(filtered);
            updateStats(allVaccines.length, filtered.length);
        });

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

async function openAddModal() {
    document.getElementById('fieldName').value = '';
    document.getElementById('fieldSeries').value = '';
    document.getElementById('fieldExpiryDate').value = '';
    document.getElementById('fieldInfection').value = '';

    hideError();

    try {
        const response = await fetch(`${API_URL}/Infection`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });
        const infections = await response.json();

        const select = document.getElementById('fieldInfection');
        select.innerHTML = '<option value=""> Выберите инфекцию </option>' +
            infections.map(i => 
                `<option value="${i.id}">${i.infectionName}</option>`
            ).join('');
    } catch (err) {
        console.error('Ошибка загрузки инфекций: ', err);
    }

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

async function saveVaccine() {
    const name = document.getElementById('fieldName').value.trim();
    const series = document.getElementById('fieldSeries').value.trim();
    const expiryDate = document.getElementById('fieldExpiryDate').value;
    const infectionId = document.getElementById('fieldInfection').value.trim();

    if (!name) return showFormError('Введите название вакцины');
    if (!series) return showFormError('Введите серию');
    if (!expiryDate) return showFormError('Укажите срок годности');

    if (new Date(expiryDate) <= new Date())
        return showFormError('Неверный срок годности');

    if (!infectionId) return showFormError('Выберите инфекцию');

    const newVaccine = {
        name,
        series,
        expiryDate,
        infectionId: parseInt(infectionId)
    };

    try {
        const response = await fetch(`${API_URL}/Vaccine`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(newVaccine)
        });

        if (response.ok) {
            closeAddModal();
            await loadVaccines();
        } else {
            const text = await response.text();
            showFormError(text || `Ошибка ${response.status}`);
        }
    } catch(err) {
        showFormError('Не удалось подключиться к серверу');
    }
}

function showFormError(message) {
    const block = document.getElementById('addModalError');
    block.textContent = message;
    block.style.display = 'block';
}

function hideError() {
    const block = document.getElementById('addModalError');
    block.textContent = '';
    block.style.display = 'none';
}

loadVaccines();