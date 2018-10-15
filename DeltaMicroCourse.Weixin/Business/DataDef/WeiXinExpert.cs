using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
   public partial class WeiXinExpert
   {
      [Display(Name = "专家")]
      public string UserName { get; set; }

      [Display(Name = "微信号（昵称）")]
      public string WeiXinName { get; set; }

   }
}
