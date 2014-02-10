using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LanguageRecognitionService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILanguageRecognitionService" in both code and config file together.
    [ServiceContract]
    public interface ILanguageRecognitionService
    {
        [OperationContract]
        [WebInvoke(Method="POST",
            ResponseFormat=WebMessageFormat.Json,
            BodyStyle=WebMessageBodyStyle.Wrapped,
            UriTemplate = "/Recognize/{sourceString}")]
        string Recognize(string sourceString);
    }
}
