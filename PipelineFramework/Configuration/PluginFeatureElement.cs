using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pipeline.Configuration
{
    // not an implemented feature of this framework yet

    [ConfigurationCollection(typeof(PluginElement))]
    public class PluginFeatureElement : ConfigurationElementCollection
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("defaultPlugin", DefaultValue = "")]
        public string DefaultPlugin
        {
            get { return (string)base["defaultPlugin"]; }
            set { base["defaultPlugin"] = value; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PluginElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PluginElement)element).Name;
        }

        internal PluginElement GetByName(string name)
        {
            PluginElement element = null;

            foreach (PluginElement item in this)
            {
                if (item.Name == name)
                {
                    element = item;
                    break;
                }
            }

            return element;
        }

        internal PluginElement this[int index]
        {
            get { return (PluginElement)this.BaseGet(index); }
        }
    }
}
