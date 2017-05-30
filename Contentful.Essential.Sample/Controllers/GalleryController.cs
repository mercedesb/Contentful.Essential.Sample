using Contentful.Core.Images;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Contentful.Essential.Sample.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Contentful.Essential.Sample.Controllers
{
    public class GalleryController : Controller
    {
        protected readonly IContentDeliveryClient _client;

        public GalleryController(IContentDeliveryClient client)
        {
            _client = client;
        }

        // GET: Gallery
        public async Task<ActionResult> Index()
        {
            GalleryViewModel model = new GalleryViewModel();
            var queryBuilder = QueryBuilder<Asset>.New.MimeTypeIs(MimeTypeRestriction.Image).Limit(4);
            var assets = await _client.Instance.GetAssetsAsync(queryBuilder);
            model.GalleryImages = assets.Select(img => img.File != null ? $"{img.File.Url}{ImageUrlBuilder.New().SetWidth(275).UseProgressiveJpg().Build()}" : string.Empty);

            return View(model);
        }
    }
}