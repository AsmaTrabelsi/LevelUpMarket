/*
window.addEventListener("load", () => {
    const form = document.getElementById("add-task-form");
    const addtask = document.getElementById("add-task-input");
    const addtype = document.getElementById("add-type-select");
    const todotask_el = document.getElementById("todo-tasks");

    form.addEventListener('submit', (e) => {
        e.preventDefault();
        const taskvalue = addtask.value;
        const typevalue = addtype.value;
        if (!taskvalue || !typevalue) {
            alert("please fill out all the info");
        } else {
            todotask_el.classList.add("add-task-wrap");
            const list_input_box_el = document.createElement("div");
            list_input_box_el.classList.add("flex-container");
            const input_filed_element = document.createElement("input");
            input_filed_element.type = "text";
            input_filed_element.classList.add("input-filed");
            input_filed_element.setAttribute("readonly", "readonly");
            input_filed_element.value = taskvalue;

            const select_element = document.createElement("input");
            select_element.classList.add("input-filed");
            select_element.setAttribute("readonly", "readonly");
            select_element.value = typevalue;

            const delete_el = document.createElement("button");
            delete_el.classList.add("btn");
            delete_el.innerHTML = `<i class="bi bi-trash-fill fs-5" ></i>`;


            list_input_box_el.appendChild(input_filed_element);
            list_input_box_el.appendChild(select_element);
            list_input_box_el.appendChild(delete_el);
            todotask_el.appendChild(list_input_box_el);

            addtask.value = "";


            delete_el.addEventListener("click", () => {
                todotask_el.removeChild(list_input_box_el);
            })
        }
    })

})
*/
// test add character
$(document).ready(function () {

    loadVideoList();
});


function loadVideoList() {
    const gameId = document.querySelector("#gameId").value;

    $.ajax({
        "url": "/Admin/Video/GetVideoByGame?GameId=" + gameId,
        type: "Get",
        dataType: "json",
        success: function (data) {
            console.log(data);
            var ul = $("#todo-tasks").empty();
            var videos = Object.values(data)[0];
            console.log(videos);
            videos.forEach(function (item) {
                console.log(item.type);
                var listItem = $("<div>").addClass("flex-container");

                var videoTypeInput = $("<input>").attr("type", "text").addClass("input-filed").attr("readonly", true).val(getVideoType(item.type)).css("width", "20%");
                var videoUrlInput = $("<input>").attr("type", "text").addClass("input-filed").attr("readonly", true).val(item.url).css("width", "60%");
                var deleteButton = $("<button>").addClass("btn").css({ marginLeft: "auto" }).html(`<i class="bi bi-trash-fill fs-5" ></i>`).on("click", function () {
                    $.ajax({
                        url: "/Admin/Video/DeleteVideoPost/" + item.id,
                        type: "DELETE",
                        dataType: "json",
                        success: function (data) {
                            toastr.success(data.message);
                            loadVideoList();
                            
                        },
                        error: function () {
                            alert("Error deleting data!");
                        }
                    });
                });
                listItem.append(videoUrlInput).append(videoTypeInput).append(deleteButton);
                    $("#todo-tasks").append(listItem);
            })

        },
        error: function () {
            alert("Error retrieving data!");
        },

    });

}



function addVideo() {
    const id = document.querySelector("#gameId").value;

    const type = document.querySelector("#video-type-select").value;
    const url = document.querySelector("#add-url-input").value;
    $.ajax({
        "url": "/Admin/Video/AddVideo",
        type: "Post",
        data: {
            gameId: id,
            videoType: type,
            url: url
        },
        dataType: "json",
        success: function (data) {
            toastr.success(data.message);
        },
        error: function () {
            toastr.error("Error while adding data");

        }

    })
}

function getVideoType(index) {
    switch (index) {
        case 0: return "trailer"; break;
        case 1: return "behind_scence"; break;
        default : return "best_scence"
    }
}