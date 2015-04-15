using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhotoProject.Startup))]
namespace PhotoProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
