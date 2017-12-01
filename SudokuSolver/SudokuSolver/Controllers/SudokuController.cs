using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
            var user = CurrentUser;

            if (user == null)
                RedirectToAction("Login", "Account");
            
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
            var user = CurrentUser;
            var oldPuzzle = user.Puzzles.Single(x => x.ID == puzzle.ID);
            user.Puzzles.Remove(oldPuzzle);
            user.Puzzles.Add(puzzle);
            UserManager.Update(user);

            return RedirectToAction("Load");
        }
    }
}