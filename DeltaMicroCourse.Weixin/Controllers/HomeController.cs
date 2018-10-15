using Business;
using Business.Weixin;
using Symber.Web.Data;
using System.Web.Mvc;
using System.Linq;

namespace TheSite.Controllers
{
    public class HomeController : BaseController
    {

        // GET:  Home/Index

        public ActionResult Index()
        {
            //MenuManager.RefreshMenu();

            return View();
        }

    }
}