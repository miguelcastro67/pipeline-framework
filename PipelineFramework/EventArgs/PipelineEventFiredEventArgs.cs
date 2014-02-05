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
    public class PipelineEventFiredEventArgs : PipelineEventArgsBase
    {
        public PipelineEventFiredEventArgs(string pipelineName)
            : this(pipelineName, "")
        {
        }

        public PipelineEventFiredEventArgs(string pipelineName, string pipelineEventName)
            : base(pipelineName)
        {
            PipelineEventName = pipelineEventName;
        }

        public string PipelineEventName { get; set; }
    }
}
