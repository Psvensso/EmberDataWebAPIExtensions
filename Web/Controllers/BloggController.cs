using EmberWebapiExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Controllers
{
    public class BloggController : ApiController
    {
        public Blogg Get(int id) {
            return new Blogg
            {
                id = 1,
                title = "My first blogg",
                body = "My first blogg body text",
                postedAt = DateTime.Now,
                author = "Per S."
            };
            
        }
    }
    public class BloggsController : ApiController
    {
        public List<Blogg> Get()
        {
            var c = new List<Comment> { 
                new Comment{ id = 1, Body="My1 body here..."},
                new Comment{ id = 2, Body="My2 body here..."},
                new Comment{ id = 3, Body="My3 body here..."}
            }; 
            
            return new List<Blogg> { 
            new Blogg { 
                id = 1,
                title = "My first blogg",
                body = "My first blogg body text",
                postedAt = DateTime.Now,
                author = "Per S.",
                Comments = c
            }
            };
        }
    }

    [EmberModel("blogg","bloggs")]
    public class Blogg {
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public DateTime postedAt { get; set; }
        public string author { get; set; }
        [HasMany]
        public virtual List<Comment> Comments { get; set; }
    }

    public class Comment {
        public int id { get; set; }
        public string Body { get; set; }
    }

}
