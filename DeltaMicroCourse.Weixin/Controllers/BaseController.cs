using Business;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.Mvc;

namespace TheSite.Controllers
{
	public class BaseController : Controller
	{
		#region [ DB ]


		private APDBDef _db;


		public APDBDef db
		{
			get
			{
				if (_db == null)
					_db = new APDBDef();
				return _db;
			}
			private set
			{
				_db = value;
			}
		}


		#endregion


		#region [ Ajax ]


		protected void ThrowNotAjax()
		{
			if (!Request.IsAjaxRequest())
				throw new NotSupportedException("Action must be Ajax call.");
		}


		#endregion

	}
}