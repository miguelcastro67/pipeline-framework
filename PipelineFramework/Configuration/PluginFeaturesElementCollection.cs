using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pipeline.Configuration
{
    // not an implemented feature of this framework yet

    [ConfigurationCollection(typeof(PluginFeatureElement), AddItemName = "plugin")]
    public class PluginFeaturesElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PluginFeatureElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PluginFeatureElement)element).Name;
        }

        internal PluginFeatureElement GetByName(string pluginFeatureName)
        {
            PluginFeatureElement element = null;

            foreach (PluginFeatureElement item in this)
            {
                if (item.Name == pluginFeatureName)
                {
                    element = item;
                    break;
                }
            }

            return element;
        }

        internal PluginFeatureElement this[int index]
        {
            get { return (PluginFeatureElement)this.BaseGet(index); }
        }
    }
}
