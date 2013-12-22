$(document).ready(function () {
    var billPaid = function() {
        var id = $(this).data("id");
        var paidDate = $("#paidDate").val();
        if (!moment(paidDate).isValid()) {
            alert("Please enter a valid paid date");
            return;
        }
        submitbillPaidTransactionToWebService(id, paidDate);
    };

    var submitbillPaidTransactionToWebService = function (id, paidDate) {
        $.ajax({
            type: "POST",
            url: "/Ajax/MarkTransactionBillPaid",
            data: { 'id': id, 'paidDate': paidDate },
            success: function (resp) {
                $(".doPaid[data-id='" + id + "']").replaceWith("<strong>" + paidDate + "</strong>");
            },
            error: function (xhr) {
                alert("there was an error");
            }
        });
    };

    var saveEditedTransaction = function() {
        var id = $(this).data("id");
        var trans = {};
        trans.Id = id;
        trans.Desc = $("[name='Transaction.Desc'][data-id='" + id + "']").val();
        trans.Amount = $("[name='Transaction.Amount'][data-id='" + id + "']").val();
        if ($("[name='Transaction.DateDue'][data-id='" + id + "']").val() !== "")
            trans.DateDue = $("[name='Transaction.DateDue'][data-id='" + id + "']").val();
        else
            trans.DateDue = null;
        if ($("[name='Transaction.DatePayed'][data-id='" + id + "']").val() !== "")
            trans.DatePayed = $("[name='Transaction.DatePayed'][data-id='" + id + "']").val();
        else
            trans.DatePayed = null;
        if ($("[name='Transaction.DateReconciled'][data-id='" + id + "']").val() !== "")
            trans.DateReconciled = $("[name='Transaction.DateReconciled'][data-id='" + id + "']").val();
        else
            trans.DateReconciled = null;
        trans.Account = $("[name='Transaction.Account'][data-id='" + id + "']").val();
        trans.Ledger = $("[name='Transaction.Ledger'][data-id='" + id + "']").val();

        if (validate(trans)) {
            submitUpdatedTransactionToWebService(trans);
        }
    };

    var submitUpdatedTransactionToWebService = function (trans) {
        var row = $("[name='Transaction.Desc'][data-id='" + trans.Id + "']").closest("tr");
        $.ajax({
            type: "POST",
            url: "/Ajax/UpdateTransaction",
            data: trans,
            success: function (resp) {
                row.replaceWith(resp);
            },
            error: function (xhr) {
                alert("there was an error");
            }
        });
    };


    var cancelEditingTransaction = function () {
        var id = $(this).data("id");
        var row = $(this).closest("tr");
        $.ajax({
            type: "GET",
            url: "/Ajax/GetRow",
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
            url: "/Ajax/GetEditRow",
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
            url: "/Ajax/DeleteTransaction",
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
            url: "/Ajax/GetCurrentBalance",
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
            url: "/Ajax/MarkTransactionReconciled",
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
            url: "/Ajax/CreateTransaction",
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
        trans.Desc = $(".newtransaction[name='desc']").val();
        trans.Amount = $(".newtransaction[name='amount']").val();
        if ($(".newtransaction[name='datedue']").val() !== "")
            trans.DateDue = $(".newtransaction[name='datedue']").val();
        else
            trans.DateDue = null;
        if ($(".newtransaction[name='datepayed']").val() !== "")
            trans.DatePayed = $(".newtransaction[name='datepayed']").val();
        else
            trans.DatePayed = null;
        if ($(".newtransaction[name='datereconciled']").val() !== "")
            trans.DateReconciled = $(".newtransaction[name='datereconciled']").val();
        else
            trans.DateReconciled = null;
        trans.Account = $(".newtransaction[name='account']").val();
        trans.Ledger = $(".newtransaction[name='ledger']").val();

        if (validate(trans)) {
            submitNewTransactionToWebService(trans);
        }
    };

    $(document).on("click", "#submitNew", submitNewTransaction);
    $(document).on("click", ".doReconcile", reconcileTransaction);
    $(document).on("click", ".doPaid", billPaid);
    $(document).on("click", ".deleteTransaction", deleteTransaction);
    $(document).on("click", ".editTransaction", editTransaction);
    $(document).on("click", ".saveEditedTransaction", saveEditedTransaction);
    $(document).on("click", ".cancelEditingTransaction", cancelEditingTransaction);
});