using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melog.Models
{
    public class SearchViewModel
    {
        public string SearchWord { get; set; }

        public List<SearchResultModel> SearchResults { get; set; }

        public class SearchResultModel
        {
            public long ArticleId { get; set; }
            public string Title { get; set; }
            public string Overview { get; set; }
            public List<string> Categories { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public DateTimeOffset? UpdatedAt { get; set; }
        }
    }
}
