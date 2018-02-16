using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace GameZone.TOOLS
{
    public class WapHeaderUtil
    {
        private static string FileLog = ConfigurationManager.AppSettings["FileLog"] + "\\";

        public void LogFileWrite(string message)
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                string text = AppDomain.CurrentDomain.BaseDirectory + FileLog;
                text = text + "AppLog-" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
                string str = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                if (!text.Equals(""))
                {
                    FileInfo fileInfo = new FileInfo(text);
                    DirectoryInfo directoryInfo = new DirectoryInfo(fileInfo.DirectoryName);
                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }
                    if (!fileInfo.Exists)
                    {
                        fileStream = fileInfo.Create();
                    }
                    else
                    {
                        fileStream = new FileStream(text, FileMode.Append);
                    }
                    streamWriter = new StreamWriter(fileStream);
                    streamWriter.WriteLine(str + message);
                }
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        public string MSISDN_HEADER(bool val = true)
        {

            string urlResponse = "", url = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host +
                (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port) + "/Default.mhtm";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();


                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    String responseString = reader.ReadToEnd();
                    urlResponse = responseString;
                }
            }
            catch (Exception ex)
            {

                urlResponse = "XXX-XXXXXXXX";
                LogFileWrite(url + Environment.NewLine + ex.ToString());
            }
            return urlResponse;
        }

        public static string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public string MSISDN_HEADER()
        {

            string urlResponse = "";
            try
            {
                LogFileWrite(LogMessage());
                //write your handler implementation here.
                string returnValue = string.Empty;
                dynamic headerDash = null;
                dynamic headerUnder = null;
                dynamic HTTP_headerDash = null;
                dynamic HTTP_headerUnder = null;
                dynamic HEADER_headerDash = null;
                dynamic HEADER_headerUnder = null;

                headerDash = "X-MSISDN";
                headerUnder = "MSISDN";
                HTTP_headerDash = "HTTP_" + headerDash;
                HTTP_headerUnder = "HTTP_" + headerUnder;
                HEADER_headerDash = "HEADER_" + headerDash;
                HEADER_headerUnder = "HEADER_" + headerUnder;

                StringBuilder b = new StringBuilder();
                if (string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables[HTTP_headerDash]) == false)
                    b.Append(HttpContext.Current.Request.ServerVariables[HTTP_headerDash] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables[HTTP_headerUnder]) == false)
                    b.Append(HttpContext.Current.Request.ServerVariables[HTTP_headerUnder] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables[HEADER_headerDash]) == false)
                    b.Append(HttpContext.Current.Request.ServerVariables[HEADER_headerDash] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables[HEADER_headerUnder]) == false)
                    b.Append(HttpContext.Current.Request.ServerVariables[HEADER_headerUnder] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers[headerDash]) == false)
                    b.Append(HttpContext.Current.Request.Headers[headerDash] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers[headerUnder]) == false)
                    b.Append(HttpContext.Current.Request.Headers[headerUnder] + Environment.NewLine);

                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["HTTP-X-MSISDN"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["HTTP-X-MSISDN"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["X_MSISDN"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["X_MSISDN"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["X-MSISDN"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["X-MSISDN"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["HTTP_X_MSISDN"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["HTTP_X_MSISDN"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["X-UP-CALLING-LINE-ID"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["X-UP-CALLING-LINE-ID"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["X_UP_CALLING_LINE_ID"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["X_UP_CALLING_LINE_ID"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["HTTP_X_UP_CALLING_LINE_ID"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["HTTP_X_UP_CALLING_LINE_ID"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["HTTP-X-UP-CALLING-LINE-ID"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["HTTP-X-UP-CALLING-LINE-ID"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["X_WAP_NETWORK_CLIENT_MSISDN"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["X_WAP_NETWORK_CLIENT_MSISDN"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["X-Forwarded-For"]) == false && HttpContext.Current.Request.Headers["X-Forwarded-For"].Contains(".") == false)
                    b.Append(HttpContext.Current.Request.Headers["X-Forwarded-For"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["Proxy-Client-IP"]) == false && HttpContext.Current.Request.Headers["Proxy-Client-IP"].Contains(".") == false)
                    b.Append(HttpContext.Current.Request.Headers["Proxy-Client-IP"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["WL-Proxy-Client-IP"]) == false && HttpContext.Current.Request.Headers["WL-Proxy-Client-IP"].Contains(".") == false)
                    b.Append(HttpContext.Current.Request.Headers["WL-Proxy-Client-IP"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["HTTP_CLIENT_IP"]) == false && HttpContext.Current.Request.Headers["HTTP_CLIENT_IP"].Contains(".") == false)
                    b.Append(HttpContext.Current.Request.Headers["HTTP_CLIENT_IP"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["HTTP_X_FORWARDED_FOR"]) == false && HttpContext.Current.Request.Headers["HTTP_X_FORWARDED_FOR"].Contains(".") == false)
                    b.Append(HttpContext.Current.Request.Headers["HTTP_X_FORWARDED_FOR"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["x-real-ip"]) == false && HttpContext.Current.Request.Headers["x-real-ip"].Contains(".") == false)
                    b.Append(HttpContext.Current.Request.Headers["x-real-ip"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["HTTP_X_HTS_CLID"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["HTTP_X_HTS_CLID"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["User-Identity-Forward-msisdn"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["User-Identity-Forward-msisdn"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["HTTP_X_NOKIA_MSISDN"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["HTTP_X_NOKIA_MSISDN"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["HTTP_X_UP_SUBNO"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["HTTP_X_UP_SUBNO"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["HTTP_X_NETWORK_INFO"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["HTTP_X_NETWORK_INFO"] + Environment.NewLine);
                else if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["DEVICEID"]) == false)
                    b.Append(HttpContext.Current.Request.Headers["DEVICEID"] + Environment.NewLine);

                else b.Append("XXX-XXXXXXXX");

                if (b != null)
                    returnValue = b.ToString();
                else
                    returnValue = "XXX-XXXXXXXX";
                //context.Response.ContentType = "text/html";
                urlResponse = returnValue;
            }
            catch (Exception ex)
            {

                urlResponse = "XXX-XXXXXXXX";
                LogFileWrite(ex.ToString());
            }
            return urlResponse;
        }

        string LogMessage()
        {
            string returnValue = string.Empty;

            returnValue += "Logging from " + GetIPAddress() + "................" + Environment.NewLine;


            dynamic headerDash = null;
            dynamic headerUnder = null;
            dynamic HTTP_headerDash = null;
            dynamic HTTP_headerUnder = null;
            dynamic HEADER_headerDash = null;
            dynamic HEADER_headerUnder = null;


            headerDash = "X-MSISDN";

            headerUnder = "MSISDN";

            HTTP_headerDash = "HTTP_" + headerDash;

            HTTP_headerUnder = "HTTP_" + headerUnder;

            HEADER_headerDash = "HEADER_" + headerDash;

            HEADER_headerUnder = "HEADER_" + headerUnder;


            returnValue += HTTP_headerDash + " = " + HttpContext.Current.Request.ServerVariables[HTTP_headerDash] + Environment.NewLine;

            returnValue += HTTP_headerUnder + " = " + HttpContext.Current.Request.ServerVariables[HTTP_headerUnder] + Environment.NewLine;

            returnValue += HEADER_headerDash + " = " + HttpContext.Current.Request.ServerVariables[HEADER_headerDash] + Environment.NewLine;

            returnValue += HEADER_headerUnder + " = " + HttpContext.Current.Request.ServerVariables[HEADER_headerUnder] + Environment.NewLine;

            returnValue += "Header(" + headerDash + ") = " + HttpContext.Current.Request.Headers[headerDash] + Environment.NewLine;


            returnValue += "Header(" + headerUnder + ") = " + HttpContext.Current.Request.Headers[headerUnder] + Environment.NewLine;


            return returnValue;
        }
    }
}
