function GetToday() {
	var now = new Date();
	return new Date(now.getFullYear(), now.getMonth(), now.getDate());
}

function GetBeginningOfWeek() {
	var today = GetToday();
	var day = today.getDay();
	var diff = today.getDate() - day;
	return new Date(today.setDate(diff));
}

function GetEndOfWeek() {
	var today = GetToday();
	var beginningOfWeek = GetBeginningOfWeek();
	return new Date(today.setDate(beginningOfWeek.getDate() + 6));
}

function GetBeginningOfQuarter() {
	var now = new Date();
	var quarter = Math.floor((now.getMonth() / 3));
	return new Date(now.getFullYear(), quarter * 3, 1);
}

function GetEndOfQuarter() {
	var beginningOfQuarter = GetBeginningOfQuarter();
	return new Date(beginningOfQuarter.getFullYear(), beginningOfQuarter.getMonth() + 3, 0);
}

function AddMonths(x) {
	var now = new Date();
	now.setMonth(now.getMonth() + x);
	return now;
}

function GetBeginningOfYear() {
	return new Date(new Date().getFullYear(), 0, 1);
}

function GetEndOfYear() {
	return new Date(new Date().getFullYear(), 11, 31);
}
