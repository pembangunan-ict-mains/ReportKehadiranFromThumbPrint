//window.initializeDataTable = (tableId) => {
//    //jQuery.noConflict();
//    jQuery(function ($) {
//        console.log("Initializing DataTable for: " + tableId);
//        $('#' + tableId).DataTable(
//        );
//    });
//};

window.initializeDataTable = (tableId) => {
    setTimeout(() => {
        var table = document.getElementById(tableId);
        if (!table) {
            console.error("Table not found!");
            return;
        }
        $(document).ready(function () {
            $('#' + tableId).DataTable();
        });
    }, 500); // Delay to allow Blazor rendering
};
