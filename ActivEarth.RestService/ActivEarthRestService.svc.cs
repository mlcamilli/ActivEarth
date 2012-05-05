using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ActivEarth.RestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ActivEarthRestService" in code, svc and config file together.
    public class ActivEarthRestService : IActivEarthRestService
    {
        public void DoWork()
        {
        }

        public string XMLData(string id)
        {
            return "You requested id: " + id;
        }
    }
}
