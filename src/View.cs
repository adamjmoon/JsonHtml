using System;

namespace JsonHtml
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class View : Attribute
    {
        public string Name;
        public bool Display;
    }
}