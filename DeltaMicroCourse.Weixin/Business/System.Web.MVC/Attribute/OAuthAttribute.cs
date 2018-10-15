using Business;
using Business.Weixin.Utilities;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;

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

				var result = OAuth2Helper.ProcessGetAccessTokenUrl(code);


				#region  [如果有需求需要使用微信身份则取消注释，将userInfo 放入session中]

				var code1 = AccessTokenContainer.TryGetAccessToken(WeixinConfigHelper.AppId, WeixinConfigHelper.AppSecret);
				var userinfo = UserApi.Info(code1, result.openid, Senparc.Weixin.Language.zh_CN);
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