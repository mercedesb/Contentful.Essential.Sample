using Contentful.CodeFirst;
using Contentful.Core;
using Contentful.Core.Images;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Contentful.Essential.Models;
using Contentful.Essential.Models.Configuration;
using Contentful.Essential.Sample.Models;
using Contentful.Essential.Sample.Models.ViewModels;
using Contentful.Essential.Utility;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Contentful.Essential.Sample.Controllers
{
    public class PatternController : Controller
    {
        protected readonly IContentRepository<Pattern> _repo;
        protected readonly IContentDeliveryClient _client;
        protected readonly IContentfulOptions _config;
        protected readonly IContentManagementClient _mgmtClient;

        public PatternController(IContentRepository<Pattern> repo, IContentDeliveryClient client, IContentfulOptions config, IContentManagementClient mgmtClient)
        {
            _repo = repo;
            _client = client;
            _config = config;
            _mgmtClient = mgmtClient;
        }

        public async Task<ActionResult> Index(string id)
        {
            PatternViewModel model = new PatternViewModel();

            Pattern pattern = await _repo.Get(id);

            if (pattern == null)
                return View(model);

            model.CurrentPattern = pattern;

            if (model.CurrentPattern.FinishedProductImages != null)
                model.ManipulatedImageUrls = model.CurrentPattern.FinishedProductImages.Select(img => $"{img.File.Url}{ImageUrlBuilder.New().SetWidth(250).Build()}");

            return View(model);
        }


        public async Task<ActionResult> IndexTwo(string id)
        {
            PatternViewModel model = new PatternViewModel();

            // will not resolve references
            Pattern pattern = await _client.Instance.GetEntryAsync<Pattern>(id);

            // will resolve references (IContentRepository<> does this for you)
            ContentTypeAttribute contentTypeIdAttr = (ContentTypeAttribute)typeof(Pattern).GetCustomAttributes(typeof(ContentTypeAttribute), true).First() ?? new ContentTypeAttribute();
            var builder = new QueryBuilder<Pattern>().ContentTypeIs(contentTypeIdAttr.Id ?? typeof(Pattern).FullName).FieldEquals(f => f.Sys.Id, id);
            pattern = (await _client.Instance.GetEntriesAsync<Pattern>(builder)).FirstOrDefault();

            if (pattern == null)
                return View(model);

            model.CurrentPattern = pattern;

            if (model.CurrentPattern.FinishedProductImages != null)
                model.ManipulatedImageUrls = model.CurrentPattern.FinishedProductImages.Select(img => $"{img.File.Url}{ImageUrlBuilder.New().SetWidth(250).Build()}");

            return View("Index", model);
        }

        public async Task<ActionResult> IndexThree(string id)
        {
            PatternViewModel model = new PatternViewModel();

            var httpClient = new HttpClient();
            var client = new ContentfulClient(httpClient, _config.DeliveryAPIKey, _config.SpaceID);
            // will not resolve references
            Pattern pattern = await client.GetEntryAsync<Pattern>(id);

            if (pattern == null)
                return View(model);

            model.CurrentPattern = pattern;

            if (model.CurrentPattern.FinishedProductImages != null)
                model.ManipulatedImageUrls = model.CurrentPattern.FinishedProductImages.Select(img => $"{img.File.Url}{ImageUrlBuilder.New().SetWidth(250).Build()}");

            return View("Index", model);
        }

        public ActionResult Add()
        {
            Pattern model = new Pattern();
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> Add(Pattern model)
        {
            Entry<PatternMgmt> newEntry = model.ToManagementEntry<PatternMgmt, Pattern>("en");
            newEntry = await _mgmtClient.Instance.CreateEntryAsync(newEntry, typeof(Pattern).GetContentTypeId());
            newEntry = await _mgmtClient.Instance.PublishEntryAsync<PatternMgmt>(newEntry.SystemProperties.Id, newEntry.SystemProperties.Version.Value);

            return RedirectToAction("Index", new { id = newEntry.SystemProperties.Id });
        }
    }
}