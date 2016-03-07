using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Melog.Startup))]
namespace Melog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
