using EmberWebapiExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    [EmberModel("Comment", "Comments")]
    public class Comment
    {
        public int id { get; set; }
        public string CommentBody { get; set; }

        [BelongsTo]
        [EmberProperty(name="post")]
        public Post Post { get; set; }

    }
}
