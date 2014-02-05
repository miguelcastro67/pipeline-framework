using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using Pipeline.Configuration;

namespace Pipeline
{
    public abstract class PipelineCancelEventArgsBase : PipelineEventArgsBase
    {
        public PipelineCancelEventArgsBase(string pipelineName)
            : base(pipelineName)
        {
        }

        public bool Cancel { get; set; }
    }
}
