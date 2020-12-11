using System.Collections.Generic;
using WISEsearch.Domain;

namespace WISEsearch.MVC.Models.Management
{
    public class QueryVisualizerViewModel
    {

        public QueryVisualizerViewModel(IEnumerable<QueryConfiguration>  queryConfigurations)
        {
            QueryConfigurations = queryConfigurations;
        }

        public IEnumerable<QueryConfiguration> QueryConfigurations { get; set; }
    }
}