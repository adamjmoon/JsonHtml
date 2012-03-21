using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using JsonHtml;
using ServiceStack.Text;

namespace SqlReportManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Sql Report";

            return View();
        }

        public ContentResult Report()
        {
            var s = new JsonSerializer<List<ConnectionStringConfig>>();
            
            var x = s.DeserializeFromReader(new StreamReader(HttpContext.Server.MapPath("../json") +  "\\" + "ConnectionStrings.json"));
            var reportConfig = new SqlReportConfig { ReportTemplateRoot = HttpContext.Server.MapPath("../sql") + "\\", ReportName = "employeeList", ConnectionString = "Data Source=CORPSPLRPTDB1;Initial Catalog=ApplicationLog;Persist Security Info=True;Trusted_Connection=True;" };
            var jsonHtmlTableGenerator = new JsonHtmlTableGenerator(reportConfig);
            var jsonHtmlTable = jsonHtmlTableGenerator.GetJsonTable(JsonHtmlTableType.DataTables);
            
            return new ContentResult { Content = jsonHtmlTable, ContentType = "application/json" };
        }

    }
}
