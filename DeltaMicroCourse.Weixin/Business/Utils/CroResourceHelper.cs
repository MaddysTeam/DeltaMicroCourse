using Symber.Web.Report;
using System.Collections.Generic;

namespace Business
{

   public static class CroResourceHelper
   {

      static CroResourceHelper()
      {
      }

      // 作品状态
      public static long StateWait = 10351;
      public static long StateAllow = 10352;
      public static long StateDeny = 10353;
      public static long StateDelete = 10359;

      // 搜索类型
      public static string Hot = "rmyc";
      public static string Latest = "zxyc";

      // 作品类型
      public static long MicroClass = 5010;
      public static long MicroCourse = 5011;

      // 作品省份
      public static long Zhejiang = 1312;
      public static long Jiangsu = 1181;
      public static long Shanghai = 1161;
      public static long Anhui = 1425;

      // 公开状态
      public static long Public = 10450;
      public static long Private = 10451;

      // 下载状态
      public static long AllowDownload = 10452;
      public static long DenyDownload = 10453;

      // 奖项
      public static long WinLevelSpecial = 208;
      public static long WinLevel1 = 205;
      public static long WinLevel2 = 206;
      public static long WinLevel3 = 207;

   }

}