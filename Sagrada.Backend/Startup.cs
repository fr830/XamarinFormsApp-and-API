using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sagrada.Backend.Startup))]
namespace Sagrada.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
