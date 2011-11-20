var SqlReportManager = function ($) {
    var srm = {};

    srm.Selected = {};
    srm.Selected.Server = "CORPSPLRPTDB1";
    srm.Selected.DB = "ApplicationLog";
    srm.Selected.Report = "employeeList";
    srm.Selected.WhereClause = "";
    srm.windowHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight || 0;
    srm.windowWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth || 0;
    srm.pageHeight = (srm.windowHeight - 90);


    srm.Init = function(){
    //populate dropdown selects
        $.ajaxSetup({ scriptCharset: "utf-8" , contentType: "application/json; charset=utf-8"});
        $.getJSON("/json/ServerList.json", null, function (data) {
                $("#serverList").fillSelect(data);
            });

         $.getJSON("/json/DBList.json", null, function (data) {
                $("#dbList").fillSelect(data);
            });
         $.getJSON("/Report/List", null, function (data) {
                $("#reportList").fillSelect(data);
            });


        // selection onchange
        $("#dbList").change(function () {
            SqlReportManager.Selected.DB = $("#dbList option:selected").text();
        });

        $("#serverList").change(function () {
            SqlReportManager.Selected.Server = $("#serverList option:selected").text();
        });

        $("#reportList").change(function () {
            SqlReportManager.Selected.Report = $("#reportList option:selected").text();
        });

        $("#filter").click(function () {
            SqlReportManager.Selected.WhereClause = $("#filter").text();
            SqlReportManager.LoadTable();
        });


         $("#pullReport").click(function () {
            $("#loading").fadeIn();
            $("#filter").val("");
            SqlReportManager.LoadTable();
            SqlReportManager.LoadSql();
            $("#tabs").fadeIn();

        });

//        //setup Sql File Uploader
//        var uploader = new qq.FileUploader({
//                element: document.getElementById('fileupload'),
//                action: '/Report/Upload',
//                debug: false,
//                params: {  },
//                allowedExtensions: ['sql'],
//                onSubmit: function (id, fileName) {
//                },
//                onComplete: function (id, fileName, result) {
//                    // pull down table of files now
//                    if (result.toString().indexOf("Error") != -1) {
//                        alert(result);
//                    } else {
//                        $("#successStatus").val(fileName + " Uploaded Successfully");
//                        $("#successStatus").fadeIn().delay(3000).fadeOut();
//                    }
//                }
//            });

        $("#reportTab").height(SqlReportManager.pageHeight);
        $("#sqlTab").height(SqlReportManager.pageHeight);
        $('#datatable-wrapper').width(SqlReportManager.windowWidth-10);
        $('#sqlTextArea').width(SqlReportManager.windowWidth-50).height(SqlReportManager.pageHeight-100);
        $('#datatable-wrapper').append('<table cellpadding="0" cellspacing="0" border="0" class="display" id="reportTable"></table>');

    };
    
    return srm;
} (jQuery);
    
