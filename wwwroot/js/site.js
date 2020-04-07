// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const endpoint="https://maps.googleapis.com/maps/api/place/queryautocomplete/json?key=AIzaSyAa_05unLMf5OQPJyPUwx8eo7rx01Njqsg&input="

$("#querybox").keyup(event => {
    console.log(event.target.value);
    $.get()
})