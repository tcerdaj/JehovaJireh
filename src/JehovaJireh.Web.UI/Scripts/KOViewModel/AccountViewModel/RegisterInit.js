var Init = function (data, templates) {
	var ViewModel = new RegisterViewModel(data);
	ko.applyBindings(ViewModel, $('.page-register.form-horizontal')[0]);
};