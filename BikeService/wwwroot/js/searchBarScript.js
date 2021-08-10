/* Skrypt do wyszukiwarki klientów */

// elementy formularza
const searchBar = document.querySelector(".search-input");
const inputBox = searchBar.querySelector("input");
const suggBox = document.querySelector(".autocom-box");
const searchInput = document.getElementById('CustomerName');
const email = document.getElementById("Email");
const phone = document.getElementById("Phone");
const customerId = document.getElementById("CustomerId");
const cross = document.getElementById("Cross");
const search = document.getElementById("Search");
// rower nowego klieta
const bikeDiv = document.getElementById("BikeNew");
const bikeInput = document.getElementById("Bike");
// div i select z listą rowerów
const bikesList = document.getElementById("BikeSelectList");
const bikesListSelect = document.getElementById("Bikes");
// dodanie roweru, do istniejącego klienta
const newBikeName = document.getElementById("NewBike");
const newBikeDescript = document.getElementById("NewBikeDesc");
const saveBike = document.getElementById("SaveBike");

// pobranie Jsona z filtrem
const searchCustomer = async searchText => {

    var sPath = window.location.pathname;
    var path = "";
    if (sPath == "/Serviceman/Request/Create") {
        path = "../Request/GetCustomerNameAndPhone"; 
    }
    else {
        path = "../GetCustomerNameAndPhone";
    }
    const res = await fetch(path);
    //const res = await fetch("../Request/GetCustomerNameAndPhone");
    const customers = await res.json();

    // dopasowanie listy do aktualnie szukanego ciągu
    let matches = customers.filter(customer => {
        const regex = new RegExp(`${searchText}`, 'gi');
        return customer.nameAndPhone.match(regex);
    });

    if (matches.length > 0) {
        searchBar.classList.add("active");
    }
    else {
        suggBox.innerHTML = '';
        searchBar.classList.remove("active");
    }

    if (searchText.length === 0) {
        matches = [];
        suggBox.innerHTML = '';
        searchBar.classList.remove("active");
    }

    output(matches);
};

// uzupełnianie danych klienta, po wybraniu z listy
function select(element) {

    let selectUserData = element.textContent;
    getCustomerName();
    searchBar.classList.remove("active");

    function getCustomerName() {
        var sPath = window.location.pathname;
        if (sPath == "/Serviceman/Request/Create") {
            url = "../Request/GetCustomerName";
        }
        else {
            var url = "../GetCustomerName";
        }

        //var url = "../Request/GetCustomerName";
        $.getJSON(url, { nameAndPhone: selectUserData }, function (data) {

            // ustawiam dane, które znalazłem w bazie
            inputBox.value = data.customer.name;
            inputBox.setAttribute("readonly", true);
            email.value = data.customer.eMail;
            email.setAttribute("readonly", true);
            phone.value = data.customer.phone;
            phone.setAttribute("readonly", true);
            customerId.value = data.customer.id;
            cross.removeAttribute("hidden");
            search.setAttribute("hidden", true);
            let bikes = data.bikesList;

            if (bikes.length >= 1) {
                bikeInput.value = '';
                bikeDiv.setAttribute("hidden", true);
                bikesList.removeAttribute("hidden");
                var options = "";
                for (let i = 0; i < bikes.length; i++) {
                    options += "<option value=" + bikes[i].id + ">" + bikes[i].name + "</option>";
                }
                document.getElementById("Bikes").innerHTML = options;
            }
            else {
                bikeDiv.removeAttribute("hidden");
                bikesList.setAttribute("hidden", true);
                if ($('#Bikes option').length != 0) {
                    var length = bikesList.options.length;
                    if (length > 0) {
                        for (i = length - 1; i >= 0; i--) {
                            bikesList.options[i] = null;
                        }
                    }
                }
            }
        })
    }
}

// wyświetalnie listy klientów
const output = matches => {

    if (matches.length > 0) {
        const html = matches.map(match => `
            <li>${match.name} <span class="text-primary">${match.phone}</span> </li>
        `).join('');

        suggBox.innerHTML = html;
    }
    let allList = suggBox.querySelectorAll("li");
    for (let i = 0; i < allList.length; i++) {
        allList[i].setAttribute("onclick", "select(this)");
    }
}

var ignoreClickOnMeElement = document.getElementById('autocombo');

document.addEventListener('click', function (event) {

    var isClickInsideElement = ignoreClickOnMeElement.contains(event.target);
    if (!isClickInsideElement) {
        searchBar.classList.remove("active");
    }
});

searchInput.addEventListener('input', () => searchCustomer(searchInput.value));

// usuwanie danych klienta
function clearData() {

    inputBox.value = '';
    inputBox.removeAttribute("readonly");
    email.value = '';
    email.removeAttribute("readonly");
    phone.value = '';
    phone.removeAttribute("readonly");
    customerId.value = '';
    cross.setAttribute("hidden", true);
    search.removeAttribute("hidden");

    bikeDiv.removeAttribute("hidden");
    bikesList.setAttribute("hidden", true);
    if ($('#Bikes option').length != 0) {
        var length = bikesListSelect.options.length;
        if (length > 0) {
            for (i = length - 1; i >= 0; i--) {
                bikesListSelect.options[i] = null;
            }
        }
    }
}

// dodawanie nowego roweru
saveBike.addEventListener("click", function () {

    var sPath = window.location.pathname;
    if (sPath == "/Serviceman/Request/Create") {
        url = "../Request/AddNewBicycle";
    }
    else {
        var url = "../AddNewBicycle";
    }

    $.getJSON(url, { customer: customerId.value, bicycle: newBikeName.value, comment: newBikeDescript.value }, function (data) {
        let bikes = data;
        if (bikes.length >= 1) {
            var length = bikesListSelect.options.length;
            if (length > 0) {
                for (i = length - 1; i >= 0; i--) {
                    bikesListSelect.options[i] = null;
                }
            }
            var options = "";
            for (let i = 0; i < bikes.length; i++) {
                options += "<option value=" + bikes[i].id + ">" + bikes[i].name + "</option>";
            }
            document.getElementById("Bikes").innerHTML = options;
        }
    })
});
