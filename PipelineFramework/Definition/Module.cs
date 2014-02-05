using System;
using System.Collections.Specialized;

namespace Pipeline.Definition
{
    public class Module
    {
        public Module(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public Module(string name, string type, NameValueCollection parameters)
        {
            Name = name;
            Type = type;
            Parameters = parameters;
        }
        
        public string Name { get; set; }
        public string Type { get; set; }

        public NameValueCollection Parameters { get; set; }
    }
}
