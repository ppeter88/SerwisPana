

$(document).ready(function () {
    getCustomerPhoneNumber();
    $('#CustomerId').change(function() {
        getCustomerPhoneNumber();
    });
});

function getCustomerPhoneNumber() {
    var url = '@Url.Content("~/")' + "Serviceman/Request/GetCustomerPhoneNumber";
    var ddlsource = '#CustomerId';
    $.getJSON(url, {id: $(ddlsource).val() }, function (data) {
        var item = '';
        $('#Phone').empty();
        item = data.phone;
        document.getElementById("Phone").value= item;
    })
}
