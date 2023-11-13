// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $("#btnAddNewLink").click(function () {
        var count = $("#linkBody >tr").length;
        $("#linkBody").append("<tr><td> <input placeholder='Link URL here' class='form-control' id='Links[" + count + "].Url' name='Links[" + count +"].Url'></td></tr>");
    });
});