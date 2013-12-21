$(document).ready(function () {
    var saveEditedTransaction = function() {

    };

    var cancelEditingTransaction = function () {
        console.log("cancel!");
        var id = $(this).data("id");
        var row = $(this).closest("tr");
        $.ajax({
            type: "GET",
            url: "/Home/GetRow",
            data: { 'id': id },
            success: function (resp) {
                row.replaceWith(resp);
            },
            error: function (xhr) {
                alert("there was an error");
            }
        });
    };

    var editTransaction = function () {
        var id = $(this).data("id");
        var row = $(this).closest("tr");
        $.ajax({
            type: "GET",
            url: "/Home/GetEditRow",
            data: { 'id': id },
            success: function (resp) {
                row.replaceWith(resp);
            },
            error: function (xhr) {
                alert("there was an error");
            }
        });
    };

    var deleteTransaction = function() {
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "/Home/DeleteTransaction",
            data: { 'id': id },
            success: function (resp) {
                window.location.href = document.URL;
            },
            error: function (xhr) {
                alert("there was an error");
            }
        });
    };

    var updateCurrentBalance = function (ledger) {
        $.ajax({
            type: "GET",
            url: "/Home/GetCurrentBalance",
            data: { 'ledger' : ledger },
            success: function (resp) {
                $("#currentBalance").replaceWith('<span id="currentBalance">' + resp.CurrentBalance + "</span>");
            },
            error: function (xhr) {
                alert("there was an error");
            }
        });
    };

    var submitReconcileTransactionToWebService = function (id, reconcileDate) {
        $.ajax({
            type: "POST",
            url: "/Home/MarkTransactionReconciled",
            data: { 'id' : id, 'reconcileDate' : reconcileDate },
            success: function (resp) {
                var ledger = $("#ledgerId").val();
                $(".doReconcile[data-id='" + id + "']").replaceWith("<strong>" + reconcileDate + "</strong>");
                updateCurrentBalance(ledger);
            },
            error: function (xhr) {
                alert("there was an error");
            }
        });
    };

    var reconcileTransaction = function () {
        var id = $(this).data("id");
        var reconcileDate = $("#reconcileDate").val();
        if (!moment(reconcileDate).isValid()) {
            alert("Please enter a valid reconcile date");
            return;
        }
        submitReconcileTransactionToWebService(id, reconcileDate);
    };
    
    var validate = function (trans) {
        if (trans.Desc == "") {
            alert("Must enter a description");
            return false;
        }
        if (trans.Amount == "") {
            alert("Must enter an amount");
            return false;
        }
        if (trans.DateDue != null && !moment(trans.DateDue).isValid()) {
            alert("Must enter a valid date due");
            return false;
        }
        if (trans.DatePayed != null && !moment(trans.DatePayed).isValid()) {
            alert("Must enter a valid date payed");
            return false;
        }
        if (trans.DateReconciled != null && !moment(trans.DateReconciled).isValid()) {
            alert("Must enter a valid date reconciled");
            return false;
        }
        if (trans.DateDue == null && trans.DatePayed == null) {
            alert("Must enter a date due or a date payed");
            return false;
        }
        if (trans.Account == "") {
            alert("Must select an account");
            return false;
        }
        if (trans.Ledger == "") {
            alert("Must select a ledger");
            return false;
        }
        return true;
    };

    var submitNewTransactionToWebService = function (trans) {
        $.ajax({
            type: "POST",
            url: "/Home/CreateTransaction",
            data: trans,
            success: function(resp) {
                window.location.href = document.URL;
            },
            error: function(xhr) {
                alert("there was an error");
            }
        });
    };

    var submitNewTransaction = function() {
        var trans = {};
        trans.Desc = $("#desc").val();
        trans.Amount = $("#amount").val();
        if ($("#datedue").val() !== "")
            trans.DateDue = $("#datedue").val();
        else
            trans.DateDue = null;
        if ($("#datepayed").val() !== "")
            trans.DatePayed = $("#datepayed").val();
        else
            trans.DatePayed = null;
        if ($("#datereconciled").val() !== "")
            trans.DateReconciled = $("#datereconciled").val();
        else
            trans.DateReconciled = null;
        trans.Account = $("#account").val();
        trans.Ledger = $("#ledger").val();

        if (validate(trans)) {
            submitNewTransactionToWebService(trans);
        }
    };

    $(document).on("click", "#submitNew", submitNewTransaction);
    $(document).on("click", ".doReconcile", reconcileTransaction);
    $(document).on("click", ".deleteTransaction", deleteTransaction);
    $(document).on("click", ".editTransaction", editTransaction);
    $(document).on("click", ".saveEditedTransaction", saveEditedTransaction);
    $(document).on("click", ".cancelEditingTransaction", cancelEditingTransaction);
});