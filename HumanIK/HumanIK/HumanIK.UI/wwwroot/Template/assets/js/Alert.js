function refuseAdvance(id) {
    let confirmAction = confirm("Avans Talebini Reddetmek İstediğinizden Emin Misiniz?");
    if (confirmAction) {
        fetch(`/Manager/Advance/Refuse?id=${id}`).then((res) => {
            location.reload()
        })
    }
}

function confirmAdvance(id) {
    let confirmAction = confirm("Avans Talebini Onaylamak İstediğinizden Emin Misiniz?");
    if (confirmAction) {
        fetch(`/Manager/Advance/Confirm?id=${id}`).then(function (res) {
            location.reload()
        })
    }
}

function cancelAdvance(id) {
    let answer = confirm("Avans talebinizi iptal etmek istediğinizden emin misiniz?");
    if (answer) {
        fetch(`/Employee/Advance/Cancel?id=${id}`).then((res) => {
            location.reload()
        })
    }
}

function refuseExpense(id) {
    let confirmAction = confirm("Harcama Talebini Reddetmek İstediğinizden Emin Misiniz?");
    if (confirmAction) {
        fetch(`/Manager/Expense/Refuse?id=${id}`).then((res) => {
            location.reload()
        })
    }
}

function confirmExpense(id) {
    let confirmAction = confirm("Harcama Talebini Onaylamak İstediğinizden Emin Misiniz?");
    if (confirmAction) {
        fetch(`/Manager/Expense/Confirm?id=${id}`).then(function (res) {
            location.reload()
        })
    }
}

function cancelExpense(id) {
    let answer = confirm("Harcama talebinizi iptal etmek istediğinizden emin misiniz?");
    if (answer) {
        fetch(`/Employee/Expense/Cancel?id=${id}`).then((res) => {
            location.reload()
        })
    }
}

function refusePermission(id) {
    let confirmAction = confirm("İzin Talebini Reddetmek İstediğinizden Emin Misiniz?");
    if (confirmAction) {
        fetch(`/Manager/Permission/Refuse?id=${id}`).then((res) => {
            location.reload()
        })
    }
}

function confirmPermission(id) {
    let confirmAction = confirm("İzin Talebini Onaylamak İstediğinizden Emin Misiniz?");
    if (confirmAction) {
        fetch(`/Manager/Permission/Confirm?id=${id}`).then(function (res) {
            location.reload()
        })
    }
}

function cancelPermission(id) {
    let answer = confirm("İzin talebinizi iptal etmek istediğinizden emin misiniz?");
    if (answer) {
        fetch(`/Employee/Permission/Cancel?id=${id}`).then((res) => {
            location.reload()
        })
    }
}

function deleteCompany(id) {
    let answer = confirm(`Şirketi silmek istediğinizden emin misiniz?`);
    if (answer) {
        fetch(`/Admin/Company/Delete?id=${id}`).then((res) => {
            location.reload()
        })
    }
}