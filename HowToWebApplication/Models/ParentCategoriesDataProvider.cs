using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HowToWebApplication.Models
{
    public class ParentCategoriesDataProvider
    {
        HowToDbEntities _db = new HowToDbEntities();


        public bool Exist(ParentCategoriesCustomClass parentCategories)
        {
            return _db.parentCategories.FirstOrDefault(e => e.name == parentCategories.Name) == null ? false : true;
        }


        public parentCategories GetParentCategoriesById(int id)
        {
            return _db.parentCategories.FirstOrDefault(e => e.Id == id);
        }

        //Create
        public void CreateParentCategories(ParentCategoriesCustomClass parentCategories)
        {
            if (!Exist(parentCategories))
            {
                _db.parentCategories.Add(new parentCategories()
                {
                    name = parentCategories.Name,
                    Id = parentCategories.Id
                });
            }
            _db.SaveChanges();
        }

        //Edit 
        public void EditParentCategories(ParentCategoriesCustomClass parentCategories)
        {
            var result = _db.parentCategories.FirstOrDefault(e => e.Id == parentCategories.Id);
            if (!Exist(parentCategories))
            {
                result.name = parentCategories.Name;
            }
            _db.SaveChanges();
        }


        ////Delete
        ////public void ParentCategories(Users user)
        ////{
        ////    var result = _db.Users.FirstOrDefault(e => e.Id == user.Id);
        ////    result.IsActive = false;
        ////    _db.SaveChanges();
        ////}


        public void FullDeleteParentCategories(parentCategories parentCategories)
        {
            var categoryResult = _db.categories.Where(e => e.parentId == parentCategories.Id).ToList();

            //var result = _db.articleCategories.Where(i => categoryResult.Any(j => j.Id == i.categoriesId));

            
            //var deleteImage = _db.images.Where(i => result.Any(j => j.articlesId == i.articlesId));
            //var deleteFavourite = _db.favourites.Where(i => result.Any(j => j.articlesId == i.articlesId));
            //var deleteRating = _db.ratings.Where(i => result.Any(j => j.articlesId == i.articlesId));
            //var deleteArticleTags = _db.articlesTags.Where(i => result.Any(j => j.articlesId == i.articlesId));
            //var deleteArticle = _db.articles.Where(i => result.Any(j => j.articlesId == i.Id));
            //var deleteArticleCategories = _db.articleCategories.Where(i => categoryResult.Any(j => j.Id == i.categoriesId));

            var deleteCategories = _db.categories.Where(e => e.parentId == parentCategories.Id);
            var deleteparentCategories = _db.parentCategories.Where(e => e.Id == parentCategories.Id);



            //_db.images.RemoveRange(deleteImage);
            //_db.favourites.RemoveRange(deleteFavourite);
            //_db.ratings.RemoveRange(deleteRating);
            //_db.articlesTags.RemoveRange(deleteArticleTags);
            //_db.articles.RemoveRange(deleteArticle);
            //_db.articleCategories.RemoveRange(deleteArticleCategories);
            _db.categories.RemoveRange(deleteCategories);
            _db.parentCategories.RemoveRange(deleteparentCategories);
            _db.SaveChanges();
        }

        ////search 
        //public IEnumerable<categories> GetCategories(string name)
        //{
        //    return _db.categories.Where(e => e.name.Contains(name));
        //}


        public IEnumerable<parentCategories> AllParentCategories()
        {
            return _db.parentCategories;
        }
    }
}