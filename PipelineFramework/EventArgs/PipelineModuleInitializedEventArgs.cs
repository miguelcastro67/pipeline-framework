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
    public class PipelineModuleInitializedEventArgs : PipelineEventArgsBase
    {
        public PipelineModuleInitializedEventArgs(string pipelineName, string pipelineModuleName)
            : base(pipelineName)
        {
            PipelineModuleName = pipelineModuleName;
        }

        public string PipelineModuleName { get; set; }
    }
}
