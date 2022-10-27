using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRME.Startup))]
namespace CRME
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
