using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pipeline.Configuration
{
    public class PipelineElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("invokeAll", IsRequired = false)]
        public bool InvokeAll
        {
            get { return (bool)base["invokeAll"]; }
            set { base["invokeAll"] = value; }
        }

        [ConfigurationProperty("modules", IsRequired = true)]
        public ProviderSettingsCollection Modules
        {
            get { return (ProviderSettingsCollection)base["modules"]; }
            set { base["modules"] = value; }
        }
    }
}