using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheSite.Controllers
{

	public class NoticeController : BaseController
	{

		// GET: Notice/Details

		public ActionResult Details(int noticeId)
		{
			var notice= db.NoticeDal.PrimaryGet(noticeId);

			return View(notice);
		}

	}

}