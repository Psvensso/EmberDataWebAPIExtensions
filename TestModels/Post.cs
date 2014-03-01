using EmberWebapiExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    [EmberModel(name="Post", pluralName="Posts")]
    public class Post
    {
        [EmberPrimaryKey("id")]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [EmberModel(name = "Post", pluralName = "Posts")]
    public class Comment
    {
        [EmberPrimaryKey("id")]
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
