using Contentful.Core.Models;
using Contentful.Essential.Sample.Models;
using Contentful.Essential.Sample.Models.ViewModels;
using Contentful.Essential.Utility;
using System.Threading.Tasks;
using System.Web.Http;

namespace Contentful.Essential.Sample.Controllers.api
{
    [RoutePrefix("api/reservation")]
    public class ReservationController : ApiController
    {
        protected readonly IContentManagementClient _mgmtClient;
        public ReservationController(IContentManagementClient mgmtClient)
        {
            _mgmtClient = mgmtClient;
        }

        public async Task<IHttpActionResult> Post(string id, RoomReservation requestedReservation)
        {
            requestedReservation.Start = requestedReservation.Start.Value.ToUniversalTime();
            requestedReservation.End = requestedReservation.End.Value.ToUniversalTime();

            // create and publish new reservation
            Entry<RoomReservationManagement> newReservationEntry = requestedReservation.ToManagementEntry<RoomReservationManagement, RoomReservation>(Constants.Locale);
            newReservationEntry = await _mgmtClient.Instance.CreateEntryAsync(newReservationEntry, requestedReservation.GetContentTypeId());
            newReservationEntry = await _mgmtClient.Instance.PublishEntryAsync<RoomReservationManagement>(newReservationEntry.SystemProperties.Id, newReservationEntry.SystemProperties.Version.Value);

            // add new reservation to room and publish
            Entry<RoomManagement> updatedRoom = await _mgmtClient.Instance.GetEntryAsync<RoomManagement>(id);
            updatedRoom.Fields.Reservations.AddEntryToArray<RoomReservation>(newReservationEntry.SystemProperties.Id, Constants.Locale);

            updatedRoom = await _mgmtClient.Instance.CreateOrUpdateEntryAsync<RoomManagement>(updatedRoom, version: updatedRoom.SystemProperties.Version.Value);
            updatedRoom = await _mgmtClient.Instance.PublishEntryAsync<RoomManagement>(updatedRoom.SystemProperties.Id, updatedRoom.SystemProperties.Version.Value);

            requestedReservation = newReservationEntry.ToDeliveryEntry<RoomReservation, RoomReservationManagement>(Constants.Locale);
            var calendarEvent = new Event
            {
                Id = requestedReservation.Sys.Id,
                Title = requestedReservation.Subject,
                Start = requestedReservation.Start.Value,
                End = requestedReservation.End.Value,
                RoomId = requestedReservation.Sys.Id,
                Url = $"/reservation/Index/{requestedReservation.Sys.Id}"
            };
            return Ok(calendarEvent);
        }
    }
}
