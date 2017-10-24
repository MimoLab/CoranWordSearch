using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CoranWordSearch.Startup))]
namespace CoranWordSearch
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
