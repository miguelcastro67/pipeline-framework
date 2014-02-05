using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pipeline
{
    public abstract class PipelineModule<T> : IPipelineModule where T : PipelineEvents
    {
        void IPipelineModule.Initialize(PipelineEvents events, NameValueCollection parameters)
        {
            Initialize((T)events, parameters);
        }

        public abstract void Initialize(T events, NameValueCollection parameters);
    }
}
