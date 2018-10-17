using Symber.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSite.Models;

namespace Business
{

   public interface IHandler<T, V>
   {
      void Handle(T t, V v);
   }


   public class CourseSearchHandler : IHandler<APSqlSelectCommand, SearchOption>
   {
      protected APDBDef.CroResourceTableDef cr = APDBDef.CroResource;
      protected APDBDef.MicroCourseTableDef mc = APDBDef.MicroCourse;
      protected APDBDef.WeiXinFavoriteTableDef wxf = APDBDef.WeiXinFavorite;
      protected APDBDef.ResCompanyTableDef rc = APDBDef.ResCompany;

      public virtual void Handle(APSqlSelectCommand query, SearchOption option)
      {
         throw new NotImplementedException();
      }

   }


   public class CourseHotListSearchHandler : CourseSearchHandler
   {

      public override void Handle(APSqlSelectCommand query, SearchOption option)
      {
         query.order_by(mc.PlayCount.Desc);
      }

   }


   public class CoursePraiseListSearchHandler : CourseSearchHandler
   {

      public override void Handle(APSqlSelectCommand query, SearchOption option)
      {
         query.order_by(cr.PraiseCount.Desc);
      }

   }


   public class CourseByRegionListSearchHandler : CourseSearchHandler
   {

      public override void Handle(APSqlSelectCommand query, SearchOption option)
      {
         if (option == null) return;

         var id = option.SearchTerm;
         var provinceId = Convert.ToInt32(id);
         query.where_and(cr.ProvinceId == provinceId).order_by(cr.CreatedTime.Desc);
      }

   }


   public class CourseNameSearchHandler : CourseSearchHandler
   {

      public override void Handle(APSqlSelectCommand query, SearchOption option)
      {
         if (option == null) return;

         var name = option.SearchTerm;
         if (!string.IsNullOrEmpty(name))
            query.where_and(mc.CourseTitle.Match(name));
      }

   }

   public class CourseMyFavoriteListSearchHandler : CourseSearchHandler
   {

      public override void Handle(APSqlSelectCommand query, SearchOption option)
      {
         if (option == null || option.CurrentOpenId == null)
            return;

         query.where_and(wxf.OpenId == option.CurrentOpenId);
      }

   }

   public class CourseSubejctSearchHandler : CourseSearchHandler
   {

      public override void Handle(APSqlSelectCommand query, SearchOption option)
      {
         //TODO:
         query.where_and(cr.SubjectPKID == option.CategoryItemId);

         if (!string.IsNullOrEmpty(option.SearchTerm))
         {
            query.where_and(mc.CourseTitle.Match(option.SearchTerm));
         }
      }

   }

   public class CourseSchoolNameSearchHandler : CourseSearchHandler
   {

      public override void Handle(APSqlSelectCommand query, SearchOption option)
      {
         query.where_and(rc.CompanyName.Match(option.SearchTerm));
      }

   }


   public class CourseGradeNameSearchHandler : CourseSearchHandler
   {

      public override void Handle(APSqlSelectCommand query, SearchOption option)
      {
         query.where_and(cr.GradePKID == option.CategoryItemId);

         if (!string.IsNullOrEmpty(option.SearchTerm))
         {
            query.where_and(mc.CourseTitle.Match(option.SearchTerm));
         }
      }

   }

   public class CourseAuthorNameSearchHandler : CourseSearchHandler
   {

      public override void Handle(APSqlSelectCommand query, SearchOption option)
      {
         if (!string.IsNullOrEmpty(option.SearchTerm))
         {
            query.where_and(cr.Author.Match(option.SearchTerm));
         }
      }

   }



   public static class HandleManager
   {

      public static Dictionary<SearchOptionType, CourseSearchHandler> SearchHandlers = new Dictionary<SearchOptionType, CourseSearchHandler>
        {
             {SearchOptionType.HotList, new CourseHotListSearchHandler()  },
             {SearchOptionType.PraiseList, new CoursePraiseListSearchHandler() },
             {SearchOptionType.LatestListByRegion, new CourseByRegionListSearchHandler() },
             {SearchOptionType.CourseName, new CourseNameSearchHandler() },
             {SearchOptionType.MyFavorite, new CourseMyFavoriteListSearchHandler() },
             {SearchOptionType.Subject, new CourseSubejctSearchHandler() },
             {SearchOptionType.School, new CourseSchoolNameSearchHandler() },
             {SearchOptionType.Grade, new CourseGradeNameSearchHandler() },
             {SearchOptionType.Author, new CourseAuthorNameSearchHandler() },
        };

   }


}
