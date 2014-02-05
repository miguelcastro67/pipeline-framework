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
    public class Backbone<TEvents> : IBackbone<TEvents>
        where TEvents : PipelineEvents, new()
    {
        public static TEvents InitializePipeline(string pipelineName)
        {
            return new Backbone<TEvents>(pipelineName).Initialize();
        }

        public static TEvents InitializePipeline(Definition.Pipeline pipeline)
        {
            return new Backbone<TEvents>(pipeline).Initialize();
        }

        private Backbone() // this class cannot be directly instantiated, instead should use Backbone<T,U>
        {
        }

        protected Backbone(string pipelineName) // the Backbone<T,U> class should be able to call this constructor
        {
            Contract.Requires(pipelineName != string.Empty);

            _PipelineName = pipelineName;
        }

        protected Backbone(Definition.Pipeline pipeline)
        {
            Contract.Requires(pipeline != null);

            _Pipeline = pipeline;
        }

        protected string _PipelineName = string.Empty;
        protected Definition.Pipeline _Pipeline = null;

        public event EventHandler<PipelineModuleInitializingEventArgs> PipelineModuleInitializing;
        public event EventHandler<PipelineModuleInitializedEventArgs> PipelineModuleInitialized;

        protected virtual void OnPipelineModuleInitializing(PipelineModuleInitializingEventArgs e)
        {
            if (this.PipelineModuleInitializing != null)
                this.PipelineModuleInitializing(this, e);
        }

        protected virtual void OnPipelineModuleInitialized(PipelineModuleInitializedEventArgs e)
        {
            if (this.PipelineModuleInitialized != null)
                this.PipelineModuleInitialized(this, e);
        }

        public TEvents Initialize()
        {
            TEvents pipelineEvents = new TEvents();
            Definition.Pipeline pipeline = null;
            
            if (_Pipeline == null)
                pipeline = GetPipelineDefinition();
            else
                pipeline = _Pipeline;

            foreach (Definition.Module moduleItem in pipeline.Modules)
            {
                PipelineModuleInitializingEventArgs beforeArgs = new PipelineModuleInitializingEventArgs(pipeline.Name, moduleItem.Name);
                OnPipelineModuleInitializing(beforeArgs);

                if (!beforeArgs.Cancel)
                {
                    object obj = Activator.CreateInstance(Type.GetType(moduleItem.Type));
                    IPipelineModule module = (IPipelineModule)obj;

                    if (module is IPipelineModuleBehavior)
                    {
                        IPipelineModuleBehavior behavior = module as IPipelineModuleBehavior;
                        behavior.ModuleInitializing(beforeArgs);
                    }

                    if (!beforeArgs.Cancel)
                    {
                        object[] attributes = module.GetType().GetCustomAttributes(false);
                        if (attributes != null)
                        {
                            foreach (object attribute in attributes)
                            {
                                if (attribute is IPipelineModuleBehavior)
                                {
                                    if (!beforeArgs.Cancel)
                                    {
                                        IPipelineModuleBehavior behavior = attribute as IPipelineModuleBehavior;
                                        behavior.ModuleInitializing(beforeArgs);
                                    }
                                }
                            }
                        }
                        
                        module.Initialize(pipelineEvents, moduleItem.Parameters);

                        PipelineModuleInitializedEventArgs afterArgs =
                            new PipelineModuleInitializedEventArgs(pipeline.Name, moduleItem.Name);
                        OnPipelineModuleInitialized(afterArgs);

                        if (module is IPipelineModuleBehavior)
                        {
                            IPipelineModuleBehavior behavior = module as IPipelineModuleBehavior;
                            behavior.ModuleInitialized(afterArgs);
                        }
                        
                        if (attributes != null)
                        {
                            foreach (object attribute in attributes)
                            {
                                if (attribute is IPipelineModuleBehavior)
                                {
                                    IPipelineModuleBehavior behavior = attribute as IPipelineModuleBehavior;
                                    behavior.ModuleInitialized(afterArgs);
                                }
                            }
                        }
                    }
                }
            }

            return pipelineEvents;
        }

        protected PipelineElement GetPipelineConfigurationElement()
        {
            return GetPipelineConfigurationElement(_PipelineName);
        }

        protected PipelineElement GetPipelineConfigurationElement(string pipelineName)
        {
            Contract.Requires(pipelineName != string.Empty);
            
            PipelineFrameworkConfigurationSection section =
                            (PipelineFrameworkConfigurationSection)(ConfigurationManager.GetSection("pipelineFramework"));

            PipelineElement pipelineElement = section.Pipelines.GetByName(pipelineName);

            if (pipelineElement == null)
                throw new ConfigurationErrorsException(
                    string.Format("Pipeline '{0}' is missing from the pipelineFramework configuration section.", pipelineName));

            return pipelineElement;
        }

        protected Definition.Pipeline GetPipelineDefinition()
        {
            return GetPipelineDefinition(_PipelineName);
        }
        
        protected Definition.Pipeline GetPipelineDefinition(string pipelineName)
        {
            PipelineElement pipelineElement = GetPipelineConfigurationElement(pipelineName);
            Definition.Pipeline pipeline = new Definition.Pipeline(pipelineElement.Name)
            {
                InvokeAll = pipelineElement.InvokeAll
            };

            foreach (ProviderSettings item in pipelineElement.Modules)
                pipeline.Modules.Add(item.Name, item.Type, item.Parameters);
            
            return pipeline;
        }
    }
}
