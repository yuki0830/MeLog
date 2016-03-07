using Melog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melog.Logics
{
    public class ArticleLogic
    {
        public Articles GetLatestArticle(long userId)
        {
            using (var context = new MeLogContext())
            {
                return (from a in context.Articles
                        where a.UserId == userId
                        orderby a.CreatedAt descending
                        select a).FirstOrDefault();
            }
        }

        public List<string> GetCategories(long userId, long articleId)
        {
            using (var context = new MeLogContext())
            {
                return (from ac in context.ArticleCategories
                        join c in context.Categories on ac.CategoryId equals c.CategoryId
                        where ac.ArticleId == articleId
                        select c.CategoryName).ToList();
            }
        }

        public UserDetails GetUserDetail(long userId)
        {
            using (var context = new MeLogContext())
            {
                return (from ud in context.UserDetails
                        where ud.UserId == userId
                        select ud).Single();
            }
        }

        public Dictionary<string, List<Tuple<string, string, long>>> GetPastArticleList(long userId)
        {
            using (var context = new MeLogContext())
            {

                var dateIndex = (from a in context.Articles
                              where a.UserId == userId
                              select new { Year = a.CreatedAt.Year, Month = a.CreatedAt.Month }).Distinct();

                var pastArticleList = new Dictionary<string, List<Tuple<string, string, long>>>();
                foreach (var index in dateIndex)
                {
                    var articleList = (from a in context.Articles
                                   where a.UserId == userId
                                   where a.CreatedAt.Year == index.Year
                                   where a.CreatedAt.Month == index.Month
                                   select a)
                                   .ToList().ConvertAll(x => Tuple.Create<string, string, long>(x.CreatedAt.ToString("dd日"), x.Title, x.ArticleId));
                    pastArticleList[$"{index.Year:D4}年{index.Month:D2}月"] = articleList;
                }

                return pastArticleList;
            }
        }
    }
}
