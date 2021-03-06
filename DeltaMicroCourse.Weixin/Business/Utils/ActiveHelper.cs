﻿using Symber.Web.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business
{

   public static class ActiveHelper
   {

      static ActiveHelper()
      {
      }

      public static List<Active> All => APBplDef.ActiveBpl.GetAll();

      // 项目级别
      public static long CountryLevel = 10217;
      public static long CityLevel = 10218;
      public static long AreaLevel = 10219;
      public static long County = 10220;

      // 学期
      public static long Semester1 = 10454;
      public static long Semester2 = 10455;

      // 公开状态
      public static long Public = 10450;
      public static long Private = 10451;

      // 下载状态
      public static long AllowDownload = 10452;
      public static long DenyDownload = 10453;

   }

}