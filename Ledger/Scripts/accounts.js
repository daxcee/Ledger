$(document).ready(function () {
    var saveEditedAccount = function () {
        var id = $(this).data("id");
        var acct = {};
        acct.Id = id;
        acct.Desc = $("[name='Desc'][data-id='" + id + "']").val();
        acct.Category = $("[name='Category'][data-id='" + id + "']").val();
        acct.Comment = $("[name='Comment'][data-id='" + id + "']").val();

        if (validate(acct)) {
            saveEditedAccountToWebService(acct);
        }
    };

    var saveEditedAccountToWebService = function (acct) {
        var row = $("[name='Desc'][data-id='" + acct.Id + "']").closest("tr");
        $.ajax({
            type: "POST",
            url: "/Ajax/UpdateAccount",
            data: acct,
            success: function (resp) {
                row.replaceWith(resp);
            },
            error: function (xhr) {
                alert("there was an error");
            }
        });
    };

    var cancelEditingAccount = function () {
        var id = $(this).data("id");
        var row = $(this).closest("tr");
        $.ajax({
            type: "GET",
            url: "/Ajax/GetAccountRow",
            data: { 'id': id },
            success: function (resp) {
                row.replaceWith(resp);
            },
            error: function (xhr) {
                alert("there was an error");
            }
        });
    };

    var editAccount = function () {
        var id = $(this).data("id");
        var row = $(this).closest("tr");
        $.ajax({
            type: "GET",
            url: "/Ajax/GetAccountEditRow",
            data: { 'id': id },
            success: function (resp) {
                row.replaceWith(resp);
            },
            error: function (xhr) {
                alert("there was an error");
            }
        });
    };

    var validate = function (acct) {
        if (acct.Desc == "") {
            alert("Must enter a description");
            return false;
        }
        if (acct.Category == "") {
            alert("Must enter a category");
            return false;
        }
        return true;
    };

    var submitNewAccountToWebService = function (acct) {
        $.ajax({
            type: "POST",
            url: "/Ajax/CreateAccount",
            data: acct,
            success: function(resp) {
                window.location.href = document.URL;
            },
            error: function(xhr) {
                alert("there was an error");
            }
        });
    };

    var submitNewAccount = function () {
        var acct = {};
        acct.Desc = $(".newAccount[name='Desc']").val();
        acct.Category = $(".newAccount[name='Category']").val();
        acct.Comment = $(".newAccount[name='Comment']").val();

        if (validate(acct)) {
            submitNewAccountToWebService(acct);
        }
    };

    $(document).on("click", "#submitNewAccount", submitNewAccount);
    $(document).on("click", ".editAccount", editAccount);
    $(document).on("click", ".saveEditedAccount", saveEditedAccount);
    $(document).on("click", ".cancelEditingAccount", cancelEditingAccount);
});