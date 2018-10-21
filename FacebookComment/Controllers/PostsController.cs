using FacebookComment.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FacebookComment.Controllers
{
    //[Authorize]
    public class PostsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string imgFolder = "/AvatarExt/";
        private string defaultAvatar = "user.png";



        //private void GetDetails()
        //{
        //    using (var context = db)
        //    {
        //        if (User.Identity.IsAuthenticated)
        //        {
        //            var ID = User.Identity.GetUserId();
        //            var userProfile = context.Users.Where(x => x.Id.Equals(ID)).FirstOrDefault();
        //            Email = userProfile.Email;
        //        }
        //    }
        //}

        // GET: api/Posts
        [Route("api/NewPosts")]
        public dynamic GetPosts()
        {
            try
            {
                //var usr = db.UserProfiles
                var ret = (from post in db.Posts.ToList()
                           join usr in db.UserProfiles.ToList() on post.PostedBy equals usr.Email into PostUsr
                           join comm in db.PostComments.ToList() on post.PostId equals comm.PostId into PostCommUsr
                           orderby post.PostedDate descending
                           from set in PostUsr
                           select new
                           {
                               Message = post.Message,
                               PostedBy = post.PostedBy,
                               PostedByName = set.UserName,
                               PostedByAvatar = imgFolder + (String.IsNullOrEmpty(set.AvatarExt) ? defaultAvatar : set.AvatarExt),
                               PostedDate = post.PostedDate,
                               PostId = post.PostId,
                               PostComments = (from comme in db.PostComments.ToList()
                                               join nett in PostUsr.ToList() on comme.PostId equals post.PostId into PCU
                                               join usr in db.UserProfiles.ToList() on comme.CommentedBy equals usr.Email
                                               from comm in PCU
                                               select new
                                               {
                                                   CommentedBy = comme.CommentedBy,
                                                   CommentedByName = usr.UserName,
                                                   CommentedByAvatar = imgFolder + (String.IsNullOrEmpty(usr.AvatarExt) ? defaultAvatar : usr.AvatarExt),
                                                   CommentedDate = comme.CommentedDate,
                                                   CommentId = comme.CommentId,
                                                   Message = comme.Message,
                                                   PostId = comme.PostId

                                               }).ToList()

                           }).AsEnumerable();
                //descending
                //select new
                //{
                //    Message          = post.Message,
                //    PostedBy         = post.PostedBy,
                //    PostedByName     = usr.UserName,
                //    PostedByAvatar   = imgFolder + (String.IsNullOrEmpty(usr.AvatarExt) ? defaultAvatar : usr.AvatarExt),
                //    PostedDate       = post.PostedDate,
                //    PostId           = post.PostId,
                //    PostComments = (from comm in db.PostComments.ToList()
                //                    join pst in db.Posts.ToList() on comm.CommentId equals pst.PostId
                //                    join usr in db.UserProfiles.ToList() on comm.CommentedBy equals usr.Email
                //                    orderby comm.CommentedDate
                //                    select new
                //                    {
                //                        CommentedBy          = comm.CommentedBy,
                //                        CommentedByName      = usr.UserName,
                //                        CommentedByAvatar    = imgFolder + (String.IsNullOrEmpty(usr.AvatarExt) ? defaultAvatar : usr.AvatarExt),
                //                        CommentedDate        = comm.CommentedDate,
                //                        CommentId            = comm.CommentId,
                //                        Message              = comm.Message,
                //                        PostId               = comm.PostId

                //                    }).ToList()
                //}).AsEnumerable();
                //var ret = (from post in db.Posts.ToList()
                //           orderby post.PostedDate descending
                //           select new
                //           {
                //               Message = post.Message,
                //               PostedBy = post.PostedBy,
                //               PostedByName = post.UserProfile.UserName,
                //               PostedByAvatar = imgFolder + (String.IsNullOrEmpty(post.UserProfile.AvatarExt) ? defaultAvatar : post.UserProfile.AvatarExt),
                //               PostedDate = post.PostedDate,
                //               PostId = post.PostId,
                //               PostComments = from comment in post.PostComments.ToList()
                //                              orderby comment.CommentedDate
                //                              select new
                //                              {
                //                                  CommentedBy = comment.CommentedBy,
                //                                  CommentedByName = comment.UserProfile.UserName,
                //                                  CommentedByAvatar = imgFolder + (String.IsNullOrEmpty(comment.UserProfile.AvatarExt) ? defaultAvatar : comment.UserProfile.AvatarExt),
                //                                  CommentedDate = comment.CommentedDate,
                //                                  CommentId = comment.CommentId,
                //                                  Message = comment.Message,
                //                                  PostId = comment.PostId

                //                              }
                //           }).AsEnumerable();
                foreach (var item in ret)
                {
                    var Message = item.Message;
                    var PostedBy = item.PostedBy;
                    var PostedByName = item.PostedByName;
                    var PostedByAvatar = item.PostedByAvatar;
                    var PostedDate = item.PostedDate;
                    var PostId = item.PostId;
                    //var PostComments = item.PostComments;
                }
                return Json(ret);
            }
            catch (Exception err)
            {
                string error = err.Message;
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, err.Message);
            }


        }

        // GET: api/Posts/5
        [ResponseType(typeof(Post))]
        public IHttpActionResult GetPost(int id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // PUT: api/Posts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPost(int id, Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != post.PostId)
            {
                return BadRequest();
            }

            db.Entry(post).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Posts
        [ResponseType(typeof(Post))]
        [Route("api/NewPosts")]
        public HttpResponseMessage PostPost(Post post)
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {

                    if (User.Identity.IsAuthenticated)
                    {
                        var ID = User.Identity.GetUserId();
                        var userProfile = context.Users.Where(x => x.Id.Equals(ID)).FirstOrDefault();
                        post.PostedBy = userProfile.Email;
                        post.PostedDate = DateTime.UtcNow;
                        ModelState.Remove("post.PostedBy");
                        ModelState.Remove("post.PostedDate");

                        if (ModelState.IsValid)
                        {

                            //db.Database.Connection.Open();
                            db.Posts.Add(post);
                            db.SaveChanges();


                            var usr = db.UserProfiles.FirstOrDefault(x => x.Email == post.PostedBy);
                            var ret = new
                            {
                                Message = post.Message,
                                PostedBy = post.PostedBy,
                                PostedByName = usr.UserName,
                                PostedByAvatar = imgFolder + (String.IsNullOrEmpty(usr.AvatarExt) ? defaultAvatar : usr.AvatarExt),
                                PostedDate = post.PostedDate,
                                PostId = post.PostId
                            };
                            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, ret);
                            //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = post.PostId }));
                            return response;

                        }
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "You are not authorized");

                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }

        }

        // DELETE: api/Posts/5
        [ResponseType(typeof(Post))]
        public IHttpActionResult DeletePost(int id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            db.Posts.Remove(post);
            db.SaveChanges();

            return Ok(post);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostExists(int id)
        {
            return db.Posts.Count(e => e.PostId == id) > 0;
        }
    }
}