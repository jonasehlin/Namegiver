module Namegiver {

	interface NameInfo {
		Id: number;
		NameId: number;
		Name: string;
		Accepted: boolean;
		RejectedCount: number;
	}

	let currentName: NameInfo = null;

	$(document).ready(function () {
		updateRandomName();
		bindEvents();
	});

	function bindEvents() {
		$('#accept').click(function () {
			API.acceptName(
				currentName,
				function () {
					alert('Name "' + currentName.Name + '" accepted!');
					updateRandomName();
				},
				function (_jqXhr, _textStatus, _errorMessage) {
					alert('Error, ' + currentName.Name);
				}
			);
		});

		$('#reject').click(function () {
			API.rejectName(
				currentName,
				function () {
					updateRandomName();
				},
				function (_jqXhr, _textStatus, _errorMessage) {
					alert('Error, ' + currentName.Name);
				}
			);
		});

		$('#newNameBtn').on('click', function (ev) {
			ev.preventDefault();
			$('#name').hide();
			$('.loader').show();
			updateRandomName();
		});
	}

	function updateRandomName() {
		API.getRandomName(function (name: NameInfo) {
			$('#name').text(name.Name).show();
			$('.loader').hide();
			currentName = name;
		}, function (_jqXhr, _textStatus, _errorMessage) {
			currentName = null;
		});
	}

	module API {
		export function getRandomName(doneCallback: (name: NameInfo) => void, failCallback: (jqXhr, textStatus, errorMessage) => void) {
			$.ajax({
				url: '/api/names',
				type: 'GET'
			}).done(function (data, status, xhr) {
				console.log('getRandomName.done, data', data);
				doneCallback(data);
			}).fail(function (jqXhr, textStatus, errorMessage) {
				console.log('getRandomName: Error in response from /api/names', textStatus, errorMessage);
				failCallback(jqXhr, textStatus, errorMessage);
			});
		}

		export function acceptName(name: NameInfo, doneCallback: () => void, failCallback: (jqXhr, textStatus, errorMessage) => void) {
			if (name) {
				$.ajax({
					url: '/api/names/' + name.Id + '/accept',
					type: 'PUT'
				}).done(function (data, status, xhr) {
					console.log('acceptName.done');
					doneCallback();
				}).fail(function (jqXhr, textStatus, errorMessage) {
					console.log('acceptName failed');
					failCallback(jqXhr, textStatus, errorMessage);
				});
			}
		}

		export function rejectName(name: NameInfo, doneCallback: () => void, failCallback: (jqXhr, textStatus, errorMessage) => void) {
			if (name) {
				$.ajax({
					url: '/api/names/' + name.Id + '/reject',
					type: 'PUT'
				}).done(function (data, status, xhr) {
					console.log('rejectName.done');
					doneCallback();
				}).fail(function (jqXhr, textStatus, errorMessage) {
					console.log('rejectName failed');
					failCallback(jqXhr, textStatus, errorMessage);
				});
			}
		}
	}
}