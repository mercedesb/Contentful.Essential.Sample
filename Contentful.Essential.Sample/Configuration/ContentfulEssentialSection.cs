using System.Configuration;

namespace Contentful.Essential.Sample.Configuration
{
    public class ContentfulEssentialSection : ConfigurationSection
    {
        [ConfigurationProperty("contentfulOptions", IsRequired = true)]
        public ContentfulOptionsElement ContentfulOptions
        {
            get
            {
                return (ContentfulOptionsElement)this["contentfulOptions"];
            }
            set
            { this["contentfulOptions"] = value; }
        }
    }
}