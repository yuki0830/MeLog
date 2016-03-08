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

        public ArchiveListViewModel ArchiveListView { get; set; }

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

        public class ArchiveListViewModel
        {
            public List<YearlyArchiveModel> YearlyArchiveList { get; set; }

            public class ArchivedArticleModel
            {
                public string Title { get; set; }
                public long ArticleId { get; set; }
            }

            public class DailyArchiveModel
            {
                public string Day { get; set; }
                public long Count { get; set; }
                public List<ArchivedArticleModel> ArchivedArticleList { get; set; }
            }

            public class MonthlyArchiveModel
            {
                public string Month { get; set; }
                public long Count { get; set; }
                public List<DailyArchiveModel> DailyArchiveList { get; set; }
            }

            public class YearlyArchiveModel
            {
                public string Year { get; set; }
                public long Count { get; set; }
                public List<MonthlyArchiveModel> MonthlyArchiveList { get; set; }
            }
        }
    }
}
