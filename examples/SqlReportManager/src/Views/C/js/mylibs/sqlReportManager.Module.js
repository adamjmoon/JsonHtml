var SqlReportManager = function ($) {
    var srm = {};

    srm.Selected = {};
    //srm.Selected.Server = "CORPSPLRPTDB1";
    //srm.Selected.DB = "ApplicationLog";
    //srm.Selected.Report = "RecentErrors";
    srm.Selected.Server = "CORPSPLRPTDB1";
    srm.Selected.DB = "ApplicationLog";
    srm.Selected.Report = "RecentErrors";
    srm.Selected.WhereClause = "";
    srm.SqlReport = {};
    srm.SqlReport.Sql = "";
    srm.SqlReport.ReportName = "";
    srm.SqlReport.DB = "";
    srm.windowHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight || 0;
    srm.windowWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth || 0;
	
    srm.pageHeight = (srm.windowHeight - 90);		
	
    srm.ParseError = function(error) {
        var idx1 = error.responseText.indexOf("<title>");
        var idx2 = error.responseText.indexOf("</title>");
        var len = idx2 - idx1;
        return error.responseText.substr(idx1 + 7, len - 7);
    };

    srm.LoadSqlReportList = function(){
            $.getJSON("/Report/List", { report: srm.Selected.Report, db: srm.Selected.DB}, function (data) {
            $("#reportList").fillSelect(data);
             SqlReportManager.Selected.Report = $("#reportList option:selected").text();
        });
	return false;
    };

    srm.Init = function() {
        $("#loading").hide();


        // populate dropdowns
        $.getJSON("/json/ServerList.json", null, function (data) {
            $("#serverList").fillSelect(data);
            srm.Selected.Server = $("#serverList option:selected").text();
        });

        $.getJSON("/json/DBList.json", null, function (data) {
            $("#dbList").fillSelect(data);
             srm.Selected.DB = $("#dbList option:selected").text();
        });

        srm.LoadSqlReportList();

        // selection onchange
        $("#dbList").change(function () {
            srm.Selected.DB = $("#dbList option:selected").text();
            srm.LoadSqlReportList();
            return false;
        });

        $("#serverList").change(function () {
            srm.Selected.Server = $("#serverList option:selected").text();
            return false;
        });

        $("#reportList").change(function () {
            srm.Selected.Report = $("#reportList option:selected").text();
            return false;
        });

        $("#filter").click(function () {
            srm.Selected.WhereClause = $("#filter").text();
            srm.LoadTable();
            return false;
        });

        // pull report button
        var pullReport = $("#pullReport").click(function () {
            srm.PullReport();
            return false;
        });

        // save report button
        var saveReport = $("#saveReport").click(function () {
            srm.SaveReport();
        });

        var uploader = new qq.FileUploader({
            element: document.getElementById('fileupload'),
            action: '/Report/Upload',
            debug: false,
            allowedExtensions: ['sql'],
            onComplete: function (id, fileName, result) {
                // pull down table of files now
                if (result.toString().indexOf("Error") != -1) {
                    alert(result);
                } else {
                    srm.LoadSqlReportList();
                    $("#successStatus").val(fileName + " Uploaded Successfully");
                    $("#successStatus").fadeIn().delay(2000).fadeOut();
                }

            }
        });

        // adjust div heights
        $("#reportTab").height(srm.pageHeight);
        $("#sqlTab").height(srm.pageHeight);

        $('#sqlTextArea').width(srm.windowWidth - 20).height(srm.pageHeight - 100);

        //pull initial report
        srm.PullReport();

        return false;
    };

    return srm;
}(jQuery);
    
