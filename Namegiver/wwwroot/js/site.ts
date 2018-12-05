module Namegiver {

	interface Name {
		Id: number;
		Text: string;
		Accepted: boolean;
		RejectedCount: number;
		Language: string;
		Gender: number;
	}

	let currentName: Name = null;

	$(document).ready(function () {

		updateRandomName();

		$('#accept').click(function () {
			if (currentName) {

				$.ajax({
					url: '/api/names/' + currentName.Id + '/accept',
					type: 'PUT'
				}).done(function (data, status, xhr) {
					alert('Name "' + + '" accepted!');
					updateRandomName();
				}).fail(function (jqXhr, textStatus, errorMessage) {
					alert('Error: ' + currentName.Id);
				});
			}
		});

		$('#reject').click(function () {
			if (currentName) {

				$.ajax({
					url: '/api/names/' + currentName.Id + '/reject',
					type: 'PUT'
				}).done(function (data, status, xhr) {
					updateRandomName();
				}).fail(function (jqXhr, textStatus, errorMessage) {
					alert('Error: ' + currentName.Id);
				});
			}
		});
	});

	function updateRandomName() {
		API.getRandomName(function (name: Name) {
			$('#name').text(name.Text);
			currentName = name;
		}, function (jqXhr, textStatus, errorMessage) {
			currentName = null;
		});
	}

	module API {
		export function getRandomName(success: (name: Name) => void, fail: (jqXhr, textStatus, errorMessage) => void) {
			$.ajax({
				url: '/api/names',
				type: 'GET'
			}).done(function (data, status, xhr) {
				console.log('Success, data', data);
				success(data);
			}).fail(function (jqXhr, textStatus, errorMessage) {
				console.log('Error in response from /api/names', textStatus);
				fail(jqXhr, textStatus, errorMessage);
			});
		}
	}
}