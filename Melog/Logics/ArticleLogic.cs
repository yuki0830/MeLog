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
        public Articles GetArticle(long userId, long articleId)
        {
            using (var context = new MeLogContext())
            {
                return (from a in context.Articles
                        where a.UserId == userId
                        where a.ArticleId == articleId
                        select a).FirstOrDefault();
            }
        }

        public Articles GetLatestArticle(long userId)
        {
            using (var context = new MeLogContext())
            {
                return (from a in context.Articles
                        where a.UserId == userId
                        select a).OrderByDescending(x => x.ArticleId).FirstOrDefault();
            }
        }

        public List<string> GetCategories(long articleId)
        {
            using (var context = new MeLogContext())
            {
                return (from ac in context.ArticleCategories
                        join c in context.Categories on ac.CategoryId equals c.CategoryId
                        where ac.ArticleId == articleId
                        orderby c.CategoryName
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

        public long NewArticle(long userId, ArticleViewModel model)
        {
            using (var context = new MeLogContext())
            {
                var article = new Articles();

                var latestArticleId = GetLatestArticle(userId)?.ArticleId;
                var newArticleId = latestArticleId + 1 ?? null;

                article.ArticleId = newArticleId.Value;
                article.Title = model.Title;
                article.Description = model.Description;
                article.VersionID = 1;
                article.UserId = userId;
                article.CreatedAt = DateTimeOffset.Now;
                article.UpdatedAt = DateTimeOffset.Now;

                context.Articles.Add(article);
                context.SaveChanges();

                return newArticleId.Value;
            }
        }

        public long RegistCategory(string categoryName)
        {
            using (var context = new MeLogContext())
            {
                var category = new Categories();
                category.CategoryName = categoryName;
                context.Categories.Add(category);
                context.SaveChanges();

                return GetCategoryIdByName(categoryName);
            }
        }

        public void RegistArticleCategory(long articleId, long categoryId)
        {
            using (var context = new MeLogContext())
            {
                context.ArticleCategories.Add(new ArticleCategories
                {
                    ArticleId = articleId,
                    CategoryId = categoryId
                });
                context.SaveChanges();
            }
        }

        public void RegistArticleCategories(long articleId, string categoryText)
        {
            using (var context = new MeLogContext())
            {
                if (!string.IsNullOrEmpty(categoryText))
                {
                    foreach (var categoryName in categoryText.Split(','))
                    {
                        var categoryId = (from c in context.Categories
                                          where c.CategoryName == categoryName
                                          select c.CategoryId).SingleOrDefault();
                        if (categoryId == 0)
                        {
                            categoryId = RegistCategory(categoryName);
                        }
                        RegistArticleCategory(articleId, categoryId);
                    }
                }
            }
        }

        public void EditArticle(long userId, ArticleViewModel model)
        {
            using (var context = new MeLogContext())
            {
                var article = (from a in context.Articles
                               where a.UserId == userId
                               where a.ArticleId == model.ArticleId.Value
                               select a).FirstOrDefault();

                if (article == null)
                {
                    // TODO 記事ＩＤが不正
                }

                article.Title = model.Title;
                article.Description = model.Description;
                article.UpdatedAt = DateTimeOffset.Now;
                context.SaveChanges();
            }
        }

        public void DeleteArticleCategories(long articleId, List<long> categoryIdList)
        {
            using (var context = new MeLogContext())
            {
                var deleteList = (from ac in context.ArticleCategories
                                  where ac.ArticleId == articleId
                                  where !categoryIdList.Contains(ac.CategoryId)
                                  select ac);
                context.ArticleCategories.RemoveRange(deleteList);
                context.SaveChanges();
            }
        }

        public bool NotExistArticleCategory(long articleId, long categoryId)
        {
            using (var context = new MeLogContext())
            {
                return (from ac in context.ArticleCategories
                        where ac.ArticleId == articleId
                        where ac.CategoryId == categoryId
                        select ac).Count() == 0;
            }
        }

        public long GetCategoryIdByName(string categoryName)
        {
            using (var context = new MeLogContext())
            {
                return (from c in context.Categories
                        where c.CategoryName == categoryName
                        select c.CategoryId).SingleOrDefault();
            }
        }

        public void UpdateArticleCategories(long articleId, string categoryText)
        {
            using (var context = new MeLogContext())
            {
                var categoryIdList = new List<long>();
                foreach (var categoryName in categoryText.Split(','))
                {
                    var categoryId = GetCategoryIdByName(categoryName);
                    if (categoryId == 0)
                    {
                        categoryId = RegistCategory(categoryName);
                    }

                    if (NotExistArticleCategory(articleId, categoryId))
                    {
                        RegistArticleCategory(articleId, categoryId);
                    }
                    categoryIdList.Add(categoryId);
                }
                DeleteArticleCategories(articleId, categoryIdList);
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

        public List<string> GetTagList()
        {
            using (var context = new MeLogContext())
            {
                var countList = (from ac in context.ArticleCategories
                                 group ac by ac.CategoryId into grp
                                 select new
                                 {
                                     key = grp.Key,
                                     count = grp.Count()
                                 }).Distinct();
                return (from ac in countList
                        join c in context.Categories on ac.key equals c.CategoryId
                        orderby ac.count descending
                        select c.CategoryName).ToList();
            }
        }

        public List<Tuple<long, string>> GetCommentList(long articleId)
        {
            using (var context = new MeLogContext())
            {
                return (from ac in context.ArticleComments
                        join c in context.Comments on ac.CommentId equals c.CommentId
                        where ac.ArticleId == articleId
                        select c).ToList().ConvertAll(x => Tuple.Create<long, string>(x.CommentId, x.Comment));
            }
        }
    }
}
