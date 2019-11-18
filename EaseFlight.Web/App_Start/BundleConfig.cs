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
                        "~/Scripts/user/basejs/window-scroll-action.js",
                        "~/Scripts/user/basejs/fitvid.js",
                        "~/Scripts/user/basejs/custom.js",
                        "~/Scripts/user/basejs/toastify.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminbasejs").Include(
                        "~/Scripts/admin/basejs/jquery.min.js",
                        "~/Scripts/admin/basejs/bootstrap.bundle.min.js",
                        "~/Scripts/admin/basejs/jquery.datatable.js",
                        "~/Scripts/admin/basejs/bs-datatable.js",
                        "~/Scripts/admin/basejs/adminlte.js",
                        "~/Scripts/admin/basejs/demo.js",
                        "~/Scripts/admin/basejs/select2.full.min.js",
                        "~/Scripts/admin/basejs/moment.min.js",
                        "~/Scripts/admin/basejs/daterangepicker.js",
                        "~/Scripts/admin/basejs/admin.js",
                        "~/Scripts/user/basejs/toastify.js"));

            bundles.Add(new ScriptBundle("~/bundles/admincontextmenu").Include(
                        "~/Scripts/admin/basejs/jquery.contextMenu.min.js",
                        "~/Scripts/admin/basejs/jquery.ui.position.js"));

            bundles.Add(new ScriptBundle("~/bundles/admincountry").Include(
                       "~/Scripts/admin/pages/country.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminplane").Include(
                       "~/Scripts/admin/pages/plane.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminairport").Include(
                       "~/Scripts/admin/pages/airport.js"));

            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                          "~/Scripts/user/pages/home.js"));

            bundles.Add(new ScriptBundle("~/bundles/flight").Include(
                        "~/Scripts/user/pages/flight.js"));

            bundles.Add(new ScriptBundle("~/bundles/account").Include(
                        "~/Scripts/user/pages/account.js"));

            bundles.Add(new ScriptBundle("~/bundles/booking").Include(
                        "~/Scripts/user/pages/booking.js"));

            bundles.Add(new ScriptBundle("~/bundles/geodata").Include(
                       "~/Scripts/user/basejs/geodatasource-cr.js",
                       "~/Scripts/user/basejs/Gettext.js"));

            bundles.Add(new ScriptBundle("~/bundles/myprofile").Include(
                        "~/Scripts/user/pages/myprofile.js"));

            bundles.Add(new ScriptBundle("~/bundles/ticket").Include(
                        "~/Scripts/user/pages/ticket.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminlogin").Include(
                        "~/Scripts/admin/pages/login.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminflight").Include(
                       "~/Scripts/admin/pages/flight.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminticket").Include(
                       "~/Scripts/admin/pages/ticket.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminaccount").Include(
                       "~/Scripts/admin/pages/account.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminchart").Include(
                       "~/Scripts/admin/basejs/chart.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/admindashboard").Include(
                       "~/Scripts/admin/pages/dashboard.js"));
            #endregion

            #region Styles
            bundles.Add(new StyleBundle("~/Content/basecss").Include(
                    "~/Content/user/css/basecss/bootstrap.css",
                      "~/Content/user/css/basecss/font-awesome.css",
                      "~/Content/user/css/basecss/lineicons.css",
                      "~/Content/user/css/basecss/styles.css",
                      "~/Content/user/css/basecss/toastify.css"));

            bundles.Add(new StyleBundle("~/Content/home").Include(
                      "~/Content/user/css/pages/home.css"));

            bundles.Add(new StyleBundle("~/Content/flight").Include(
                     "~/Content/user/css/pages/flight.css"));

            bundles.Add(new StyleBundle("~/Content/accessDenied").Include(
                     "~/Content/user/css/pages/accessDenied.css"));

            bundles.Add(new StyleBundle("~/Content/booking").Include(
                     "~/Content/user/css/pages/booking.css"));

            bundles.Add(new StyleBundle("~/Content/myprofile").Include(
                    "~/Content/user/css/pages/myprofile.css"));

            bundles.Add(new StyleBundle("~/Content/ticket").Include(
                    "~/Content/user/css/pages/ticket.css"));

            bundles.Add(new StyleBundle("~/Content/adminbasecss").Include(
                    "~/Content/admin/basecss/adminlte.min.css",
                    "~/Content/admin/basecss/datatable.css",
                     "~/Content/admin/plugins/fontawesome-free/css/all.min.css",
                     "~/Content/admin/plugins/select2/select2.min.css",
                     "~/Content/admin/plugins/select2/select2-bootstrap4.min.css",
                     "~/Content/admin/basecss/basecss.css",
                     "~/Content/admin/basecss/jquery.contextMenu.min.css",
                     "~/Content/admin/basecss/daterangepicker.css",
                     "~/Content/user/css/basecss/toastify.css"));

            bundles.Add(new StyleBundle("~/Content/adminlogin").Include(
                      "~/Content/admin/pages/login.css"));

            bundles.Add(new StyleBundle("~/Content/adminticket").Include(
                     "~/Content/admin/pages/ticket.css"));
            #endregion
        }
    }
}