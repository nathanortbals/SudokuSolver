using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SudokuSolver.Startup))]
namespace SudokuSolver
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}