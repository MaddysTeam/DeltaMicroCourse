using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSite.Models
{

    public class CategoryModel
    {

         public int Id { get; set; }
         public string Name { get; set; }
         public string LinkUrl { get; set; }
         //public CategoryType Type { get; set; }
    }


	public class CategoryItemModel
	{

		public int itemId { get; set;}
		public string ItemName { get; set; }
		public string LinkUrl { get; set; }

	}

}
