using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OwinAndKatana.Startup))]
namespace OwinAndKatana
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
