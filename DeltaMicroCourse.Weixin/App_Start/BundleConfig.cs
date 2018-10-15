using System.Web;
using System.Web.Optimization;

namespace TheSite
{
	public class BundleConfig
	{

		public static void RegisterBundles(BundleCollection bundles)
		{
			//JS jquery
			bundles.Add(new Bundle("~/js/jquery").Include("~/Assets/js/jquery-2.1.3.min.js"));
			//JS jquery mobile
			bundles.Add(new Bundle("~/js/jquery/mobile").Include("~/Assets/js/jquery.mobile.custom.js"));
			//JS IScroll
			bundles.Add(new Bundle("~/js/IScroll").Include("~/Assets/js/iscroll.js"));
			//Angile.Themes
			bundles.Add(new Bundle("~/js/angile")
							.Include("~/Assets/js/agile.js")
							.Include("~/Assets/js/agile.exmobi.js")
							.Include("~/Assets/js/exmobi.js"));
			//JS seedsui
			bundles.Add(new Bundle("~/js/app/seedsui").Include("~/Assets/js/app.seedsui.js"));
			//JS app
			bundles.Add(new Bundle("~/js/appJs").Include("~/Assets/js/app.js"));
         //CSS bootstrap
         bundles.Add(new StyleBundle("~/css/bootstrap").Include("~/Assets/css/bootstrap.min.css"));
         //JS bootstrap
         bundles.Add(new Bundle("~/js/bootstrap").Include("~/Assets/js/bootstrap.min.js"));
         //JS flot
         bundles.Add(new Bundle("~/js/flot")
                      .Include("~/Assets/js/jquery.flot.min.js")
                      .Include("~/Assets/js/jquery.flot.barnumbers.js")
                      .Include("~/Assets/js/app.flot.js"));
      }
	}
}
