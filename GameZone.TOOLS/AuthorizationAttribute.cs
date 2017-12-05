using GameZone.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using GameZone.VIEWMODEL;
namespace GameZone.TOOLS
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        public string token { get; set; }
        /// <summary>
        /// This method is responsible for checking if a user has an active session
        /// and if user has permission to access the targetted form
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var subscriber = new Subscriber();
            var s = subscriber.GetUser(token);
            if (s == null)
            {
                RedirectToAction(filterContext, "Games", "Index");
            }
            else
            {
                RedirectToAction(filterContext, "Games", "GameList");
            }



            ////Check if User is Logged in
            //if (ApplicationUserIdentity.LoggedInUser == null)
            //{
            //    //Get Configuration from Web/App Config for Auto Routing to Login Controller Action
            //    var LoginArea = System.Configuration.ConfigurationManager.AppSettings.Get("LoginArea").ToString();
            //    var LoginController = System.Configuration.ConfigurationManager.AppSettings.Get("LoginController").ToString();
            //    var LoginAction = System.Configuration.ConfigurationManager.AppSettings.Get("LoginAction").ToString();

            //    filterContext.Result = new RedirectToRouteResult(
            //            new System.Web.Routing.RouteValueDictionary{
            //            { "area", LoginArea },
            //            { "controller", LoginController },
            //                              { "action", LoginAction },
            //                              { "returnUrl",ReturnURL}
            //                                 });
            //}
            //else
            //{
            //    //Check if User has permission
            //    if (ApplicationUserIdentity.LoggedInUserRolePermisions != null)
            //    {
            //        var areaName = ReturnURL.Split('/')[1];
            //        var userPermissions = ApplicationUserIdentity.LoggedInUserRolePermisions.
            //                        Where(c => c.ApplicationModuleText == $"{areaName}/{filterContext.RouteData.Values["controller"].ToString()}"
            //                        && c.ApplicationFormText == filterContext.RouteData.Values["action"].ToString()
            //                        && c.CanRead).FirstOrDefault();
            //        if (userPermissions == null)
            //        {
            //            //Get Configuration from Web/App Config for Auto Routing to NoPermission Controller Action
            //            var NoPermissionArea = System.Configuration.ConfigurationManager.AppSettings.Get("NoPermissionArea").ToString();
            //            var NoPermissionController = System.Configuration.ConfigurationManager.AppSettings.Get("NoPermissionController").ToString();
            //            var NoPermissionAction = System.Configuration.ConfigurationManager.AppSettings.Get("NoPermissionAction").ToString();

            //            RedirectToAction(filterContext, NoPermissionArea, NoPermissionController, NoPermissionAction);
            //        }
            //    }
            //    else
            //    {
            //        //Redirect to Home Page
            //        //Get Configuration from Web/App Config for Auto Routing to Home Page
            //        var HomeArea = System.Configuration.ConfigurationManager.AppSettings.Get("HomeArea").ToString();
            //        var HomeController = System.Configuration.ConfigurationManager.AppSettings.Get("HomeController").ToString();
            //        var HomeAction = System.Configuration.ConfigurationManager.AppSettings.Get("HomeAction").ToString();

            //        RedirectToAction(filterContext, HomeArea, HomeController, HomeAction);
            //    }
            //}
        }

        /// <summary>
        /// This method automatically redirects to specified area/controller action
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="ToController"></param>
        /// <param name="ToAction"></param>
        void RedirectToAction(ActionExecutingContext filterContext, string ToController, string ToAction)
        {
            filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary{
                        { "controller", ToController },
                                          { "action", ToAction }
                                         });
        }
    }
}
