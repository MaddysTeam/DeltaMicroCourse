using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP;

namespace Business.Weixin.Utilities
{

	public class OAuth2Helper
	{

		public static string ProcessGetOAuthUrl(string url, bool userinfo = false)
		{
			return OAuthApi.GetAuthorizeUrl(WeixinConfigHelper.AppId, url, "1", userinfo ? OAuthScope.snsapi_userinfo : OAuthScope.snsapi_base,"code");
		}


		public static OAuthAccessTokenResult ProcessGetAccessTokenUrl(string code)
		{
			return OAuthApi.GetAccessToken(WeixinConfigHelper.AppId, WeixinConfigHelper.AppSecret, code);
		}


		public static OAuthAccessTokenResult RefreshAccessToken(string token)
		{
			return OAuthApi.RefreshToken(WeixinConfigHelper.AppId, token);
		}

		public static OAuthUserInfo GetUserInfo(string accessToken,string openId)
		{
			return OAuthApi.GetUserInfo(accessToken, openId);
		}

	}

}