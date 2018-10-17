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
         var dics = APBplDef.ResPickListItemBplBase.GetAll();
         Business.Cache.ThisAppCache.SetCache(dics);

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

         var query = APQuery.select(cr.CrosourceId, cr.Title, cr.Author, cr.FavoriteCount, cr.WeiXinFavoriteCount,
                                    cr.ProvinceId, cr.AreaId, cr.PraiseCount,cr.WeiXInPraiseCount,
                                    cr.AuthorCompany, cr.Description, cr.CreatedTime,
                                    mc.CourseId, mc.CourseTitle, mc.PlayCount, mc.WeiXinPlayCount,
                                    cf.FilePath, rc.CompanyName
                                   )
                             .from(cr,
                                    mc.JoinInner(mc.ResourceId == cr.CrosourceId),
                                    rc.JoinInner(rc.CompanyId == cr.CompanyId),
                                    cf.JoinLeft(cf.FileId == mc.CoverId)
                                    //wxf.JoinLeft(wxf.ResId == cr.CrosourceId & wxf.OpenId == OpenId),
                                    //wxp.JoinLeft(wxp.ResourceId == cr.CrosourceId)
                                    //TODO:a.JoinInner(a.ActiveId==cr.ActiveId)
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
               ResId = cr.CrosourceId.GetValue(r),
               CourseId = mc.CourseId.GetValue(r),
               ImagePath = cf.FilePath.GetValue(r),
               Name = mc.CourseTitle.GetValue(r),
               TeacherName = cr.Author.GetValue(r),
               PlayCount = mc.PlayCount.GetValue(r) + mc.WeiXinPlayCount.GetValue(r),
               PraiseCount = cr.PraiseCount.GetValue(r) + cr.WeiXInPraiseCount.GetValue(r),
               SchoolName = rc.CompanyName.GetValue(r),
               LinkUrl = Url.Action("Details", new { resId = cr.CrosourceId.GetValue(r), courseId = mc.CourseId.GetValue(r) }),
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

      //[OAuth]
      public ActionResult Details(long resId, long courseId)
      {
         if (resId == 0 || courseId == 0)
         {
            return RedirectToAction("List");
         }

         var cf = APDBDef.Files;
         var vf = APDBDef.Files.As("Videos");
         var rc = APDBDef.ResCompany;

         var dicCache = Business.Cache.ThisAppCache.GetCache<List<ResPickListItem>>();
         if (dicCache == null)
         {
            var dics = APBplDef.ResPickListItemBplBase.GetAll();
            Business.Cache.ThisAppCache.SetCache(dics);
            dicCache = Business.Cache.ThisAppCache.GetCache<List<ResPickListItem>>();
         }

         var courseViewModel = APQuery.select(mc.CourseId, mc.CourseTitle, mc.PlayCount, mc.WeiXinPlayCount,
                                              cr.CrosourceId, cr.Title, cr.PraiseCount,
                                              cr.CrosourceId, cr.WeiXInPraiseCount, cr.WeiXinFavoriteCount,
                                              rc.CompanyName, cr.SubjectPKID, cr.Author, cr.GradePKID,
                                              cr.CreatedTime, cf.FilePath, vf.FilePath.As("VideoPath"), wxf.OccurId)
                                          .from(cr,
                                                mc.JoinInner(mc.ResourceId == cr.CrosourceId),
                                                rc.JoinInner(rc.CompanyId == cr.CompanyId),
                                                   cf.JoinLeft(mc.CoverId == cf.FileId),//关联获取视频图片
                                                   vf.JoinInner(mc.VideoId == vf.FileId), //关联获取视频地址
                                                   wxf.JoinLeft(wxf.OccurId == cr.CrosourceId & wxf.OpenId == OpenId) // 关联获取微信收藏表
                                                   )
                                          .where(cr.CrosourceId == resId & mc.CourseId == courseId)
                                          .query(db, r =>
                                          {
                                             var grade = dicCache.Find(x => x.PickListItemId == cr.GradePKID.GetValue(r)).Name;
                                             var subject = dicCache.Find(x => x.PickListItemId == cr.SubjectPKID.GetValue(r)).Name;
                                             return new CourseViewModel
                                             {
                                                ResId = cr.CrosourceId.GetValue(r),
                                                CourseId = mc.CourseId.GetValue(r),
                                                Name = cr.Title.GetValue(r),
                                                CourseName = mc.CourseTitle.GetValue(r),
                                                PlayCount = mc.PlayCount.GetValue(r) + mc.WeiXinPlayCount.GetValue(r),
                                                PraiseCount = cr.PraiseCount.GetValue(r) + cr.WeiXInPraiseCount.GetValue(r),
                                                SchoolName = rc.CompanyName.GetValue(r),
                                                GradeName = grade,
                                                SubjectName = subject,
                                                TeacherName = cr.Author.GetValue(r),
                                                ImagePath = cf.FilePath.GetValue(r),
                                                VideoPath = vf.FilePath.GetValue(r, "VideoPath"),
                                                CreateDate = cr.CreatedTime.GetValue(r),
                                                IsFavorite = wxf.OccurId.GetValue(r) > 0
                                             };
                                          }).FirstOrDefault();

         return View(courseViewModel);
      }

      [HttpPost]
      public ActionResult Details(long resId)
      {
         var cf = APDBDef.Files;
         var items = APQuery.select(mc.CourseId, mc.ResourceId, cf.FilePath)
                                     .from(mc, cf.JoinLeft(mc.CoverId == cf.FileId))
                                     .where(mc.ResourceId < resId)
                                     .order_by(mc.ResourceId.Desc)
                                     .take(Paging.PageSize)
                                     .query(db, r =>
                                     {
                                        return new CourseViewModel
                                        {
                                           ImagePath = cf.FilePath.GetValue(r),
                                           LinkUrl = Url.Action("Details", new { resId = mc.ResourceId.GetValue(r), courseId = mc.CourseId.GetValue(r) }),
                                        };
                                     }).ToList();


         return Json(new
         {
            items
         });
      }


      // Post-Ajax:  Course/PraiseResource

      [HttpPost]
      public ActionResult Praise(long resId)
      {
         var wxp = APDBDef.WeiXinPraise;

         var isNotPraised = db.WeiXinPraiseDal.ConditionQueryCount(wxp.ResId == resId & wxp.OpenId == OpenId) == 0;
         if (isNotPraised)
         {
            db.WeiXinPraiseDal.Insert(new WeiXinPraise
            {
               OpenId = OpenId,
               ResId = resId,
               OccurTime = DateTime.Now
            });

            APQuery.update(cr)
               .set(cr.WeiXInPraiseCount, APSqlRawExpr.Expr("WeiXInPraiseCount +1"))
               .where(wxp.ResId == resId)
               .execute(db);
         }

         return Json(new
         {
            isSuccess = isNotPraised // 如果重复点赞，暂时设定为已经点赞，点赞失败
         });
      }


      // Post-Ajax:  Course/Play

      [HttpPost]
      public ActionResult Play(long resId, long courseId)
      {
         var isNotPlayed = db.WeiXinPlayCountDal.ConditionQueryCount(wxp.CourseId == courseId & wxp.OpenId == OpenId) == 0;
         if (isNotPlayed)
         {
            db.WeiXinPlayCountDal.Insert(
            new WeiXinPlayCount
            {
               OpenId = OpenId,
               ResourceId = resId,
               CourseId = courseId,
               OccurTime = DateTime.Now
            });

            APQuery.update(mc)
               .set(mc.WeiXinPlayCount, APSqlRawExpr.Expr("WeiXinPlayCount +1"))
               .where(wxp.ResourceId == resId & wxp.CourseId == courseId)
               .execute(db);
         }

         return Json(new
         {
            isSuccess = true
         });
      }



      // Post-Ajax:  Course/ClickVideo
      // Post-Ajax:  Course/CancelFavorite

      [HttpPost]
      public ActionResult Favorite(long resId)
      {
         var isNotFavorited = db.WeiXinFavoriteDal.ConditionQueryCount(wxf.ResId == resId & wxf.OpenId == OpenId) == 0;
         if (isNotFavorited)
         {
            db.WeiXinFavoriteDal.Insert(
            new WeiXinFavorite
            {
               OpenId = OpenId,
               ResId = resId,
               OccurTime = DateTime.Now
            });

            APQuery.update(cr)
               .set(cr.WeiXinFavoriteCount, APSqlRawExpr.Expr("WeiXinFavoriteCount +1"))
               .where(wxp.ResourceId == resId)
               .execute(db);
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