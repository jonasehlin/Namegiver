module Namegiver {
	$(document).ready(function () {

		var nameId = 0;

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

		$('#accept').click(function () {
			if (nameId > 0) {

				$.ajax({
					url: '/api/names/' + nameId + '/accept',
					type: 'PUT'
				}).done(function (data, status, xhr) {
					alert('Accept: ' + nameId);
					// TODO: Load new random name
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
					alert('Reject: ' + nameId);
					// TODO: Load new random name
				}).fail(function (jqXhr, textStatus, errorMessage) {
					alert('Error: ' + nameId);
				});
			}
		});
	});
}