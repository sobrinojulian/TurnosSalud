// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function confirmDeleteAll(event) {
    event.preventDefault(); // Prevent the form from submitting immediately
    if (confirm("Are you sure you want to delete all records?")) {
        document.getElementById("deleteAllForm").submit(); // Submit the form
    }
}