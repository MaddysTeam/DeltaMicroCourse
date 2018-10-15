//using Business;
//using Symber.Web.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace TheSite.Controllers
//{

//   public class ChartController : BaseController
//   {

//      static APDBDef.UserInfoTableDef u = APDBDef.UserInfo;
//      static APDBDef.WeiXinUserInfoTableDef wu = APDBDef.WeiXinUserInfo;
//      static APDBDef.CourseInfoTableDef ci = APDBDef.CourseInfo;
//      static APDBDef.DictionaryTableDef dic = APDBDef.Dictionary;
//      static APDBDef.WeiXinPlayCountViewTableDef wxpv = APDBDef.WeiXinPlayCountView;
//      static APDBDef.WxPraiseViewTableDef wp = APDBDef.WxPraiseView;


//      public ActionResult Users() => View("User");


//      public ActionResult Course(string category) => View("Course");


//      [HttpPost]
//      public ActionResult UserCount()
//      {

//         var pcUserCount = db.UserInfoDal.ConditionQueryCount(null);
//         var weiXinUserCount = db.WeiXinUserInfoDal.ConditionQueryCount(null);

//         var list = new List<ChartViewModel>()
//         {
//            new ChartViewModel() { label = "微信用户", data = weiXinUserCount },
//            new ChartViewModel() { label = "PC用户", data = pcUserCount }
//         };


//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = list.ToArray()
//         });
//      }


//      [HttpPost]
//      public ActionResult PCUserRegionCount()
//      {
//         var result = APQuery.select(u.UserId.Count().As("totalCount"), u.ParentArea)
//                           .from(u)
//                           .group_by(u.ParentArea)
//                           .query(db, r =>
//                           {
//                              var title = u.ParentArea.GetValue(r);
//                              return new ChartViewModel()
//                              {
//                                 label = string.IsNullOrEmpty(title) ? "其他" : title,
//                                 data = r.GetInt32(r.GetOrdinal("totalCount")),
//                              };
//                           }).ToArray();


//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = result
//         });
//      }


//      [HttpPost]
//      public ActionResult WeiXinUserRegionCount()
//      {
//         var result = APQuery.select(wu.OpenId.Count().As("totalCount"), wu.Prov)
//                          .from(wu)
//                          .group_by(wu.Prov)
//                          .query(db, r =>
//                          {
//                             var title = wu.Prov.GetValue(r);
//                             return new ChartViewModel()
//                             {
//                                label = string.IsNullOrEmpty(title) ? "其他" : title,
//                                data = r.GetInt32(r.GetOrdinal("totalCount")),
//                             };
//                          }).ToArray();


//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = result
//         });
//      }


//      [HttpPost]
//      public ActionResult WeiXinUserGenderCount()
//      {
//         var result = APQuery.select(wu.OpenId.Count().As("totalCount"), wu.Gender)
//                          .from(wu)
//                          .group_by(wu.Gender)
//                          .query(db, r =>
//                          {
//                             var gender = wu.Gender.GetValue(r);
//                             var title = gender == 0 ? "未知" : gender == 1 ? "男" : "女";
//                             return new ChartViewModel()
//                             {
//                                label = title,
//                                data = r.GetInt32(r.GetOrdinal("totalCount")),
//                             };
//                          }).ToArray();


//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = result
//         });
//      }


//      [HttpPost]
//      public ActionResult CourseRegionCount()
//      {
//         var result = APQuery.select(ci.ProvinceId.Count().As("courseCount"), dic.ItemName)
//                           .from(ci, dic.JoinLeft(ci.ProvinceId == dic.ItemId)
//                   )
//                   .group_by(ci.ProvinceId, dic.ItemName)
//                   .query(db, r =>
//                    {
//                       return new ChartViewModel
//                       {
//                          label = dic.ItemName.GetValue(r),
//                          data = r.GetInt32(r.GetOrdinal("courseCount"))
//                       };
//                    }).ToArray();


//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = result
//         });

//      }

//      [HttpPost]
//      public ActionResult CourseCityRegionCount(int provinceId)
//      {
//         var result = APQuery.select(ci.ProvinceId.Count().As("courseCount"), dic.ItemName)
//                           .from(ci, dic.JoinLeft(ci.CityId == dic.ItemId)
//                   )
//                   .group_by(ci.CityId, dic.ItemName)
//                   .where(ci.ProvinceId == provinceId)
//                  // .order_by(ci.ProvinceId.Count().Desc)
//                   .query(db, r =>
//                   {
//                      return new ChartViewModel
//                      {
//                         label = dic.ItemName.GetValue(r),
//                         data = r.GetInt32(r.GetOrdinal("courseCount"))
//                      };
//                   }).ToArray();


//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = result
//         });

//      }


//      [HttpPost]
//      public ActionResult CourseSubjectCount()
//      {
//         var result = APQuery.select(ci.SubjectId.Count().As("courseCount"), ci.SubjectName)
//                            .from(ci)
//                            .group_by(ci.SubjectId, ci.SubjectName)
//                            //.order_by(ci.SubjectId.Count().Desc)
//                            .query(db, r =>
//                            {
//                               return new ChartViewModel
//                               {
//                                  label = ci.SubjectName.GetValue(r),
//                                  data = r.GetInt32(r.GetOrdinal("courseCount"))
//                               };
//                            }).ToArray();


//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = result
//         });
//      }


//      [HttpPost]
//      public ActionResult CourseGradeCount()
//      {
//         var result = APQuery.select(ci.GradeId.Count().As("courseCount"), ci.GradeName)
//                            .from(ci)
//                            .group_by(ci.GradeId, ci.GradeName)
//                            //.order_by(ci.GradeId.Count().Desc)
//                            .query(db, r =>
//                            {
//                               return new ChartViewModel
//                               {
//                                  label = ci.GradeName.GetValue(r),
//                                  data = r.GetInt32(r.GetOrdinal("courseCount"))
//                               };
//                            }).ToArray();


//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = result
//         });
//      }


//      [HttpPost]
//      public ActionResult CoursePlayCount()
//      {
//         var pcSideClickCount = db.CourseInfoDal.ConditionQuery(null, null, null, null).Sum(x => x.PlayCount);
//         var weiXinSideClickCount = db.WeiXinPlayCountViewDal.ConditionQuery(null, null, null, null).Sum(x => x.WeiXinPlayCount);

//         var list = new List<ChartViewModel>()
//         {
//            new ChartViewModel() { label = "微信播放", data = weiXinSideClickCount },
//            new ChartViewModel() { label = "PC端播放", data = pcSideClickCount }
//         };

//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = list.ToArray()
//         });
//      }


//      [HttpPost]
//      public ActionResult Top10PCCoursePlayCount()
//      {
//         var list = new List<ChartViewModel>();

//         var courses = db.CourseInfoDal.ConditionQuery(null, ci.PlayCount.Desc, 10, 0);

//         courses.ForEach(item =>
//         {
//            list.Add(
//                new ChartViewModel() { label = item.Name, data = item.PlayCount }
//               );
//         });

//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = list.ToArray()
//         });
//      }


//      [HttpPost]
//      public ActionResult Top10WeiXinCoursePlayCount()
//      {
//         var list = new List<ChartViewModel>();

//         var result = APQuery
//            .select(ci.Name, wxpv.WeiXinPlayCount)
//            .from(ci, wxpv.JoinInner(ci.VideoId == wxpv.WksId))
//            .order_by(wxpv.WeiXinPlayCount.Desc)
//            .skip(0)
//            .take(10)
//            .query(db, r =>
//                new ChartViewModel()
//                {
//                   label = ci.Name.GetValue(r),
//                   data = wxpv.WeiXinPlayCount.GetValue(r)
//                }
//            ).ToArray();


//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = result
//         });
//      }


//      [HttpPost]
//      public ActionResult CoursePraiseCount()
//      {
//         var pcSidePraiseCount = db.CourseInfoDal.ConditionQuery(null, null, null, null).Sum(x => x.PraiseCount);
//         var weiXinSidePraiseCount = db.WxPraiseViewDal.ConditionQuery(null, null, null, null).Sum(x => x.PariseNum);

//         var list = new List<ChartViewModel>()
//         {
//            new ChartViewModel() { label = "微信点赞", data = weiXinSidePraiseCount },
//            new ChartViewModel() { label = "PC端点赞", data = pcSidePraiseCount }
//         };

//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = list.ToArray()
//         });
//      }


//      [HttpPost]
//      public ActionResult Top10PCCoursePraiseCount()
//      {
//         var list = new List<ChartViewModel>();

//         var courses = db.CourseInfoDal.ConditionQuery(null, ci.PraiseCount.Desc, 10, 0);

//         courses.ForEach(item =>
//         {
//            list.Add(
//                new ChartViewModel() { label = item.Name, data = item.PraiseCount }
//               );
//         });

//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = list.ToArray()
//         });
//      }


//      [HttpPost]
//      public ActionResult Top10WeiXinCoursePraiseCount()
//      {
//         var list = new List<ChartViewModel>();

//         var result = APQuery
//            .select(ci.Name, wp.PariseNum)
//            .from(ci, wp.JoinInner(ci.VideoId == wp.WksId))
//           // .order_by(wp.PariseNum.Desc)
//            .skip(0)
//            .take(10)
//            .query(db, r =>
//                new ChartViewModel()
//                {
//                   label = ci.Name.GetValue(r),
//                   data = wp.PariseNum.GetValue(r)
//                }
//            ).ToArray();


//         return Json(new
//         {
//            result = AjaxResults.Success,
//            data = result
//         });
//      }

//   }


//   public class ChartViewModel
//   {
//      public string label { get; set; }
//      public int data { get; set; }
//   }

//}