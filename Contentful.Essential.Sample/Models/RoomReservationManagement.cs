using Contentful.Essential.Models;
using System;
using System.Collections.Generic;

namespace Contentful.Essential.Sample.Models
{
    public class RoomReservationManagement : IManagementContentType
    {
        public Dictionary<string, string> Subject { get; set; }
        public Dictionary<string, DateTime?> Start { get; set; }
        public Dictionary<string, DateTime?> End { get; set; }
    }
}