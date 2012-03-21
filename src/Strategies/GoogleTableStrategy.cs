using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace JsonHtml.Strategies
{
    public class GoogleTableStrategy : JsonTableStrategy
    {
        //                FORMAT OF GOOGLE JSON TABLE 
        //                cols: [
        //                    {id: 'task', label: 'Task', type: 'string'},
        //                    {id: 'hours', label: 'Hours per Day', type: 'number'}
        //                ],
        //                rows: [
        //                    {c:[
        //                        {v: 'Work', p: {'style': 'border: 7px solid orange;'}},
        //                        {v: 11}
        //                    ]},
        //                    {c:[
        //                        {v: 'Eat'},
        //                        {v: 2}
        //                    ]},
        //                    {c:[
        //                        {v: 'Commute'},
        //                        {v: 2, f: '2.000'}
        //                    ]}
        //                ]};

        public GoogleTableStrategy(SqlReportConfig config) : base(config)
        {
        }

        protected override void GenerateTable(SqlDataReader reader)
        {
            var googleTable = new JsonBuilder();
            var cols = new JsonBuilder();
            var rows = new JsonBuilder();

            int rowId = 0;
            object value;
            string colName;
            Type type;
            var row = new JsonBuilder();
            var rowCol = new JsonBuilder();
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
                        rowCol.Clear();
                        type = reader.GetFieldType(index);
                        value = reader.GetValue(index);
                        colName = reader.GetName(index);

                        // build list of columns with just the columns from the first row
                        if (rowId == 1)
                        {
                            col.Clear();

                            // add column details to cols
                            //{id: 'task', label: 'Task', type: 'string'},

                            col.AppendProperty("id", colName);
                            col.AppendComma();
                            col.AppendProperty("label", colName);
                            col.AppendComma();
                            col.AppendProperty("type", GetTypeStr(type));
                            col.WrapObject(null);


                            cols.Append(col.Json.ToString());
                        }

                        if (value is DBNull)
                        {
                            rowCol.AppendProperty("v", "");
                        }
                        else
                        {
                            rowCol.AppendProperty("v", type, value.ToString());
                        }

                        rowCol.WrapObject(null);


                        // add current field value as row property

                        // add comma if this is the last field in the result
                        if (index < reader.FieldCount - 1)
                        {
                            if (rowId == 1)
                            {
                                cols.AppendComma();
                            }
                            rowCol.AppendComma();
                        }

                        row.Append(rowCol.Json.ToString());
                    }

                    row.WrapArray("c");
                    row.WrapObject(null);
                    rows.Append(row.Json.ToString());

                    // read next field
                    canRead = reader.Read();

                    // if last field don't append just closing brace else append closing brace and comma
                    if (canRead)
                        rows.AppendComma();
                } while (canRead);
            // close reader
            reader.Close();

            cols.WrapArray("cols");
            rows.WrapArray("rows");
            googleTable.Append(cols.Json.ToString());
            googleTable.AppendComma();
            googleTable.Append(rows.Json.ToString());
            googleTable.WrapObject(null);

            this.JsonTableString = googleTable.Json.ToString();
        }

        protected override void GenerateDynamicTable(SqlDataReader reader)
        {
            dynamic googleTable = new
                                      {
                                          cols = new List<object>(),
                                          rows = new List<object>()
                                      };

            int rowId = 0;
            object value;
            string colName;
            Type type;
            dynamic row;
            dynamic col;
            dynamic cell;

            bool canRead = reader.Read();
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
                            googleTable.cols.Add(col);
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

                    googleTable.rows.Add(row);

                    // read next field
                    canRead = reader.Read();
                } while (canRead);

            // close reader
            reader.Close();

            this.JsonTableDynamic = googleTable;
        }

        public override void GenerateTable<T>(IList<T> entities)
        {
            throw new NotImplementedException();
        }

        public override void GenerateDynamicTable<T>(IList<T> entities)
        {
            dynamic googleTable = new
                                      {
                                          cols = new List<object>(),
                                          rows = new List<object>()
                                      };

            int rowId = 0;
            object value;
            string colName;
            Type type;
            dynamic row;
            dynamic col;
            dynamic cell;

            foreach (T entity in entities)
            {
                row = new {c = new List<object>()};
                // increment row
                rowId++;

                PropertyInfo[] propertyInfos = entity.GetType().GetProperties();
                foreach (var propertyInfo in propertyInfos)
                {
                    type = propertyInfo.PropertyType;
                    value = propertyInfo.GetValue(entity, null);
                    
                    colName = propertyInfo.Name;

                    if (rowId == 1)
                    {
                        col = new
                                  {
                                      id = colName,
                                      label = colName,
                                      type = GetTypeStr(type)
                                  };
                        googleTable.cols.Add(col);
                    }

                    dynamic val;
                    if (value == null)
                        val = "";
                    else if (value is DateTime)
                        val = value.ToString();
                    else
                        val = value;

                    cell = new {v = val};

                    row.c.Add(cell);
                }

                googleTable.rows.Add(row);
            }

            this.JsonTableDynamic = googleTable;
        }
    }
}