using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JehovaJireh.Web.UI.Startup))]
namespace JehovaJireh.Web.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
