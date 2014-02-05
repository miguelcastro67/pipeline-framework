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
    public interface IBackbone<TEvents, TContext> : IBackbone<TEvents>
        where TEvents : PipelineEvents, new()
        where TContext : PipelineContext
    {
        event EventHandler<PipelineEventFiringEventArgs> PipelineEventFiring;
        event EventHandler<PipelineEventFiredEventArgs> PipelineEventFired;
        void Execute(PipelineContext<TContext> pipelineEvent, TContext context);
        void Execute(PipelineContext<TContext> pipelineEvent, TContext context, TransactionScopeOption transactionScope);
        void Execute(TEvents pipelineEvents, TContext context);
    }
}
