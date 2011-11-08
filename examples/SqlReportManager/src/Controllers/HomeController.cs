using System.Configuration;
using System.Web.Mvc;
using JsonHtmlTable;

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
            var reportConfig =
                JsonConfigReader<SqlReportConfig>.GetConfig(ConfigurationManager.AppSettings["SqlReportConfigPath"]);
            var jsonHtmlTableGenerator = new JsonHtmlTableGenerator(reportConfig);
            var jsonHtmlTable = jsonHtmlTableGenerator.GetTable(JsonHtmlTableType.DataTables);
            return new ContentResult { Content = jsonHtmlTable, ContentType = "application/json" };
        }

    }
}
