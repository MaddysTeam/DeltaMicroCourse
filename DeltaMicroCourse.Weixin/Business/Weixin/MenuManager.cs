//using Business.Weixin.Utilities;
using Business.Weixin.Utilities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.Weixin
{

	public class MenuManager
	{

      /// <summary>
      /// 公众号可以使用AppID和AppSecret调用本接口来获取access_token。
      /// AppID和AppSecret可在“微信公众平台-开发-基本配置”页中获得（需要已经成为开发者，且帐号没有异常状态）。
      /// 调用接口时，请登录“微信公众平台-开发-基本配置”提前将服务器IP地址添加到IP白名单中，点击查看设置方法，否则将无法调用成功。
      /// 
      /// 
      /// 接口调用请求说明
      /// http请求方式：POST（请使用https协议） https://api.weixin.qq.com/cgi-bin/menu/create?access_token=ACCESS_TOKEN
      /// </summary>
		public static void RefreshMenu()
		{
			var accessToken = AccessTokenContainer.TryGetAccessToken(WeixinConfigHelper.AppId, WeixinConfigHelper.AppSecret);
			var result = Senparc.Weixin.MP.CommonAPIs.CommonApi.CreateMenu(accessToken, BuildMenu(WeixinConfigHelper.OAServUrl));
		}


		private static ButtonGroup BuildMenu(string url)
		{
			ButtonGroup bg = new ButtonGroup();

			var button1 = new SingleViewButton(){name= "长三角微课程作品展示", url= OAuth2Helper.ProcessGetOAuthUrl(WeixinConfigHelper.AuthReturnUrl)};
         var button2 = new SingleViewButton(){name = "专家微信号绑定", url = OAuth2Helper.ProcessGetOAuthUrl(WeixinConfigHelper.ExpertIndexUrl) };

         bg.button.Add(button1);
         bg.button.Add(button2);

			return bg;
		}

	}

}