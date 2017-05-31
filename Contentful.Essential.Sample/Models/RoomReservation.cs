using Contentful.CodeFirst;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Essential.Models;
using System;

namespace Contentful.Essential.Sample.Models
{
    [ContentType(Name = "Room Reservation", DisplayField = "subject")]
    public class RoomReservation : BaseEntry
    {
        [ContentField(Type = SystemFieldTypes.Symbol)]
        public string Subject { get; set; }

        [ContentField(Required = true)]
        public DateTime? Start { get; set; }

        [ContentField(Required = true)]
        public DateTime? End { get; set; }
    }
}
