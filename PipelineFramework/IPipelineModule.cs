using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pipeline
{
    public interface IPipelineModule
    {
        void Initialize(PipelineEvents events, NameValueCollection parameters);
    }
}