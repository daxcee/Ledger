$(document).ready(function () {
//    var billPaid = function() {
//        var id = $(this).data("id");
//        var paidDate = $("#paidDate").val();
//        if (!moment(paidDate).isValid()) {
//            alert("Please enter a valid paid date");
//            return;
//        }
//        submitbillPaidTransactionToWebService(id, paidDate);
//    };
//
//    var submitbillPaidTransactionToWebService = function (id, paidDate) {
//        $.ajax({
//            type: "POST",
//            url: "/Ajax/MarkTransactionBillPaid",
//            data: { 'id': id, 'paidDate': paidDate },
//            success: function (resp) {
//                $(".doPaid[data-id='" + id + "']").replaceWith("<strong>" + paidDate + "</strong>");
//            },
//            error: function (xhr) {
//                alert("there was an error");
//            }
//        });
//    };
//
//    var saveEditedTransaction = function() {
//        var id = $(this).data("id");
//        var trans = {};
//        trans.Id = id;
//        trans.Desc = $("[name='Transaction.Desc'][data-id='" + id + "']").val();
//        trans.Amount = $("[name='Transaction.Amount'][data-id='" + id + "']").val();
//        if ($("[name='Transaction.DateDue'][data-id='" + id + "']").val() !== "")
//            trans.DateDue = $("[name='Transaction.DateDue'][data-id='" + id + "']").val();
//        else
//            trans.DateDue = null;
//        if ($("[name='Transaction.DatePayed'][data-id='" + id + "']").val() !== "")
//            trans.DatePayed = $("[name='Transaction.DatePayed'][data-id='" + id + "']").val();
//        else
//            trans.DatePayed = null;
//        if ($("[name='Transaction.DateReconciled'][data-id='" + id + "']").val() !== "")
//            trans.DateReconciled = $("[name='Transaction.DateReconciled'][data-id='" + id + "']").val();
//        else
//            trans.DateReconciled = null;
//        trans.Account = $("[name='Transaction.Account'][data-id='" + id + "']").val();
//        trans.Ledger = $("[name='Transaction.Ledger'][data-id='" + id + "']").val();
//
//        if (validate(trans)) {
//            submitUpdatedTransactionToWebService(trans);
//        }
//    };
//
//    var submitUpdatedTransactionToWebService = function (trans) {
//        var row = $("[name='Transaction.Desc'][data-id='" + trans.Id + "']").closest("tr");
//        console.log(row);
//        $.ajax({
//            type: "POST",
//            url: "/Ajax/UpdateTransaction",
//            data: trans,
//            success: function (resp) {
//                console.log(resp);
//                row.replaceWith(resp);
//            },
//            error: function (xhr) {
//                alert("there was an error");
//            }
//        });
//    };

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
//    $(document).on("click", ".saveEditedTransaction", saveEditedTransaction);
    $(document).on("click", ".cancelEditingLedger", cancelEditingLedger);
});