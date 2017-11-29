using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SudokuSolver.Models;

namespace SudokuSolver.Controllers
{
    [Authorize]
    public class SudokuController : Controller
    {
        // GET: Sudoku
        public ActionResult Index()
        {
            return RedirectToAction("Play");
        }

        // Get: Play
        public ActionResult Play()
        {
            return View();
        }
    }
}