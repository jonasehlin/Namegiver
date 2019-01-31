module Namegiver.Index {

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
			let nameNode: HTMLElement = document.getElementById('name');
			nameNode.innerHTML = '<a href="/Character/' + name.Id + '">' + name.Name + '</a>';
			nameNode.style.display = '';
			$('.loader').hide();
			currentName = name;
		}, function (_jqXhr, _textStatus, _errorMessage) {
			currentName = null;
		});
	}
}