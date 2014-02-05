using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pipeline
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PipelineEventAttribute : Attribute
    {
        private int _Order = 0;
        private TransactionScopeOption _TransactionScopeOption = TransactionScopeOption.Suppress;

        public int Order
        {
            get { return _Order; }
            set { _Order = value; }
        }

        public TransactionScopeOption TransactionScopeOption
        {
            get { return _TransactionScopeOption; }
            set { _TransactionScopeOption = value; }
        }
    }
}
