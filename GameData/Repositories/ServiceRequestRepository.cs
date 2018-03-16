using System;
using System.Linq;
using GameData;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Web.UI;
using GameZone.TOOLS;
using GameZone.VIEWMODEL;
using System.Threading;

namespace GameZone.Repositories
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly NGSubscriptionsEntities context = new NGSubscriptionsEntities();
        //private readonly EFDbContext context = new EFDbContext();
        ServiceResponseRepository _IServiceResponseRepository = new Repositories.ServiceResponseRepository();
        public ServiceRequestRepository()
        {
        }
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

        public ReturnMessage Subscribe(int headerId, string ipAddress, string msisdn, bool IsHeaderEnabled = true, string sourceChannel = "Standard")
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

                    ServiceResponses rep = new ServiceResponses();
                    new Thread(() =>
                    {
                        LocalLogger.LogFileWrite(reply);
                    }).Start();

                    rep.Description = reply;
                    rep.RequestId = req.RequestId;
                    var statCode = string.Concat(reply.Take(50));
                    rep.StatusCode = statCode;
                    rep.Timestamped = DateTime.Now;

                    _IServiceResponseRepository.SaveServiceResponse(rep);

                    new Thread(() =>
                    {
                        LocalLogger.LogFileWrite($"RequestId: {rep.RequestId} - Description: {rep.Description}");
                    }).Start();

                    return new ReturnMessage()
                    {
                        ID = rep.RequestId,
                        Message = rep.Description,
                        Success = true,
                        Data = rep
                    };
                }
                else
                {
                    return new ReturnMessage()
                    {
                        Message = "This service does not exist on this platform.",
                        Success = false,
                        Data = serv
                    };
                }
            }
            else
            {
                return new ReturnMessage()
                {
                    Message = "This service does not exist on this platform.",
                    Success = false,
                    Data = serv
                };
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
            new Thread(() =>
            {
                LocalLogger.LogFileWrite(val);
            }).Start();

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