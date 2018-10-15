using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.CommonAPIs;

namespace Business.Weixin.Utilities
{

	public class SignatureHelper
	{

	   public static string Singnature(string nonceStr,string timestamp,string url)
		{
			var ticket = JsApiTicketContainer.TryGetJsApiTicket(WeixinConfigHelper.AppId, WeixinConfigHelper.AppSecret);
			var signature = JSSDKHelper.GetSignature(ticket, nonceStr, timestamp, url);

			return signature;
		}

	}

}