$(document).ready(function() {
    $("#search-button").click(function () {
        var searchText = $("#search-text").val();
        var startDate = $("#range-start-date").val();
        var endDate = $("#range-end-date").val();
        var filterLedger = $("#ledger-filter").val();

        var url = "/";
        var querystrings = [];
        if (searchText) querystrings.push("Query=" + encodeURIComponent(searchText));
        if (startDate) querystrings.push("StartDate=" + encodeURIComponent(startDate));
        if (endDate) querystrings.push("EndDate=" + encodeURIComponent(endDate));
        if (filterLedger) querystrings.push("Ledger=" + encodeURIComponent(filterLedger));

        if (querystrings.length > 0) {
            url = url + "?" + querystrings.join("&");
        }

        window.location.href = url;
    });
});