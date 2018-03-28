using System.Web;
using System.Web.Optimization;

namespace GameZone.WEB
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;   //enable CDN support

            //add link to jquery on the CDN
            var jqueryCdnPath = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.12.0.min.js";

            //bundles.Add(new ScriptBundle("~/bundles/jquery",
            //            jqueryCdnPath).Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery", jqueryCdnPath).Include(
                        "~/Content/js/jquery.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Content/js/bootstrap.min.js",
                      "~/Scripts/respond.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/angularGlobal").Include(
                "~/Scripts/angular.min.js",
                      "~/Scripts/global.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/ContentScript").Include(
                "~/Content/js/magnific-popup.min.js",
                      "~/Content/js/imagesloaded.pkgd.min.js",
                      "~/Content/js/appear.js",
                      "~/Content/js/base.js",
                      "~/Content/js/smooth-scroll.min.js",
                      "~/Content/js/isotope.pkgd.min.js",
                      "~/Content/js/lightbox.min.js",
                      "~/Content/js/dofilter.js",
                      "~/Content/js/scrolla.jquery.min.js",
                      "~/Content/js/site.js",
                      "~/Content/js/notify.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/layout").Include(
                "~/Scripts/layout.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/homeIndex").Include(
                "~/Scripts/Home/Index.js",
                "~/Content/js/jquery.lazyload.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/gamesList").Include(
                "~/Scripts/Games/List.js",
                "~/Content/js/jquery.lazyload.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/homeSubscription").Include(
                "~/Scripts/Home/Subscription.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/accountLogin").Include(
                "~/Scripts/Account/Login.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/accountRegister").Include(
                "~/Scripts/Account/Register.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/accountForgotPassword").Include(
                "~/Scripts/Account/ForgotPassword.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/accountResetPassword").Include(
                "~/Scripts/Account/ResetPassword.js"
                      ));

            bundles.Add(new StyleBundle("~/bundles/contentCss").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/animate.min.css",
                      "~/Content/css/font-awesome.min.css",
                      "~/Content/css/base-stylesheet.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/css/contentCssStyle").Include(
                      "~/Content/css/style.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/contentGamesList").Include(
                      "~/Content/css/slider-pro.min.css"
                      ));
        }
    }
}
