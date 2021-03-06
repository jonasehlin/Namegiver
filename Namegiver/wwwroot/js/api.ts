﻿module Namegiver.API {
	export function getRandomName(doneCallback: (name: Dtos.NameInfo) => void, failCallback: (jqXhr, textStatus, errorMessage) => void) {
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

	export function getNameInfo(nameInfoId: number, doneCallback: (name: Dtos.NameInfo) => void, failCallback: (jqXhr, textStatus, errorMessage) => void) {
		$.ajax({
			url: '/api/names/' + nameInfoId,
			type: 'GET'
		}).done(function (data, status, xhr) {
			console.log('getNameInfo.done, data', data);
			doneCallback(data);
		}).fail(function (jqXhr, textStatus, errorMessage) {
			console.log('getNameInfo: Error in response from /api/names/' + nameInfoId, textStatus, errorMessage);
			failCallback(jqXhr, textStatus, errorMessage);
		});
	}

	export function acceptName(name: Dtos.NameInfo, doneCallback: () => void, failCallback: (jqXhr, textStatus, errorMessage) => void) {
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

	export function rejectName(name: Dtos.NameInfo, doneCallback: () => void, failCallback: (jqXhr, textStatus, errorMessage) => void) {
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