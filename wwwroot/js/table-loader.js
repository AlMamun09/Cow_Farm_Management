function loadTable(tableId, dataUrl, columnsConfig) {
    // This function now directly initializes DataTables and tells it
    // where to fetch its data from.
    $('#' + tableId).DataTable({
        "ajax": {
            "url": dataUrl,
            "type": "GET",
            "dataType": "json",
            "dataSrc": "" // Use the root of the JSON array as the data source
        },
        "columns": columnsConfig,
        // Optional: Add more DataTables configuration here for a better look and feel
        "responsive": true,
        "autoWidth": false,
        "language": {
            "emptyTable": "No data available in table",
            "zeroRecords": "No matching records found"
        }
    });
}