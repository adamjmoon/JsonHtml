var SqlReportManager = (function (parent, $) {
    // add capabilities...

    parent.LoadTable = function() {
        $('#datatable-wrapper').html("");
        $('#datatable-wrapper').append('<table cellpadding="0" cellspacing="0" border="0" class="display" id="reportTable"></table>');
        TableTools.DEFAULTS.aButtons = ["copy", "csv", "xls", "print"];

        var request = $.ajax(
            {
                type: "POST",
                url: "/Report/Table",
                data: parent.Selected,
                dataType: "json",

                success: function (json) {
                    json.bProcessing = true;
                    json.bPaginate = true;
                    json.bFilter = true;
                    json.bSort = true;
                    json.bInfo = true;
                    json.bDestroy = true;
                    json.bJQueryUI = true;
                    json.sPaginationType = "full_numbers";
                    json.sDom = 'Tlfrtip';
                    json.oTableTools = { sSwfPath: "/Views/C/js/TableTools-2.0.1/media/swf/copy_cvs_xls.swf" };
                    json.sScrollX = "100%";
                    json.bAutoWidth = true;
                    json.bLengthChange = false;
                    json.bScrollCollapse = true;
                    json.sScrollY = parent.pageHeight - 120 + "px";
                    json.bDeferRender = false;
                    json.sSearch = "Where:";

                    $('#reportTable').dataTable(json);
                    var filterRow = "<tr>";

                    for (i = 0; i < json.aoColumns.length; i++) {
                        filterRow += '<th>' + json.aoColumns[i].sTitle + '</th>';

                    }
                    filterRow += '</tr>';
                    $(filterRow).prependTo('.dataTables_scrollHeadInner table thead');
                    $('#reportTable').dataTable().columnFilter();
                }
            });

        request.fail(function (xmlRequest, message, err) {
            var idx1 = xmlRequest.responseText.indexOf("<title>");
            var idx2 = xmlRequest.responseText.indexOf("</title>");
            var len = idx2 - idx1;
            $("#loading").fadeOut();
            alert("Query failed with Error: " + xmlRequest.responseText.substr(idx1 + 7, len - 7));
        });


    };

    parent.LoadSql = function() {
        $.getJSON("/Report/Sql", { name: parent.Selected.Report }, function (data) {
            $("#sqlTextArea").text(data.sql);
        });

        $("#loading").delay(1000).fadeOut();
    };


    return parent;
}(SqlReportManager || {}, jQuery));