using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using Pipeline.Configuration;
using Pipeline.Definition;

namespace Pipeline
{
    public interface IBackbone<TEvents>
            where TEvents : PipelineEvents, new()
    {
        event EventHandler<PipelineModuleInitializingEventArgs> PipelineModuleInitializing;
        event EventHandler<PipelineModuleInitializedEventArgs> PipelineModuleInitialized;
        TEvents Initialize();
    }
}
