//using Symber.Web.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TheSite.Models;

//namespace Business
//{

//    public interface IHandler<T, V>
//    {
//        void Handle(T t, V v);
//    }


//    public class CourseSearchHandler : IHandler<APSqlSelectCommand, SearchOption>
//    {
//        //protected APDBDef.CourseInfoTableDef ci = APDBDef.CourseInfo;
//        //protected APDBDef.WeiXinFavoriteInfoTableDef wxf = APDBDef.WeiXinFavoriteInfo;

//        public virtual void Handle(APSqlSelectCommand query, SearchOption option)
//        {
//            throw new NotImplementedException();
//        }

//    }


//    public class CourseHotListSearchHandler : CourseSearchHandler
//    {

//        public override void Handle(APSqlSelectCommand query, SearchOption option)
//        {
//            query.order_by(ci.PlayCount.Desc);
//        }

//    }


//    public class CoursePraiseListSearchHandler : CourseSearchHandler
//    {

//        public override void Handle(APSqlSelectCommand query, SearchOption option)
//        {
//            query.order_by(ci.PraiseCount.Desc);
//        }

//    }


//    public class CourseByRegionListSearchHandler : CourseSearchHandler
//    {

//        public override void Handle(APSqlSelectCommand query, SearchOption option)
//        {
//            if (option == null) return;

//            var id = option.SearchTerm;
//            var provinceId = Convert.ToInt32(id);
//            query.where_and(ci.ProvinceId == provinceId).order_by(ci.CreateDate.Desc);
//        }

//    }


//    public class CourseNameSearchHandler : CourseSearchHandler
//    {

//        public override void Handle(APSqlSelectCommand query, SearchOption option)
//        {
//            if (option == null) return;

//            var name = option.SearchTerm;
//            if (!string.IsNullOrEmpty(name))
//                query.where_and(ci.Name.Match(name));
//        }

//    }

//    public class CourseMyFavoriteListSearchHandler : CourseSearchHandler
//    {

//        public override void Handle(APSqlSelectCommand query, SearchOption option)
//        {
//			if (option == null || option.CurrentOpenId == null)
//				return;

//            query.where_and(wxf.OpenId == option.CurrentOpenId);
//        }

//    }

//    public class CourseSubejctSearchHandler : CourseSearchHandler
//    {

//        public override void Handle(APSqlSelectCommand query, SearchOption option)
//        {
//            query.where_and(ci.SubjectId == option.CategoryItemId);

//            if (!string.IsNullOrEmpty(option.SearchTerm))
//            {
//                query.where_and(ci.Name.Match(option.SearchTerm));
//            }
//        }

//    }

//    public class CourseSchoolNameSearchHandler : CourseSearchHandler
//    {

//        public override void Handle(APSqlSelectCommand query, SearchOption option)
//        {
//            query.where_and(ci.SchoolName .Match( option.SearchTerm));
//        }

//    }


//    public class CourseGradeNameSearchHandler : CourseSearchHandler
//    {

//        public override void Handle(APSqlSelectCommand query, SearchOption option)
//        {
//            query.where_and(ci.GradeId == option.CategoryItemId);

//            if (!string.IsNullOrEmpty(option.SearchTerm))
//            {
//                query.where_and(ci.Name.Match(option.SearchTerm));
//            }
//        }

//    }

//	public class CourseAuthorNameSearchHandler : CourseSearchHandler
//	{

//		public override void Handle(APSqlSelectCommand query, SearchOption option)
//		{
//			if (!string.IsNullOrEmpty(option.SearchTerm))
//			{
//				query.where_and(ci.TeacherName.Match(option.SearchTerm));
//			}
//		}

//	}



//	public static class HandleManager
//    {

//        public static Dictionary<SearchOptionType, CourseSearchHandler> SearchHandlers = new Dictionary<SearchOptionType, CourseSearchHandler>
//        {
//             {SearchOptionType.HotList, new CourseHotListSearchHandler()  },
//             {SearchOptionType.PraiseList, new CoursePraiseListSearchHandler() },
//             {SearchOptionType.LatestListByRegion, new CourseByRegionListSearchHandler() },
//             {SearchOptionType.CourseName, new CourseNameSearchHandler() },
//             {SearchOptionType.MyFavorite, new CourseMyFavoriteListSearchHandler() },
//             {SearchOptionType.Subject, new CourseSubejctSearchHandler() },
//             {SearchOptionType.School, new CourseSchoolNameSearchHandler() },
//             {SearchOptionType.Grade, new CourseGradeNameSearchHandler() },
//				 {SearchOptionType.Author, new CourseAuthorNameSearchHandler() },
//		  };

//    }


//}
