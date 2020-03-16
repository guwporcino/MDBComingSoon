using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MDBComingSoon.Startup))]
namespace MDBComingSoon
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
