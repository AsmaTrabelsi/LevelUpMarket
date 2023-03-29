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
            {
                "data": "images",
                "width": "10%",
                "render": function (imgs) {
                    var src = imgs.filter(item => item.imageType === 4);
                    return `<img src="/${src[0].imageUrl}" width="120" height="200" alt="test"/>`;
                }
            },
            { "data": "name", "width": "10%" },
            {
                "data": "intro", "width": "15%",
                "render": function (data) {
                    return data.substring(0, 150) + "..."
                }            },
            {
                "data": "story", "width": "15%",
                "render": function (data) {
                    return data.substring(0,150)+"..."
                }
            },
            {"data": "releaseDate",
                "width": "10%",
                "render": function (date) {

                    return date.substring(0, 10); 
                }
                },
            { "data": "price", "width": "10%" },
            {
                "data": "offlinePlayEnable", "width": "7%",
                "render": function (data) {
                    if (data == true) {
                        return `<i class="bi bi-check-lg fs-4" style="color:green"></i>`
                    } else {
                        return `<i class="bi bi-x fs-4" style="color:red"></i>`
                    }
                }
                },
            {
                "data": "available", "width": "7%",
                "render": function (data) {
                    if (data == true) {
                        return `<i class="bi bi-check-lg fs-4" style="color:green"></i>`
                    } else {
                        return `<i class="bi bi-x fs-4" style="color:red"></i>`
                    }
                }            },
            { "data": "developer.name", "width": "12%" },
            {
                "data": "id",
                "width": "10%",
                "render": function (data) {
                    return `
                             <div class="btn-group" role="group">
                        <a href="/Admin/Game/Upsert?id=${data}"class="btn" >
                            <i class="bi bi-pencil-square fs-4"></i> 
                        </a>
                        <a class="btn" >
                            <i class="bi bi-trash-fill fs-4" ></i> 
                        </a>
                   </div>
                    `
                }
            }


    

        ]
    });
}


