using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Pipeline.Definition
{
    public class Modules : List<Module>
    {
        public void Add(string name, string type)
        {
            this.Add(new Module(name, type));
        }

        public void Add(string name, string type, NameValueCollection parameters)
        {
            this.Add(new Module(name, type, parameters));
        }
    }
}
