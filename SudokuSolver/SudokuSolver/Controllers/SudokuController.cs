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
using static SudokuSolver.Models.IdentityModel;

namespace SudokuSolver.Controllers
{
    [Authorize]
    public class SudokuController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public User CurrentUser
        {
            get
            {
                return UserManager.FindById(User.Identity.GetUserId());
            }
        }

        // GET: Sudoku
        public ActionResult Index()
        {
            return RedirectToAction("Load");
        }

        // GET: Sudoku/Play
        public ActionResult Play(int PuzzleID)
        {
            var puzzle = CurrentUser.Puzzles.Single(x => x.ID == PuzzleID);
            return View(puzzle);
        }

        // Get: Sudoku/Load
        public ActionResult Load()
        {
            return View(CurrentUser);
        }

        // Get: Sudoku/NewPuzzle
        public ActionResult NewPuzzle()
        {
            new Puzzle(CurrentUser);
            UserManager.Update(CurrentUser);
            
            return RedirectToAction("Load");
        }

        // Get: Sudoku/LoadPuzzle
        public ActionResult OpenPuzzle(int PuzzleID)
        {
            return RedirectToAction("Play", new { PuzzleID = PuzzleID });
        }

        // Get: Sudoku/DeletePuzzle
        public ActionResult DeletePuzzle(int PuzzleID)
        {
            var puzzle = CurrentUser.Puzzles.Single(p => p.ID == PuzzleID);
            CurrentUser.Puzzles.Remove(puzzle);
            UserManager.Update(CurrentUser);

            return RedirectToAction("Load");
        }

        // Post: Sudoku/SavePuzzle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePuzzle(Puzzle puzzle)
        {
            TryUpdateModel(puzzle);
            
            return RedirectToAction("Load");
        }
    }
}