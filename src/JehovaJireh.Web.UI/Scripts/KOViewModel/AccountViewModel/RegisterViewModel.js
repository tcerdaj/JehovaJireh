ko.validation.init({
	registerExtenders: true,
	messagesOnModified: true,
	insertMessages: true,
	parseInputAttributes: true,
	messageTemplate: null
}, true);

data = function () { };

var RegisterViewModel = function (data) {
	var self = this;

	data = data || {};

	//If the data is coming from edit, use it to fill the fields.
	if (data !== null) {

		self.FileData = ko.observable({
			dataURL: ko.observable(data.ImageUrl ||  ''),
			base64String: ko.observable(data.ImageUrl || '')
		});

		self.ImageUrl = ko.observable(data.ImageUrl || '');

		self.FileDataChange = ko.observable(false);
		//self.ImageUrl = ko.observable(data.ImageUrl || '');
		self.FileData().dataURL.subscribe(function (dataURL) {
			// dataURL has changed do something with it!
			self.FileDataChange(true);
			self.ImageUrl(dataURL);
		});

		self.onClear = function (imageFile) {
			if (confirm('Are you sure?')) {
				imageFile.clear && imageFile.clear();
			}
		};

		self.UserName = ko.observable(data.UserName || '').extend({
			required: true,
			maxLength: 35
		});

		//Properties from NameEntity
		self.FirstName = ko.observable(data.FirstName || '').extend({
			required: true,
			maxLength: 35
		});

		self.LastName = ko.observable(data.LastName || '').extend({
			required: true,
			maxLength: 35
		});

		self.Gender = ko.observable(data.Gender || '0');
		self.Email = ko.observable(data.Email || '').extend({
			pattern: {
				params: /^([\d\w-\.]+@([\d\w-]+\.)+[\w]{2,4})?$/,
				message: "Invalid email address."
			} 
		});
		
		self.PhoneNumber = ko.observable(data.PhoneNumber || '');
		self.IsChurchMember = ko.observable(data.IsChurchMember || '');
		self.ChurchName = ko.observable(data.ChurchName || '');
		self.ChurchAddress = ko.observable(data.ChurchAddress || '');
		self.ChurchPhone = ko.observable(data.ChurchPhone || '');
		self.ChurchPastor = ko.observable(data.ChurchPastor || '');
		self.NeedToBeVisited = ko.observable(data.NeedToBeVisited || '');
		self.Comments = ko.observable(data.Comments || '');
		self.Address = ko.observable(data.Address || '').extend({
			maxLength: 80
		});
		self.City = ko.observable(data.City || '').extend({
				maxLength: 31
			});
		self.State = ko.observable(data.State || '').extend({
			maxLength: 21
		});
		self.Zip = ko.observable(data.Zip || '').extend({
			maxLength: 13
		});
	}

	//Computed Name property
	self.FullName = ko.computed(function () { return self.FirstName() + ' ' + self.LastName(); });

	// Sex List  
	self.genders = [{ SexValue: "", SexLabel: "" },
	{ SexValue: 0, SexLabel: "Male" },
	{ SexValue: 1, SexLabel: "Female" }];

	this.genderChanged = function (obj, event) {
		if (event.originalEvent) { //user changed
		
		} else { // program changed

		}
	}
	self.isSubmiting = ko.observable(false);
	self.showChurchMember = ko.observable(function () {
		return this.IsChurchMember();
	});

	self.errors = ko.validation.group(this),

		self.submit = function () {
			//Ajax call to Insert the Employee
			$.ajax({
				type: "POST",
				url: "/Account/Register",
				data: ko.toJSON(PerData),
				contentType: "application/json",
				dataType: 'json',
				//cache: false,
				mimeType: "multipart/form-data",
				//processData: false,
				success: function () {
					alert("successful");
				},
				error: function () {
					alert("fail");
				}
			});
		};
};


