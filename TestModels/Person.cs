using EmberWebapiExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    [EmberModel(name="Person", pluralName="Persons", primaryKey="Id")]
    public class Person
    {

        public int Id { get; set; }
        [EmberProperty(emberType="string", name="name")]
        public string Name { get; set; }
    }
}
