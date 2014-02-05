using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pipeline
{
    public delegate void PipelineContext<T>(T context) where T : PipelineContext;
}
