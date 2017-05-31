using Contentful.Essential.Models;
using System.Collections.Generic;

namespace Contentful.Essential.Sample.Models
{
    public class RoomManagement : IManagementContentType
    {
        public Dictionary<string, string> Name { get; set; }

        public Dictionary<string, int> Seats { get; set; }

        public Dictionary<string, bool> HasTable { get; set; }

        public Dictionary<string, bool> HasAppleTV { get; set; }

        public Dictionary<string, string> AppleTVName { get; set; }

        public Dictionary<string, bool> HasChromecast { get; set; }

        public Dictionary<string, bool> HasConferencing { get; set; }

        public Dictionary<string, List<RoomReservation>> Reservations { get; set; }
    }
}