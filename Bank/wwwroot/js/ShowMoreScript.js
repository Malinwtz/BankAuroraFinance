

pageNum = 1;
CustomerId = @Model.SingleCustomer.CustomerId;

document.addEventListener("DOMContentLoaded", function () {
    showMore();
});

function showMore() {
    fetch(`/Customers/Customer?handler=ShowMore&customerId=${CustomerId}&pageNum=${pageNum}`)
        .then((response) => response.json())
        .then((json) => {
            console.log('JSON data:', json);
            pageNum = pageNum + 1;
            json.transactions.forEach(drawElements);
        })
        .catch((error) => console.log(error));
}

function drawElements(element) {
    console.log('Element:', element);
    document.querySelector('#posts-table tbody').innerHTML +=
        `<tr>
                            <td>${element.amount}</td>
                            <td>${element.type}</td>
                            <td>${element.balance}</td>
                            <td>${element.date.substring(0, 10)}</td>
                            <td>
                              
                            </td>
                        </tr>`;
}