using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Contentful.Essential.Sample.Startup))]
namespace Contentful.Essential.Sample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
