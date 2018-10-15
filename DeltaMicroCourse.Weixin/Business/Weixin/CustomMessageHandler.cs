using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System.Web;

namespace Business.Weixin
{

	/// <summary>
	/// 自定义 MessageHandler
	/// 把 MessageHandler 作为基类，重写对应请求的处理方法
	/// </summary>
	public class CustomMessageHandler : MessageHandler<CustomMessageContext>
	{

		// 重要提示：v1.5起，MessageHandler提供了一个DefaultResponseMessage的抽象方法，
		// DefaultResponseMessage必须在子类中重写，用于返回没有处理过的消息类型（也可以用于默认消息，如帮助信息等）；
		// 其中所有原OnXX的抽象方法已经都改为虚方法，可以不必每个都重写。若不重写，默认返回DefaultResponseMessage方法中的结果。


		//#if DEBUG
		//		string agentUrl = "http://localhost:12222/App/Weixin/4";
		//		string agentToken = "27C455F496044A87";
		//		string wiweihiKey = "CNadjJuWzyX5bz5Gn+/XoyqiqMa5DjXQ";
		//#else
		//		// 下面的Url和Token可以用其他平台的消息，或者到www.weiweihi.com注册微信用户，将自动在“微信营销工具”下得到
		//		private string agentUrl = WebConfigurationManager.AppSettings["WeixinAgentUrl"];//这里使用了www.weiweihi.com微信自动托管平台
		//		private string agentToken = WebConfigurationManager.AppSettings["WeixinAgentToken"];//Token
		//		private string wiweihiKey = WebConfigurationManager.AppSettings["WeixinAgentWeiweihiKey"];//WeiweihiKey专门用于对接www.Weiweihi.com平台，获取方式见：http://www.weiweihi.com/ApiDocuments/Item/25#51
		//#endif


		#region [ Fields ]


		private string appId = WeixinConfigHelper.AppId;
		private string appSecret = WeixinConfigHelper.AppSecret;



		#endregion


		#region [ Constructors ]


		public CustomMessageHandler(HttpRequestBase request, PostModel postModel, int maxRecordCount = 0)
			: base(request.InputStream, postModel, maxRecordCount)
		{
			Request = request;

			// 这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
			// 比如MessageHandler<MessageContext>.GlobalWeixinContext.ExpireMinutes = 3。

			WeixinContext.ExpireMinutes = WeixinConfigHelper.ExpireMinutes;


			if (!string.IsNullOrEmpty(postModel.AppId))
			{
				// 通过第三方开放平台发送过来的请求

				appId = postModel.AppId;
			}


			// 在指定条件下，不使用消息去重
			base.OmitRepeatedMessageFunc = requestMessage =>
			{
				var textRequestMessage = requestMessage as RequestMessageText;
				if (textRequestMessage != null && textRequestMessage.Content == "容错")
				{
					return false;
				}

				return true;
			};
		}


		#endregion


		#region [ Properties ]


		public HttpRequestBase Request { get; }


		#endregion


		#region [ Events ]


		/// <summary>
		/// 缺省消息处理
		/// </summary>
		/// <param name="requestMessage"></param>
		/// <returns></returns>
		public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
		{
			// 所有没有被处理的消息会默认返回这里的结果，
			// 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
			// 只需要在这里统一发出委托请求，如：
			// var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
			// return responseMessage;

			var responseMessage = CreateResponseMessage<ResponseMessageText>();
			responseMessage.Content = "DefaultResponseMessage。";

			return responseMessage;
		}


		#endregion

	}

}