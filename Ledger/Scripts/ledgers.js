$(document).ready(function () {
    var saveEditedLedger = function () {
        var id = $(this).data("id");
        var ledger = {};
        ledger.Ledger = id;
        ledger.LedgerDesc = $("[name='LedgerDesc'][data-id='" + id + "']").val();
        ledger.IsActive = $("[name='IsActive'][data-id='" + id + "']").is(":checked");

        if (validate(ledger)) {
            submitUpdatedLedgerToWebService(ledger);
        }
    };

    var submitUpdatedLedgerToWebService = function (ledger) {
        var row = $("[name='LedgerDesc'][data-id='" + ledger.Ledger + "']").closest("tr");
        $.ajax({
            type: "POST",
            url: "/Ajax/UpdateLedger",
            data: ledger,
            success: function (resp) {
                row.replaceWith(resp);
            },
            error: function (xhr) {
                alert("there was an error");
            }
        });
    };

    var cancelEditingLedger = function () {
        var id = $(this).data("id");
        var row = $(this).closest("tr");
        $.ajax({
            type: "GET",
            url: "/Ajax/GetLedgerRow",
            data: { 'id': id },
            success: function (resp) {
                row.replaceWith(resp);
            },
            error: function (xhr) {
                alert("there was an error");
            }
        });
    };

    var editLedger = function () {
        var id = $(this).data("id");
        var row = $(this).closest("tr");
        $.ajax({
            type: "GET",
            url: "/Ajax/GetLedgerEditRow",
            data: { 'id': id },
            success: function (resp) {
                row.replaceWith(resp);
            },
            error: function (xhr) {
                alert("there was an error");
            }
        });
    };

    var validate = function (ledger) {
        if (ledger.LedgerDesc == "") {
            alert("Must enter a description");
            return false;
        }
        return true;
    };

    var submitNewLedgerToWebService = function (ledger) {
        $.ajax({
            type: "POST",
            url: "/Ajax/CreateLedger",
            data: ledger,
            success: function(resp) {
                window.location.href = document.URL;
            },
            error: function(xhr) {
                alert("there was an error");
            }
        });
    };

    var submitNewLedger = function () {
        var ledger = {};
        ledger.LedgerDesc = $(".newLedger[name='LedgerDesc']").val();

        if (validate(ledger)) {
            submitNewLedgerToWebService(ledger);
        }
    };

    $(document).on("click", "#submitNewLedger", submitNewLedger);
    $(document).on("click", ".editLedger", editLedger);
    $(document).on("click", ".saveEditedLedger", saveEditedLedger);
    $(document).on("click", ".cancelEditingLedger", cancelEditingLedger);
});