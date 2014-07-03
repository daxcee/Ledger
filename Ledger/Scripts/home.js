$(document).ready(function() {
    $("#search-button").click(function() {
        window.location.href = "/?q=" + encodeURIComponent($("#search-text").val());
    });
});