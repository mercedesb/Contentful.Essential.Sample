using Contentful.CodeFirst;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Essential.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contentful.Essential.Sample.Models
{
    [ContentType(Id = "makingWavesRoom", DisplayField = "name")]
    public class Room : BaseEntry
    {
        [ContentField(Type = SystemFieldTypes.Symbol)]
        public string Name { get; set; }

        [ContentField(Required = true)]
        public int Seats { get; set; }

        [ContentField(Name = "Has Table")]
        [Display(Name = "Has Table")]
        public bool HasTable { get; set; }

        [ContentField(Name = "Has Apple TV")]
        [Display(Name = "Has Apple TV")]
        public bool HasAppleTV { get; set; }

        [ContentField(Name = "Apple TV Name", Type = SystemFieldTypes.Symbol)]
        [Display(Name = "Apple TV Name")]
        public string AppleTVName { get; set; }

        [ContentField(Name = "Has Chromecast")]
        [Display(Name = "Has Chromecast")]
        public bool HasChromecast { get; set; }

        [ContentField(Name = "Has Conferencing")]
        [Display(Name = "Has Conferencing")]
        public bool HasConferencing { get; set; }

        [LinkContentType(ContentTypeIds = new[] { "RoomReservation" })]
        public List<RoomReservation> Reservations { get; set; }

    }
}