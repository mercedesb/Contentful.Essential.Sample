using Contentful.Essential.Models;
using Contentful.Essential.Sample.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Contentful.Essential.Sample.Controllers
{
    public class CategoryController : Controller
    {
        protected readonly IContentRepository<Category> _repo;

        public CategoryController(IContentRepository<Category> repo)
        {
            _repo = repo;
        }

        // GET: Category
        /// <summary>
        /// id is the string name of the category in Contentful
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string id)
        {
            Category category = await _repo.Get(id);
            if (category == null)
                return HttpNotFound();

            return View(category);
        }
    }
}