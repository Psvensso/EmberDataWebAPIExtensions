using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmberWebapiExtensions
{
    public class EmberClass
    {
        public string primaryKey { get; set; }
        public string name { get; set; }
        public string pluralName { get; set; }
        public List<EmberClassProperty> properties { get; set; }
        public List<EmberClassRelationProperty> relationproperties { get; set; }

        public EmberClass() {
            properties = new List<EmberClassProperty>();
            relationproperties = new List<EmberClassRelationProperty>();
        }
    }
}
