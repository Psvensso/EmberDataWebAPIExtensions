using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmberWebapiExtensions
{
    public class EmberModel
    {
        public List<EmberClass> Classes { get; set; }

        public EmberModel() {
            Classes = new List<EmberClass>();
        }
    }
}
