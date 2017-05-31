namespace Contentful.Essential.Sample.Models.Data
{
	public class CommunityArea
	{
		public CommunityArea(string community_area, string count)
		{
			AreaNumber = community_area;
			int parsedCount;
			if (int.TryParse(count, out parsedCount))
				Count = parsedCount;
			else
				Count = 0;
			//Requests = new List<ServiceRequest>();
		}

		public string AreaNumber { get; protected set; }
		public int Count { get; protected set; }
		//public List<ServiceRequest> Requests { get; set; }
	}
}