using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmberWebapiExtensions
{
    public interface IScriptGenerator
    {
        /// <summary>
        /// Generates the proxy script.
        /// </summary>
        /// <returns>The script content.</returns>
        string GenerateModelScript();
    }
}