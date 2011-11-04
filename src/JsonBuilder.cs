using System;
using System.Collections.Generic;
using System.Text;

namespace JsonHtmlTable
{
    public class JsonBuilder
    {
        private readonly Dictionary<Type, Action> typeGuide;
        private readonly Dictionary<Type, String> typeStrGuide;
        public StringBuilder Json;
        private string value = "";

        public JsonBuilder()
        {
            Json = new StringBuilder();
            typeGuide = new Dictionary<Type, Action>
                            {
                                {typeof (Int32), () => AppendNumberValue()},
                                {typeof (string), () => AppendStringValue()},
                                {typeof (DateTime), () => AppendStringValue()},
                                {typeof (Boolean), () => AppendStringValue()}
                            };
            typeStrGuide = new Dictionary<Type, String>
                               {
                                   {typeof (Int32), "number"},
                                   {typeof (String), "string"},
                                   {typeof (DateTime), "Date"},
                                   {typeof (Boolean), "Bool"}
                               };
        }

        public void Append(string str)
        {
            Json.Append(str);
        }

        public void WrapArray(string arrayName)
        {
            Json.Insert(0, !String.IsNullOrEmpty(arrayName) ? WrapInQuotes(arrayName) + ":[" : "[");
            Json.Append("]");
        }

        public void AppendComma()
        {
            Json.Append(",");
        }

        public void AppendStringValue()
        {
            Json.Append(WrapInQuotes(value));
        }

        public void AppendNumberValue()
        {
            Json.Append(value);
        }

        public string GetTypeStr(Type type)
        {
            return typeStrGuide[type];
        }


        public void AppendProperty(string name, string value)
        {
            Json.Append(name != null ? WrapInQuotes(name) + ":" + WrapInQuotes(value) : WrapInQuotes(value));
        }

         public void AppendProperty(string name, Type type, string value)
    {
            this.value = value;
            Json.Append(WrapInQuotes(name) + ":");
            typeGuide[type].Invoke();
        }
        public void AppendProperty(Type type, string value)
        {
            this.value = value;
            if (typeGuide.ContainsKey(type))
                typeGuide[type].Invoke();
            else
            {
                AppendStringValue();
            }
        }

        public void WrapObject(string objectName)
        {
            Json.Insert(0, !String.IsNullOrEmpty(objectName) ? WrapInQuotes(objectName) + ":{" : "{");
            Json.Append("}");
        }

//        public string WrapInQuotes(string str)
//        {
//            if (str != null)
//                return '"' + str + '"';
//            else
//            {
//                return "";
//            }
//        }

        public string WrapInQuotes(string s) 
{
        if (string.IsNullOrEmpty(s)) 
        {
                return "\"\"";
        }
        char         c;
        int          i;
        int          len = s.Length;
        var sb = new StringBuilder(len + 4);
        string       t;

        sb.Append('"');
        for (i = 0; i < len; i += 1) 
        {
                c = s[i];
                if ((c == '\\') || (c == '"') || (c == '>'))
                {
                        sb.Append('\\');
                        sb.Append(c);
                }
                else if (c == '\b')
                        sb.Append("\\b");
                else if (c == '\t')
                        sb.Append("\\t");
                else if (c == '\n')
                        sb.Append("\\n");
                else if (c == '\f')
                        sb.Append("\\f");
                else if (c == '\r')
                        sb.Append("\\r");
                else
                {
                        if (c < ' ') 
                        {
                                //t = "000" + Integer.toHexString(c);
                                string tmp = new string(c,1);
                                t = "000" + int.Parse(tmp,System.Globalization.NumberStyles.HexNumber);
                                sb.Append("\\u" + t.Substring(t.Length - 4));
                        } 
                        else 
                        {
                                sb.Append(c);
                        }
                }
        }
        sb.Append('"');
        return sb.ToString();
}

        public void Clear()
        {
            Json.Clear();
        }
    }
}