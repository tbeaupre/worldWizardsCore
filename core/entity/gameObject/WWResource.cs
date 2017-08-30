using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace worldWizards.core.entity.gameObject
{
    /// <summary>
    /// 
    /// </summary>
    public class WWResource
    {
        //TODO: Refactor this. Add getter.
        public string path;

        public WWResource(string path)
        {
            this.path = path;
        }
    }
}
