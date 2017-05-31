using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Contentful.Essential.Sample.Models.ViewModels
{
    public class RoomViewModel
    {
        public Room CurrentRoom { get; set; }
        public List<Event> Events { get; set; }
        public string EventsJson
        {
            get
            {
                return JsonConvert.SerializeObject(Events, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            }
        }
    }
}