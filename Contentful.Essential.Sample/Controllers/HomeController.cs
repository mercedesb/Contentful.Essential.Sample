using Contentful.Essential.Models;
using Contentful.Essential.Sample.Models;
using Contentful.Essential.Sample.Models.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Contentful.Essential.Sample.Controllers
{
    public class HomeController : Controller
    {
        protected readonly IContentRepository<Room> _repo;
        public HomeController(IContentRepository<Room> repo)
        {
            _repo = repo;
        }
        public async Task<ActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.Rooms = await _repo.GetAll();

            return View(model);
        }
    }
}