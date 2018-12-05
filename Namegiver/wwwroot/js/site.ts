module Namegiver {
	var nameId = 0;

	interface Name {
		Id: number;
		Text: string;
		Accepted: boolean;
		RejectedCount: number;
		Language: string;
		Gender: number;
	}

	$(document).ready(function () {

		updateRandomName();

		$('#accept').click(function () {
			if (nameId > 0) {

				$.ajax({
					url: '/api/names/' + nameId + '/accept',
					type: 'PUT'
				}).done(function (data, status, xhr) {
					alert('Name "' + + '" accepted!');
					updateRandomName();
				}).fail(function (jqXhr, textStatus, errorMessage) {
					alert('Error: ' + nameId);
				});
			}
		});

		$('#reject').click(function () {
			if (nameId > 0) {

				$.ajax({
					url: '/api/names/' + nameId + '/reject',
					type: 'PUT'
				}).done(function (data, status, xhr) {
					updateRandomName();
				}).fail(function (jqXhr, textStatus, errorMessage) {
					alert('Error: ' + nameId);
				});
			}
		});
	});

	function updateRandomName() {
		$.ajax({
			url: '/api/names',
			type: 'GET'
		}).done(function (data, status, xhr) {
			console.log('Success, data', data);
			$('#name').text(data.text);
			nameId = data.id;
		}).fail(function (jqXhr, textStatus, errorMessage) {
			console.log('Error in response from /api/names', textStatus);
			nameId = -1;
		});
	}
}