using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pipeline.Configuration
{
    [ConfigurationCollection(typeof(ProviderElement))]
    public class ProviderFeatureElement : ConfigurationElementCollection
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("defaultProvider", DefaultValue = "")]
        public string DefaultProvider
        {
            get { return (string)base["defaultProvider"]; }
            set { base["defaultProvider"] = value; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ProviderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProviderElement)element).Name;
        }

        internal ProviderElement GetByName(string name)
        {
            ProviderElement element = null;

            foreach (ProviderElement item in this)
            {
                if (item.Name == name)
                {
                    element = item;
                    break;
                }
            }

            return element;
        }

        internal ProviderElement this[int index]
        {
            get { return (ProviderElement)this.BaseGet(index); }
        }
    }
}