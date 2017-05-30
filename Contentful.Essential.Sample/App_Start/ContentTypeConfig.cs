using Contentful.CodeFirst;
using Contentful.Essential.Models;
using Contentful.Essential.Models.Configuration;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Contentful.Essential.Sample
{
    public class ContentTypeConfig
    {

        public static void RegisterContentTypes()
        {
            var configuration = GetConfig();
            IEnumerable<IContentType> registeredContentTypes = ServiceLocator.Current.GetAllInstances<IContentType>();
            var types = registeredContentTypes.Select(ct => ct.GetType()).Where(c => c.GetTypeInfo().IsClass && c.GetTypeInfo().GetCustomAttribute<ContentTypeAttribute>() != null).ToList();
            var contentTypesToCreate = ContentTypeBuilder.InitializeContentTypes(types, configuration.CamelcaseFieldIdsAutomatically);
            var createdContentTypes = ContentTypeBuilder.CreateContentTypes(contentTypesToCreate, configuration, ServiceLocator.Current.GetInstance<IContentManagementClient>().Instance).Result;
        }

        private static MWContentfulCodeFirstConfiguration GetConfig()
        {
            return new MWContentfulCodeFirstConfiguration
            {
                ApiKey = ServiceLocator.Current.GetInstance<IContentfulOptions>().ManagementAPIKey,
                SpaceId = ServiceLocator.Current.GetInstance<IContentfulOptions>().SpaceID,
                ForceUpdateContentTypes = true,
                PublishAutomatically = true,
                CamelcaseFieldIdsAutomatically = true
            };
        }
    }
}