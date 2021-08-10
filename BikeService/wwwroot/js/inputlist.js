//const search = document.getElementById('CustomerId');
//const matchList = document.getElementById('match-list');

//const searchCustomers = async searchText => {
//    const res = await fetch("../Request/GetCustomerNameAndPhone");
//    const customers = await res.json();

//    //Gest matches to current text input
//    let matches = customers.filter(customer => {
//        const regex = new RegExp(`${searchText}`, 'gi');
//        return customer.name.match(regex) || customer.phone.match(regex);
//    });

//    if (searchText.length === 0) {
//        matches = [];
//        matchList.innerHTML = '';
//    }

//    //console.log(matches);

//    outputHtml(matches);
//};

//// Show results in HTML

//const outputHtml = matches => {
//    if (matches.length > 0) {
//        const html = matches.map(match => `
//            <div class="card card-body">
//                ${match.name} - <span class="text-primary">${match.phone}</span>
//            </div>
//        `).join('');

//        matchList.innerHTML = html;
//    }
//}

//search.addEventListener('input', () => searchCustomers(search.value));