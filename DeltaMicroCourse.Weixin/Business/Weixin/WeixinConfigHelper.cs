using Business.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Business
{

	public static class WeixinConfigHelper
	{
		/// <summary>
		/// 项目名称
		/// </summary>
		public static string ProjectName
			=> GetAppSetting("weixin:ProjectName", "");

		/// <summary>
		/// 微信公众号
		/// </summary>
		public static string OfficialAccount
			=> GetAppSetting("weixin:OfficialAccount", "");

		/// <summary>
		/// 服务地址
		/// </summary>
		public static string OAServUrl
			=> GetAppSetting("weixin:OAServUrl", "");


		/// <summary>
		/// 微网站地址
		/// </summary>
		public static string OASiteUrl
			=> GetAppSetting("weixin:OASiteUrl", "");


		/// <summary>
		/// 微信 AppId
		/// </summary>
		public static string AppId
			=> GetAppSetting("weixin:AppId", "");


		/// <summary>
		/// 微信 AppSecret
		/// </summary>
		public static string AppSecret
			=> GetAppSetting("weixin:AppSecret", "");


		/// <summary>
		/// 微信公众号后台 Token，区分大小写
		/// </summary>
		public static string Token
			=> GetAppSetting("weixin:Token", "");

		/// <summary>
		/// 微信公众号后台 EncodingAESKey，区分大小写
		/// </summary>
		public static string EncodingAESKey
			=> GetAppSetting("weixin:EncodingAESKey", "");


		/// <summary>
		/// 微信支付商户号
		/// </summary>
		public static string MchId
			=> GetAppSetting("weixin:MchId", "");


		/// <summary>
		/// 接收财付通通知的地址
		/// </summary>
		public static string PayNotify
			=> GetAppSetting("weixin:PayNotify", "");


		/// <summary>
		/// 微信支付签名
		/// </summary>
		public static string PaySignKey
			=> GetAppSetting("weixin:PaySignKey", "");


		/// <summary>
		/// 消息过期时间（分钟）
		/// </summary>
		public static int ExpireMinutes
			=> GetAppSetting("weixin:ExpireMinutes", 3);


		/// <summary>
		/// 调试模式，在 App_Data 目录下记录日志
		/// </summary>
		public static bool Debug
			=> GetAppSetting("weixin:Debug", false);


		/// <summary>
		/// 群发消息链接
		/// </summary>
		public static string SendMsg
			=> GetAppSetting("weixin:SendMsg", "");


		/// <summary>
		/// oauth验证重定向url
		/// </summary>
		public static string AuthReturnUrl => "http://weikecheng.yrdedu.cn/weixin/Course/Index";

      public static string ExpertIndexUrl => "http://weikecheng.yrdedu.cn/weixin/Expert/Index";




		public static T GetAppSetting<T>(string key, T defaultValue)
		{
			if (!string.IsNullOrEmpty(key))
			{
				string value = ConfigurationManager.AppSettings[key];
				try
				{
					if (value != null)
					{
						var theType = typeof(T);
						if (theType.IsEnum)
							return (T)Enum.Parse(theType, value.ToString(), true);

						return (T)Convert.ChangeType(value, theType);
					}

					return default(T);
				}
				catch { }
			}

			return defaultValue;
		}

	}

}