namespace JsonHtmlTable
{
    public class SqlReportConfig
    {
        public string ReportTemplateRoot { get; set; }
        public string ReportName { get; set; }
        public string ConnectionString { get; set; }
        public string WhereClause { get; set; }
    }

    public class SqlReport
    {
        public string ReportName { get; set; }
        public string Sql { get; set; }
        public string DB { get; set; }
    }

    public class SqlReportRequest
    {
        public string Report { get; set; }
        public string DB { get; set; }
        public string Server { get; set; }
        public string WhereClause { get; set; }
    }

    public class ConnectionStringConfig
    {
        public string DB { get; set; }
        public string Server { get; set; }
        public string ConnectionString { get; set; }
    }

}