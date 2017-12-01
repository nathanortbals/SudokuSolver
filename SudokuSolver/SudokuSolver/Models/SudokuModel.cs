using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using static SudokuSolver.Models.IdentityModel;
using System.ComponentModel;

namespace SudokuSolver.Models
{
    public class SudokuModel
    {
        public DbSet<Puzzle> Puzzles { get; set; }
        public DbSet<Position> Positions { get; set; }
    }

    public class Puzzle
    {
        [DisplayName("Puzzle Id")]
        public int ID { get; set; }

        public string UserID { get; set; }
        public virtual User User { get; set; }

        [DisplayName("Last Edited")]
        public DateTime LastEdited { get; set; }
        
        public virtual List<Position> Positions { get; set;}

        public const int SizeX = 9;
        public const int SizeY = 9;
        public const int BlockSize = 3;
        
        public Puzzle()
        {
            LastEdited = DateTime.Now;
        }

        public Puzzle(User user) : this()
        {
            user.Puzzles.Add(this);
            this.UserID = user.Id;
            this.User = user;

            Positions = new List<Position>();
            for(int x = 0; x < SizeX; x++)
            {
                for(int y = 0; y < SizeY; y++)
                {
                    Position position = new Position(this, x, y);
                    this.Positions.Add(position);
                }
            }
        }

        public void UpdatePuzzle(Puzzle puzzle)
        {
            if (this.ID != puzzle.ID)
                return;

            LastEdited = DateTime.Now;

            this.Positions.RemoveAll(x => x.PuzzleId == this.ID);

            foreach(Position position in puzzle.Positions)
            {
                position.ReassignParent(this);
                this.Positions.Add(position);
            }
        }
    }

    public class Position
    {
        public int ID { get; set; }
        public int PuzzleId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int? Value { get; set; }

        public virtual Puzzle Puzzle { get; set; }

        public Position()
        {

        }

        public Position(Puzzle puzzle, int x, int y)
        {
            this.Puzzle = puzzle;
            this.PuzzleId = puzzle.ID;

            this.X = x;
            this.Y = y;

            this.Value = null;
        }

        public void ReassignParent(Puzzle puzzle)
        {
            this.PuzzleId = puzzle.ID;
            this.Puzzle = puzzle;
        }
    }
}