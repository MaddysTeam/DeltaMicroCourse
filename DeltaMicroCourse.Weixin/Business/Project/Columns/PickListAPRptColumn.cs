using Business;
using Symber.Web.Data;
using System;
using System.Collections.Generic;

namespace Symber.Web.Report
{

	public class PickListAPRptColumn : APRptColumn
	{

		#region [ Fields ]


		private string _innerKey;
		private bool _ajaxLoad;


		#endregion


		#region [ Constructors ]


		public PickListAPRptColumn(Int64APColumnDef columnDef, string innerKey)
			: base(columnDef)
		{
			_innerKey = innerKey;
		}


		public PickListAPRptColumn(Int64APColumnDef columnDef, string id, string title, string innerKey)
			: base(columnDef, id, title)
		{
			_innerKey = innerKey;
		}


		#endregion


		#region [ Properties ]


		public string PickListInnerKey
		{
			get { return _innerKey; }
		}


		public bool AjaxLoad
		{
			get { return _ajaxLoad; }
			set { _ajaxLoad = value; }
		}


		#endregion



		#region [ Override Implementation of APColumnEx ]



		public override APSqlWherePhrase ParseQueryWherePhrase(APRptFilterComparator comparator, params string[] values)
		{
			throw new NotImplementedException();
		}


		#endregion
	}

}