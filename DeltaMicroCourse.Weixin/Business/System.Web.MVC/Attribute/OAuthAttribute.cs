using Business;
using Business.Weixin.Utilities;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using System.Linq;

namespace System.Web.Mvc
{
	class GetOpenIdAttribute : AuthorizeAttribute
	{
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			if (HttpContext.Current.Session["openId"] != null)
			{
				return true;
			}

			var code = httpContext.Request["code"];

			if (code == null)
			{
				httpContext.Response.Write("用户拒绝授权");

				return false;
			}
			else{

            #region  [如果有需求需要使用微信身份则取消注释，将userInfo 放入session中,注意全局accessToken 每2小时过期，过期后重取，否则放入数据库保存]

            var ac = APDBDef.WeiXinAccessToken;
            var gloableToken = APBplDef.WeiXinAccessTokenBpl.ConditionQuery(null, ac.StartDate.Desc).FirstOrDefault();
            if (null == gloableToken || (DateTime.Now - gloableToken.StartDate).Hours > 2)
            {
               gloableToken.AccessToken = AccessTokenContainer.TryGetAccessToken(WeixinConfigHelper.AppId, WeixinConfigHelper.AppSecret);
               APBplDef.WeiXinAccessTokenBpl.Insert(new WeiXinAccessToken { AccessToken= gloableToken.AccessToken, StartDate=DateTime.Now });
            }

            var result = OAuth2Helper.ProcessGetAccessTokenUrl(code);
            var userinfo = UserApi.Info(gloableToken.AccessToken, result.openid, Senparc.Weixin.Language.zh_CN);
				HttpContext.Current.Session["userinfo"] = userinfo;

				#endregion

				HttpContext.Current.Session["openId"] = result.openid;
				HttpContext.Current.Session.Timeout = 100000;
			}


			return true;
		}

	}


	class OAuthAttribute : AuthorizeAttribute
	{

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			var openId = HttpContext.Current.Session["openId"];
			if (openId == null)
			{
				httpContext.Response.StatusCode = 403;

				return false;
			}

			return true;
		}

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			base.OnAuthorization(filterContext);

			if (filterContext.HttpContext.Response.StatusCode == 403)
			{
				var redirectUrl = OAuth2Helper.ProcessGetOAuthUrl(WeixinConfigHelper.AuthReturnUrl);
				filterContext.Result = new RedirectResult(redirectUrl);
			}

		}

	}

}