window.initializeDataTablesForAll = () => {
    jQuery.noConflict
    jQuery(function ($) {
        console.log("Initializing DataTable 2 for: ");

        $('.datatable').DataTable({
            pagingType: "full_numbers", // Use default pagination style
            paging: true,
            searching: true,
            ordering: true
        });

        $('.datatable').each(function () {
            var datatable = $(this);

            // Add a placeholder to the search input field
            var search_input = datatable.closest('.dataTables_wrapper').find('div[id$=_filter] input');
            search_input.attr('placeholder', 'Search');
            search_input.addClass('form-control input-sm');

            // Modify length selector
            var length_sel = datatable.closest('.dataTables_wrapper').find('div[id$=_length] select');
            length_sel.addClass('form-control input-sm');
        });
    });
};