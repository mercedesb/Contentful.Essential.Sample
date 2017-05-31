using Contentful.Essential.Models;
using Contentful.Essential.Sample.Models;
using Contentful.Essential.Sample.Models.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Contentful.Essential.Sample.Controllers
{
    public class HomeController : Controller
    {
        protected readonly IContentRepository<Category> _categoryRepo;

        public HomeController(IContentRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<ActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.DataSetCategories = await _categoryRepo.GetAll();
            return View(model);
        }
    }
}