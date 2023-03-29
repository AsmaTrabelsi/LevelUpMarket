var dataTable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Game/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "intro", "width": "15%" },
            { "data": "story", "width": "15%" },
            { "data": "releaseDate", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "offlinePlayEnable", "width": "15%" },
            { "data": "available", "width": "15%" },
    

        ]
    });
}


