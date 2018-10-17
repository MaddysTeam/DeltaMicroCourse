using Symber.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business
{

   public class PickListHelper
   {

      static APDBDef.ResPickListTableDef rp = APDBDef.ResPickList;
      static APDBDef.ResPickListItemTableDef rpi = APDBDef.ResPickListItem;

      /// <summary>
      /// get all pick list items in cache
      /// </summary>
      /// <returns></returns>
      public static List<ResPickListItem> GetItems()
      {
         var dics = Business.Cache.ThisAppCache.GetCache<List<ResPickListItem>>();
         if (dics == null)
         {
            dics = APBplDef.ResPickListItemBplBase.GetAll();
            Business.Cache.ThisAppCache.SetCache(dics);
         }

         return dics;
      }

      /// <summary>
      ///  get name by using picklist item id
      /// </summary>
      /// <param name="itemId"></param>
      /// <returns></returns>
      public static string GetName(long itemId)
      {
         var items = GetItems();
         var isExists = items.Exists(x => x.PickListItemId == itemId);

         return isExists ? items.Find(x => x.PickListItemId == itemId).Name : null;
      }


      /// <summary>
      /// Get dic items by innerkey witch locate in app settings
      /// </summary>
      public static Dictionary<long,string> GetItemsByPKID(string innerKey)
      {
         var items = Business.Cache.ThisAppCache.GetCache<Dictionary<long, string>>(innerKey);
         if (items == null)
         {
            var subquery = APQuery.select(rp.PickListId).from(rp).where(rp.InnerKey == innerKey);
            items=APBplDef.ResPickListItemBpl.ConditionQuery(rpi.PickListId.In(subquery), null).ToDictionary(x=>x.PickListItemId,y=>y.Name);
            Business.Cache.ThisAppCache.SetCache(items,ThisApp.StableCacheMinutes,innerKey);
         }

         return items; 
      }

   }

}