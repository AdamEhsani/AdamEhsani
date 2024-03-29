﻿var resultExtraItem = [];
$(document).ready(function () {
    $("#divLoading").hide(0);
    $(window).resize(function () {
        if ($(window).width() < 768) {
            $('.mainCard').removeClass('d-flex');
            $('.colums').removeClass('col-8');
            $('#tableBill').removeClass('col-4');
            $('.mainCard').addClass('d-block');
            $('.colums').addClass('col-12');
            $('#tableBill').removeClass('col-12');
        } else {
            $('.mainCard').removeClass('d-block');
            $('.colums').removeClass('col-12');
            $('#tableBill').removeClass('col-12');
            $('.mainCard').addClass('d-flex');
            $('.colums').addClass('col-8');
            $('#tableBill').addClass('col-4');
        }
    });
    $(window).trigger('resize');
});


function getLocationInfo() {
    var searchInput = document.getElementById("AddressStr").value;

    var baseUrl = "https://nominatim.openstreetmap.org/search";
    var query = "?q=" + searchInput + "&format=json";

    var xhr = new XMLHttpRequest();
    xhr.open("GET", baseUrl + query, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            var response = JSON.parse(xhr.responseText);
            displayResults(response);
        }
    };
    xhr.send();
}


function displayResults(result) {
    var availableAddresses = result.map(function (item) {
        if (item.display_name.split(',')[0] != null && item.display_name.split(',')[1] != null) {
            return item.display_name.split(',')[0].trim() + " ," + item.display_name.split(',')[1].trim();
        }
        return item.display_name.split(',')[0].trim();
    });

    $('#AddressStr').autocomplete({
        source: availableAddresses
    });
}

function SaveOrder() {
    var TotalPrice = document.querySelector('#SumePrice').value;
    var name = document.querySelector('#bestandKundeInputName');
    var phone = document.querySelector('#SearchPhone');
    var address = document.querySelector('#AddressStr').value;

    if (name.value.trim() != "" && phone.value.trim() != "") {
        name.classList.remove("is-valid");
        phone.classList.remove("is-valid");

        $.ajax({
            url: '/api/SaveOrders/SaveOrder',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: 'POST',
            data: JSON.stringify({ Name: name.value, Phone: phone.value, Address: address, TotalPrice: TotalPrice }),
            success: function (result) {
                var OrderId = result;
                saveBillDetails(OrderId);
            }
        });
    } else {
        name.classList.add("is-invalid");
        phone.classList.add("is-invalid");
    }
}

function BillPrint() {
    document.querySelector('#StoreInfo').classList.remove('d-none');
    document.querySelector('#CustomerInfo').classList.remove('d-none');
    var name = document.querySelector("#bestandKundeInputName").value;
    var phone = document.querySelector("#SearchPhone").value;
    var address = document.querySelector("#AddressStr").value;

    document.querySelector('#CustomerNameInfo').innerText = "Name: " + name;
    document.querySelector('#CustomerPhoneInfo').innerText = "Phone: " + phone;
    document.querySelector('#CustomerAddressInfo').innerText = "Address: " + address;

    var Sum = document.querySelector('#PrintSumePrice');
    var SumeTable = document.querySelector('#SumePrice').value;
    Sum.classList.remove('d-none');
    Sum.innerText = "Total price: " + SumeTable;

    var StoreDeliveryPrice = document.querySelector('#StoreDeliveryPrice').value;
    var deliveryPrice = document.querySelector('#DeliveryPrice');
    deliveryPrice.classList.remove('d-none');
    deliveryPrice.innerText = "Delivery price: " + StoreDeliveryPrice;

    var printHideElements = document.querySelectorAll('.PrintHide');
    printHideElements.forEach(function (element) {
        element.classList.add('d-none');
    });

    var table = document.getElementById('tableBill');
    var printContents = table.innerHTML;
    document.body.innerHTML = printContents;
    window.print();

    setTimeout(function () {
        window.location.href = "/Clients/Home";
    }, 1000);

}

function saveBillDetails(OrderId) {
    var table = document.getElementById("tableBill");
    var rows = table.querySelectorAll("tbody tr");
    var Id = parseInt(OrderId);
    rows.forEach(function (row) {
        var cells = row.getElementsByTagName("td");

        var innerText = cells[1].innerText;
        var lines = innerText.split('\n');
        var productName = lines[0].trim();

        var ExtraItems = cells[1].firstElementChild.textContent;

        var priceArticle = cells[2].textContent;

        $.ajax({
            url: '/api/SaveBillDetails/SaveDetails',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: 'POST',
            data: JSON.stringify({ Product: productName, Price: priceArticle, ExtraItems: ExtraItems, OrderId: Id }),
            success: function (result) {

                if (result) {
                    BillPrint();
                }
            }
        });
    });
}


function updateSize() {
    var ProductPrice = document.getElementById('ProduktInputPrice');
    var ProductSize = document.getElementById('ProduktInputSize');
    ProductPrice.value = "";
    ProductSize.value = "";
    var selectElement = document.getElementById("ProduktInputName");
    var selectedProductId = selectElement.value;

    $("#divLoading").fadeIn(300);

    if (selectedProductId != "") {
        $.ajax({
            url: '/api/Product/GetProductSizes',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: 'POST',
            data: JSON.stringify({ ProductId: parseInt(selectedProductId) }),
            success: function (result) {
                $("#divLoading").fadeOut(300);
                var productSizes = result;
                var ProductSize = document.getElementById('ProduktInputSize');
                var ProductPrice = document.getElementById('ProduktInputPrice');
                ProductPrice.value = productSizes[0];
                var options = "";
                for (var i = 1; i < productSizes.length; i++) {
                    if (productSizes[i] != null) {
                        options += "<option value='" + productSizes[i] + "'>" + productSizes[i] + "</option>";
                    }
                }
                ProductSize.innerHTML = options;
            }
        });
    }
    SetExtraItemsDefault();
    $("#divLoading").hide(0);
}

function updatePrice() {
    var selectElement = document.getElementById("ProduktInputSize");
    var showCard = document.querySelector('#ShowItemsCard');
    showCard.classList.remove('d-none');

    var selectedProductId = document.getElementById('ProduktInputName').value;
    var selectedProductSize = document.getElementById('ProduktInputSize').value;
    $("#divLoading").fadeIn(300);
    if (selectedProductId != "" && selectedProductSize != "") {
        showCard.classList.remove("d-none");
        $.ajax({
            url: '/api/Price/GetProductPrices',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: 'POST',
            data: JSON.stringify({ Size: selectedProductSize, Id: parseInt(selectedProductId) }),
            success: function (result) {
                $("#divLoading").fadeOut(300);
                var ProductPrice = document.getElementById('ProduktInputPrice');

                ProductPrice.value = result;
            }
        });
        updateItems();
    }
    $("#divLoading").hide(0);
}

function SetExtraItemsDefault() {
    var selectedProductSize = "Small";
    var AddExtraItems = document.getElementById('AddExtraItems');
    var showCard = document.querySelector('#ShowItemsCard');
    showCard.classList.remove('d-none');

    AddExtraItems.innerHTML = "";
    $("#divLoading").fadeIn(300);
    if (selectedProductSize != "") {
        $.ajax({
            url: '/api/Price/GetExtraItems',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: 'POST',
            data: JSON.stringify({ Size: selectedProductSize }),
            success: function (result) {
                $("#divLoading").fadeOut(300);

                for (var i = 0; i < result.length; i++) {
                    var listItem = document.createElement("li");
                    listItem.textContent = result[i]["name"] + " " + result[i]["price"] + "€";
                    listItem.classList.add("col-3");

                    var checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.classList.add("ms-2");

                    listItem.appendChild(checkbox);
                    AddExtraItems.appendChild(listItem);
                }
            }
        });
    }
    $("#divLoading").hide(0);
}

function updateItems() {
    var selectedProductSize = document.getElementById('ProduktInputSize').value;
    var AddExtraItems = document.getElementById('AddExtraItems');
    AddExtraItems.innerHTML = "";
    $("#divLoading").fadeIn(300);
    if (selectedProductSize != "") {
        $.ajax({
            url: '/api/Price/GetExtraItems',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: 'POST',
            data: JSON.stringify({ Size: selectedProductSize }),
            success: function (result) {
                $("#divLoading").fadeOut(300);

                for (var i = 0; i < result.length; i++) {
                    var listItem = document.createElement("li");
                    listItem.textContent = result[i]["name"] + " " + result[i]["price"] + "€";
                    listItem.classList.add("col-3");

                    var checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.classList.add("ms-2");

                    listItem.appendChild(checkbox);
                    AddExtraItems.appendChild(listItem);
                }
            }
        });
    }
    $("#divLoading").hide(0);
}

var sum = 0;
var totalSum = 0;

function AddProduct() {
    var ProductPrice = document.getElementById('ProduktInputPrice');
    var SumePrice = document.getElementById('SumePrice');
    var DeliveryPrice = document.getElementById('StoreDeliveryPrice').value;

    var selectElement = document.getElementById("ProduktInputName");
    var selectedOption = selectElement.options[selectElement.selectedIndex];
    var selectedName = selectedOption.text;

    ProductPrice.classList.remove("is-invalid");
    selectElement.classList.remove("is-invalid");

    if (selectedName != "" && ProductPrice.value.trim() != "") {
        var Bill = document.getElementById('tableBill');
        Bill.classList.remove("d-none");
        var tbody = document.querySelector("tbody");
        var row = document.createElement("tr");

        var priceExtraItems = PriceExtraItems();

        var countCell = document.createElement("td");
        countCell.textContent = sum += 1;
        row.appendChild(countCell);

        var nameCell = document.createElement("td");
        nameCell.classList.add('ProName');

        var nameExtraList = document.createElement('ul');
        for (var i = 0; i < resultExtraItem.length; i++) {
            var nameExtraItems = document.createElement('li');
            nameExtraItems.textContent = resultExtraItem[i].replace("€", "");
            nameExtraList.appendChild(nameExtraItems);
        }

        nameCell.appendChild(document.createTextNode(selectedName));

        var smallFontSpan = document.createElement('span');
        smallFontSpan.style.fontSize = "smaller";
        smallFontSpan.style.display = "block";

        smallFontSpan.appendChild(nameExtraList);

        nameCell.appendChild(smallFontSpan);
        row.appendChild(nameCell);

        var priceCell = document.createElement("td");
        var priceValue = parseFloat(ProductPrice.value);
        var PricWithItems = priceValue + priceExtraItems;
        priceCell.innerText = PricWithItems.toFixed(1) + " €";
        row.appendChild(priceCell);

        var deleteCell = document.createElement("td");
        var deleteButton = document.createElement("button");
        deleteButton.textContent = "Delete";
        deleteButton.classList.add("btn");
        deleteButton.classList.add("btn-danger");
        deleteButton.classList.add("btn-sm");
        deleteButton.classList.add("PrintHide");

        deleteCell.appendChild(deleteButton);
        row.appendChild(deleteCell);
        tbody.appendChild(row);

        deleteButton.addEventListener("click", async function () {
            tbody.removeChild(row);
            updateRowNumbers();
            sum -= 1;
            totalSum -= PricWithItems;

            await updateSumePrice();
        });

        totalSum += PricWithItems;
        SumePrice.value = totalSum + parseFloat(DeliveryPrice);
        updateRowNumbers();
    } else {
        ProductPrice.classList.add("is-invalid");
        selectElement.classList.add("is-invalid");
    }

    async function updateSumePrice() {
        SumePrice.value = totalSum + parseFloat(DeliveryPrice);
    }

}

function PriceExtraItems() {
    while (resultExtraItem.length > 0) {
        resultExtraItem.pop();
    }

    var checkboxes = document.querySelectorAll("#AddExtraItems input[type='checkbox']");
    var extrasList = document.querySelectorAll("#AddExtraItems li");
    var checkboxSum = 0;

    for (var i = 0; i < checkboxes.length; i++) {
        var checkbox = checkboxes[i];
        var listItem = extrasList[i];

        var price = parseFloat(listItem.textContent.match(/\d+\.?\d*/));
        var itemName = listItem.textContent.replace(/\d+\.?\d*/, "").trim();

        if (checkbox.checked) {
            resultExtraItem.push(`${itemName}`);
            checkboxSum += price;
        }
    }
    return checkboxSum;
}

function updateRowNumbers() {
    var tbody = document.querySelector("tbody");
    var rows = tbody.rows;

    for (var i = 0; i < rows.length; i++) {
        var countCell = rows[i].cells[0];
        countCell.textContent = i + 1;
    }
}