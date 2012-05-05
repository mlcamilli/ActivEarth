using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ActivEarth.RestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IActivEarthRestService" in both code and config file together.
    [ServiceContract]
    public interface IActivEarthRestService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "xml/{id}")]
        string XMLData(string id);
    }
}
