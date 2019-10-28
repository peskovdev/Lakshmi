using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lakshmi.Startup))]
namespace Lakshmi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
