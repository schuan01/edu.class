using System.Web;
using System.Web.Optimization;

namespace EduClass.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
           bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

           bundles.Add(new ScriptBundle("~/bundles/template").Include(
                      "~/Scripts/template/lib_jquery-ui_jquery-ui.js",
                      "~/Scripts/template/lib_jquery-toggles_toggles.js",
                      "~/Scripts/template/lib_bootstrap_js_bootstrap.js",
                      "~/Scripts/template/lib_morrisjs_morris.js",
                      "~/Scripts/template/lib_raphael_raphael.js",
                      "~/Scripts/template/lib_flot_jquery.flot.js",
                      "~/Scripts/template/lib_flot_jquery.flot.resize.js",
                      "~/Scripts/template/lib_jquery.steps_jquery.steps.js",
                      "~/Scripts/template/lib_flot-spline_jquery.flot.spline.js",
                      "~/Scripts/template/lib_jquery-knob_jquery.knob.js",
                      "~/Scripts/template/lib_jquery.gritter_jquery.gritter.js",
                      "~/Scripts/template/lib_select2_select2.js",
                      "~/Scripts/template/lib_wysihtml5x_wysihtml5x-toolbar.js",
                      "~/Scripts/template/lib_bootstrap3-wysihtml5-bower_bootstrap3-wysihtml5.all.js",
                      "~/Scripts/template/lib_wysihtml5x_wysihtml5x.js",
                      "~/Scripts/template/js_quirk.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/template").Include(
                          "~/Content/template/lib_Hover_hover.css",
                          "~/Content/template/lib_fontawesome_css_font-awesome.css",
                          "~/Content/template/lib_weather-icons_css_weather-icons.css",
                          "~/Content/template/lib_ionicons_css_ionicons.css",
                          "~/Content/template/lib_jquery-toggles_toggles-full.css",
                          "~/Content/template/lib_jquery.steps_jquery.steps.css",
                          "~/Content/template/lib_morrisjs_morris.css",
                          "~/Content/template/lib_jquery.gritter_jquery.gritter.css",
                          "~/Content/template/lib_select2_select2.css",
                          "~/Content/template/lib_bootstrap3-wysihtml5-bower_bootstrap3-wysihtml5.css",
                          "~/Content/template/css_quirk.css").IncludeDirectory("~/Content/font", "*.css", false));
        }
    }
}
