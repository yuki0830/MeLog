using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melog.Models
{
    public class IndexViewModels
    {

        public ArticleViewModel ArticleView { get; set; }

        public FavorRankingViewModel FavorRankingView { get; set; }

        public PastArticleListViewModel PastArticleListView { get; set; }

        public class ArticleViewModel
        {
            public string Title { get; set; }

            public string Description { get; set; }

            public List<string> Categories { get; set; }

            public DateTimeOffset CreatedAt { get; set; }

            public DateTimeOffset? UpdatedAt { get; set; }
        }

        public class FavorRankingViewModel
        {
            /// <summary>
            /// long:記事ＩＤ、string:記事タイトル、DateTimeOffset:投稿日
            /// </summary>
            public List<Tuple<long, string, DateTimeOffset>> FavorRankingList { get; set; }
        }

        public class PastArticleListViewModel
        {
            public Dictionary<string, List<Tuple<string, string, long>>> PastArticleList { get; set; }
        }
    }
}
