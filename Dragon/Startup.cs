using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dragon.Startup))]
namespace Dragon
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
