using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using JsonHtmlTable;
using ServiceStack.Text;

namespace SqlReportManager.Controllers
{
    public class ReportController : Controller
    {

        public ContentResult Table(SqlReportRequest sqlReportRequest)
        {
            var sqlReportConfig = BaseReportConfig();

            var connectionStringConfig = ConnectionStrings().Where(cs => cs.DB.ToLower().Equals(sqlReportRequest.DB.ToLower()) && cs.Server.ToLower().Equals(sqlReportRequest.Server.ToLower())).FirstOrDefault();

            if (connectionStringConfig == null)
                throw new Exception("No Valid Connection String Exists for: Server = " + sqlReportRequest.Server + " and DB = " + sqlReportRequest.DB);

            sqlReportConfig.ConnectionString = connectionStringConfig.ConnectionString;
            sqlReportConfig.ReportName = sqlReportRequest.Report;

            var jsonHtmlTableGenerator = new JsonHtmlTableGenerator(sqlReportConfig);
            string jsonHtmlTable = jsonHtmlTableGenerator.GetJsonTable(JsonHtmlTableType.DataTables);
            return new ContentResult {Content = jsonHtmlTable, ContentType = "application/json"};
        }

        public JsonResult List()
        {

            string[] files = Directory.GetFiles(BaseReportConfig().ReportTemplateRoot);

            
            
            List<dynamic> reportList = new List<dynamic>();

            reportList.Add( new
                            {
                                Selected = false,
                                Text = "",
                                Value =""});

            
            var reports = files.Select(fileName => new
                    {
                        Selected = false,
                        Text =
                    fileName.Replace(".sql", "").Replace(
                        BaseReportConfig().ReportTemplateRoot, ""),
                        Value = fileName.Replace(".sql", "")
                    }).Cast<dynamic>().ToList();

            reportList.AddRange(reports);

            return new JsonResult {Data = reportList, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        public JsonResult Upload()
        {
            try
            {
                foreach (string inputTagName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[inputTagName];
                    if (file != null)
                        if (file.ContentLength > 0)
                        {
                            string filePath = Path.Combine(HttpContext.Server.MapPath("../sql")
// ReSharper disable AssignNullToNotNullAttribute
                                                           , Path.GetFileName(file.FileName));
// ReSharper restore AssignNullToNotNullAttribute
                            file.SaveAs(filePath);
                        }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult {Data = new {error = ex.Message}};
            }

            return new JsonResult {Data = new {success = true}};
        }

        public JsonResult Save(SqlReport sqlReport)
        {
            string path = BaseReportConfig().ReportTemplateRoot + sqlReport.ReportName + ".sql";

            try
            {
                // Delete the file if it exists.
                if (System.IO.File.Exists(path))
                {
                    // Note that no lock is put on the
                    // file and the possibility exists
                    // that another process could do
                    // something with it between
                    // the calls to Exists and Delete.
                    System.IO.File.Delete(path);
                }

                // Create the file.
                using (FileStream fs = System.IO.File.Create(path))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(sqlReport.Sql);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                return new JsonResult {Data = new {success = false, error = ex.Message}};
            }

            return new JsonResult {Data = new {success = true}};
        }

        public JsonResult Sql(string name)
        {
            string path = BaseReportConfig().ReportTemplateRoot + name + ".sql";
            var sqlQuery = new StringBuilder();
            using (StreamReader sr = System.IO.File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    sqlQuery.AppendLine(s);
                }
            }

            return new JsonResult
                       {Data = new {sql = sqlQuery.ToString()}, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        private SqlReportConfig BaseReportConfig()
        {
            const string cachedConfigKey = "BaseReportConfig";
            var reportConfig = System.Web.HttpContext.Current.Cache.Get(cachedConfigKey) as SqlReportConfig;
            if (reportConfig == null)
            {
                reportConfig = new SqlReportConfig {ReportTemplateRoot = HttpContext.Server.MapPath("../sql") + "\\"};
                System.Web.HttpContext.Current.Cache.Insert(cachedConfigKey, reportConfig);
            }

            return reportConfig;
        }


        private List<ConnectionStringConfig> ConnectionStrings()
        {
//            var connectionStrings = System.Web.HttpContext.Current.Cache.Get("ConnectionStrings") as List<ConnectionStringConfig>;
//            if (connectionStrings == null)
//            {
                var serializer = new JsonSerializer<List<ConnectionStringConfig>>();
               var connectionStrings = serializer.DeserializeFromReader(new StreamReader(BaseReportConfig().ReportTemplateRoot.Replace("sql","json") + "ConnectionStrings.json"));

                System.Web.HttpContext.Current.Cache.Insert("ConnectionStrings", connectionStrings);
//            }

            return connectionStrings;
        }
    }
}