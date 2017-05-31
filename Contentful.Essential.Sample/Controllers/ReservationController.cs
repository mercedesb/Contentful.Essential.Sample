using Contentful.Essential.Models;
using Contentful.Essential.Sample.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Contentful.Essential.Sample.Controllers
{
    public class ReservationController : Controller
    {
        protected readonly IContentRepository<RoomReservation> _repo;
        public ReservationController(IContentRepository<RoomReservation> repo)
        {
            _repo = repo;
        }

        // GET: Reservation
        public async Task<ActionResult> Index(string id)
        {
            RoomReservation model = await _repo.Get(id);

            return View(model);
        }
    }
}