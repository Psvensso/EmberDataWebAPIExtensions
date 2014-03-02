using EmberWebapiExtensions.Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Controllers
{
    public class PostController : ApiController
    {
        // GET api/post
        public IEnumerable<Post> Get()
        {
            return new Post[] { new Post{
                id = 1,
                isPosted = true,
                postedAt = DateTime.Now,
                postBody = "This is post nr 1"

            },
            new Post{
                id = 2,
                isPosted = true,
                postedAt = DateTime.Now.AddHours(2),
                postBody = "This is post nr 2"
            }};
        }

        // GET api/post/5
        public Post Get(int id)
        {
            return new Post
            {
                id = id,
                isPosted = true,
                postedAt = DateTime.Now,
                postBody = "This is post nr " + id
            };
        }

        // POST api/post
        public void Post([FromBody]string value)
        {
        }

        // PUT api/post/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/post/5
        public void Delete(int id)
        {
        }
    }
}
