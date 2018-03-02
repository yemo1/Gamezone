using GameZone.Repositories;
using GameZone.TOOLS;
using GameZone.VIEWMODEL;
using GameZone.WEB.Mappings;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GameZone.WEB.Controllers
{
    public class HeaderController : Controller
    {

        private readonly IServiceRequestRepository repository;
        private readonly IServiceHeaderRepository h_repository;
        WapHeaderUtil _WapHeaderUtil;
        public HeaderController(IServiceRequestRepository repo, IServiceHeaderRepository h_repo, WapHeaderUtil wapHeaderUtil)
        {
            repository = repo;
            h_repository = h_repo;
            _WapHeaderUtil = wapHeaderUtil;
        }

        [Route("{category}/Header-{headerId}/Pay")]
        public ActionResult Pay(MSISDNRepository msisdn, string category, string headerId)
        {
            //msisdn = (MSISDN)Session["XMSISDN"];

            if ((MSISDNRepository)Session["XMSISDN"] != null)
            {
                msisdn = (MSISDNRepository)Session["XMSISDN"];
            }
            else
            {
                msisdn = FillMSISDN();
            }

            try
            {
                //msisdn.Lines.FirstOrDefault().IsHeader)
                if (msisdn != null && msisdn.Lines.Count() > 0 && msisdn.Lines.FirstOrDefault().Phone != "XXX-XXXXXXXX")
                {
                    repository.Subscribe(Convert.ToInt32(headerId), msisdn.Lines.FirstOrDefault().IpAddress, msisdn.Lines.FirstOrDefault().Phone, msisdn.Lines.FirstOrDefault().IsHeader);
                }
                else
                {
                    return Redirect(Url.Action("Fill", new { category = category, headerId = headerId }));
                }
                //if (msisdn == null || msisdn.Lines.Count() < 1 || msisdn.Lines.FirstOrDefault().Phone == "XXX-XXXXXXXX")
                //{
                //    return Redirect(Url.Action("Fill", new { category = category, headerId = headerId }));

                //}
                //else
                //{
                //    repository.Subscribe(Convert.ToInt32(headerId), msisdn.Lines.FirstOrDefault().IpAddress, msisdn.Lines.FirstOrDefault().Phone, msisdn.Lines.FirstOrDefault().IsHeader);
                //}
            }
            catch (Exception ex)
            {
                _WapHeaderUtil.LogFileWrite(ex.ToString());
            }
            if (string.IsNullOrEmpty(category)) category = "Home";

            return Redirect(Url.Action("Index", new { controller = category, action = "Index" }));
        }

        [Route("{category}/Header-{headerId}/Fill")]
        //[Route("Header/Fill/{category}{headerId}")]
        public ActionResult Fill(string msisdn, string category, string headerId)
        {
            var header = h_repository.GetServiceHeader(Convert.ToInt32(headerId));
            header.Category = category;
            var model = new HeaderVM();
            model.header = header.ToModel();
            return View(model);
        }

        [Route("{category}/Header-{headerId}/Add")]
        [HttpPost]
        public ActionResult Add(string textPhone, string category, string headerId)
        {
            if (!string.IsNullOrEmpty(textPhone))
            {
                if (textPhone.StartsWith("0"))
                    textPhone = "234" + textPhone.TrimStart('0');
                var msisdn = new MSISDNRepository();
                msisdn = (MSISDNRepository)Session["XMSISDN"];
                //if (msisdn == null)
                //    msisdn = FillMSISDN();
                //13/02/2017

                if ((MSISDNRepository)Session["XMSISDN"] != null)
                {
                    msisdn = (MSISDNRepository)Session["XMSISDN"];
                }
                else
                {
                    msisdn = FillMSISDN();
                }

                var ipthis = msisdn.Lines.FirstOrDefault().IpAddress;
                msisdn.Clear();
                msisdn.AddItem(textPhone, ipthis, false);
                var newmsisdn = (MSISDNRepository)Session["XMSISDN"];
                HttpContext.Session["XMSISDN"] = msisdn;

                try
                {
                    if (msisdn != null && msisdn.Lines.Count() > 0 && msisdn.Lines.FirstOrDefault().Phone != "XXX-XXXXXXXX")

                        repository.Subscribe(Convert.ToInt32(headerId), msisdn.Lines.FirstOrDefault().IpAddress, msisdn.Lines.FirstOrDefault().Phone, msisdn.Lines.FirstOrDefault().IsHeader);

                    //to return manual numbers here
                    else
                        return Redirect(Url.Action("Fill", new { category = category, headerId = headerId }));

                    var newmsisdn1 = (MSISDNRepository)Session["XMSISDN"];

                }
                catch (Exception ex)
                {
                    _WapHeaderUtil.LogFileWrite(ex.Message);
                }
                if (string.IsNullOrEmpty(category)) category = "Home";
                return Redirect(Url.Action("Index", new { controller = category, action = "Index" }));
            }
            return Redirect(Request.Url.PathAndQuery);
        }

        public MSISDNRepository FillMSISDN()
        {
            //HTTPService.HeaderIndexSoapClient d = new HTTPService.HeaderIndexSoapClient();
            MSISDNRepository msisdntory = new MSISDNRepository();
            msisdntory.Clear();
            string msisdn = _WapHeaderUtil.MSISDN_HEADER();
            if (msisdn == "XXX-XXXXXXXX")
            {
                msisdntory.AddItem(msisdn, WapHeaderUtil.GetIPAddress(), false);
            }
            else
            {
                msisdntory.AddItem(msisdn, WapHeaderUtil.GetIPAddress());
            }


            return msisdntory;
        }



    }
}