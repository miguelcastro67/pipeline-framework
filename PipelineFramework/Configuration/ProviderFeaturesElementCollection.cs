using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pipeline.Configuration
{
    [ConfigurationCollection(typeof(PipelineElement), AddItemName = "provider")]
    public class ProviderFeaturesElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProviderFeatureElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProviderFeatureElement)element).Name;
        }

        internal ProviderFeatureElement GetByName(string providerFeatureName)
        {
            ProviderFeatureElement element = null;

            foreach (ProviderFeatureElement item in this)
            {
                if (item.Name == providerFeatureName)
                {
                    element = item;
                    break;
                }
            }

            return element;
        }

        internal ProviderFeatureElement this[int index]
        {
            get { return (ProviderFeatureElement)this.BaseGet(index); }
        }
    }
}
