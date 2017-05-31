using System.Collections.Generic;
using System.Linq;

namespace Contentful.Essential.Sample.Models.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Rooms = Enumerable.Empty<Room>();
        }
        public IEnumerable<Room> Rooms { get; set; }
    }
}