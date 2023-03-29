let uploadButtonImg = document.getElementById("upload-button");
let chosenImage = document.getElementById("chosen-image");
let fileName = document.getElementById("file-name");
let container = document.querySelector(".drag-container");
let error = document.getElementById("error");
let imageDisplay = document.getElementById("image-display");

const fileHandler = (file, name, type) => {
    if (type.split("/")[0] !== "image") {
        //File Type Error
        error.innerText = "Please upload an image file";
        return false;
    }
    error.innerText = "";
    let reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onloadend = () => {
        //image and file name
        let imageContainer = document.createElement("figure");

        let img = document.createElement("img");
        img.src = reader.result;
        imageContainer.appendChild(img);
        imageContainer.innerHTML += `<figcaption>${name}</figcaption>`;
        imageDisplay.appendChild(imageContainer);
    };
};

//Upload Button
uploadButtonImg.addEventListener("change", () => {
    imageDisplay.innerHTML = "";
    Array.from(uploadButtonImg.files).forEach((file) => {
        fileHandler(file, file.name, file.type);
    });
});

container.addEventListener(
    "dragenter",
    (e) => {
        e.preventDefault();
        e.stopPropagation();
        container.classList.add("active");
    },
    false
);

container.addEventListener(
    "dragleave",
    (e) => {
        e.preventDefault();
        e.stopPropagation();
        container.classList.remove("active");
    },
    false
);

container.addEventListener(
    "dragover",
    (e) => {
        e.preventDefault();
        e.stopPropagation();
        container.classList.add("active");
    },
    false
);

container.addEventListener(
    "drop",
    (e) => {
        e.preventDefault();
        e.stopPropagation();
        container.classList.remove("active");
        let draggedData = e.dataTransfer;
        let files = draggedData.files;
        imageDisplay.innerHTML = "";
        Array.from(files).forEach((file) => {
            fileHandler(file, file.name, file.type);
        });
    },
    false
);

window.onload = () => {
    error.innerText = "";
};

// add character and videos 
let app = new Vue({
    el: '#app',
    data: {
        msg: 'My msg',
        addSub: '',
        subjects: [{
            id: 1,
            sub: 'Learn JavaScript',
            checked: false
        },
        {
            id: 2,
            sub: 'Learn Vue',
            checked: false
        },
        {
            id: 3,
            sub: 'Create a App',
            checked: false
        },
        ],
    },
    methods: {
        pushSub: function () {
            if (this.addSub) {
                this.subjects.push({
                    sub: this.addSub
                });
                this.addSub = ''
            }
        },
        clearList: function () {
            this.subjects = []
        },
        check: function (subject) {
            subject.checked = true;
        },
        // get all todos when loading the page
        getTodos() {
            if (localStorage.getItem('todo_list')) {
                this.subjects = JSON.parse(localStorage.getItem('todo_list'));
            }
        },
    },
    // for loacat storage
    mounted() {
        this.getTodos();
    },
    // for loacat storage
    watch: {
        subjects: {
            handler: function (updatedList) {
                localStorage.setItem('todo_list', JSON.stringify(updatedList));
            },
            deep: true
        }
    },
    computed: {}
})