ko.validation.init({
    registerExtenders: true,
    messagesOnModified: true,
    insertMessages: true,
    parseInputAttributes: true,
    messageTemplate: null
}, true);

ko.extenders.numeric = function (target, precision) {
    //create a writable computed observable to intercept writes to our observable
    var result = ko.pureComputed({
        read: target,  //always return the original observables value
        write: function (newValue) {
            var current = target(),
                roundingMultiplier = Math.pow(10, precision),
                newValueAsNum = isNaN(newValue) ? 0 : +newValue,
                valueToWrite = Math.round(newValueAsNum * roundingMultiplier) / roundingMultiplier;

            //only write if it changed
            if (valueToWrite !== current) {
                target(valueToWrite);
            } else {
                //if the rounded value is the same, but a different value was written, force a notification for the current field
                if (newValue !== current) {
                    target.notifySubscribers(valueToWrite);
                }
            }
        }
    }).extend({ notify: 'always' });

    //initialize with current value to make sure it is rounded appropriately
    result(target());

    //return the new computed observable
    return result;
};

data = function () { };
var itemTypes = [];
var DonationLine = function () {
    var self = this;

    self.Index = ko.observable(0);
    self.ItemType = ko.observable().extend({ required: { message: 'Donation Type is required' } });;
    self.ItemName = ko.observable().extend({ required: { message: 'Item Name is required' } });
    self.ImageUrl = ko.observable();
    self.DonationStatus = ko.observable();
    self.WantThis = ko.observable(false);

    self.fileData = ko.observable({
        dataURL: ko.observable(),
        // base64String: ko.observable(),
    });

    self.MultiFileData = ko.observable({
        dataURLArray: ko.observableArray(),
        base64String: ko.observable()
    });


    self.ItemTypes = ko.observableArray(itemTypes);
    self.onClear = function (imageFile) {
        if (confirm('Are you sure?')) {
            imageFile.clear && imageFile.clear();
        }
    };

    self.errors = ko.validation.group(self);
}

var MakeDonationViewModel = function (data) {
    var self = this;
    data = data || {};

    if (data !== null) {

        //========** Properties **=============

        self.Title = ko.observable(data.Title || '').extend({
            required: true,
            maxLength: 50
        });

        self.Description = ko.observable(data.Description || '').extend({
            required: true
        });

        itemTypes = data.ItemTypes;
        self.Amount = ko.observable(data.Amount || 0.00).extend({ numeric: 2 });
        self.IsMoney = ko.observable(data.IsMoney || false);
        self.ExpireOn = ko.observable(moment().add(3, 'M').format('YYYY-MM-DD'));
        self.ItemTypes = ko.observableArray(data.ItemTypes);
        self.DonationDetails = ko.observableArray([new DonationLine(data.DonationDetails)]);
        self.errors = ko.validation.group(self);
        self.ShowAddButton = ko.observable(false);
        self.AddDetails = ko.observable(data.AddDetails || false);
        self.onIsMoneyChange = function () {
            if (!this.IsMoney())
                this.Amount('');
        };

        self.onAddDetails = function (e) {
            if (!this.AddDetails()) {
                self.DonationDetails = ko.observableArray([new DonationLine(data.DonationDetails)]);
            }
        };

        self.HeaderMultiFileData = ko.observable({
            dataURLArray: ko.observableArray(),
            base64String: ko.observable()
        });

        //========**Computed properties**=============


        //========** Functions **=============

        self.CanShowDetails = ko.observable(false);

        self.onClear = function (imageFile) {
            if (confirm('Are you sure?')) {
                imageFile.clear && imageFile.clear();
            }
        };

        self.ImageCount = ko.computed(function () {
            if (self.HeaderMultiFileData().fileArray)
                return self.HeaderMultiFileData().fileArray().length;
            else
                return 0;
        });

        self.descriptionKeyup = function (data, event) {
            console.log('key:' + event.key);

            if (event.key === "Delete" || (event.key === "Backspace" && event.currentTarget.value.length === 1)) {
                self.CanShowDetails(false);
                return true;
            }

            if (self.Title() !== '' && event.key !== '' && !self.IsMoney())
                self.CanShowDetails(true);
            else
                self.CanShowDetails(false);

            return true;
        };

        self.titleKeyup = function (data, event) {
            console.log('key:' + event.key);

            if (event.key === "Enter")
                return true;

            if (event.key === "Delete" || (event.key === "Backspace" && event.currentTarget.value === "")) {
                self.CanShowDetails(false);
                return true;
            }

            if (self.Description() !== '' && event.key !== '' && !self.IsMoney())
                self.CanShowDetails(true);
            else
                self.CanShowDetails(false);

            return true;
        };

        //========** Events **=============
        // Operations
        self.addLine = function () {

            if (this.errors().length === 0) {
                var list = self.DonationDetails();
                var index = list.length - 1;
                var line = list[index];
                line.Index(index);
                self.DonationDetails.push(new DonationLine())
                self.ShowAddButton(true);
            }
            else {
                alert('Please check your submission.');

                if (this.errors().length > 0)
                    this.errors.showAllMessages(true);

                this.errors().forEach(function (data) {
                    alert(data.error);
                });
            }
        };
        self.removeLine = function (line) { self.DonationDetails.remove(line) };
    }

    self.submit = function (e) {
        var form = new FormData($('.page-make-donation.form-horizontal')[0]);
        var headerImages = [];
        var details = [];
        var index = 0;

        this.DonationDetails().forEach(function (e) {
            var u = ko.mapping.toJS(e);
            if (u.ItemName !== undefined) {
                details.push({
                    Index: index,
                    ItemType: u.ItemType,
                    ItemName: u.ItemName,
                    ImageUrl: u.ImageUrl,
                    DonationStatus: u.DonationStatus,
                    WantThis: u.WantThis,
                    MultiFileData: u.MultiFileData.fileArray
                });
                index++;
            }
        });

        this.HeaderMultiFileData().fileArray().forEach(function (e) {
            var u = ko.mapping.toJS(e);
            headerImages.push(u);
        });

        form.append("details", JSON.stringify(details));
        form.append("headerImages", JSON.stringify(headerImages));

        //Ajax call to Insert the Employee
        $.ajax({
            type: "POST",
            url: "/Donations/MakeADonation",
            data: form,
            enctype: 'multipart/form-data',
            processData: false,  // Important!
            contentType: false,
            cache: false,
            timeout: 600000,
            success: function (response) {
                if (response.result === 'Redirect')
                    window.location = response.url;
                if (response.result === 'Error')
                    alert(response.Message);
            },
            error: function (ex) {
                alert(ex.statusText);
            }
        });

    };

};