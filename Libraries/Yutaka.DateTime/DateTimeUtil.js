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

function Get7DaysAgo() {
	var today = GetToday();
	today.setDate(today.getDate() - 7);
	return today;
}

function GetBeginningOfMonth() {
	var today = GetToday();
	return new Date(today.getFullYear(), today.getMonth(), 1);
}

function GetEndOfMonth() {
	var today = GetToday();
	return new Date(today.getFullYear(), today.getMonth() + 1, 0);
}

function Get30DaysAgo() {
	var today = GetToday();
	today.setDate(today.getDate() - 30);
	return today;
}

function GetBeginningOfQuarter() {
	var today = GetToday();
	var quarter = Math.floor((today.getMonth() / 3));
	return new Date(today.getFullYear(), quarter * 3, 1);
}

function GetEndOfQuarter() {
	var beginningOfQuarter = GetBeginningOfQuarter();
	return new Date(beginningOfQuarter.getFullYear(), beginningOfQuarter.getMonth() + 3, 0);
}

function Get3MonthsAgo() {
	var today = GetToday();
	today.setMonth(today.getMonth() - 3);
	return today;
}

function GetBeginningOfYear() {
	return new Date(new Date().getFullYear(), 0, 1);
}

function GetEndOfYear() {
	return new Date(new Date().getFullYear(), 11, 31);
}

function Get12MonthsAgo() {
	var today = GetToday();
	today.setMonth(today.getMonth() - 12);
	return today;
}