module Namegiver.Index {

	// Declared in Index.cshtml
	declare let nameInfoIdArg: number;

	let currentName: Dtos.NameInfo = null;

	$(document).ready(function () {
		if (nameInfoIdArg) {
			updateName(nameInfoIdArg);
		}
		else {
			updateRandomName();
		}
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

	function updateNameUi(nameInfo: Dtos.NameInfo) {
		let nameNode: HTMLElement = document.getElementById('name');
		nameNode.innerHTML = '<a href="/Character/' + nameInfo.Id + '">' + nameInfo.Name + '</a>';
		nameNode.style.display = '';
		$('.loader').hide();
		currentName = name;
	}

	function updateRandomName() {
		API.getRandomName(
			function (name: Dtos.NameInfo) {
				updateNameUi(name);
			},
			function (_jqXhr, _textStatus, _errorMessage) {
				currentName = null;
			}
		);
	}

	function updateName(nameInfoId: number) {
		API.getNameInfo(
			nameInfoId,
			function (name: Dtos.NameInfo) {
				updateNameUi(name);
			},
			function (_jqXhr, _textStatus, _errorMessage) {
				currentName = null;
			}
		);
	}
}