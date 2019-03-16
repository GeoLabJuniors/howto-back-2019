using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace HowToWebApplication.Models
{
    public class RequestsDataProvider
    {
        HowToDbEntities _db = new HowToDbEntities();
        
        

        public bool Exist(requests request)
        {
            return _db.requests.FirstOrDefault(e => e.content == request.content) == null ? false : true;
        }

        public bool ExistCustomRequest(RequestsCustomClass article)
        {
            return _db.requests.FirstOrDefault(e => e.content == article.Content) == null ? false : true;
        }

        public requests GetRequestById(int id)
        {
            return _db.requests.FirstOrDefault(e => e.Id == id);
        }

        


        public void CreateRequest(RequestsCustomClass request)
        {
            
            var newRequest = new requests();
            newRequest.number = request.Number;
            newRequest.title = request.Title;
            newRequest.content = request.Content;
            newRequest.date = DateTime.Now;
            newRequest.upvote = request.Upvote;
            newRequest.isDone = request.IsDone;
            newRequest.usersId = request.UsersId;

            if (!ExistCustomRequest(request))
            {
                _db.requests.Add(newRequest);

                _db.SaveChanges();
               

                _db.requestsArticles.Add(
                   new requestsArticles()
                   {
                       requestsId = newRequest.Id,
                       articlesId = null
                   });
                _db.SaveChanges();
            }
           
        }
  
        //Edit 
        public void EditRequest(RequestsCustomClass request)
        {
            var result = _db.requests.FirstOrDefault(e => e.Id == request.Id);

            if (!ExistCustomRequest(request) || result.content == request.Content)
            {
                result.title = result.title;
                result.content = request.Content;
                result.upvote = request.Upvote;
                result.isDone = request.IsDone;
                result.usersId = request.UsersId;
            }
            _db.SaveChanges();
        }


        public void FullDeleteRequest(requests request)

        {          
            var deleteRequestsArticles = _db.requestsArticles.Where(e => e.requestsId == request.Id);
            var deleteRequests = _db.requests.Where(e => e.Id == request.Id);

            _db.requestsArticles.RemoveRange(deleteRequestsArticles);
            _db.requests.RemoveRange(deleteRequests);
            _db.SaveChanges();
        }


        public IEnumerable<requests> AllRequest()
        {
            return _db.requests;
        }


        public IEnumerable<requests> GetRequestsByUserId(int id)
        {
            var getUser = _db.users.FirstOrDefault(e => e.Id == id);
            return _db.requests.Where(e => e.usersId == getUser.Id);
        }
    }


    

}
