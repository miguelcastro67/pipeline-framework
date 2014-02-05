using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Pipeline.Configuration;

namespace Pipeline
{
    public class ProviderFactory
    {
        public static T NewUp<T>(string providerFeature) where T : class
        {
            ProviderFeatureElement feature = GetProviderFeatureElement(providerFeature);

            string providerName = (feature.DefaultProvider != "" ? feature.DefaultProvider : feature[0].Name);

            return NewUp<T>(providerFeature, providerName);            
        }

        public static T NewUp<T>(string providerFeature, string providerName) where T : class
        {
            ProviderFeatureElement feature = GetProviderFeatureElement(providerFeature);

            ProviderElement provider = feature.GetByName(providerName);

            if (provider == null)
                throw new ConfigurationErrorsException(
                    string.Format("Pipeline framework provider feature '{0}' has default provider '{1}' defined and there is provider listed with that name.",
                    feature.DefaultProvider));

            object obj = Activator.CreateInstance(Type.GetType(provider.Type));
            T providerInstance = obj as T;

            if (providerInstance == null)
                throw new PipelineCastingException(
                    string.Format("Provider named '{0}' in provider feature '{1}' in the pipeline framework section does not case to the request type ({2}).",
                    providerName, providerFeature, typeof(T).ToString()));

            return providerInstance;
        }

        private static ProviderFeatureElement GetProviderFeatureElement(string providerFeature)
        {
            PipelineFrameworkConfigurationSection config =
                (PipelineFrameworkConfigurationSection)ConfigurationManager.GetSection("pipelineFramework");

            ProviderFeatureElement feature = config.Providers.GetByName(providerFeature);

            if (feature == null)
                throw new ConfigurationErrorsException(
                    string.Format("The provider feature '{0}' is missing from the pipeline framework's provider section.",
                    providerFeature));

            if (feature.Count == 0)
                throw new ConfigurationErrorsException(
                    string.Format("The provider feature '{0}' has no providers defined.",
                    providerFeature));

            return feature;
        }
    }
}
