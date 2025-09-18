

window.initializeDataTable = (tableId) => {
    setTimeout(() => {
        var $table = $('#' + tableId);

        if ($table.length === 0) {
            console.error("Table not found: " + tableId);
            return;
        }

        if ($.fn.DataTable.isDataTable($table)) {
            $table.DataTable().destroy(true);
        }

        $table.DataTable({
            responsive: true,
            autoWidth: false,
            paging: true,
            searching: true,
            ordering: true,
            destroy: true,
            language: {
                emptyTable: "Tiada rekod untuk dipaparkan",
                search: "Cari:",
                paginate: {
                    first: "Pertama",
                    last: "Akhir",
                    next: "Seterusnya",
                    previous: "Sebelum"
                }
            }
        });
    }, 1000); // bagi lebih masa Blazor render penuh
};

window.initializeDataTableX = function (id) {
    if (!id) return;
    var el = document.getElementById(id);
    if (!el) {
        console.warn("Table element not found:", id);
        return;
    }

    // If you use DataTables plugin:
    if (window.jQuery && $.fn && $.fn.DataTable) {
        var selector = '#' + id;

        // If already initialized, destroy first (prevents "Cannot reinitialise" error)
        try {
            if ($.fn.DataTable.isDataTable(selector)) {
                $(selector).DataTable().destroy(true);
                // optionally remove markup of DataTables wrapper so it re-creates cleanly
                $(selector).find('thead').show();
            }
        } catch (e) {
            console.warn('Error destroying existing DataTable for', id, e);
        }

        // initialize
        $(selector).DataTable({
            paging: true,
            searching: true,
            responsive: true,
            autoWidth: false
        });
        return;
    }

    // Fallback if DataTables not present: add minimal styling or console warning
    console.warn("jQuery/DataTables not present. Please include them if you want full table features.");
};
//window.initializeDataTable = (tableId) => {
//    setTimeout(() => {
//        var table = document.getElementById(tableId);
//        if (!table) {
//            console.error("Table not found!");
//            return;
//        }
//        $(document).ready(function () {
//            $('#' + tableId).DataTable();
//        });
//    }, 500); // Delay to allow Blazor rendering
//};

//window.initializeDataTable = (tableId) => {
//    setTimeout(() => {
//        var table = document.getElementById(tableId);
//        if (!table) {
//            console.error("Table not found: " + tableId);
//            return;
//        }

//        // Check & destroy dulu kalau dah ada DataTable
//        if ($.fn.DataTable.isDataTable('#' + tableId)) {
//            try {
//                $('#' + tableId).DataTable().clear().destroy();
//            } catch (e) {
//                console.warn("Failed to destroy DataTable:", e);
//            }
//        }

//        // Init baru
//        $('#' + tableId).DataTable({
//            responsive: true,
//            autoWidth: false,
//            paging: true,
//            searching: true,
//            ordering: true,
//            destroy: true,
//            language: {
//                "emptyTable": "Tiada rekod untuk dipaparkan",
//                "search": "Cari:",
//                "paginate": {
//                    "first": "Pertama",
//                    "last": "Akhir",
//                    "next": "Seterusnya",
//                    "previous": "Sebelum"
//                }
//            }
//        });
//    }, 500); // Delay untuk bagi Blazor render habis
//};
