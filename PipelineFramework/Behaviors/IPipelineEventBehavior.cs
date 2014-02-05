using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pipeline
{
    public interface IPipelineEventBehavior
    {
        void EventFiring(PipelineEventFiringEventArgs e);
        void EventFired(PipelineEventFiredEventArgs e);
    }
}
