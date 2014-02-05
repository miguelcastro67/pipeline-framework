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

namespace Pipeline
{
    public class Backbone<TEvents, TContext> : Backbone<TEvents>, IBackbone<TEvents, TContext>
        where TEvents : PipelineEvents, new()
        where TContext : PipelineContext
    {
        public static void ExecutePipelineEvent(string pipelineName, PipelineContext<TContext> pipelineEvent, TContext context)
        {
            new Backbone<TEvents, TContext>(pipelineName).Execute(pipelineEvent, context, TransactionScopeOption.Suppress);
        }

        public static void ExecutePipelineEvent(string pipelineName, PipelineContext<TContext> pipelineEvent, TContext context, TransactionScopeOption transactionScope)
        {
            new Backbone<TEvents, TContext>(pipelineName).Execute(pipelineEvent, context, transactionScope);
        }

        public static void ExecutePipeline(string pipelineName, TEvents pipelineEvents, TContext context)
        {
            new Backbone<TEvents, TContext>(pipelineName).Execute(pipelineEvents, context);
        }

        public static void ExecutePipelineEvent(Definition.Pipeline pipeline, PipelineContext<TContext> pipelineEvent, TContext context)
        {
            new Backbone<TEvents, TContext>(pipeline).Execute(pipelineEvent, context, TransactionScopeOption.Suppress);
        }

        public static void ExecutePipelineEvent(Definition.Pipeline pipeline, PipelineContext<TContext> pipelineEvent, TContext context, TransactionScopeOption transactionScope)
        {
            new Backbone<TEvents, TContext>(pipeline).Execute(pipelineEvent, context, transactionScope);
        }
        
        public static void ExecutePipeline(Definition.Pipeline pipeline, TEvents pipelineEvents, TContext context)
        {
            new Backbone<TEvents, TContext>(pipeline).Execute(pipelineEvents, context);
        }

        public Backbone(string pipelineName)
            : base(pipelineName)
        {
        }

        public Backbone(Definition.Pipeline pipeline)
            : base(pipeline)
        {
        }

        public event EventHandler<PipelineEventFiringEventArgs> PipelineEventFiring;
        public event EventHandler<PipelineEventFiredEventArgs> PipelineEventFired;

        protected virtual void OnPipelineEventFiring(PipelineEventFiringEventArgs e)
        {
            if (this.PipelineEventFiring != null)
                this.PipelineEventFiring(this, e);
        }

        protected virtual void OnPipelineEventFired(PipelineEventFiredEventArgs e)
        {
            if (this.PipelineEventFired != null)
                this.PipelineEventFired(this, e);
        }

        public void Execute(PipelineContext<TContext> pipelineEvent, TContext context)
        {
            Execute(pipelineEvent, context, TransactionScopeOption.Suppress);
        }

        public void Execute(PipelineContext<TContext> pipelineEvent, TContext context, TransactionScopeOption transactionScope)
        {
            Contract.Requires(pipelineEvent != null);
            Contract.Requires(context != null);

            Definition.Pipeline pipeline = GetPipelineDefinition();
            
            System.Transactions.TransactionScopeOption scopeOption =
                GetTransactionScopeOption(transactionScope);

            if (pipelineEvent != null)
            {
                using (TransactionScope eventScope = new TransactionScope(scopeOption))
                {
                    PipelineEventFiringEventArgs args = 
                        new PipelineEventFiringEventArgs(pipeline.Name);
                    OnPipelineEventFiring(args);
                    
                    if (!args.Cancel)
                    {
                        if (pipeline.InvokeAll)
                        {
                            if (pipelineEvent != null)
                                pipelineEvent(context);
                        }
                        else
                        {
                            Delegate[] list = pipelineEvent.GetInvocationList();

                            foreach (PipelineContext<TContext> item in list)
                            {
                                item(context);
                                if (context.Cancel)
                                    break;
                            }
                        }

                        OnPipelineEventFired(new PipelineEventFiredEventArgs(pipeline.Name));
                    }

                    eventScope.Complete();
                }
            }
        }

        public void Execute(TEvents pipelineEvents, TContext context)
        {
            Contract.Requires(pipelineEvents != null);
            Contract.Requires(context != null);

            Definition.Pipeline pipeline = GetPipelineDefinition();

            PropertyInfo[] properties = pipelineEvents.GetType().GetProperties();

            List<PropertyInfo> sortedProperties = properties.ToList<PropertyInfo>();
            sortedProperties.Sort(new PropertyComparer());

            System.Transactions.TransactionScopeOption pipelineScopeOption = TransactionRequirement(sortedProperties);

            using (TransactionScope pipelineScope = new TransactionScope(pipelineScopeOption))
            {
                sortedProperties.ForEach(property =>
                {
                    object[] attributes =
                        property.GetCustomAttributes(typeof(PipelineEventAttribute), true);

                    if (attributes.Length > 0)
                    {
                        PipelineEventAttribute attr = (PipelineEventAttribute)attributes[0];

                        System.Transactions.TransactionScopeOption scopeOption =
                            GetTransactionScopeOption(attr.TransactionScopeOption);

                        object value = property.GetValue(pipelineEvents, null);
                        PipelineContext<TContext> eventProp = (PipelineContext<TContext>)value;

                        if (eventProp != null)
                        {
                            using (TransactionScope eventScope = new TransactionScope(scopeOption))
                            {
                                PipelineEventFiringEventArgs args = new PipelineEventFiringEventArgs(pipeline.Name, property.Name);
                                OnPipelineEventFiring(args);

                                if (!args.Cancel)
                                {
                                    if (pipeline.InvokeAll)
                                    {
                                        if (eventProp != null)
                                            eventProp(context);
                                    }
                                    else
                                    {
                                        Delegate[] list = eventProp.GetInvocationList();

                                        foreach (PipelineContext<TContext> item in list)
                                        {
                                            item(context);
                                            if (context.Cancel)
                                                break;
                                        }
                                    }

                                    OnPipelineEventFired(new PipelineEventFiredEventArgs(pipeline.Name, property.Name));
                                }

                                eventScope.Complete();
                            }
                        }
                    }
                });

                pipelineScope.Complete();
            }
        }

        private System.Transactions.TransactionScopeOption GetTransactionScopeOption(TransactionScopeOption transactionScopeOption)
        {
            System.Transactions.TransactionScopeOption scopeOption = System.Transactions.TransactionScopeOption.Required;

            if (transactionScopeOption == TransactionScopeOption.RequiredNew)
                scopeOption = System.Transactions.TransactionScopeOption.RequiresNew;
            else if (transactionScopeOption == TransactionScopeOption.Suppress)
                scopeOption = System.Transactions.TransactionScopeOption.Suppress;

            return scopeOption;
        }

        private System.Transactions.TransactionScopeOption TransactionRequirement(List<PropertyInfo> sortedProperties)
        {
            System.Transactions.TransactionScopeOption pipelineScopeOption = System.Transactions.TransactionScopeOption.Suppress;

            foreach (PropertyInfo property in sortedProperties)
            {
                object[] attributes = property.GetCustomAttributes(typeof(PipelineEventAttribute), true);
                if (attributes.Length > 0)
                {
                    PipelineEventAttribute attr = (PipelineEventAttribute)attributes[0];
                    if (attr.TransactionScopeOption != TransactionScopeOption.Suppress)
                    {
                        pipelineScopeOption = System.Transactions.TransactionScopeOption.Required;
                        break;
                    }
                }
            }

            return pipelineScopeOption;
        }
    }
}
