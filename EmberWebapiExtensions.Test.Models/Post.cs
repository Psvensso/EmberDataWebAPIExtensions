using EmberWebapiExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmberWebapiExtensions.Test.Models
{
    [EmberModel("post","posts", "id")]
    public class Post
    {
        public int id { get; set; }
        public bool isPosted { get; set; }
        public DateTime postedAt { get; set; }
        public string postBody { get; set; }

        //[HasMany]
        //[EmberProperty(name="comments")]
        //public List<Comment> Comments { get; set; }

        [BelongsTo]
        [EmberProperty(name = "person")]
        public Person Person { get; set; }

    }

    
}
