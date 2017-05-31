using System;

namespace Contentful.Essential.Sample.Models.ViewModels
{
    public class RoomReservationRequest
    {
        public string Subject { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
        public string RoomId { get; set; }
    }
}