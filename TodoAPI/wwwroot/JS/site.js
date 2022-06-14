﻿const uri = 'api/todoitems';
let todos = [];

function getItem() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get item.', error));
}

function addItem() {
    const addNameTextbox = document.getElementById("add-name");

    const item = {
        isComplete = false,
        name: addNameTextbox.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isComplete: document.getElementById('edit-isComplete').checked,
        name: document.getElementById('edit-name').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = todos.find(Item => Item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isComplete').checked = item.isComplete;
    document.getElementById('editForm').style.display = 'block';
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function displayCount(itemcount) {
    const name = (itemcount == 1) ? 'to-do' : 'to-dos';

    document.getElementById('counter').innerHTML = `${itemcount},${name}`;
}

function _displayItem(data) {
    const tbody = document.getElementById('todos');
    tbody.innerHTML = '';

    _displayCount(data.length);

    const button = document.getElementById('button');

    data.foreach(item => {
        let isCompleteCheckbox = document.createElement('input');
        isCompleteCheckbox.type = 'checkbox';
        isCompleteCheckbox.disabled = true;
        isCompleteCheckbox.checked = item.isComplete;

        let editbutton = button.cloneNode(false);
        editbutton.innerText = 'Edit';
        editbutton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deletebutton = button.cloneNode(false);
        editbutton.innerText = 'Delete';
        editbutton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tbody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isCompleteCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editbutton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deletebutton);
    });

    todos = data;
}