using Contentful.Core.Configuration;
using Contentful.Essential.Models.Configuration;

namespace Contentful.Essential.Sample.Configuration
{
    public class ContentfulConfigurationManager : IContentfulOptions
    {
        protected ContentfulEssentialSection _configSection;
        protected ContentfulEssentialSection ConfigSection
        {
            get
            {
                if (_configSection == null)
                {
                    _configSection = (ContentfulEssentialSection)System.Configuration.ConfigurationManager.GetSection("contentful.essential");
                }
                return _configSection;
            }
        }
        public string DeliveryAPIKey { get { return ConfigSection.ContentfulOptions.DeliveryAPIKey; } }
        public string ManagementAPIKey { get { return ConfigSection.ContentfulOptions.ManagementAPIKey; } }
        public string SpaceID { get { return ConfigSection.ContentfulOptions.SpaceID; } }
        public bool UsePreviewAPI { get { return ConfigSection.ContentfulOptions.UsePreviewAPI; } }
        public int MaxNumberOfRateLimitRetries { get { return ConfigSection.ContentfulOptions.MaxNumberOfRateLimitRetries; } }

        public virtual ContentfulOptions GetOptionsObject()
        {
            return new ContentfulOptions
            {
                DeliveryApiKey = DeliveryAPIKey,
                ManagementApiKey = ManagementAPIKey,
                SpaceId = SpaceID,
                UsePreviewApi = UsePreviewAPI,
                MaxNumberOfRateLimitRetries = MaxNumberOfRateLimitRetries
            };
        }
    }
}