using Contentful.Essential.Sample.Models.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Contentful.Essential.Sample.Models
{
    public class DatasetViewModel
    {
        public DatasetViewModel()
        {
            MapData = new Dictionary<int, IEnumerable<CommunityArea>>();
        }
        public DataSet Dataset { get; set; }
        public string DatasetDescription { get; set; }
        public Category Category { get; set; }
        public Dictionary<int, IEnumerable<CommunityArea>> MapData { get; set; }

        public int MinYear
        {
            get
            {
                var min = MapData.FirstOrDefault();
                if (!min.Equals(default(KeyValuePair<int, IEnumerable<CommunityArea>>)))
                    return min.Key;
                return 0;
            }
        }

        public int MaxYear
        {
            get
            {
                var max = MapData.LastOrDefault();
                if (!max.Equals(default(KeyValuePair<int, IEnumerable<CommunityArea>>)))
                    return max.Key;
                return 0;
            }
        }

        public int MaxValue { get; set; }

        public string JsonMapData
        {
            get
            {
                Dictionary<int, object> test = new Dictionary<int, object>();
                foreach (var grp in MapData)
                {
                    int key = grp.Key;
                    object val = grp.Value.Select(ca => new object[] { ca.AreaNumber, ca.Count });
                    test.Add(key, val);
                }

                return JsonConvert.SerializeObject(test);
            }
        }
    }
}