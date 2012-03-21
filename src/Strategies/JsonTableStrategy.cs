using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace JsonHtml.Strategies
{
    public abstract class JsonTableStrategy
    {
        private SqlReportConfig config;
        protected SqlDataReader reader;
        public dynamic JsonTableDynamic { get; protected set; }
        public string JsonTableString { get; protected set; }

        protected abstract void GenerateTable(SqlDataReader reader);
        protected abstract void GenerateDynamicTable(SqlDataReader reader);
        public abstract void GenerateTable<T>(IList<T> entities);
        public abstract void GenerateDynamicTable<T>(IList<T> entities);

        protected JsonTableStrategy(SqlReportConfig config)
        {
            this.config = config;
        }

        protected string GetTypeStr(Type type)
        {
            var typeStrGuide = new Dictionary<Type, String>
                               {
                                   {typeof (Int32), "number"},
                                   {typeof (String), "string"},
                                   {typeof (DateTime), "string"},
                                   {typeof (Boolean), "boolean"}
                               };

            if (typeStrGuide.ContainsKey(type))
                return typeStrGuide[type];
            else
                return "string";

        }

        public dynamic GetJsonTableDynamic()
        {
            ExecuteSql(this.GenerateDynamicTable);
            return this.JsonTableDynamic;
        }

        public string GetJsonTableString()
        {
            ExecuteSql(this.GenerateTable);
            return this.JsonTableString;
        }

        private void ExecuteSql(Action<SqlDataReader> generateTableAction)
        {
            var sqlTemplateReader = new SqlTemplateReader();
            var templatePath = config.ReportTemplateRoot + config.ReportName + ".sql";
            string sql = sqlTemplateReader.GetSql(templatePath).ToString();
            if (!String.IsNullOrEmpty(config.WhereClause))
            {
            }

            using (var connection = new SqlConnection(config.ConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.Text;

                    using (reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        //return this.GenerateTable(reader);
                        generateTableAction.Invoke(reader);
                    }
                }
            }
        }

    }
}