////////////////*Skrypty działajace w oknie z zestawieniem zleceń serwisowych*////////////////

const assignee = document.getElementById("AssigneeList");
const requestValue = document.getElementById("RequestValue");
const descrpit = document.getElementById("Descript");
const displayPhotosModal = document.getElementById("displayPhotosModal");
const selectedRequest = document.getElementById("SelectedRequest");
const statusOfRequest = document.getElementById("StatusOfRequest");
const modalButtonsSection = document.getElementById("ModalButtonsSection");
const changeStatusLabel = document.getElementById("ChangeStatusLabel");

/*Funkcja działająca pod przyciskiem zmiany statusu na danym rekordzie ze zleceniem - podaje dane zlecenia do okna modalnego*/
function changeStatusButton(obj, requestStatus, direction) {

    var url = "../Serviceman/Request/ChangeStatus";

    //alert(requestStatus);

    $.getJSON(url, { requestIdForm: obj.id }, function (data) {
        let request = data;
        let selectedAssignee = data.request.employeeAssignee.id;
        //console.log(request);
        $(assignee).empty();
        $(displayPhotosModal).empty();
        $(selectedRequest).empty();
        requestValue.value = "";
        statusOfRequest.value = "";
        tinymce.get('Descript').setContent('');

        if (request.assignee.length >= 1) {
            var options = "";
            for (let i = 0; i < request.assignee.length; i++) {
                if (selectedAssignee == request.assignee[i].id) {
                    options += '<option selected = "selected" value=' + request.assignee[i].id + ">" + request.assignee[i].name + " " + request.assignee[i].surname + "</option>";
                }
                else {
                    options += "<option value=" + request.assignee[i].id + ">" + request.assignee[i].name + " " + request.assignee[i].surname + "</option>";
                }
            }
            assignee.innerHTML = options;
        }

        if (request.images.length >= 1) {
            var images = "";
            for (let im = 0; im < request.images.length; im++) {
                if (im == 0) {
                    images += '<a class="btn btn-light text-dark form-control" id="displayPhotosButton" data-dismiss="modal" aria-label="Close" href="' + request.images[im].path + '">';
                    images += '<span class="iconify" data-icon="bx:bx-image" data-inline="false" data-width="26" data-height="26"></span>';
                    images += 'Wyświetl</a>';
                }
                else {
                    images += '<a href="' + request.images[im].path + '"> </a>';  
                }
            }
            displayPhotosModal.innerHTML = images;
        }

        requestValue.value = request.request.grossValue;
        statusOfRequest.value = request.request.statusId;
        selectedRequest.innerHTML = '<input class="form-control" id="SelectedRequestId" value="' + request.request.id + '" hidden/>'
        if (request.request.description != null) {
            tinymce.get('Descript').setContent(request.request.description);
        }
    })

    var buttonName = "";
    var arrowDirection = "";

    if (requestStatus == "1") {
        buttonName = "Do realizacji";
        arrowDirection = "right";
    }
    else if (requestStatus == "2") {
        if (direction == "B") {
            buttonName = "Wycofaj z realizacji";
            arrowDirection = "left";
        }
        else {
            buttonName = "Zakończ realizację";
            arrowDirection = "right";
        }
    }
    else if (requestStatus == "3") {
        if (direction == "B") {
            buttonName = "Cofnij do realizacji";
            arrowDirection = "left";
        }
        else {
            buttonName = "Potwierdź odbiór";
            arrowDirection = "right";
        }
    }
    else if (requestStatus == "4") {
        buttonName = "Cofnij do zrobionych";
        arrowDirection = "left";
    }
    direction = "'" + direction + "'";


    var modalButtonSectionContent = '<button type="button" class="btn btn-light text-dark" data-dismiss="modal">';
    modalButtonSectionContent += '<i class="fas fa-times" ></i>';
    modalButtonSectionContent += '&nbsp;&nbsp;Zamknij</button >';
    modalButtonSectionContent += '<button type="button" class="btn btn-success" name="ConfirmNewStatus" id="ConfirmNewStatus" data-dismiss="modal" onclick="changeStatusConfirm(' + direction + ')">';
    modalButtonSectionContent += '<span class="iconify" data-icon="el:arrow-' + arrowDirection + '" data-inline="false"></span>&nbsp;&nbsp;' + buttonName + '</button>';
    modalButtonsSection.innerHTML = modalButtonSectionContent;

    changeStatusLabel.innerHTML = '<span class="iconify" data-icon="el:arrow-' + arrowDirection + '" data-inline="false"></span>&nbsp;&nbsp;' + buttonName;


}   

function changeStatusConfirm(mode) {
    var assignee = document.getElementById("AssigneeList");
    var value = document.getElementById("RequestValue");
    var descript = tinymce.get("Descript").getContent();
    var selectedRequestId = document.getElementById("SelectedRequestId");
    var newSatus = 0;
    if (mode == "F") {
        newSatus = Number(statusOfRequest.value) + 1;
    }
    else if (mode == "B") {
        newSatus = Number(statusOfRequest.value) - 1; 
    }

    $.ajax({
        url: "../Serviceman/Request/SaveLastUpdatedRequest",
        data: {
            requestIdForm: selectedRequestId.value
        }
    });

    $.ajax({
        url: "../Serviceman/Request/ChangeStatusConfirm",
        data: {
            requestIdForm: selectedRequestId.value,
            assigneIdForm: assignee.value,
            valueForm: value.value,
            descriptForm: descript,
            statusOfRequestForm: statusOfRequest.value,
            newStatusForm: newSatus
        },
        success: function (data) {
            window.location.href = "../Serviceman/Request?status=" + newSatus;
        }

    });
}