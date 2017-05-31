// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Contentful.Essential.Sample.DependencyResolution
{
    using Configuration;
    using Essential.Models;
    using Essential.Models.Configuration;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Models;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.Assembly("Contentful.Essential");
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                    scan.AddAllTypesOf<IContentType>();
                });
            //For<IExample>().Use<Example>();
            For<IContentfulOptions>().Use<ContentfulConfigurationManager>();
            For<IContentDeliveryClient>().Use<ContentDelivery>().Singleton();
            For<IContentManagementClient>().Use<ContentManagement>().Singleton();
            For(typeof(IContentRepository<>)).Use(typeof(BaseCachedContentRepository<>));
            For<IContentRepository<Room>>().Use<BaseContentRepository<Room>>();
            For<IPurgeCachedContentRepository>().Use<BaseCachedContentRepositoryPurger>();
            For<IMemoryCache>().Use<MemoryCache>().Singleton();
            For<IOptions<MemoryCacheOptions>>().Use<MemoryCacheOptions>();
        }

        #endregion
    }
}