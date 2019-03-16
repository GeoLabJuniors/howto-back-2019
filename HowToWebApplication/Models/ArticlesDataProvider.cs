using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace HowToWebApplication.Models
{
    public class ArticlesDataProvider
    {
        HowToDbEntities _db = new HowToDbEntities();


        public bool Exist(articles article)
        {
            return _db.articles.FirstOrDefault(e => e.title == article.title) == null ? false : true;
        }

        public bool ExistCustomArticle(ArticlesCustomClass article)
        {
            return _db.articles.FirstOrDefault(e => e.title == article.Title) == null ? false : true;
        }

        public articles GetArticlesById(int id)
        {
            return _db.articles.FirstOrDefault(e => e.Id == id);
        }

        




        //Create
        public void CreateArticles(ArticlesCustomClass article, HttpPostedFileBase[] images)
        {

            var newarticle = new articles();
           
            newarticle.title = article.Title;
            newarticle.content = article.Content;
            newarticle.date = DateTime.Now;
            newarticle.isBlocked = false;
            newarticle.usersId = article.UsersId;


            if (!ExistCustomArticle(article))
            {
                _db.articles.Add(newarticle);

                _db.SaveChanges();
                foreach (var categoryId in article.CategoriesList)
                {

                    _db.articleCategories.Add(
                       new articleCategories()
                       {
                           categoriesId = categoryId,
                           articlesId = newarticle.Id
                       });
                    _db.SaveChanges();
                }

                if (article.RequestsList != null)
                {
                    foreach (var requestID in article.RequestsList)
                    {

                        _db.requestsArticles.Add(
                           new requestsArticles()
                           {
                               requestsId = requestID,
                               articlesId = newarticle.Id
                           });
                        _db.SaveChanges();
                    }
                }

                //სურათების create გავაკეთე, ედით ჯერ არ გითქვამს მარა ვცადე მაინც, 
                // ედითშიწაშლა გამოდის,  აი ისევ დამატება არა და მაგი გასარკვევი მექნება
                var mapPath = HostingEnvironment.MapPath("~/images/");
                foreach (var file in images)
                {
                    if (file != null)
                    {
                        file.SaveAs(mapPath + file.FileName);
                        _db.images.Add(new images() { name = file.FileName, url = "~/images/" + file.FileName,  articlesId = newarticle.Id, usersId = null});
                        _db.SaveChanges();
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        //Edit 
        public void EditArticles(ArticlesCustomClass article)
        {
            var result = _db.articles.FirstOrDefault(e => e.Id == article.Id);
            var categoryResult = _db.articleCategories.Where(e => e.articlesId == article.Id);
            var requestResult = _db.requestsArticles.Where(e => e.articlesId == article.Id);
            if (!ExistCustomArticle(article) || result.title == article.Title)
            {         
                result.title = article.Title;
                result.content = article.Content;
                result.usersId = article.UsersId;

            }
            _db.SaveChanges();

            //მთავარი პრობლემა ამ ბლოკში : ვერ გამომაქვს შექმნის დროს უკვე დამატებული კატეგორიები და მოთხოვნები
            //ჩვეულებრივი დროფდაუნისგან განსხვავებით მულტისელექტლისტს ვიყენებ და მანდ სხვანაირად უნდა მარა ჯერ გზა ვერ მოვნახე


            //კატეგორიის ცვლილებას აკეთებს
            var notTheseCategoryIds = categoryResult.Select(e => e.categoriesId);
                var CategorySelects = article.CategoriesList.Where(f => !notTheseCategoryIds.Contains(f)).ToList();
          
                foreach (var categoryId in CategorySelects)
                {
                    _db.articleCategories.Add(
                   new articleCategories()
                   {
                       categoriesId = categoryId,
                       articlesId = result.Id
                   });
                    _db.SaveChanges();
                }
            

            //რექუესტის ცვლილების აკეტებს
            if (article.RequestsList != null)
            {
                var notTheseRequestsIds = requestResult.Select(e => e.requestsId);
                var RequestsSelects = article.RequestsList.Where(f => !notTheseRequestsIds.Contains(f)).ToList();
                foreach (var requestID in RequestsSelects)
                {
                        _db.requestsArticles.Add(
                                   new requestsArticles()
                                   {
                                       requestsId = requestID,
                                       articlesId = result.Id
                                   });   
                }
                _db.SaveChanges();
            }
        }
        ////Delete
        //public void deleteCategories(articles articles)
        //{
        //    var result = _db.articles.FirstOrDefault(e => e.Id == articles.Id);
        //    result.isBlocked = false;
        //    _db.SaveChanges();
        //}

  

        public void FullDeleteArticle(articles article)
        {
            var result = _db.articleCategories.Where(e => e.categoriesId == article.Id).ToList();

            var deleteFavourite = _db.favourites.Where(e => e.articlesId == article.Id);
            var deleteImage = _db.images.Where(e => e.articlesId == article.Id);
            var deleteRating = _db.ratings.Where(e => e.articlesId == article.Id);
            var BlockArticlesRequests = _db.requestsArticles.Where(e => e.articlesId == article.Id);
            var deleteArticleTags = _db.articlesTags.Where(e => e.articlesId == article.Id);
            var deleteArticleCategories = _db.articleCategories.Where(e => e.articlesId == article.Id);
            var deleteArticle = _db.articles.Where(e => e.Id == article.Id);


            _db.favourites.RemoveRange(deleteFavourite);
            _db.images.RemoveRange(deleteImage);
            _db.ratings.RemoveRange(deleteRating);
          

            foreach(var artReq in BlockArticlesRequests)
            {
                artReq.articlesId = null;
            }

            _db.articlesTags.RemoveRange(deleteArticleTags);
            _db.articleCategories.RemoveRange(deleteArticleCategories);
            _db.articles.RemoveRange(deleteArticle);
            _db.SaveChanges();
        }


        public void DeleteImages(images img)
        {

           // var findImage = _db.images.FirstOrDefault(e => e.articlesId == article.Id);
            string fullPath = HostingEnvironment.MapPath(img.url);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            var deletableImages = _db.images.Where(e =>e.Id == img.Id);
            _db.images.RemoveRange(deletableImages);

            _db.SaveChanges();
        }


        public void BlockArticle(int id )
        {
            var article = _db.articles.FirstOrDefault(e => e.Id == id);
            article.isBlocked = true;
            _db.SaveChanges();
        }
        public void UnBlockArticle(int id)
        {
            var article = _db.articles.FirstOrDefault(e => e.Id == id);
            article.isBlocked = false;
            _db.SaveChanges();
        }

        public IEnumerable<articles> AllArticles()
        {
            return _db.articles;
        }

        public IEnumerable<articles> GetArticlesByUserId(int id)
        {
            var getUser = _db.users.FirstOrDefault(e => e.Id == id);
            return _db.articles.Where(e => e.usersId == getUser.Id);
        }
    }
}
