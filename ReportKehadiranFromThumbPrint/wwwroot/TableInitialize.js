
//window.initializeDataTableX = (tableId) => {
//    const table = document.getElementById(tableId);

//    if (table && !$.fn.DataTable.isDataTable(table)) {
//        $(table).DataTable({
//            responsive: true,
//            pageLength: 10,
//            lengthChange: false,
//            ordering: true,
//            searching: true
//        });
//    }
//};
window.initializeDataTablesX = () => {
    const tables = document.querySelectorAll(".my-datatable");

    tables.forEach(function (table) {
        new DataTable(table, {
            paging: true,
            searching: true,
            ordering: true,
            responsive: true,
            language: {
                url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/ms-MY.json"
            }
        });
    });
};