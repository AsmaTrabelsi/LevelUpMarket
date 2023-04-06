
$(document).ready(function () {

    loadCharacterList();
});


function loadCharacterList() {
    const gameId = document.querySelector("#idGame").value;


    $.ajax({
        "url": "/Admin/Character/GetCharacterByGame?GameId=" + gameId,
        type: "Get",
        dataType: "json",
        success: function (data) {
            console.log(data);
            var ul = $("#characters").empty();
            var characters = Object.values(data)[0];
            console.log("chacatetrs");
            console.log(characters);
            characters.forEach(function (item) {
                console.log(item.imageUrl);
                var listItem = $("<div>").addClass("flex-container row");
                var characterImg = $("<img>").attr({ alt: item.characterName, width: "100", height: "160", src: "/" + item.imageUrl }).css({ "width": "10%", "align-self": "center" }).addClass("p-3");
                var characterNameInput = $("<label>").text(item.characterName).css("width", "10%");
                var characterTypeInput = $("<label>").text(getCharacterType(item.characterType)).css("width", "7%");
                var characterDesInput = $("<label>").text(item.description).css("width", "50%").addClass("p-3");
                var characterMainCheckbox = $("<i>").attr("style", item.MainCharacter ? "color:green" : "color:red").addClass(item.mainCharacter ? "bi bi-check-lg fs-4" : "bi bi-x fs-4").attr("readonly", true).val(item.mainCharacter).css("width", "3%");
                var characterMainLabel = $("<label>").text("Main Character").css("width", "12%");
                var buttons = $("<div>").addClass("row").css("width", "7%");

                var deleteButton = $("<button>").addClass("btn").css({"marginLeft": "auto"}).html(`<i class="bi bi-trash-fill fs-5" ></i>`).on("click", function () {
                    $.ajax({
                        url: "/Admin/Character/DeleteCharacterPost/" + item.id,
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
                    }).css("width", "3%");
                var editButton = $("<button>").addClass("btn").css({ "marginLeft": "auto" }).html(`<i class="bi bi-trash-fill fs-5" ></i>`).on("click", function () {
                    $.ajax({
                        url: "/Admin/Character/DeleteCharacterPost/" + item.id,
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
                }).css("width", "3%");
                listItem.css({
                    "align-items": "center"
                });
                buttons.css({
                    "display": "flex",
                    "align-items": "center"
                });
                buttons.append(deleteButton).append(editButton);
                listItem.append(characterImg).append(characterNameInput).append(characterTypeInput).append(characterDesInput).append(characterMainCheckbox).append(characterMainLabel).append(buttons);
                $("#characters").append(listItem);
            })

        },
        error: function () {
            alert("Error retrieving data!");
        },

    });

}





function getCharacterType(index) {
    switch (index) {
        case 0: return "Creature"; break;
        case 1: return "Antagonist"; break;
        default: return "Hero";
    }
}
