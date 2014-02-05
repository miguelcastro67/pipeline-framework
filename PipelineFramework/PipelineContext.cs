using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pipeline
{
    public abstract class PipelineContext
    {
        public bool Cancel { get; set; }
    }
}
