﻿using Business;
using Symber.Web.Report;

namespace Business
{

   public static class IndicationHelper
   {
      //public static PickListAPRptColumn Level;
      //public static PickListAPRptColumn Type;

      static IndicationHelper()
      {
      //   Level = new PickListAPRptColumn(APDBDef.Indication.LevelPKID, ThisApp.PLKey_Level);
      //   Type = new PickListAPRptColumn(APDBDef.Indication.TypePKID,ThisApp.PLKey_IndicationType);
      }

      public static int ProvinceLevel=5005;
      public static int CityLevel = 5004;
      public static int UnionLevel = 5006;

   }

}