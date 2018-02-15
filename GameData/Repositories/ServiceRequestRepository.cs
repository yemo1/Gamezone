using System;
using System.Linq;
using GameData;
using GameZone.Entities;
using GameZone.VIEWMODEL;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Configuration;
using System.Web.UI;

namespace GameZone.Repositories
{
    public class ServiceRequestRepository : IServiceRequest
    {
        private readonly NGSubscriptionsEntities context;
        //private readonly EFDbContext context = new EFDbContext();

        public IEnumerable<ServiceRequests> ServiceRequests
        {
            get
            {
                return context.ServiceRequests;
            }
        }

        public ServiceRequests DeleteServiceRequest(int requestId)
        {
            ServiceRequests dbentry = context.ServiceRequests.Find(requestId);
            if (dbentry != null)
            {
                context.ServiceRequests.Remove(dbentry);
                context.SaveChanges();
            }
            return dbentry;
        }

        public ServiceRequests GetServiceRequest(string transactionId)
        {
            return context.ServiceRequests.FirstOrDefault(r => r.TransactionId.ToString() == transactionId);
        }

        public ServiceRequests GetServiceRequest(int requestId)
        {
            return context.ServiceRequests.FirstOrDefault(r => r.RequestId == requestId);
        }

        public ServiceRequests GetServiceRequest(int requestId, string msisdn)
        {
            return context.ServiceRequests.FirstOrDefault(r => r.RequestId == requestId & r.MSISDN == msisdn);
        }

        public ServiceRequests GetServiceRequest(int requestId, string msisdn, string transactionId)
        {
            return context.ServiceRequests.FirstOrDefault(r => r.TransactionId.ToString() == transactionId & r.MSISDN == msisdn & r.RequestId == requestId);
        }

        public void SaveServiceRequest(ServiceRequests servicerequest)
        {
            if (servicerequest.RequestId == 0)
            {
                context.ServiceRequests.Add(servicerequest);
            }
            else
            {
                ServiceRequests dbentry = context.ServiceRequests.Find(servicerequest.RequestId);
                if (dbentry != null)
                {
                    dbentry.ClientIp = servicerequest.ClientIp;
                    dbentry.HeaderEnabled = servicerequest.HeaderEnabled;
                    dbentry.MSISDN = servicerequest.MSISDN;
                    dbentry.ServiceHeaderId = servicerequest.ServiceHeaderId;
                    dbentry.SourceChannel = servicerequest.SourceChannel;
                    dbentry.Timestamped = servicerequest.Timestamped;
                    dbentry.TransactionId = servicerequest.TransactionId;
                }
            }
            context.SaveChanges();
        }
        //Chat(code, phone)
        //mns(pid, phone, tid, descr)
        //Voice(msisdn, packid, requestimestamp(timeformat),trasactionid)

        public void Subscribe(int headerId, string ipAddress, string msisdn, bool IsHeaderEnabled = true, string sourceChannel = "Standard")
        {
            ServiceHeaders serv = context.ServiceHeaders.FirstOrDefault(f => f.HeaderId == headerId);
            if (serv != null)
            {
                ServiceRequests req = new ServiceRequests();
                req.HeaderEnabled = IsHeaderEnabled; //else number was obtained manually -false
                req.MSISDN = msisdn;
                req.ClientIp = ipAddress;
                req.ServiceHeaderId = serv.HeaderId;
                req.SourceChannel = sourceChannel; //Opera
                req.Timestamped = DateTime.Now;
                req.TransactionId = Guid.NewGuid();

                SaveServiceRequest(req);


                if (req.RequestId > 0)
                {
                    var reply = "";// new WebClient().DownloadString(ServiceUrlFill(serv, req));/////Need to follow up with return data

                    if (serv.ParamsType == 3 | serv.ParamsType == 4)
                    {
                        //request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                        //request.Method = "POST"; // Doesn't work with "GET"
                        //ASCIIEncoding encoding = new ASCIIEncoding();
                        //byte[] data = encoding.GetBytes(serv.ServiceParams.TrimStart('?'));
                        //request.ContentLength = data.Length;

                        Pop(ServiceUrlFill(serv, req));
                        HttpContext.Current.Session["Param3"] = ServiceUrlFill(serv, req);
                        //request.GetRequestStream().Write(data, 0, data.Length);
                        reply = "OK";
                    }
                    else
                    {
                        HttpContext.Current.Session["Param3"] = null;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ServiceUrlFill(serv, req));

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();


                        using (Stream stream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                            String responseString = reader.ReadToEnd();
                            reply = responseString;
                        }

                    }

                    ServiceResponseRepository repo = new ServiceResponseRepository();
                    ServiceResponses rep = new ServiceResponses();
                    LogFileWrite(reply);
                    rep.Description = reply;
                    //LogFileWrite("Description: "+ reply.ToString());
                    rep.RequestId = req.RequestId;
                    var statCode = string.Concat(reply.Take(50));
                    rep.StatusCode = statCode;
                    //LogFileWrite("StatusCode: " + string.Concat(reply.Take(50)));

                    rep.Timestamped = DateTime.Now;

                    repo.SaveServiceResponse(rep);
                }
            }
        }

        public string ServiceUrlFill(ServiceHeaders s, ServiceRequests r)
        {
            string val = s.ServiceUrl;
            switch ((ServiceUrlFiller)s.ParamsType)
            {
                default:
                    val += ServiceUrlFill(s.ServiceParams.Trim(), r.MSISDN.Trim(), s.ProductCode.Trim());
                    break;
                case ServiceUrlFiller.standard:
                    val += ServiceUrlFill(s.ServiceParams.Trim(), r.MSISDN.Trim(), s.ProductCode.Trim(), r.TransactionId.ToString().Replace("-", "").Trim(), s.Description.Trim());
                    break;
                case ServiceUrlFiller.multiplex:
                    val += ServiceUrlFill(s.ServiceParams, r.MSISDN.Trim(), s.ProductCode.Trim(), r.TransactionId.ToString().Replace("-", "").Trim(), DateTime.Now, s.TimeFormat);
                    break;
                case ServiceUrlFiller.nil:
                    val += ServiceUrlFill(s.ServiceParams);
                    break;
                case ServiceUrlFiller.lone:
                    val += ServiceUrlFill(s.ServiceParams, s.ProductCode);
                    break;
            }
            LogFileWrite(val);
            return val;
        }
        //?phone={0}&pID={1}
        //?msisdn={0}&code={1}

        public string ServiceUrlFill(string _params, string msisdn, string code)
        {
            return string.Format(_params, msisdn, code);
        }
        //?phone={0}&pID={1}&tID={2}&ds={3}
        public string ServiceUrlFill(string _params, string msisdn, string code, string tid, string descr)
        {
            return string.Format(_params, msisdn, code, tid, descr);
        }
        public string ServiceUrlFill(string _params, string msisdn, string code, string tid, DateTime requesttime, string timeformart)
        {
            return string.Format(_params, msisdn, code, tid, requesttime.ToString(timeformart));
        }

        public string ServiceUrlFill(string _params)
        {
            return string.Format(_params);
        }

        public string ServiceUrlFill(string _params, string code)
        {
            return string.Format(_params, code);
        }


        public enum ServiceUrlFiller
        {
            basic,
            standard,
            multiplex,
            nil,
            lone
        }

        private static string FileLog = ConfigurationManager.AppSettings["FileLog"] + "\\";

        public static void LogFileWrite(string message)
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

        public void Pop(string url)
        {
            var page = HttpContext.Current.CurrentHandler as Page;
            if (page != null)
            {
                ClientScriptManager script = page.ClientScript;

                script.RegisterClientScriptBlock(this.GetType(), "PayWindow", String.Format("<script>window.open('{0}');</script>", url));
            }

        }


    }
}