using System;

namespace Contentful.Essential.Sample.Models.Data
{
	public class ServiceRequest
	{
		public DateTime creation_date { get; set; }
		public string status { get; set; }
		public DateTime completion_date { get; set; }
		public string service_request_number { get; set; }
		public string type_of_service_request { get; set; }
		//public int number_of_premises_baited { get; set; }
		public string current_activity { get; set; }
		public string most_recent_action { get; set; }
		public string street_address { get; set; }
		public int zip_code { get; set; }
		public int zip { get; set; }
		public double x_coordinate { get; set; }
		public double y_coordinate { get; set; }
		public int ward { get; set; }
		public int police_district { get; set; }
		public string community_area { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
	}
}