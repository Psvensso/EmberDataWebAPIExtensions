using EmberWebapiExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmberWebapiExtensions.Test.Models
{
    [EmberModel("person","persons")]
    public class Person
    {
        public int Id { get; set; }

        [EmberProperty("firstName", "string")]
        public string FirstName { get; set; }

        [EmberProperty("lastName", "string")]
        public string LastName { get; set; }

        [EmberIgnoreAttribute]
        public string NotInUse { get; set; }
    }
}
