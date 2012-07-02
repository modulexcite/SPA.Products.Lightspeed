/// <reference path="knockout-2.1.0.debug.js" />
/// <reference path="knockout.validation.js" />

toastr.options.timeOut = 2000;

var global = {
    createAlert: function (type, interval, title, text) {
        //var alert = $("<div />").addClass("alert alert-" + type).html("<h4 class='alert-heading'>" + title + "</h4>" + text).hide().prependTo(".container").fadeIn(500);
        var alert = $("<div />").addClass("alert alert-block alert-" + success).append($("<h4 />").addClass("alert-heading").text(title)).append(text).prependTo(".container");
        window.setTimeout(function () { alert.fadeOut(500).remove(); }, interval);
    }
};

//models
function Product(productDTO) {

    var self = this;

    self.Name = ko.observable("").extend({required: true});
    self.Price = ko.observable(0).extend({ required: true, number: true });
    self.Quantity = ko.observable(0).extend({required: true, number: true});
    self.ProductColor = ko.observable(new ProductColor());

    self.isEdited = ko.observable(false);

    if(productDTO != undefined) {
        self.ID = productDTO.ID;
        
        self.Name(productDTO.Name);
        self.Price(productDTO.Price);
        self.Quantity(productDTO.Quantity);

        self.ProductColor = new ProductColor(productDTO.ProductColor);
        self.Created = productDTO.Created;
        self.LastUpdated = productDTO.LastUpdated;
    }
}

function ProductColor(productColorDTO) {
    var self = this;
    self.Colors = ko.observableArray([]);

    if (productColorDTO != undefined) {
        //self.Color = new Color(productColorDTO.Color);
        $.each(productColorDTO.Colors, function (index, item) {
            self.Colors.push(new Color(item));
        });
        self.ID = productColorDTO.ID;
    }
}

function Color(ColorDTO) {
    var self = this;
    self.Name = ko.observable("").extend({ pattern: {
        params: '^#(?:[0-9a-fA-F]{3}){1,2}$'
        }
    });
    self.ID = ko.observable();

    if(ColorDTO != undefined) {
        self.Name(ColorDTO.Name);

        self.ID(ColorDTO.ID);
        self.Created = ColorDTO.Created;
        self.LastUpdated = ColorDTO.LastUpdated;    
    }
}

//viewmodel
function viewModel() {

    var self = this;

    self.products = ko.observableArray([]);
    self.colors = ko.observableArray([]);
    self.productColors = ko.observableArray([]);

    self.createdItem = new Product();
    self.createdColor = new Color();
    self.createdProductColor = new ProductColor();
    self.selectedColors = ko.observableArray([]);

    self.totalResults = ko.observable(0);

    self.selectedColors.subscribe(function (colors) {
        self.createdItem.ProductColor().Colors.removeAll();
        
        $.each(colors, function (index, item) {
            self.createdItem.ProductColor().Colors.push(new Color(ko.mapping.toJS(item)));
        });
    }, this);

    self.deleteProduct = function(product) {
        _deleteProd(product);
        self.totalResults(self.totalResults() - 1);
    };

    self.editProduct = function (product) {
        product.isEdited(true);
    };

    self.updateProduct = function (product) {
        product.isEdited(false);
        _updateProd(product);
    };

    self.addProduct = function (productDTO) {
        _addProductInternal(new Product(productDTO));
    };

    self.addColor = function (colorDTO) {
        _addColorInternal(new Color(colorDTO));
    };

    self.addProductColor = function (productColorDTO) {
        _addProductColorInternal(new ProductColor(productColorDTO));
    };

    var _addProductColorInternal = function (productcolor) {
        self.productColors.push(productcolor);
    }

    var _addProductInternal = function (product) {
        if (product.Created == undefined)
            product.Created = new Date();

        if (product.LastUpdated == undefined)
            product.LastUpdated = new Date();

        self.products.push(product);
    }

    var _addColorInternal = function (color) {
        self.colors.push(color);
    }

    self.itemAddedEvent = function (elements, data) {
        $(elements).overlay({opacity:0.75, animation: "slide"});
    }

    self.create = function () {
        $.ajax({
            url: "/ninjectjson/addProduct",
            data: JSON.stringify(ko.mapping.toJS(self.createdItem)),
            type: "POST",
            contentType: "application/json",
            success: function (response) {
                if (response != null) {
                    if (response.success) {
                        self.createdItem.ID = response.ID;
                        _addProductInternal(self.createdItem);
                        self.totalResults(self.totalResults() + 1);
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
                self.createdItem = new Product();
            }
        });
    };

    self.createColor = function () {
        var color = self.createdColor;
        self.createdColor = new Color();

        $.ajax({
            url: "/ninjectjson/addColor",
            data: JSON.stringify(ko.mapping.toJS(color)),
            type: "POST",
            contentType: "application/json",
            success: function (response) {
                if (response != null) {
                    if (response.success) {
                        color.ID(response.ID);
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
                self.createdColor = new Color();
            }
        });
    };

    self.deleteColor = function (color) {
        console.log(color);
        $.ajax({
            url: "/ninjectjson/DeleteColor",
            data: JSON.stringify({ "guid": color.ID() }),
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

//    self.createProductColor = function () {
//        $.ajax({
//            url: "/json/addProductColor",
//            data: JSON.stringify(ko.mapping.toJS(self.createdProductColor)),
//            type: "POST",
//            contentType: "application/json",
//            success: function (response) {
//                if (response != null) {
//                    if (response.success) {
//                        self.createdProductColor.ID = response.ID;
//                        _addProductColorInternal(self.createdProductColor);
//                    } else {
//                        alert("error: " + response);
//                    }
//                }
//            },
//            error: function (response) {
//                alert("error: " + response);
//            },
//            complete: function () {
//                self.createdColor = new Color();
//            }
//        });
//    };

    var _updateProd = function (product) {
        $.ajax({
            url: "/ninjectjson/updateProduct",
            data: JSON.stringify(ko.mapping.toJS(product)),
            type: "POST",
            contentType: "application/json",
            success: function (response) {
                if (response != null) {
                    if (response.success) {
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

    var _deleteProd = function (product) {
        $.ajax({
            url: "/ninjectjson/DeleteProduct",
            data: JSON.stringify({ "guid": product.ID }),
            type: "POST",
            contentType: "application/json",
            success: function (response) {
                if (response != null) {
                    if (response.success) {
                        self.products.remove(product);
                        toastr.success('Product has been deleted!', 'Hooray!');
                    } else {
                        toastr.error('Product has not been deleted due to an error. Sorry!', 'Well, this is embarassing!');
                    }
                }
            },
            error: function (response) {
                toastr.error('Product has not been deleted due to an error. Sorry!', 'Well, this is embarassing!');
            },
            complete: function () {
                
            }
        });
    };
};

ko.extenders.liveEditor = function (target) {
    target.editing = ko.observable(false);

    target.edit = function () {
        target.editing(true);
    };

    target.stopEditing = function () {
        target.editing(false);
        toastr.info("Don't forget to save your changes!");
    };
    return target;
};

ko.bindingHandlers.liveEditor = {
    init: function (element, valueAccessor) {
        var observable = valueAccessor();
        observable.extend({ liveEditor: this });
    },
    update: function (element, valueAccessor) {
        var observable = valueAccessor();
        ko.bindingHandlers.css.update(element, function () { return { editing: observable.editing }; });
    }
};

//ko.dirtyFlag = function (root) {
//    var result = function () { }
//    var _initialState = ko.observable(ko.toJSON(root));

//    result.isDirty = ko.computed(function () {
//        return _initialState() !== ko.toJSON(root);
//    });

//    result.reset = function () {
//        _initialState(ko.toJSON(root));
//    };

//    return result;
//};


//init
var vm;
var take = 3;
var offset = 0;

$(function () {
    vm = new viewModel();

    /*$.get("/ninjectjson/products", function (result) {
    $.each(result, function (index, item) {
    vm.addProduct(item);
    });
    }, "json");*/

    $.get("/ninjectjson/productsFilter?take=" + take + "&offset=" + offset, function (result) {
        $.each(result.result, function (index, item) {
            vm.addProduct(item);
        });
        addPagination(Math.ceil(result.count / take), Math.floor(offset / take) + 1);
        vm.totalResults(result.count);
    }, "json");

    $.get("/ninjectjson/colors", function (result) {
        $.each(result, function (index, item) {
            vm.addColor(item);
        });
    }, "json");

    $("a.pager").live("click", function (event) {
        event.preventDefault();

        offset = $(this).data("offset");
        //offset = offset + take;
        console.log(offset);
        //if (offset >= vm.totalResults())
        //  offset = $(this).parent().index() * take - take;

        vm.products.removeAll();
        $.get("/ninjectjson/productsFilter?take=" + take + "&offset=" + offset, function (result) {
            $.each(result.result, function (index, item) {
                vm.addProduct(item);
            });
        }, "json");

        $(this).parent().parent().find("li").removeClass("active");
        $(this).parent().addClass("active");
    });

    ko.applyBindings(vm);
});

function addPagination(pages, active) {
    var $target = $("div.pagination ul");

    for (i = 1; i <= pages; i++) {
        var $item = $("<li />").append($("<a />").attr("data-offset",take*(i-1)).addClass("pager").text(i)).appendTo($target);
        if (i == active)
            $item.addClass("active");
    }
};