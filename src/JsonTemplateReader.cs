using System.IO;
using System.Text;
using ServiceStack.Text;

namespace JsonHtml
{
    public static class JsonConfigReader<T>
    {
        public static T GetConfig(string templatePath)
        {
            var jsonTemplate = new StreamReader(templatePath);
            var jsonSerializer = new JsonSerializer<T>();
            var config = jsonSerializer.DeserializeFromReader(jsonTemplate);

            return config;
        }

        public static StringBuilder GetJson(string templatePath)
        {
            var json = new StringBuilder();

            //read template line by line and append to StringBuilder
            using (var rwOpenTemplate = new StreamReader(templatePath))
            {
                while (!rwOpenTemplate.EndOfStream)
                {
                    var line = rwOpenTemplate.ReadLine();
                    json.Append(line);
                }
            }

            return json;
        }
    }
}