using Contentful.Core.Configuration;
using Contentful.Essential.Models.Configuration;
using System.Configuration;

namespace Contentful.Essential.Sample.Configuration
{
    public class ContentfulAppSettingsManager : IContentfulOptions
    {
        public string DeliveryAPIKey { get { return ConfigurationManager.AppSettings["ContentfulDeliveryApiKey"]; } }
        public string ManagementAPIKey { get { return ConfigurationManager.AppSettings["ContentfulManagementApiKey"]; } }
        public string SpaceID { get { return ConfigurationManager.AppSettings["ContentfulSpaceId"]; } }
        public bool UsePreviewAPI { get { return bool.Parse(ConfigurationManager.AppSettings["ContentfulUsePreviewApi"]); } }
        public int MaxNumberOfRateLimitRetries { get { return int.Parse(ConfigurationManager.AppSettings["ContentfulMaxNumberOfRateLimitRetries"]); } }

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