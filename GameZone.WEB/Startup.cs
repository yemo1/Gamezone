using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GameZone.WEB.Startup))]
namespace GameZone.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
