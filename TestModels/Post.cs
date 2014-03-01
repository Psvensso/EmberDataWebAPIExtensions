using EmberWebapiExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    [EmberModel("Post","Posts")]
    public class Post
    {
        public int id { get; set; }
        public bool isPosted { get; set; }
        public DateTime postedAt { get; set; }
        public string postBody { get; set; }

        [EmberHasMany]
        [EmberProperty(name="comments")]
        public List<Comment> Comments { get; set; }
    }

    
}
