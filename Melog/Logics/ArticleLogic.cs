using Melog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Melog.Models.IndexViewModels;
using static Melog.Models.IndexViewModels.ArchiveListViewModel;

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

        public ArchiveListViewModel GetArchiveList(long userId)
        {
            using (var context = new MeLogContext())
            {
                var years = (from a in context.Articles
                             where a.UserId == userId
                             select new { Year = a.CreatedAt.Year }).Distinct();

                var yearlyArchiveList = new List<YearlyArchiveModel>();
                foreach (var y in years)
                {
                    var months = (from a in context.Articles
                                  where a.UserId == userId
                                  where a.CreatedAt.Year == y.Year
                                  select new { Month = a.CreatedAt.Month }).Distinct();

                    var monthlyArchiveList = new List<MonthlyArchiveModel>();
                    foreach (var m in months)
                    {
                        var days = (from a in context.Articles
                                    where a.UserId == userId
                                    where a.CreatedAt.Year == y.Year
                                    where a.CreatedAt.Month == m.Month
                                    select new { Day = a.CreatedAt.Day }).Distinct();

                        var daylyArchiveList = new List<DailyArchiveModel>();
                        foreach (var d in days)
                        {
                            var articles = (from a in context.Articles
                                            where a.UserId == userId
                                            where a.CreatedAt.Year == y.Year
                                            where a.CreatedAt.Month == m.Month
                                            where a.CreatedAt.Day == d.Day
                                            select new ArchivedArticleModel
                                            {
                                                ArticleId = a.ArticleId,
                                                Title = a.Title
                                            });
                            daylyArchiveList.Add(
                                new DailyArchiveModel
                                {
                                    Day = $"{d.Day:D2}日",
                                    Count = articles.Count(),
                                    ArchivedArticleList = articles.ToList()
                                }
                            );
                        }
                        monthlyArchiveList.Add(
                            new MonthlyArchiveModel
                            {
                                Month = $"{m.Month:D2}月",
                                Count = daylyArchiveList.Select(x => x.Count).Sum(),
                                DailyArchiveList = daylyArchiveList
                            }
                        );
                    }
                    yearlyArchiveList.Add(
                        new YearlyArchiveModel
                        {
                            Year = $"{y.Year:D4}年",
                            Count = monthlyArchiveList.Select(x => x.Count).Sum(),
                            MonthlyArchiveList = monthlyArchiveList
                        }
                    );
                }
                return new ArchiveListViewModel
                {
                    YearlyArchiveList = yearlyArchiveList
                };
            }
        }
    }
}
