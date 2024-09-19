const API_BASE_URL = 'https://localhost:7065/api/UserData';
const API_DATA_ENDPOINT = `${API_BASE_URL}/data`;
const API_UPLOAD_ENDPOINT = `${API_BASE_URL}/upload`;
const API_UPDATE_ENDPOINT = `${API_BASE_URL}/update`; 
const API_DELETE_ENDPOINT = `${API_BASE_URL}`; 

function editUser(button){
    const row = button.closest('tr');

    const id = row.cells[0].querySelector('input').value;
    const name = row.cells[1].querySelector('input').value;
    const dateOfBirth = row.cells[2].querySelector('input').value;
    let parts = dateOfBirth.split('.');
    let formattedDate = `${ parts[2]}-${ parts[1] }-${ parts[0] }`; 
    const married = row.cells[3].querySelector('input').value == 'Yes' ? true : false;
    
    const phone = row.cells[4].querySelector('input').value;
    const salary = row.cells[5].querySelector('input').value;

    const data = {
        id: id,
        name: name,
        dateOfBirth: formattedDate,
        married: married,
        phone: phone,
        salary: salary
    };

    fetch(`${API_UPDATE_ENDPOINT}/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            console.log('Response status:', response.status);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(text => {
            console.log('Response text:', text);
            return JSON.parse(text); 
        })
        .then(data => {
            console.log('Success:', data);
        })
        .catch(error => {
            console.error('Error:', error);
        });

} 

async function fetchData() {
    const filter = collectFilterData();
    console.log(JSON.stringify(filter));
    try {
        const response = await fetch(API_DATA_ENDPOINT, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(filter),
        });

        if (!response.ok) throw new Error('Failed to fetch data');

        const result = await response.json();
        handleFetchResponse(result);
    } catch (error) {
        console.error('Error fetching data:', error);
    }
}

function handleFetchResponse(result) {
    if (result.success) {
        populateTable(result.data);
    } else {
        console.error('Failed to fetch data:', result.message);
    }
}

function populateTable(data) {
    const tableBody = document.querySelector('#dataTable tbody');
    tableBody.innerHTML = '';

    data.forEach(user => {
        const row = document.createElement('tr');
        row.innerHTML = createTableRowHTML(user);
        tableBody.appendChild(row);
    });
}

function createTableRowHTML(user) {
    return `
        <td><input value='${user.id}'/></td>
        <td><input value='${user.name}'/></td>
        <td><input value='${formatDate(user.dateOfBirth)}'/></td>
        <td><input value='${user.married ? 'Yes' : 'No'}'/></td>
        <td><input value='${user.phone}'/></td>
        <td><input value='${user.salary.toFixed(2)}'/></td>
        <td>
            <button onclick="editUser(this)">Edit</button>
            <button onclick="removeUser(${user.id})">Remove</button>
        </td>
    `;
}

function formatDate(dateString) {
    return new Date(dateString).toLocaleDateString();
}

async function uploadCsv() {
    const fileInput = document.querySelector('#fileInput');
    const file = fileInput.files[0];

    if (!file) {
        alert('Please select a CSV file.');
        return;
    }

    const formData = new FormData();
    formData.append('file', file);

    try {
        const response = await fetch(API_UPLOAD_ENDPOINT, {
            method: 'POST',
            body: formData,
        });

        if (!response.ok) throw new Error('Failed to upload CSV file');

        const result = await response.json();
        handleUploadResponse(result);
    } catch (error) {
        console.error('Error uploading file:', error);
        alert('Error uploading file: ' + error.message);
    }
}

function handleUploadResponse(result) {
    if (result.success) {
        clearTable();
        fetchData();
    } else {
        alert(result.message);
    }
}

function sortTable(columnIndex) {
    const table = document.querySelector('#dataTable');
    const rows = Array.from(table.querySelectorAll('tbody tr'));
    const header = table.querySelectorAll('thead th')[columnIndex];
    const isAsc = header.classList.toggle('asc');

    rows.sort((a, b) => compareRows(a, b, columnIndex, isAsc));
    rows.forEach(row => table.querySelector('tbody').appendChild(row));
}

function compareRows(a, b, columnIndex, isAsc) {
    const aText = a.children[columnIndex].innerText.trim();
    const bText = b.children[columnIndex].innerText.trim();
    return isAsc
        ? aText.localeCompare(bText, undefined, { numeric: true })
        : bText.localeCompare(aText, undefined, { numeric: true });
}

async function removeUser(id) {
    if (!confirm(`Are you sure you want to remove user with ID ${id}?`)) return;

    try {
        const response = await fetch(`${API_DELETE_ENDPOINT}/${id}`, {
            method: 'DELETE',
        });

        if (!response.ok) throw new Error('Failed to remove user');

        alert('User removed successfully');
        fetchData();  
    } catch (error) {
        console.error('Error removing user:', error);
        alert('Error removing user: ' + error.message);
    }
}

function collectFilterData() {
    const name = document.querySelector('#filterName').value;
    const phone = document.querySelector('#filterPhone').value;
    const salary = document.querySelector('#filterSalary').value;
    const dateOfBirth = document.querySelector('#filterDateOfBirth').value;
    const married = document.querySelector('#filterMarried').checked;
    const orderBy = document.querySelector('#filterOrderBy').value || 'CreatedDate';
    const ascending = document.querySelector('#filterAscending').checked ? 'Ascending' : 'Descending';

    const filter = {};

    if (name) filter.name = name;
    if (phone) filter.phone = phone;
    if (salary) filter.salary = salary;
    if (dateOfBirth) filter.dateOfBirth = dateOfBirth;
    if (married) filter.married = married;

    filter.orderBy = orderBy;
    filter.ascending = ascending;

    return filter;
}

function filterTable() {
    const input = document.querySelector('#searchInput').value.toLowerCase();
    const rows = document.querySelectorAll('#dataTable tbody tr');
    rows.forEach(row => {
        const isMatch = Array.from(row.querySelectorAll('td')).some(cell =>
            cell.innerText.toLowerCase().includes(input)
        );
        row.style.display = isMatch ? '' : 'none';
    });
}
function clearTable() {
    const tableBody = document.querySelector('#dataTable tbody');
    tableBody.innerHTML = '';
}
