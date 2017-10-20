using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Web.Script.Serialization;

namespace Shield
{
    class ApiParser
    {
        public List<string> getHacks(string requestUri)
        {
            string data;
            JavaScriptSerializer oJS = new JavaScriptSerializer();
            RootObject oRootObject = new RootObject();
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            WebResponse res = httpWebRequest.GetResponse();
            using (var sr = new StreamReader(res.GetResponseStream()))
            {
                data = sr.ReadToEnd();
                oRootObject = oJS.Deserialize<RootObject>(data);
            }
            if (oRootObject.success)
            {
                return oRootObject.hacks;
            }
            else
            {
                return null;
            }
        }
    }
    public class RootObject
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<string> hacks { get; set; }
    }
}
