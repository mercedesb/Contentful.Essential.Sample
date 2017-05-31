using Contentful.Essential.Models;
using Contentful.Essential.Sample.Http;
using Contentful.Essential.Sample.Models;
using Contentful.Essential.Sample.Models.Data;
using SODA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Contentful.Essential.Sample.Controllers
{
    public class DataSetController : Controller
    {
        protected readonly IContentRepository<DataSet> _dataSetRepo;
        protected readonly IContentRepository<Category> _categoryRepo;


        public DataSetController(IContentRepository<DataSet> dataSetRepo, IContentRepository<Category> categoryRepo)
        {
            _dataSetRepo = dataSetRepo;
            _categoryRepo = categoryRepo;
        }

        // GET: DataSet
        /// <summary>
        /// id is the entry id of the DataSet entry in Contentful
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string id)
        {
            DatasetViewModel model = new DatasetViewModel();
            DataSet cmsDataset = await _dataSetRepo.Get(id);
            if (cmsDataset == null)
                return HttpNotFound();

            model.Dataset = cmsDataset;

            Category category = (await _categoryRepo.GetAll()).FirstOrDefault(c => c.Datasets.Any(ds => ds.Sys.Id == cmsDataset.Sys.Id));
            model.Category = category;

            //read metadata of a dataset using the resource identifier (Socrata 4x4)
            var chicagoSodaClient = new SodaClient("data.cityofchicago.org");
            ResourceMetadata metadata = chicagoSodaClient.GetMetadata(cmsDataset.APIEndpoint);
            model.DatasetDescription = metadata.Description;
            //var soql1 = new SoqlQuery().Select("community_area", "count(*)", "date_trunc_y(creation_date)")
            //				.As("community_area", "count", "year")
            //				.Group("year", "community_area");
            //IEnumerable<YearlyRequestAggregate> results = chicagoClient.Query<YearlyRequestAggregate>(soql1, cmsDataset.APIEndpoint);

            IEnumerable<YearlyRequestAggregate> results = GetYearlyRequestAggregates(cmsDataset.APIEndpoint);

            IEnumerable<IGrouping<int, YearlyRequestAggregate>> groupedByYear = results.GroupBy(r => r.year.Year).Where(grp => grp.Key >= model.Dataset.StartDate.Year).OrderBy(grp => grp.Key);
            model.MapData = groupedByYear.ToDictionary(grp => grp.Key, grp => grp.Select(yra => new CommunityArea(yra.community_area, yra.count)));
            model.MaxValue = results.Max(yra => int.Parse(yra.count));
            return View(model);
        }

        protected IEnumerable<YearlyRequestAggregate> GetYearlyRequestAggregates(string apiEndpoint)
        {
            string cacheKey = $"{Constants.DATASET_DATA_CACHE_PREFIX}_{apiEndpoint}";
            IEnumerable<YearlyRequestAggregate> result = MemoryCache.Default[cacheKey] as IEnumerable<YearlyRequestAggregate>;
            if (result == null)
            {
                var chicagoClient = new GenericClient("https://data.cityofchicago.org");
                result = chicagoClient.Get<IEnumerable<YearlyRequestAggregate>>($"resource/{apiEndpoint}.json?$select=community_area, count(*) AS count, date_trunc_y(creation_date) as year&$group=community_area, year");

                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddDays(1);
                if (result != null)
                    MemoryCache.Default.Add(cacheKey, result, policy);
            }
            return result;
        }
    }
}