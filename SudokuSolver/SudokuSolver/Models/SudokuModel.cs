using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using static SudokuSolver.Models.IdentityModel;

namespace SudokuSolver.Models
{
    public class SudokuModel : DbContext
    {
        public DbSet<Puzzle> Puzzles { get; set; }
        public DbSet<Position> Positions { get; set; }
    }

    public class Puzzle
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public virtual User User { get; set; }
        public virtual List<Position> Positions { get; set;}
    }

    public class Position
    {
        public int ID { get; set; }
        public int PuzzleId { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int? value { get; set; }

        public virtual Puzzle Puzzle { get; set; }
    }
}