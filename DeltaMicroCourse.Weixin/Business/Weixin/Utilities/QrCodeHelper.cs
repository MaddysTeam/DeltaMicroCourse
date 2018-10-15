//using Business.Weixin.Utilities;
using Senparc.Weixin.CommonAPIs;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using Senparc.Weixin.MP.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.Weixin.Utilities
{

	public class QrCodeHelper
	{
	   private string _createQrCodeUrl= "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
		private string _qrCodeReturnUrl= "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";


		public string CreateQrCode(string accessToken, QrCodeParam param)
		{
			if (string.IsNullOrEmpty(accessToken) || string.IsNullOrWhiteSpace(accessToken))
			{
				throw new ArgumentNullException("accessToken 不能为空");
			}

			if (param == null)
			{
				throw new ArgumentNullException("param 不能为空");
			}

			var data = new
			{
				expire_seconds = param.ExpireSeconds,
				action_name = param.IsTemp ? "QR_SCENE" : "QR_LIMIT_SCENE",
				action_info = new
				{
					scene = new
					{
						scene_id = param.SceneId
					}
				}
			};

			var createResult = CommonJsonSend.Send<CreateQrCodeResult>(accessToken, _createQrCodeUrl, data);

			return string.Format(_qrCodeReturnUrl, createResult.ticket);
		}


		public class QrCodeParam
		{

			public bool IsTemp { get; set; }
			public int ExpireSeconds { get; set; }
			public int SceneId { get; set; }

		}

	}

}