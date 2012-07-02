var Models = Models || {};

    //models
Models.Product = function (productDTO) {

    var self = this;

    self.Name = ko.observable("").extend({ required: true });
    self.Price = ko.observable(0).extend({ required: true, number: true });
    self.Quantity = ko.observable(0).extend({ required: true, number: true });
    self.Colors = ko.observableArray([]);

    self.isEdited = ko.observable(false);

    if (productDTO != null) {
        self.ID = productDTO.ID;

        self.Name(productDTO.Name);
        self.Price(productDTO.Price);
        self.Quantity(productDTO.Quantity);

        self.Created = Models.Helpers.DateHelper.FormatDate(productDTO.Created);
        self.LastUpdated = Models.Helpers.DateHelper.FormatDate(productDTO.LastUpdated);

        if (productDTO.Colors) {

            $.each(productDTO.Colors, function (index, item) {
                self.Colors.push(new Models.Color(item));
            });
        }
    }
}

    Models.Color = function(ColorDTO) {
        var self = this;
        self.Name = ko.observable("").extend({ pattern: {
            params: '^#([0-9a-fA-F]{2}){3}|([0-9a-fA-F]){3}$'
        }
        });
        self.ID = ko.observable();

        if (ColorDTO != undefined) {
            self.Name(ColorDTO.Name);

            self.ID(ColorDTO.ID);
            self.Created = Models.Helpers.DateHelper.FormatDate(ColorDTO.Created);
            self.LastUpdated = Models.Helpers.DateHelper.FormatDate(ColorDTO.LastUpdated);
        }
    }

    Models.Helpers = {

        DateHelper: {

            FormatDate: function (date) {
                formatted = "";

                if (date != undefined) {
                    if (date.length == 22)
                        date += 0;

                    var now = new Date(date);

                    formatted = now.getUTCFullYear().toString() + "/" +
          (now.getUTCMonth() + 1).toString() +
          "/" + now.getUTCDate() + " " + now.getUTCHours() +
          ":" + now.getUTCMinutes() + ":" + now.getUTCSeconds();
                }
                return formatted;
            }

        }

    }