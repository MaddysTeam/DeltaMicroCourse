using Business;
using Symber.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TheSite.Models;

namespace TheSite.Controllers
{

   public class ExpertController : BaseController
   {

      static APDBDef.WeiXinUserInfoTableDef wu = APDBDef.WeiXinUserInfo;
      static APDBDef.WeiXinExpertTableDef wxe = APDBDef.WeiXinExpert;
      static APDBDef.ResUserTableDef u = APDBDef.ResUser;

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


      [GetOpenId]
      public ActionResult Index()
      {
         return RedirectToAction("WeiXinBind");
      }

      // GET: Expert/List
      //Ajax-Post: Expert/List

      //public ActionResult List()
      //{
      //   return View();
      //}

      //[HttpPost]
      //public ActionResult List(int current, int rowCount, string searchPhrase)
      //{
      //   var query = APQuery
      //      .select(pa.UserId, pa.UserName, wxe.OpenId)
      //      .from(pa, wxe.JoinLeft(pa.UserId == wxe.UserId))
      //      .skip(rowCount * current - rowCount)
      //      .take(rowCount);

      //   if (!string.IsNullOrEmpty(searchPhrase))
      //      query.where(pa.UserName.Match(searchPhrase));


      //   var total = db.ExecuteSizeOfSelect(query);


      //   var result = query.query(db, r =>
      //   {
      //      return new
      //      {
      //         id = pa.UserId.GetValue(r),
      //         name = pa.UserName.GetValue(r),
      //         openid = wxe.OpenId.GetValue(r),
      //         isBind = !string.IsNullOrEmpty(wxe.OpenId.GetValue(r))
      //      };
      //   });


      //   return Json(new
      //   {
      //      rows = result,
      //      current,
      //      rowCount,
      //      total
      //   });
      //}


      //[HttpPost]
      //public ActionResult WeiXinUserList(int current, int rowCount, string searchPhrase)
      //{
      //   var total = db.WeiXinUserInfoDal.ConditionQueryCount(null);

      //   APSqlWherePhrase condition = null;
      //   if (!string.IsNullOrEmpty(searchPhrase))
      //      condition = wu.NickName.Match(searchPhrase);

      //   var result = db.WeiXinUserInfoDal.ConditionQuery(condition, null, rowCount, rowCount * current - rowCount)
      //      .Select(r => new
      //      {
      //         openId = r.OpenId,
      //         nickName = r.NickName,
      //         createDate = r.CreateDate,
      //         gender = r.Gender,
      //         country = r.Country,
      //         province = r.Prov,
      //         city = r.City
      //      });



      //   return Json(new
      //   {
      //      rows = result,
      //      current,
      //      rowCount,
      //      total
      //   });

      //}


      // GET: Expert/Bind
      //Ajax-Post: Expert/Bind
      //Ajax-Post: Expert/UnBind

      public ActionResult Bind(long id)
      {
         var user = db.ResUserDal.PrimaryGet(id);
         var model = user == null ? null : new WeiXinExpert { UserId = user.UserId, UserName = user.UserName };

         return PartialView("Bind", model);
      }

      [HttpPost]
      public ActionResult Bind(WeiXinExpert expert)
      {
         var bindResult = BindExpert(expert);


         return Json(new
         {
            result = bindResult.IsSuccess ? AjaxResults.Success : AjaxResults.Error,
            msg = bindResult.Msg,
         });
      }

      [HttpPost]
      public ActionResult UnBind(WeiXinExpert expert)
      {

        // db.WeiXinExpertDal.PrimaryDelete(expert.OpenId, expert.UserId);

         return Json(new
         {
            result = AjaxResults.Success,
            msg = "操作成功！"
         });
      }


      // GET: Expert/WeiXinBind
      //Ajax-Post: Expert/WeiXinBind
      //Ajax-Post: Expert/WeiXinUnBind


      [OAuth]
      public ActionResult WeiXinBind()
      {
         return View();
      }

      [HttpPost]
      public ActionResult WeiXinBind(string userName)
      {
         var ac = db.ResUserDal.ConditionQuery(u.UserName == userName, null, null, null).FirstOrDefault();
         if (ac != null)
         {
            var bindResult = BindExpert(new WeiXinExpert { OpenId=OpenId, UserId=ac.UserId });

            return Json(new
            {
               result = bindResult.IsSuccess ? AjaxResults.Success : AjaxResults.Error,
               msg = bindResult.Msg,
            });
         }
         else {
            return Json(new
            {
               result = AjaxResults.Error,
               msg = "该用户不存在"
            });
         }
      }

      [HttpPost]
      public ActionResult WeiXinUnBind()
      {
         return Json(new
         {
            result = AjaxResults.Success,
            msg = "解除绑定"
         });
      }


      private BindResult BindExpert(WeiXinExpert expert)
      {
         var wxu = APDBDef.WeiXinUserInfo;
         if (expert == null
            || string.IsNullOrEmpty(expert.OpenId)
            )
            return new BindResult { IsSuccess = false, Msg = "绑定失败" };

         var expertExist = db.WeiXinExpertDal.ConditionQueryCount(wxe.OpenId == expert.OpenId | wxe.UserId == expert.UserId) > 0;

         if (expertExist)
            db.WeiXinExpertDal.ConditionDelete(wxe.OpenId == expert.OpenId);

         db.WeiXinExpertDal.Insert(expert);

         return new BindResult { IsSuccess = false, Msg = "绑定成功" };
      }


      [Obsolete]
      private string EncryptPassword(string password) =>
         System.Web.Security
         .FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5")
         .ToLower().Substring(0, 15);

      private class BindResult
      {
         public bool IsSuccess { get; set; }
         public string Msg { get; set; }
      }

   }

}