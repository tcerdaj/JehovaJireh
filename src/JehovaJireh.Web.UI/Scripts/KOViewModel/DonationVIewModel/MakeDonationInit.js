var Init = function (data, templates) {
	var ViewModel = new MakeDonationViewModel(data);
	ko.applyBindings(ViewModel, $('.page-make-donation.form-horizontal')[0]);
};