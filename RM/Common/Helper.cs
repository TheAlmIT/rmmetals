using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace RM.Common
{
    public class Helper
    {
        public static MethodResult SubmitPost(string url, string postValues, out string response)
        {
            MethodResult result = new MethodResult();
            response = string.Empty;

            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(postValues);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Timeout = System.Threading.Timeout.Infinite;

                webRequest.ContentLength = data.Length;

                Stream requestStream = webRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                requestStream.Flush();

                WebResponse webResponse = webRequest.GetResponse();
                StreamReader reader = new StreamReader(webResponse.GetResponseStream());

                var dataReceived = reader.ReadToEnd();

                webResponse.Close();
                reader.Close();
                webRequest.Abort();

                result.returnCode = MethodResult.ReturnCode.Success;
                response = dataReceived;
            }

            catch (Exception ex)
            {
                result.returnCode = MethodResult.ReturnCode.Failure;
                result.errorMessage = ex.Message;
            }

            return result;
        }


        public static string BuildPostString(Dictionary<string, string> postValues)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> value in postValues)
            {
                if (sb.Length > 0) sb.Append("&");

                sb.Append(string.Format("{0}={1}", value.Key, value.Value));
            }

            return sb.ToString();
        }
    }

    public class MethodResult
    {
        public string errorMessage;
        public ReturnCode returnCode;

        public enum ReturnCode
        {
            Unknown = 0,
            Success = 1,
            Failure
        }
    }
}