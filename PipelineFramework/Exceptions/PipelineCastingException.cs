using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Pipeline.Configuration;

namespace Pipeline
{
    public class PipelineCastingException : Exception
    {
        public PipelineCastingException() : base() { }

        public PipelineCastingException(string message) : base(message) { }

        public PipelineCastingException(string message, Exception innerException) : base(message, innerException) { }
    }
}
