using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pipeline
{
    public interface IPipelineModuleBehavior
    {
        void ModuleInitializing(PipelineModuleInitializingEventArgs e);
        void ModuleInitialized(PipelineModuleInitializedEventArgs e);
    }
}
