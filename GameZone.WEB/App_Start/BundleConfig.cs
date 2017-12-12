﻿using System.Web;
using System.Web.Optimization;

namespace GameZone.WEB
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
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

            bundles.Add(new ScriptBundle("~/bundles/baseJS").Include(
                "~/Content/js/magnific-popup.min.js",
                      "~/Content/js/imagesloaded.pkgd.min.js",
                      "~/Content/js/appear.js",
                      "~/Content/js/base.js",
                      "~/Content/js/smooth-scroll.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/silteringJs").Include(
                "~/Content/js/isotope.pkgd.min.js",
                "~/Content/js/lightbox.min.js",
                "~/Content/js/dofilter.js",
                "~/Content/js/scrolla.jquery.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/Home-IndexPageJs").Include(
               "~/Content/js/notify.min.js",
               "~/Scripts/Home/Index.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/Games-GamePlayPageJs").Include(
                "~/Content/js/jquery.sliderPro.min.js",
                "~/Content/js/jquery.fancybox.pack.js",
                "~/Content/js/games.js",
                "~/Scripts/Games/GamePlay.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/Games-ListPageJs").Include(
                "~/Content/js/jquery.sliderPro.min.js",
                "~/Content/js/jquery.fancybox.pack.js",
                "~/Content/js/games.js",
                "~/Scripts/Games/List.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/customjs").Include(
                "~/Content/js/site.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/angular.min.js",
                      "~/Scripts/global.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/animate.min.css",
                      "~/Content/css/font-awesome.min.css",
                      "~/Content/css/base-stylesheet.css",
                      "~/Content/css/style.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/pageSpecificCss").Include(
                      "~/Content/js/fancybox/jquery.fancybox.css",
                      "~/Content/css/slider-pro.min.css",
                      "~/Content/css/font-awesome.min.css",
                      "~/Content/css/base-stylesheet.css",
                      "~/Content/css/style.css"
                      ));
        }
    }
}
