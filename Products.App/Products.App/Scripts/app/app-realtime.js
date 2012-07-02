/// <reference path="knockout-2.1.0.debug.js" />

toastr.options.timeOut = 2000;

//viewmodel
function viewModel() {

    var self = this;

    self.products = ko.observableArray([]);
    self.colors = ko.observableArray([]);
    self.createdItem = new Models.Product();

    self.selectedColors = ko.observableArray([]);
    self.selectedColors.subscribe(function (colors) {
        self.createdItem.Colors.removeAll();
        
        $.each(colors, function (index, item) {
            self.createdItem.Colors.push(new Models.Color(ko.mapping.toJS(item)));
        });
    }, this);

    self.deleteProduct = function(product) {
        _deleteProd(product);
    };

    self.addProduct = function (productDTO) {
        _addProductInternal(new Models.Product(productDTO));
    };

    var _addProductInternal = function (product) {
        self.products.push(product);
    }

    self.itemAddedEvent = function (elements, data) {
        $(elements).overlay({opacity:0.75, animation: "slide"});
    }

    self.create = function () {
        $.ajax({
            url: "/api/realtimeproducts/PostProduct",
            data: JSON.stringify(ko.mapping.toJS(self.createdItem)),
            type: "POST",
            contentType: "application/json",
            success: function (response) {
                if (response != null) {
                    self.getProducts();
                    toastr.success('Product has been successfully added.', 'Hooray!');
                }
            },
            error: function (response) {
                toastr.error('Product has not been added due to an error. Sorry!', 'Well, this is embarassing!');
            },
            complete: function () {
                _resetCreatedProduct();
            }
        });
    };

    var _deleteProd = function (product) {
        $.ajax({
            url: "/api/realtimeproducts/RemoveProduct",
            data: JSON.stringify(product.ID),
            type: "POST",
            contentType: "application/json",
            success: function (response) {
                if (response != null) {
                   self.getProducts();
                   toastr.success('Product has been deleted!', 'Hooray!');
                }
            },
            error: function (response) {
                toastr.error('Product has not been deleted due to an error. Sorry!', 'Well, this is embarassing!');
            }
        });
    };

    self.getProducts = function (paginate) {
        self.products.removeAll();
        $.get("/api/values/products/", function (result) {
            $.each(result, function (index, item) {
                self.addProduct(item);
            });
        }, "json");
    }

    self.getColors = function () {
        self.colors.removeAll();
        $.get("/api/values/colors", function (result) {
            $.each(result, function (index, item) {
                self.colors.push(new Models.Color(item));
            });
        }, "json");
    }

    var _resetCreatedProduct = function () {
        self.createdItem.Name("");
        self.createdItem.Price(0);
        self.createdItem.Quantity(0);
    }
};

//init
var vm;

$(function () {
    vm = new viewModel();

    vm.getProducts(true);
    vm.getColors();

    ko.applyBindings(vm);
});