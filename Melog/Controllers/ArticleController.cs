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
            var categories = _logic.GetCategories(article.ArticleId);
            var user = _logic.GetUserDetail(userId);
            var archiveList = _logic.GetArchiveList(userId);
            var tagList = _logic.GetTagList();
            var commentList = _logic.GetCommentList(article.ArticleId);

            var model = new IndexViewModels()
            {
                ArticleView = new IndexViewModels.ArticleViewModel
                {
                    ArticleId = article.ArticleId,
                    Title = article.Title,
                    Description = article.Description,
                    Categories = categories,
                    CreatedAt = article.CreatedAt,
                    UpdatedAt = article.UpdatedAt
                },
                FavorRankingView = new IndexViewModels.FavorRankingViewModel
                {

                },
                ArchiveListView = archiveList,
                TagListView = new IndexViewModels.TagListViewModel
                {
                    TagList = tagList
                },
                CommentListView = new IndexViewModels.CommentListViewModel
                {
                    CommentList = commentList
                }
            };

            return View("Index", "_LayoutForArticle", model);
        }

        public ActionResult New()
        {
            return View(new IndexViewModels.ArticleViewModel());
        }

        public ActionResult Edit(IndexViewModels model)
        {
            var userId = 1;
            var article = _logic.GetArticle(userId, model.ArticleView.ArticleId.Value);
            var categories = _logic.GetCategories(article.ArticleId);

            var articleViewModel = new IndexViewModels.ArticleViewModel
            {
                ArticleId = article.ArticleId,
                Title = article.Title,
                Description = article.Description,
                Categories = categories,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt
            };
            return View(articleViewModel);
        }

        public ActionResult Archives(long articleId)
        {
            // TODO セッション管理
            var userId = 1;

            var article = _logic.GetArticle(userId, articleId);
            var categories = _logic.GetCategories(article.ArticleId);
            var user = _logic.GetUserDetail(userId);
            var archiveList = _logic.GetArchiveList(userId);
            var tagList = _logic.GetTagList();
            var commentList = _logic.GetCommentList(article.ArticleId);

            var model = new IndexViewModels()
            {
                ArticleView = new IndexViewModels.ArticleViewModel
                {
                    ArticleId = article.ArticleId,
                    Title = article.Title,
                    Description = article.Description,
                    Categories = categories,
                    CreatedAt = article.CreatedAt,
                    UpdatedAt = article.UpdatedAt
                },
                FavorRankingView = new IndexViewModels.FavorRankingViewModel
                {

                },
                ArchiveListView = archiveList,
                TagListView = new IndexViewModels.TagListViewModel
                {
                    TagList = tagList
                },
                CommentListView = new IndexViewModels.CommentListViewModel
                {
                    CommentList = commentList
                }
            };

            return View("Index", "_LayoutForArticle", model);
        }

        [ValidateInput(false)]
        public ActionResult Save(IndexViewModels.ArticleViewModel model)
        {
            // TODO セッション管理
            var userId = 1;
            
            using (var context = new MeLogContext())
            {
                var article = new Articles();
                
                // 新規
                if (model.ArticleId == null)
                {
                    var articleId = _logic.NewArticle(userId, model);
                    _logic.RegistArticleCategories(articleId, model.CategoryText);
                    return new RedirectResult("/");
                }
                // 編集
                else
                {
                    _logic.EditArticle(userId, model);
                    _logic.UpdateArticleCategories(model.ArticleId.Value, model.CategoryText);
                    return new RedirectResult($"/Article/Archives?articleId={model.ArticleId.Value}");
                }
            }
        }

        public ActionResult Search(string word)
        {
            using (var context = new MeLogContext())
            {
                var categories = (from c in context.Categories
                                  where c.CategoryName.Contains(word)
                                  select c.CategoryName);
                var articleCategories = (from ac in context.ArticleCategories
                                         join tmp in context.Categories on ac.CategoryId equals tmp.CategoryId into tempc
                                         from c in tempc.DefaultIfEmpty()
                                         where c.CategoryName.Contains(word)
                                         select ac.ArticleId).Distinct();

                var results = (from a in context.Articles
                               where a.Description.Contains(word) || a.Title.Contains(word) || articleCategories.Contains(a.ArticleId)
                               select a).Distinct().ToList();
                var result2 = results.ConvertAll(m => new SearchViewModel.SearchResultModel
                               {
                                   ArticleId = m.ArticleId,
                                   Title = m.Title,
                                   Overview = m.Description.Length > 100 ? m.Description.Substring(0, 100) : m.Description.Substring(0, m.Description.Length),
                                   Categories = _logic.GetCategories(m.ArticleId),
                                   CreatedAt = m.CreatedAt,
                                   UpdatedAt = m.UpdatedAt
                               });
                return View("Search", new SearchViewModel
                {
                    SearchWord = word,
                    SearchResults = result2
                });
            }
        }

        public ActionResult PostComment(IndexViewModels model)
        {
            
            using (var context = new MeLogContext())
            {
                var comment = new Comments()
                {
                    Comment = model.CommentListView.NewComment
                };
                var result = context.Comments.Add(comment);
                context.SaveChanges();

                var articleComment = new ArticleComments()
                {
                    ArticleId = model.ArticleView.ArticleId.Value,
                    CommentId = result.CommentId
                };
                context.ArticleComments.Add(articleComment);
                context.SaveChanges();

                return new RedirectResult($"/Article/Archives?articleId={model.ArticleView.ArticleId}");
            }
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