using System;
using System.Collections.Generic;
using JsonHtmlTable.Strategies;

namespace JsonHtmlTable
{
    public class JsonHtmlTableGenerator
    {
        private Dictionary<JsonHtmlTableType, Action> jsonTableStrategyMap;
        private readonly SqlReportConfig sqlReportConfig;
        private JsonTableStrategy jsonTableStrategy;

        public JsonHtmlTableGenerator(SqlReportConfig sqlReportConfig)
        {
            this.sqlReportConfig = sqlReportConfig;

            MapJsonTableStrategies();
        }

        public JsonHtmlTableGenerator()
        {
            MapJsonTableStrategies();
        }

        private void MapJsonTableStrategies()
        {
            jsonTableStrategyMap = new Dictionary<JsonHtmlTableType, Action>();
            jsonTableStrategyMap.Add(JsonHtmlTableType.Google, () => SetJsonTableStrategy(new GoogleTableStrategy(this.sqlReportConfig)));
            jsonTableStrategyMap.Add(JsonHtmlTableType.DataTables, () => SetJsonTableStrategy(new DataTablesStrategy(this.sqlReportConfig)));
        }

        public void SetJsonTableStrategy(JsonTableStrategy jsonTableStrategy)
        {

            this.jsonTableStrategy = jsonTableStrategy;
        }

        public string GetJsonTable(JsonHtmlTableType type)
        {
            jsonTableStrategyMap[type].Invoke();
            return jsonTableStrategy.GetJsonTableString();
        }

        public dynamic GetDynamicTable(JsonHtmlTableType type)
        {
            jsonTableStrategyMap[type].Invoke();
            return jsonTableStrategy.GetJsonTableDynamic();
        }

        public dynamic GetDynamicTable<T>(JsonHtmlTableType type, IList<T> entities)
        {
            jsonTableStrategyMap[type].Invoke();
            jsonTableStrategy.GenerateDynamicTable(entities);
            return jsonTableStrategy.JsonTableDynamic;
        }

        public string GetJsonTable<T>(JsonHtmlTableType type, IList<T> entities)
        {
            jsonTableStrategyMap[type].Invoke();
            jsonTableStrategy.GenerateTable(entities);
            return jsonTableStrategy.JsonTableString;
        }
    }
}