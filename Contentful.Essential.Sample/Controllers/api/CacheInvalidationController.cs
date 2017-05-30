using Contentful.Essential.Models;
using Contentful.Essential.WebHooks;
using Contentful.Essential.WebHooks.Receivers;
using System.Web.Http;

namespace Contentful.Essential.Sample.Controllers.api
{
    [RoutePrefix("api/invalidatecache")]
    public class CacheInvalidationController : ApiController
    {
        protected readonly IPurgeCachedContentRepository _purge;
        public CacheInvalidationController(IPurgeCachedContentRepository purge)
        {
            _purge = purge;
        }

        public IHttpActionResult Index()
        {
            CacheInvalidationWebHookReceiver receiver = new CacheInvalidationWebHookReceiver(_purge);
            var result = receiver.Process(new WebHookRequestMessage(Request));
            return Ok(result);
        }
    }
}