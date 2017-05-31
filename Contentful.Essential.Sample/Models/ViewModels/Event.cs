using System;

namespace Contentful.Essential.Sample.Models.ViewModels
{
    public class Event
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Url { get; set; }
        public string Color { get; set; }
        public string RoomId { get; set; }
    }
}