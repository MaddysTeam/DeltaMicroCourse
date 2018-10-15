using System;
using System.IO;
using System.Security.Cryptography;

namespace Business
{

   public static class FileHelper
   {

      /// <summary>
      /// Convert stream to MD5 string 
      /// </summary>
      /// <param name="stream">stream</param>
      /// <returns>md5 string</returns>
      public static string ConvertToMD5(Stream stream)
      {
         if (stream == null)
            throw new ArgumentNullException("stream cannot be null");

         using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
         {
            return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
         }
      }

      public static string HtmlExtName = ".HTML";

   }

}