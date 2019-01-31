module Namegiver.Dtos {
	export interface NameInfo {
		Id: number;
		NameId: number;
		Name: string;
		Accepted: boolean;
		RejectedCount: number;
	}
}