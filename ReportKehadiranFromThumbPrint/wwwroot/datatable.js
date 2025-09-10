

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
