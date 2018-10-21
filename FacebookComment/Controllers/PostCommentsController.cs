using FacebookComment.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FacebookComment.Controllers
{

    public class PostCommentsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string imgFolder = "/AvatarExt/";
        private string defaultAvatar = "user.png";

        // GET: api/PostComments
        public IQueryable<PostComment> GetPostComments()
        {
            return db.PostComments;
        }

        // GET: api/PostComments/5
        [ResponseType(typeof(PostComment))]
        public IHttpActionResult GetPostComment(int id)
        {
            PostComment postComment = db.PostComments.Find(id);
            if (postComment == null)
            {
                return NotFound();
            }

            return Ok(postComment);
        }

        // PUT: api/PostComments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPostComment(int id, PostComment postComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != postComment.CommentId)
            {
                return BadRequest();
            }

            db.Entry(postComment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostCommentExists(id))
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

        // POST: api/PostComments
        [ResponseType(typeof(PostComment))]
        //public IHttpActionResult PostPostComment(PostComment postComment)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.PostComments.Add(postComment);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = postComment.CommentId }, postComment);
        //}
        public HttpResponseMessage PostPostComment(PostComment postcomment)
        {
            using (var context = new ApplicationDbContext())
            {
                var ID = User.Identity.GetUserId();
                var userProfile = context.Users.Where(x => x.Id.Equals(ID)).FirstOrDefault();
                postcomment.CommentedBy = userProfile.Email;
                postcomment.CommentedDate = DateTime.UtcNow;
                ModelState.Remove("postcomment.CommentedBy");
                ModelState.Remove("postcomment.CommentedDate");
                if (ModelState.IsValid)
                {
                    db.PostComments.Add(postcomment);
                    db.SaveChanges();
                    var usr = db.UserProfiles.FirstOrDefault(x => x.Email == postcomment.CommentedBy);
                    var ret = new
                    {
                        CommentedBy = postcomment.CommentedBy,
                        CommentedByName = usr.UserName,
                        CommentedByAvatar = imgFolder + (String.IsNullOrEmpty(usr.AvatarExt) ? defaultAvatar : usr.AvatarExt),
                        CommentedDate = postcomment.CommentedDate,
                        CommentId = postcomment.CommentId,
                        Message = postcomment.Message,
                        PostId = postcomment.PostId
                    };

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, ret);
                    //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = postcomment.CommentId }));
                    return response;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }

        }

        // DELETE: api/PostComments/5
        [ResponseType(typeof(PostComment))]
        public IHttpActionResult DeletePostComment(int id)
        {
            PostComment postComment = db.PostComments.Find(id);
            if (postComment == null)
            {
                return NotFound();
            }

            db.PostComments.Remove(postComment);
            db.SaveChanges();

            return Ok(postComment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostCommentExists(int id)
        {
            return db.PostComments.Count(e => e.CommentId == id) > 0;
        }
    }
}