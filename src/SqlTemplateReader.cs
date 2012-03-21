using System.IO;
using System.Text;

namespace JsonHtml
{
    public class SqlTemplateReader
    {
        public StringBuilder GetSql(string templatePath)
        {

            var sql = new StringBuilder();

            //read template line by line and append to StringBuilder
            using (var rwOpenTemplate = new StreamReader(templatePath))
            {
                while (!rwOpenTemplate.EndOfStream)
                {
                    var line = rwOpenTemplate.ReadLine();
                    sql.Append(line);
                    sql.AppendLine();
                }
            }
         
            return sql;
        }
    }
}