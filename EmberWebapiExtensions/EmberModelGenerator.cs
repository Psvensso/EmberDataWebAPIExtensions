using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EmberWebapiExtensions
{
    public class EmberModelGenerator : IModelGenerator
    {
        public EmberModelGenerator()
        {
        }
        public string GenerateModelScript()
        {
            var types = ModelExtractor.GetTypesWithEmberModelAttribute("TestModels");

            var template = new EmberWebapiExtensions.EmberModelTemplate();
            return template.TransformText();
        }
        
    }
}
