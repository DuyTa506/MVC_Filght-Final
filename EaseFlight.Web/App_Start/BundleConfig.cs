using System.Web.Optimization;

namespace EaseFlight.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Scripts
            bundles.Add(new ScriptBundle("~/bundles/basejs").Include(
                        "~/Scripts/user/basejs/jquery-3.4.1.min.js",
                        "~/Scripts/user/basejs/moment.js",
                        "~/Scripts/user/basejs/bootstrap.js",
                        "~/Scripts/user/basejs/owl-carousel.js",
                        "~/Scripts/user/basejs/blur-area.js",
                        "~/Scripts/user/basejs/icheck.js",
                        "~/Scripts/user/basejs/magnific-popup.js",
                        "~/Scripts/user/basejs/ion-range-slider.js",
                        "~/Scripts/user/basejs/sticky-kit.js",
                        "~/Scripts/user/basejs/fotorama.js",
                        "~/Scripts/user/basejs/bs-datepicker.js",
                        "~/Scripts/user/basejs/typeahead.js",
                        "~/Scripts/user/basejs/quantity-selector.js",
                        "~/Scripts/user/basejs/window-scroll-action.js",
                        "~/Scripts/user/basejs/fitvid.js",
                        "~/Scripts/user/basejs/custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                "~/Scripts/user/pages/home.js"));

            bundles.Add(new ScriptBundle("~/bundles/flight").Include(
                "~/Scripts/user/pages/flight.js"));
            #endregion

            #region Styles
            bundles.Add(new StyleBundle("~/Content/basecss").Include(
                      "~/Content/user/css/basecss/bootstrap.css",
                      "~/Content/user/css/basecss/font-awesome.css",
                      "~/Content/user/css/basecss/lineicons.css",
                      "~/Content/user/css/basecss/styles.css"));

            bundles.Add(new StyleBundle("~/Content/home").Include(
                      "~/Content/user/css/pages/home.css"));

            bundles.Add(new StyleBundle("~/Content/flight").Include(
                     "~/Content/user/css/pages/flight.css"));

            bundles.Add(new StyleBundle("~/Content/accessDenied").Include(
                     "~/Content/user/css/pages/accessDenied.css"));
            #endregion
        }
    }
}