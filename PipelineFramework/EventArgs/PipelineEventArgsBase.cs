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
    public abstract class PipelineEventArgsBase : EventArgs
    {
        public PipelineEventArgsBase(string pipelineName)
        {
            PipelineName = pipelineName;
        }

        public string PipelineName { get; set; }
    }
}
