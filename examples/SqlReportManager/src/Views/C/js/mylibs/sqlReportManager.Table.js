var SqlReportManager = (function (parent, $) {
    // add capabilities...

    parent.LoadTable = function() {
        $('#datatable-wrapper').html("");
        $('#datatable-wrapper').append('<table cellpadding="0" cellspacing="0" border="0" class="display" style="width: ' + parent.windowWidth + 'px;" id="reportTable"></table>');
        TableTools.DEFAULTS.aButtons = ["copy", "csv", "xls", "print"];

        var request = $.ajax(
            {
                type: "POST",
                url: "/Report/Table",
                data: parent.Selected,
                dataType: "json",

                success: function (json) {

                    if (json.aoColumns.length == 0) {
                        alert('Query Returned 0 Results');
                        $("#loading").hide();
                    } else {
                        json.bPaginate = true;
                        json.asSorting = 'desc';
                        json.bRetrieve = true;
                        json.bFilter = true;
                        json.bSort = true;
                        json.bInfo = true;
                        json.bDestroy = true;
                        json.bJQueryUI = true;
                        json.sPaginationType = "full_numbers";
                        json.aLengthMenu = [
                            [5, 10, 25, 50, -1],
                            [5, 10, 25, 50, "All"]
                        ]
                        json.sDom = 'T<"clear">lrtip';
                        json.oTableTools = { sSwfPath: "/Views/C/js/TableTools-2.0.1/media/swf/copy_cvs_xls.swf" };
                        json.sScrollX = "100%";
                        json.bAutoWidth = true;
                        json.bLengthChange = true;
                        json.bScrollCollapse = true;
                        json.sScrollY = parent.pageHeight - 150 + "px";
                        json.bDeferRender = true;
                        json.sSearch = "Where:";

                        $('#reportTable').dataTable(json);
                        var filterRow = "<tr>";

                        for (i = 0; i < json.aoColumns.length; i++) {
                            filterRow += '<th>' + json.aoColumns[i].sTitle + '</th>';

                        }
                        filterRow += '</tr>';
                        $(filterRow).prependTo('.dataTables_scrollHeadInner table thead');
                        $('#reportTable').dataTable().columnFilter();
                        $("#loading").delay(500).fadeOut();
                    }
                }
            });

        request.fail(function (xmlRequest, message, err) {

            $("#loading").fadeOut();
            alert("Query failed with Error: " + parent.ParseError(xmlRequest));
        });

        return false;
    };

    parent.LoadSql = function() {
        $.getJSON("/Report/Sql", { name:  parent.Selected.DB + "." + parent.Selected.Report }, function (data) {
            parent.SqlReport.Sql = data.sql;
            parent.SqlReport.ReportName = SqlReportManager.Selected.Report;
            $("#sqlTextArea").text(data.sql);
            $("#reportName").val(parent.SqlReport.ReportName);
        });

        return false;
    };

    parent.PullReport = function() {

        $('a[href*="#reportTab"]').click();
        $("#loading").fadeIn();
        $("#filter").val("");
        SqlReportManager.LoadTable();
        SqlReportManager.LoadSql();
        $("#tabs").delay(100).fadeIn();

        return false;
    };

    parent.SaveReport = function() {

        parent.SqlReport.Sql = $("#sqlTextArea").val();
        parent.SqlReport.ReportName = $("#reportName").val();
        parent.SqlReport.DB = parent.Selected.DB;
        var request = $.ajax(
            {
                type: "POST",
                url: "/Report/Save",
                data: parent.SqlReport,
                dataType: "json",

                success: function (json) {
                    parent.Selected.Report = SqlReportManager.SqlReport.ReportName;

                    SqlReportManager.LoadSqlReportList();
                    SqlReportManager.PullReport();
                }
            });

        request.fail(function (xmlRequest, message, err) {

            $("#loading").fadeOut();
            alert("Query failed with Error: " + parent.ParseError(xmlRequest));
        });


    };
    return parent;
}(SqlReportManager || {}, jQuery));