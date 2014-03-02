using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EmberWebapiExtensions
{
    public class EmberScriptGenerator : IScriptGenerator
    {
        public EmberScriptGenerator()
        {
        }
        public string GenerateModelScript()
        {
            var model = ModelGenerator.Instance.GenerateModel();
            var template = new EmberWebapiExtensions.Templates.EmberModelTemplate(model);
            return template.TransformText();
        }
        
    }
}
