using EmberWebapiExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmberWebapiExtensions.Test.Models
{
    [EmberModel("comment", "comments")]
    public class Comment
    {
        public int id { get; set; }
        public string CommentBody { get; set; }

        [BelongsTo]
        [EmberProperty(name="post")]
        public Post Post { get; set; }

    }
}
