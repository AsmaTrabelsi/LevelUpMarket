var dataTable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/dmin/Game/GetAll"
        },
        "Columns": [
            { "data": "Name", "width": "15%" },
            { "data": "Intro", "width": "15%" },
            { "data": "Story", "width": "15%" },
            { "data": "RelaseDate", "width": "15%" },
            { "data": "Price", "width": "15%" },
            { "data": "OfflinePlay", "width": "15%" },
            { "data": "Available", "width": "15%" },
            { "data": "VoiceLanguages", "width": "15%" },
            { "data": "Subtitle", "width": "15%" }

        ]
    });
}


