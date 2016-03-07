using Melog.Entities;
using Melog.Logics;
using Melog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Melog.Controllers
{
    public class ArticleController : Controller
    {
        private ArticleLogic _logic = new ArticleLogic();

        public ActionResult Index()
        {
            // TODO セッション管理
            var userId = 1;

            var article = _logic.GetLatestArticle(userId);
            var categories = _logic.GetCategories(userId, article.ArticleId);
            var user = _logic.GetUserDetail(userId);
            var pastArticleList = _logic.GetPastArticleList(userId);

            var model = new IndexViewModels()
            {
                ArticleView = new IndexViewModels.ArticleViewModel
                {
                    Title = article.Title,
                    Description = article.Description,
                    Categories = categories,
                    CreatedAt = article.CreatedAt,
                    UpdatedAt = article.UpdatedAt
                },
                FavorRankingView = new IndexViewModels.FavorRankingViewModel
                {
                    

                },
                PastArticleListView = new IndexViewModels.PastArticleListViewModel
                {
                    PastArticleList = pastArticleList
                }
            };

            return View(model);
        }

        public ActionResult Edit()
        {
            return View(new IndexViewModels.ArticleViewModel());
        }

        public ActionResult Specify(long articleId)
        {
            System.Diagnostics.Debug.WriteLine(articleId.ToString());
            return new RedirectResult("/");
        }

        public ActionResult Save(IndexViewModels.ArticleViewModel model)
        {
            // TODO セッション管理
            var userId = 1;

            using (var context = new MeLogContext())
            {
                var latestArticleId = _logic.GetLatestArticle(userId)?.ArticleId;
                var newArticleId = latestArticleId + 1 ?? null;

                var article = new Articles
                {
                    ArticleId = newArticleId.Value,
                    Title = model.Title,
                    Description = model.Description,
                    VersionID = 1,
                    UserId = userId,
                    CreatedAt = DateTimeOffset.Now
                };

                context.Articles.Add(article);
                context.SaveChanges();
            }
            return new RedirectResult("/");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public void Debug()
        {
            using (var context = new MeLogContext())
            {
                //context.Database.ExecuteSqlCommand("TRUNCATE TABLE Users");
                //context.Database.ExecuteSqlCommand("TRUNCATE TABLE UserDetails");
                //context.Database.ExecuteSqlCommand("TRUNCATE TABLE ExternalAccounts");
                //context.Database.ExecuteSqlCommand("TRUNCATE TABLE Roles");
                //context.Database.ExecuteSqlCommand("TRUNCATE TABLE Authorities");
                //context.Database.ExecuteSqlCommand("TRUNCATE TABLE ArticleLists");
                //context.Database.ExecuteSqlCommand("TRUNCATE TABLE ArticleVersion");
                //context.Database.ExecuteSqlCommand("TRUNCATE TABLE Articles");
                //context.Database.ExecuteSqlCommand("TRUNCATE TABLE ArticleCategories");
                //context.Database.ExecuteSqlCommand("TRUNCATE TABLE Categories");
                //context.Database.ExecuteSqlCommand("TRUNCATE TABLE Users");

                //context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (Users, RESEED, 1)");
                //context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (Articles, RESEED, 1)");
                //context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (Categories, RESEED, 1)");
                //context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (ArticleVersion, RESEED, 1)");
                //context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (ExternalAccounts, RESEED, 1)");
                //context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (Roles, RESEED, 1)");
                //context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (Authorities, RESEED, 1)");

                context.Users.Add(new Users
                {
                    UserId = 1
                });
                context.UserDetails.Add(new UserDetails
                {
                    UserId = 1,
                    RoleId = 1,
                    UserName = "ユーザー1",
                    MailAddress = "test@test.com",
                    ExternalAccountId = 1,
                });
                context.ExternalAccounts.Add(new ExternalAccounts
                {
                    ExternalAccountId = 1,
                    TwitterId = "twitter id",
                    FacebookId = "facebook id",
                    GoogleId = "google id"
                });
                context.Roles.Add(new Roles
                {
                    RoleId = 1,
                    RoleName = "ロール1"
                });
                context.Authorities.Add(new Authorities
                {
                    AuthorityId = 1,
                    AuthorityName = "権限1"
                });
                context.Articles.Add(new Articles
                {
                    ArticleId = 1,
                    UserId = 1,
                    Title = "記事1",
                    Description = "本文1",
                    VersionID = 1,
                    CreatedAt = DateTimeOffset.Now
                });
                context.Articles.Add(new Articles
                {
                    ArticleId = 2,
                    UserId = 1,
                    Title = "記事2",
                    Description = "本文2",
                    VersionID = 1,
                    CreatedAt = DateTimeOffset.Now
                });
                context.Articles.Add(new Articles
                {
                    ArticleId = 3,
                    UserId = 1,
                    Title = "記事3",
                    Description = "本文3",
                    VersionID = 1,
                    CreatedAt = DateTimeOffset.Now
                });
                context.ArticleCategories.Add(new ArticleCategories
                {
                    ArticleId = 1,
                    CategoryId = 1
                });
                context.ArticleCategories.Add(new ArticleCategories
                {
                    ArticleId = 1,
                    CategoryId = 2
                });
                context.ArticleCategories.Add(new ArticleCategories
                {
                    ArticleId = 1,
                    CategoryId = 3
                });
                context.ArticleCategories.Add(new ArticleCategories
                {
                    ArticleId = 2,
                    CategoryId = 1
                });
                context.ArticleCategories.Add(new ArticleCategories
                {
                    ArticleId = 3,
                    CategoryId = 1
                });
                context.Categories.Add(new Categories
                {
                    CategoryId = 1,
                    CategoryName = "カテゴリ1"
                });
                context.Categories.Add(new Categories
                {
                    CategoryId = 2,
                    CategoryName = "カテゴリ2"
                });
                context.Categories.Add(new Categories
                {
                    CategoryId = 3,
                    CategoryName = "カテゴリ3"
                });
                context.SaveChanges();
            }
        }
    }
}