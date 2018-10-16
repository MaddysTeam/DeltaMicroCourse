using Business;
using Business.Weixin.Utilities;
using Symber.Web.Data;
using System.Linq;
using System.Web.Mvc;
using TheSite.Models;
using System;
using System.Collections.Generic;
using Senparc.Weixin.MP.AdvancedAPIs.User;

namespace TheSite.Controllers
{

   [HandleError]
   public class CourseController : BaseController
   {
      static APDBDef.WeiXinUserInfoTableDef wxu = APDBDef.WeiXinUserInfo;
      static APDBDef.CroResourceTableDef cr = APDBDef.CroResource;
      static APDBDef.MicroCourseTableDef mc = APDBDef.MicroCourse;
      static APDBDef.WeiXinPraiseTableDef wp = APDBDef.WeiXinPraise;
      static APDBDef.WeiXinFavoriteTableDef wxf = APDBDef.WeiXinFavorite;
      static APDBDef.WeiXinPlayCountTableDef wxp = APDBDef.WeiXinPlayCount;
      

      string requestOpenIdUrl = OAuth2Helper.ProcessGetOAuthUrl(WeixinConfigHelper.AuthReturnUrl);

      string OpenId
      {
         get
         {
            if (Session["openId"] == null)
               return null;
            else
               return Session["openId"].ToString();
         }
      }


      UserInfoJson UserJson
      {
         get
         {
            if (Session["userinfo"] == null)
               return null;
            else
               return Session["userinfo"] as UserInfoJson;
         }
      }


      // GET:  Course/Index

      //[GetOpenId]
      public ActionResult Index()
      {
         var notExist = APBplDef.WeiXinUserInfoBpl.ConditionQuery(wxu.OpenId == OpenId, null, null, null).FirstOrDefault() == null;
         if (notExist)
         {
            db.WeiXinUserInfoDal.Insert(
               new WeiXinUserInfo
               {
                  OpenId = OpenId,
                  CreateDate = DateTime.Now,
                  Gender = UserJson.sex,
                  City = UserJson.city,
                  Country = UserJson.country,
                  Prov = UserJson.province,
                  NickName = UserJson.nickname
               });
         }


         return RedirectToAction("Home");
      }


      // GET:  Course/Home

     // [OAuth]
      public ActionResult Home()
      {
         var wxe = APDBDef.WeiXinExpert;

         ViewBag.IsExpert = db.WeiXinExpertDal.ConditionQueryCount(wxe.OpenId == OpenId) > 0;


         ViewBag.Notices = db.NoticeDal.ConditionQuery(null, null, null, null);


         return View();
      }


      // GET:  Course/List

     // [OAuth]
      public ActionResult List()
      {
         return View();
      }


      // GET:  Course/FavoriteList

      [OAuth]
      public ActionResult FavoriteList()
      {
         return View();
      }


      // GET:  Course/CategoryList
      // GET:  Course/CategoryItemList
      // POST-Ajax:  Course/CategoryItemList  （暂时只有学科和年纪使用statickeys分类，其余的直接通过search搜索数据库）

      [OAuth]
      public ActionResult CategoryList()
      {
         var items = StaticKeys.Categories
          .Select(item =>
          {
             if (!StaticKeys.SourceKeys.ContainsKey(item.Value))
                return null;
             var sourceType = StaticKeys.SourceKeys[item.Value];
             return new TheSite.Models.CategoryModel
             {
                Id = item.Key,
                Name = item.Value,
                LinkUrl = sourceType == StaticKeys.SourceEnum.fromKeys ?
                   Url.Action("CategoryItemList", "Course", new { CategoryId = item.Key, SearchType = item.Key }) :
                   Url.Action("Search", "Course", new { SearchType = item.Key, CategoryId = item.Key, CategoryItemId = item.Key }),
             };
          }
          ).ToList();


         return View(items);
      }

      [OAuth]
      public ActionResult CategoryItemList(int categoryId, int searchType)
      {
         return View();
      }

      [HttpPost]
      public JsonResult CategoryItemList(int? current, int? categoryId, int? searchType)
      {
         var items = StaticKeys.CategoryItems[categoryId.Value]
             .Select(x => new CategoryItemModel
             {
                itemId = x.Key,
                ItemName = x.Value,
                LinkUrl = Url.Action("Search", "Course", new { SearchType = searchType, CategoryItemId = x.Key })
             });

         if (current > 0)
         {
            items = items.Skip(Paging.BlockListPageSize).Take(Paging.BlockListPageSize);
         }
         else
         {
            items = items.Take(Paging.BlockListPageSize);
         }

         return Json(new
         {
            rows = items,
            current = current + 1,
         });
      }


      // GET:  Course/Search
      // POST-Ajax:  Course/Search

     // [OAuth]
      public ActionResult Search(SearchOption option)
      {
         return View(option);
      }

      [HttpPost]
      public ActionResult Search(int current, SearchOption option)
      {
         var cf = APDBDef.Files;
         var rc = APDBDef.ResCompany;

         option.CurrentOpenId = OpenId;

         var query = APQuery.select(cr.CrosourceId, cr.Title, cr.Author, cr.FavoriteCount, cr.ProvinceId, cr.AreaId, cr.PraiseCount,
            cr.AuthorCompany, cr.Description, cr.CreatedTime,//cr.ViewCount, cr.CommentCount, cr.DownCount //cr.FileExtName
            mc.CourseId, mc.CourseTitle, mc.PlayCount, cf.FilePath,rc.CompanyName
            )
            .from(cr,
                  mc.JoinInner(mc.ResourceId == cr.CrosourceId),
                  rc.JoinInner(rc.CompanyId == cr.CompanyId),
                  cf.JoinLeft(cf.FileId == mc.CoverId),
                  wxf.JoinLeft(wxf.ResId == cr.CrosourceId & wxf.OpenId == OpenId),
                  wxp.JoinLeft(wxp.ResourceId == cr.CrosourceId)
                  //a.JoinInner(a.ActiveId==cr.ActiveId)
                  )
            .where(cr.StatePKID == CroResourceHelper.StateAllow & cr.PublicStatePKID == CroResourceHelper.Public) // 审核通过和公开的作品
            .order_by(cr.ActiveId.Desc)
            .primary(cr.CrosourceId);

         if (HandleManager.SearchHandlers.ContainsKey(option.SearchType))
         {
            HandleManager.SearchHandlers[option.SearchType].Handle(query, option);
         }

         //获得查询的总数量

         var total = db.ExecuteSizeOfSelect(query);

         if (current * Paging.PageSize > total)
         {
            return Json(new
            {
               current = current,
            });
         }

         query.skip(current > 0 ? current * Paging.PageSize : 0)
                   .take(Paging.PageSize);


         //查询结果集

         var result = query.query(db, r =>
         {
            return new CourseViewModel
            {
               ImagePath = cf.FilePath.GetValue(r),
               Name = mc.CourseTitle.GetValue(r),
               TeacherName = cr.Author.GetValue(r),// ci.TeacherName.GetValue(r),
               PlayCount = mc.PlayCount.GetValue(r),  //TODO:ci.PlayCount.GetValue(r) + wxpv.WeiXinPlayCount.GetValue(r, "playCount"),
               PraiseCount =cr.PraiseCount.GetValue(r),//TODO: ci.PraiseCount.GetValue(r) + wp.PariseNum.GetValue(r),
               SchoolName = rc.CompanyName.GetValue(r),
               //VideoId = 0,
               LinkUrl = Url.Action("Details", new { resId = cr.CrosourceId.GetValue(r) }),
               CreateDate = DateTime.Now,
            };
         }).ToList();


         return Json(new
         {
            rows = result,
            current = current + 1,
         });
      }


      // GET:  Course/Details
      // POST-Ajax:  Course/Details

      [OAuth]
      public ActionResult Details(int? resId)
      {
         if (resId == null)
         {
            return RedirectToAction("List");
         }

         //var courseInfo = APQuery.select(ci.Name, ci.PlayCount, ci.PraiseCount, ci.VideoId,
         //                                                        ci.SchoolName, ci.SubjectName, ci.TeacherName, ci.GradeName, ci.SubjectId,
         //                                                        ci.CreateDate, cim.PicName, wp.PariseNum, v.VideoName, wxpv.WeiXinPlayCount.As("playCount"), wxf.WksId)
         //                                 .from(ci,
         //                                          cim.JoinLeft(cim.VideoId == ci.VideoId),//关联获取视频图片
         //                                          v.JoinInner(v.VideoId == ci.VideoId), //关联获取视频地址
         //                                          wp.JoinLeft(wp.WksId == ci.VideoId), // 关联获取微信点赞数量
         //                                                           wxpv.JoinLeft(wxpv.WksId == ci.VideoId),
         //                                          wxf.JoinLeft(wxf.WksId == ci.VideoId & wxf.OpenId == OpenId) // 关联获取微信收藏表
         //                                          )
         //                                 .where(ci.VideoId == videoId)
         //                                 .query(db, r =>
         //                                 {
         //                                    return new CourseInfo
         //                                    {
         //                                       Name = ci.Name.GetValue(r),
         //                                       PlayCount = ci.PlayCount.GetValue(r) + wxpv.WeiXinPlayCount.GetValue(r, "playCount"),
         //                                       PraiseCount = ci.PraiseCount.GetValue(r) + wp.PariseNum.GetValue(r),
         //                                       SchoolName = ci.SchoolName.GetValue(r),
         //                                       GradeName = ci.GradeName.GetValue(r),
         //                                       SubjectName = ci.SubjectName.GetValue(r),
         //                                       TeacherName = ci.TeacherName.GetValue(r),
         //                                       VideoId = ci.VideoId.GetValue(r),
         //                                       ImagePath = string.Format("{0}/{1}", ThisApp.UploadFilePath, cim.PicName.GetValue(r)),
         //                                       VideoPath = string.Format("{0}/{1}", ThisApp.UploadFilePath, v.VideoName.GetValue(r)),
         //                                       CreateDate = ci.CreateDate.GetValue(r),
         //                                       IsFavorite = wxf.WksId.GetValue(r) > 0
         //                                    };
         //                                 }).FirstOrDefault();

         //var viewModel = new CourseDetailsViewModel { CourseInfo = courseInfo };


         return View();
      }

      [HttpPost]
      public ActionResult Details(int videoId)
      {
         //var items = APQuery.select(cim.VideoId, cim.PicName)
         //                            .from(cim, ci.JoinLeft(ci.VideoId == cim.VideoId))
         //                            .where(cim.VideoId < videoId & ci.PrizeType > 0)
         //                            .order_by(cim.VideoId.Desc)
         //                            .take(Paging.PageSize)
         //                            .query(db, r =>
         //                            {
         //                               videoId = cim.VideoId.GetValue(r);
         //                               return new CourseInfo
         //                               {
         //                                  VideoId = videoId,
         //                                  ImagePath = string.Format("{0}/{1}", ThisApp.UploadFilePath, cim.PicName.GetValue(r)),
         //                                  LinkUrl = Url.Action("Details", new { videoId = videoId })
         //                               };
         //                            }).ToList();


         return Json(new
         {
           // items
         });
      }


      // Post-Ajax:  Course/PraiseVideo

      [HttpPost]
      public ActionResult PraiseResource(long resId)
      {
         var wxp = APDBDef.WeiXinPraise;

         var isNotPraised = db.WeiXinPraiseDal.ConditionQueryCount(wxp.ResId == resId & wxp.OpenId == OpenId) == 0;
         if (isNotPraised)
         {
            db.WeiXinPraiseDal.Insert(new WeiXinPraise
            {
               //Attitude = 1,
               //OpenId = OpenId,
               //PraiseTime = DateTime.Now,
               //WksId = videoId
            });
         }


         return Json(new
         {
            isSuccess = isNotPraised // 如果重复点赞，暂时设定为已经点赞，点赞失败
         });
      }


      // Post-Ajax:  Course/PlayVideo

      [HttpPost]
      public ActionResult Play(int courseId)
      {
         var isNotPlayed = db.WeiXinPlayCountDal.ConditionQueryCount(wxp.CourseId == courseId & wxp.OpenId == OpenId) == 0;
         if (isNotPlayed)
         {
            db.WeiXinPlayCountDal.Insert(
            new WeiXinPlayCount
            {
               //WeiXinPlayCount = 1,
               //OpenId = OpenId,
               //WksId = videoId
            });
         }
         else
         {
            //APQuery.update(wxp)
            //              .set(wxp.WeiXinPlayCount, APSqlRawExpr.Expr("weixin_play_count +1"))
            //                      .where(wxp.WksId == videoId & wxp.OpenId == OpenId)
            //              .execute(db);
         }


         return Json(new
         {
            isSuccess = true
         });
      }



      // Post-Ajax:  Course/ClickVideo
      // Post-Ajax:  Course/CancelFavorite

      [HttpPost]
      public ActionResult Favorite(int resId)
      {
         var isNotFavorited = db.WeiXinFavoriteDal.ConditionQueryCount(wxf.ResId == resId & wxf.OpenId == OpenId) == 0;
         if (isNotFavorited)
         {
            db.WeiXinFavoriteDal.Insert(
            new WeiXinFavorite
            {
               //FavoriteDate = DateTime.Now,
               //OpenId = this.OpenId,
               //WksId = videoId
            });
         }


         return Json(new
         {
            isSuccess = isNotFavorited
         });
      }

      //[HttpPost]
      //public ActionResult CancelFavorite(int videoId)
      //{
      //   var isFavorited = db.WeiXinFavoriteInfoDal.ConditionQueryCount(wxf.WksId == videoId & wxf.OpenId == OpenId) > 0;
      //   if (isFavorited)
      //   {
      //      db.WeiXinFavoriteInfoDal.ConditionDelete(wxf.WksId == videoId & wxf.OpenId == OpenId);
      //   }


      //   return Json(new
      //   {
      //      isSuccess = isFavorited
      //   });
      //}

   }

}