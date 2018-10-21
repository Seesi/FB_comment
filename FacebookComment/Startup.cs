using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FacebookComment.Startup))]
namespace FacebookComment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
