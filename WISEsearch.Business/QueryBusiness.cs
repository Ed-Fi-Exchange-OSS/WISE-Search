using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using EDFI.Modules.Search.Extensions;
using WISEsearch.Domain;

namespace WISEsearch.Business
{
    public interface IQueryBusiness
    {
        IEnumerable<QueryConfiguration> GetQueryConfigurations();
        QueryConfiguration GetQueryConfiguration(string searchQueryName);
    }

    public class QueryBusiness : IQueryBusiness
    {
        public QueryBusiness()
        {
            _queryConfigurations = new Lazy<IEnumerable<QueryConfiguration>>(LoadQueryConfigurations);
        }

        private Lazy<IEnumerable<QueryConfiguration>> _queryConfigurations;


        public IEnumerable<QueryConfiguration> GetQueryConfigurations()
        {
            return _queryConfigurations.Value;
        }

        public QueryConfiguration GetQueryConfiguration(string searchQueryName)
        {
            return GetQueryConfigurations()
                .FirstOrDefault(x => x.Name == searchQueryName);
        }


        private static IEnumerable<QueryConfiguration> LoadQueryConfigurations()
        {
            var searchConfigurationPath = ConfigurationManager.AppSettings["WISEsearch.SearchConfigurationXml"];

            var searchConfiguration = XDocument.Load(Path.Combine(Environment.CurrentDirectory, searchConfigurationPath));

            return searchConfiguration
                .Descendants("searchQuery")
                .Select(element => new QueryConfiguration
                {
                    Name = element.AttributeValueOrNull("name"),
                    Query = element.Elements().Single().ToString(),
                    TargetIndex = element.AttributeValueOrNull("targetIndex")
                });
        }
    }
}