using Contentful.Essential.Models;
using Contentful.Essential.Sample.Models;
using Contentful.Essential.Sample.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Contentful.Essential.Sample.Controllers
{
    public class RoomController : Controller
    {
        protected readonly IContentRepository<Room> _repo;

        public RoomController(IContentRepository<Room> repo)
        {
            _repo = repo;
        }
        // GET: Room
        public async Task<ActionResult> Index(string id)
        {
            RoomViewModel model = new RoomViewModel();
            Room room = await _repo.Get(id);

            if (room != null)
                model.CurrentRoom = room;

            if (room.Reservations == null)
                return View(model);


            Random rnd = new Random();
            List<Event> events = new List<Event>();
            string color = $"rgb({rnd.Next(256)}, {rnd.Next(256)}, {rnd.Next(256)})";

            foreach (var res in room.Reservations)
            {
                events.Add(new Event
                {
                    Id = res.Sys.Id,
                    Title = res.Subject,
                    Start = res.Start.Value,
                    End = res.End.Value,
                    Color = color,
                    RoomId = room.Sys.Id,
                    Url = $"/reservation/Index/{res.Sys.Id}"
                });
            }

            model.Events = events;

            return View(model);
        }
    }
}