/// <reference path="knockout-2.1.0.debug.js" />
/// <reference path="knockout.validation.js" />

toastr.options.timeOut = 2000;

function viewModel() {

    var self = this;
    self.products = ko.observableArray([]);

    self.addProduct = function (productDTO) {
        toastr.info("New product has appeared!", "Whoa!");
        _addProductInternal(new Models.Product(productDTO));
    };

    self.deleteProduct = function (guid) {
        toastr.info("A product has been deleted!", "Whoa!");
        var prod = ko.utils.arrayFirst(self.products(), function (p) {
            return p.ID === guid;
        });
        _deleteProductInternal(prod);
    };

    var _addProductInternal = function (product) {
        self.products.push(product);
    }

    var _deleteProductInternal = function (product) {
        self.products.remove(product);
    }

    self.getProducts = function () {
        self.products.removeAll();
        $.get("/api/RealTimeProducts/GetProducts", function (result) {
            $.each(result, function (index, item) {
                _addProductInternal(new Models.Product(item));
            });
        }, "json");
    }

    self.showProductTransition = function (elem) {
        if ($(elem)[0].localName == "tr")
            $(elem).hide().fadeIn(1000);
    };

    self.hideProductTransition = function (elem, evt) {
        if ($(elem)[0].localName == "tr")
            $(elem).fadeOut(1000, function () { $(elem).remove(); });
    };
};

//init
var vm;

$(function () {
    vm = new viewModel();
    var hub = $.connection.products;

    hub.addP = function (item) {
        console.log(item);
        vm.addProduct(item);
    };

    hub.deleteP = function (guid) {
        vm.deleteProduct(guid);
    };

    $.connection.hub.start();

    vm.getProducts();
    ko.applyBindings(vm);
});