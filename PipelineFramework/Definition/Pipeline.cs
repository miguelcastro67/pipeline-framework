using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pipeline.Definition
{
    public class Pipeline
    {
        public Pipeline(string name)
        {
            Name = name;
        }

        Modules _Modules = new Modules();

        public string Name { get; set; }
        public bool InvokeAll { get; set; }

        public Modules Modules
        {
            get { return _Modules; }
        }
    }
}
