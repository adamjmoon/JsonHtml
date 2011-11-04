using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace JsonHtmlTable.Strategies
{
    public class DataTablesStrategy : JsonTableStrategy
    {
        //        {
        //        "aaData": [
        //            /* Reduced data set */
        //            [ "Trident", "Internet Explorer 4.0", "Win 95+", 4, "X" ],
        //            [ "Trident", "Internet Explorer 5.0", "Win 95+", 5, "C" ],
        //            [ "Trident", "Internet Explorer 5.5", "Win 95+", 5.5, "A" ],
        //            [ "Trident", "Internet Explorer 6.0", "Win 98+", 6, "A" ],
        //            [ "Trident", "Internet Explorer 7.0", "Win XP SP2+", 7, "A" ],
        //            [ "Gecko", "Firefox 1.5", "Win 98+ / OSX.2+", 1.8, "A" ],
        //            [ "Gecko", "Firefox 2", "Win 98+ / OSX.2+", 1.8, "A" ],
        //            [ "Gecko", "Firefox 3", "Win 2k+ / OSX.3+", 1.9, "A" ],
        //            [ "Webkit", "Safari 1.2", "OSX.3", 125.5, "A" ],
        //            [ "Webkit", "Safari 1.3", "OSX.3", 312.8, "A" ],
        //            [ "Webkit", "Safari 2.0", "OSX.4+", 419.3, "A" ],
        //            [ "Webkit", "Safari 3.0", "OSX.4+", 522.1, "A" ]
        //        ],
        //        "aoColumns": [
        //            { "sTitle": "Engine" },
        //            { "sTitle": "Browser" },
        //            { "sTitle": "Platform" },
        //            { "sTitle": "Version", "sClass": "center" },
        //            {
        //                "sTitle": "Grade",
        //                "sClass": "center",
        //                "fnRender": function(obj) {
        //                    var sReturn = obj.aData[ obj.iDataColumn ];
        //                    if ( sReturn == "A" ) {
        //                        sReturn = "<b>A</b>";
        //                    }
        //                    return sReturn;
        //                }
        //            }
        //        ]
        //    } 

        public DataTablesStrategy(SqlReportConfig config)
            : base(config)
        {
        }

        protected override void GenerateTable(SqlDataReader reader)
        {
            var dataTable = new JsonBuilder();
            var cols = new JsonBuilder();
            var rows = new JsonBuilder();

            int rowId = 0;
            object value;
            string colName;
            Type type;
            var row = new JsonBuilder();
            var cell = new JsonBuilder();
            var col = new JsonBuilder();

            bool canRead = reader.Read();
            if (canRead)
                do
                {
                    row.Clear();
                    // increment row
                    rowId++;

                    for (int index = 0; index < reader.FieldCount; index++)
                    {
                        cell.Clear();
                        type = reader.GetFieldType(index);
                        value = reader.GetValue(index);
                        colName = reader.GetName(index);

                        // build list of columns with just the columns from the first row
                        if (rowId == 1)
                        {
                            col.Clear();

                            // add column details to cols
                            //{id: 'task', label: 'Task', type: 'string'},

                            col.AppendProperty("sTitle", colName);
                            col.WrapObject(null);


                            cols.Append(col.Json.ToString());
                        }

                        if (value is DBNull)
                        {
                            cell.AppendProperty(typeof(string), "");
                        }
                        else
                        {
                            cell.AppendProperty(type, value.ToString());
                        }


                        // add current field value as row property

                        // add comma if this is the last field in the result
                        if (index < reader.FieldCount - 1)
                        {
                            if (rowId == 1)
                            {
                                cols.AppendComma();
                            }
                            cell.AppendComma();
                        }

                        row.Append(cell.Json.ToString());
                    }

                    row.WrapArray(null);
                    rows.Append(row.Json.ToString());

                    // read next field
                    canRead = reader.Read();

                    // if last field don't append just closing brace else append closing brace and comma
                    if (canRead)
                        rows.AppendComma();
                } while (canRead);
            // close reader
            reader.Close();

            cols.WrapArray("aoColumns");
            rows.WrapArray("aaData");
            dataTable.Append(rows.Json.ToString());
            dataTable.AppendComma();
            dataTable.Append(cols.Json.ToString());
            dataTable.WrapObject(null);

            this.JsonTableString = dataTable.Json.ToString();
        }

        protected override void GenerateDynamicTable(SqlDataReader reader)
        {
            dynamic dataTable = new { 
                                          cols = new List<object>(),
                                          rows = new List<object>()
                                      };

            var rowId = 0; object value; string colName; Type type;
            dynamic row; dynamic col; dynamic cell;

            var canRead = reader.Read();
            if (canRead)
                do
                {
                    row = new {c = new List<object>()};
                    // increment row
                    rowId++;

                    for (int index = 0; index < reader.FieldCount; index++)
                    {
                        type = reader.GetFieldType(index);
                        value = reader.GetValue(index);
                        colName = reader.GetName(index);

                        // build list of columns with just the columns from the first row
                        if (rowId == 1)
                        {
                            col = new
                                      {
                                          id = colName,
                                          label = colName,
                                          type = GetTypeStr(type)
                                      };
                            dataTable.cols.Add(col);
                        }

                        dynamic val;
                        if (value is DBNull)
                            val = "";
                        else if (value is DateTime)
                            val = value.ToString();
                        else
                            val = value;

                        cell = new {v = val};

                        row.c.Add(cell);
                    }

                    dataTable.rows.Add(row);

                    // read next field
                    canRead = reader.Read();
                } while (canRead);

            // close reader
            reader.Close();

            this.JsonTableDynamic = dataTable;
        }

        public override void GenerateTable<T>(IList<T> entities)
        {
            throw new NotImplementedException();
        }

        public override void GenerateDynamicTable<T>(IList<T> entities)
        {
            dynamic dataTable = new
                                      {
                                          cols = new List<object>(),
                                          rows = new List<object>()
                                      };

            var rowId = 0; object value; string colName; Type type;
            dynamic row; dynamic col; dynamic cell;

           foreach(var entity in entities)
            {
                row = new {c = new List<object>()};
                // increment row
                rowId++;

                var propertyInfos = entity.GetType().GetProperties(BindingFlags.Public |
                                                                   BindingFlags.Static);
                
                foreach(var propertyInfo in propertyInfos)
                {
                    
                    type = propertyInfo.PropertyType;
                    value = propertyInfo.GetValue(null,null);
                    colName = propertyInfo.Name;

                    if (rowId == 1)
                    {
                        col = new
                                  {
                                      id = colName,
                                      label = colName,
                                      type = GetTypeStr(type)
                                  };
                        dataTable.cols.Add(col);
                    }

                    dynamic val;
                    if (value == null)
                        val = "";
                    else if (value is DateTime)
                        val = value.ToString();
                    else
                        val = value;

                    cell = new { v = val };

                    row.c.Add(cell);
                }

                dataTable.rows.Add(row);
            }

            this.JsonTableDynamic = dataTable;
        }
    }
}