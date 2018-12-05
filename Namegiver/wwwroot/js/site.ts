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
		bindEvents();
	});

	function bindEvents() {
		$('#accept').click(function () {
			if (currentName) {

				$.ajax({
					url: '/api/names/' + currentName.Id + '/accept',
					type: 'PUT'
				}).done(function (data, status, xhr) {
					alert('Name "' + currentName.Text + '" accepted!');
					updateRandomName();
				}).fail(function (jqXhr, textStatus, errorMessage) {
					alert('Error, ' + currentName.Text);
				});
			}
		});

		$('#reject').click(function () {
			if (currentName) {
				$.ajax({
					url: '/api/names/' + currentName.Id + '/reject',
					type: 'PUT'
				}).done(function (data, status, xhr) {
					alert('Name "' + currentName.Text + '" rejected!');
					updateRandomName();
				}).fail(function (jqXhr, textStatus, errorMessage) {
					alert('Error, ' + currentName.Text);
				});
			}
		});
	}

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
				console.log('getRandomName.done, data', data);
				success(data);
			}).fail(function (jqXhr, textStatus, errorMessage) {
				console.log('getRandomName: Error in response from /api/names', textStatus, errorMessage);
				fail(jqXhr, textStatus, errorMessage);
			});
		}
	}
}