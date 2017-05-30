using Contentful.Core;
using Contentful.Essential.Models;
using Contentful.Essential.Models.Configuration;
using Contentful.Essential.Sample.Models;
using Contentful.Essential.Sample.Models.ViewModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Contentful.Essential.Sample.Controllers
{
    public class HomeController : Controller
    {
        protected readonly IContentRepository<Pattern> _repo;
        protected readonly IContentDeliveryClient _client;
        protected readonly IContentfulOptions _config;

        public HomeController(IContentRepository<Pattern> repo, IContentDeliveryClient client, IContentfulOptions config)
        {
            _repo = repo;
            _client = client;
            _config = config;
        }

        public async Task<ActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel();

            IEnumerable<Pattern> patterns = await _repo.GetAll();

            if (patterns == null)
                return View(model);

            model.Patterns = patterns;

            return View(model);
        }

        public async Task<ActionResult> IndexTwo()
        {
            HomeViewModel model = new HomeViewModel();

            IEnumerable<Pattern> patterns = await _client.Instance.GetEntriesByTypeAsync<Pattern>("Pattern");

            if (patterns == null)
                return View(model);

            model.Patterns = patterns;

            return View("Index", model);
        }

        public async Task<ActionResult> IndexThree()
        {
            HomeViewModel model = new HomeViewModel();

            var httpClient = new HttpClient();
            var client = new ContentfulClient(httpClient, _config.DeliveryAPIKey, _config.SpaceID);
            IEnumerable<Pattern> patterns = await client.GetEntriesByTypeAsync<Pattern>("Pattern");

            if (patterns == null)
                return View(model);

            model.Patterns = patterns;

            return View("Index", model);
        }
    }
}