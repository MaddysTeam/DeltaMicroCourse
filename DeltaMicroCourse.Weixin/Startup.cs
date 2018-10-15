using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheSite.Startup))]
namespace TheSite
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);
		}
	}
}
