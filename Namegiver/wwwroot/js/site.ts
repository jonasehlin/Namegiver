﻿module Namegiver {

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
			API.acceptName(
				currentName,
				function () {
					alert('Name "' + currentName.Text + '" accepted!');
					updateRandomName();
				},
				function (jqXhr, textStatus, errorMessage) {
					alert('Error, ' + currentName.Text);
				}
			);
		});

		$('#reject').click(function () {
			API.rejectName(
				currentName,
				function () {
					updateRandomName();
				},
				function (jqXhr, textStatus, errorMessage) {
					alert('Error, ' + currentName.Text);
				}
			);
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
		export function getRandomName(doneCallback: (name: Name) => void, failCallback: (jqXhr, textStatus, errorMessage) => void) {
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

		export function acceptName(name: Name, doneCallback: () => void, failCallback: (jqXhr, textStatus, errorMessage) => void) {
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

		export function rejectName(name: Name, doneCallback: () => void, failCallback: (jqXhr, textStatus, errorMessage) => void) {
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