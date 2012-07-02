/// <reference path="knockout-2.1.0.debug.js" />
/// <reference path="knockout.validation.js" />

toastr.options.timeOut = 2000;

var global = {};
global.addPagination = function (pages, active) {
    var $target = $("div.pagination ul");

    $target.empty();
    for (i = 1; i <= pages; i++) {
        var $item = $("<li />").append($("<a />").attr("data-offset", vm.pagination.take() * (i - 1)).addClass("pager").text(i)).appendTo($target);
        if (i == active)
            $item.addClass("active");
    }
};

global.resetProduct = function () {
    
}

//viewmodel
function viewModel() {

    var self = this;

    //pagination, order, results - UI stuff
    self.pagination = { take: ko.observable(3), offset: ko.observable(0) };
    self.ordering = [
        { name: "Name", isActive: ko.observable(true)},
        { name: "Created", isActive: ko.observable(false)},
        { name: "Updated", isActive: ko.observable(false)}
    ];
    self.totalResults = ko.observable(0);

    //products, colors
    self.products = ko.observableArray([]);
    self.colors = ko.observableArray([]);

    //created item holders
    self.createdItem = new Models.Product();
    self.createdColor = new Models.Color();
    
    //selected colors for created item
    self.selectedColors = ko.observableArray([]);
    self.selectedColors.subscribe(function (colors) {
        self.createdItem.Colors.removeAll();
        $.each(colors, function (index, item) {
            self.createdItem.Colors.push(new Models.Color(ko.mapping.toJS(item)));
        });
    }, this);

    self.editProduct = function (product) {
        product.isEdited(true);
    };

    self.updateProduct = function (product) {
        product.isEdited(false);
        _updateProd(product);
    };

    self.addProduct = function (productDTO) {
        _addProductInternal(new Models.Product(productDTO));
    };

    self.addColor = function (colorDTO) {
        _addColorInternal(new Models.Color(colorDTO));
    };

    var _addProductInternal = function (product) {
        self.products.push(product);
    }

    var _addColorInternal = function (color) {
        self.colors.push(color);
    }

    self.itemAddedEvent = function (elements, data) {
        $(elements).overlay({opacity:0.75, animation: "slide"});
    }


    //product CRUD
    self.create = function () {
        $.ajax({
            url: "/products/addProduct",
            data: JSON.stringify(ko.mapping.toJS(self.createdItem)),
            type: "POST",
            contentType: "application/json",
            success: function (response) {
                if (response != null) {
                    if (response.success) {
                        self.getProducts(true);
                        toastr.success('Product has been successfully added.', 'Hooray!');
                    } else {
                        toastr.error('Product has not been added due to an error. Sorry!', 'Well, this is embarassing!');
                    }
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

    var _updateProd = function (product) {
        $.ajax({
            url: "/products/updateProduct",
            data: JSON.stringify(ko.mapping.toJS(product)),
            type: "POST",
            contentType: "application/json",
            success: function (response) {
                if (response != null) {
                    if (response.success) {
                        self.getProducts(true);
                        toastr.success('Product has been updated!', 'Hooray!');
                    } else {
                        toastr.error('Product has not been updated due to an error. Sorry!', 'Well, this is embarassing!');
                    }
                }
            },
            error: function (response) {
                toastr.error('Product has not been updated due to an error. Sorry!', 'Well, this is embarassing!');
            }
        });
    };

    self.deleteProduct = function (product) {
        self.totalResults(self.totalResults() - 1);
        $.ajax({
            url: "/products/DeleteProduct",
            data: JSON.stringify({ "guid": product.ID }),
            type: "POST",
            contentType: "application/json",
            success: function (response) {
                if (response != null) {
                    if (response.success) {
                        self.getProducts(true);
                        toastr.success('Product has been deleted!', 'Hooray!');
                    } else {
                        toastr.error('Product has not been deleted due to an error. Sorry!', 'Well, this is embarassing!');
                    }
                }
            },
            error: function (response) {
                toastr.error('Product has not been deleted due to an error. Sorry!', 'Well, this is embarassing!');
            }
        });
    };

    //color crud
    self.createColor = function () {
        var color = ko.mapping.toJS(self.createdColor);

        $.ajax({
            url: "/colors/addColor",
            data: JSON.stringify(color),
            type: "POST",
            contentType: "application/json",
            success: function (response) {
                if (response != null) {
                    if (response.success) {
                        color.ID = response.ID;
                        _addColorInternal(color);
                        toastr.success('Color has been successfully added!', 'Hooray!');
                    } else {
                        toastr.error('Color has not been added due to an error. Sorry!', 'Well, this is embarassing!');
                    }
                }
            },
            error: function (response) {
                toastr.error('Color has not been added due to an error. Sorry!', 'Well, this is embarassing!');
            },
            complete: function () {
                _resetCreatedColor();
            }
        });
    };

    self.deleteColor = function (color) {

        var c = ko.mapping.toJS(color);

        $.ajax({
            url: "/colors/DeleteColor",
            data: JSON.stringify({ "guid": c.ID }),
            type: "POST",
            contentType: "application/json",
            success: function (response) {
                if (response != null) {
                    if (response.success) {
                        self.colors.remove(color);
                        toastr.success('Color has been deleted!', 'Hooray!');
                    } else {
                        toastr.error('Color has not been deleted due to an error. Sorry!', 'Well, this is embarassing!');
                    }
                }
            },
            error: function (response) {
                toastr.error('Color has not been deleted due to an error. Sorry!', 'Well, this is embarassing!');
            }
        });
    };

    self.reorderProducts = function (obj) {
        $.each(self.ordering, function (index, item) {
            item.isActive(false);
        });
        obj.isActive(true);
        self.getProducts();
    }

    self.getProducts = function (paginate) {
        self.products.removeAll();
        var order = ko.utils.arrayFirst(self.ordering, function (p) {
            return p.isActive() == true;
        });

        $.get("/products/productsFilter?take=" + self.pagination.take() + "&offset=" + self.pagination.offset() + "&order=" + order.name, function (result) {
            $.each(result.result, function (index, item) {
                self.addProduct(item);
            });
            self.totalResults(result.count);
            if (paginate)
                global.addPagination(Math.ceil(result.count / self.pagination.take()), Math.floor(self.pagination.offset() / self.pagination.take()) + 1);
        }, "json");
    }

    self.getColors = function () {
        self.colors.removeAll();
        $.get("/colors/colors", function (result) {
            $.each(result, function (index, item) {
                self.addColor(item);
            });
        }, "json");
    }

    self.regroupProducts = function (el, evt) {
        var btn = $(evt.srcElement);
        self.pagination.take(btn.data("group"));
        btn.addClass("active");
        btn.siblings().each(function () { $(this).removeClass("active"); });
        self.getProducts(true);
    };

    var _resetCreatedProduct = function () {
        self.createdItem.Name("");
        self.createdItem.Price(0);
        self.createdItem.Quantity(0);
    }

    var _resetCreatedColor = function () {
        self.createdColor.Name("");
    }
};

//init
var vm;

$(function () {
    vm = new viewModel();

    vm.getProducts(true);
    vm.getColors();

    $("a.pager").live("click", function (event) {
        event.preventDefault();

        vm.pagination.offset($(this).data("offset"));

        vm.products.removeAll();
        vm.getProducts();

        $(this).parent().parent().find("li").removeClass("active");
        $(this).parent().addClass("active");
    });

    ko.applyBindings(vm);
});