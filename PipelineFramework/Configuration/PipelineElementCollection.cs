using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pipeline.Configuration
{
    [ConfigurationCollection(typeof(PipelineElement), AddItemName = "pipeline")]
    public class PipelineElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PipelineElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PipelineElement)element).Name;
        }

        internal PipelineElement GetByName(string name)
        {
            PipelineElement element = null;

            foreach (PipelineElement item in this)
            {
                if (item.Name == name)
                {
                    element = item;
                    break;
                }
            }

            return element;
        }

        internal PipelineElement this[int index]
        {
            get { return (PipelineElement)this.BaseGet(index); }
        }
    }
}
