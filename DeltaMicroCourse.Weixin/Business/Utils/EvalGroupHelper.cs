using Business;
using Symber.Web.Report;

namespace Business
{

   public static class EvalGroupHelper
   {
      public static PickListAPRptColumn Level;

      static EvalGroupHelper()
      {
         Level = new PickListAPRptColumn(APDBDef.EvalGroup.LevelPKID, ThisApp.PLKey_Level);
      }


      // 评审级别
      public static long CityLevel = 5004;
      public static long ProvinceLevel = 5005;
      public static long UnionLevel = 5006;

   }

}