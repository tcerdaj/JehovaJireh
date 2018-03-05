using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(JehovaJireh.Web.Services.Startup))]
namespace JehovaJireh.Web.Services
{
	public partial class Startup 
	{
		public void Configuration(IAppBuilder app)
		{
            ConfigureAuth(app);
		}
	}
}
