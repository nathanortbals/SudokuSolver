using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using static SudokuSolver.Models.IdentityModel;
using System.ComponentModel;
using SudokuSolver.ServiceLayer;

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

            GetBlankPuzzle();
        }

        public void SyncPuzzle(Puzzle newPuzzle)
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    Position currentPosition = this.Positions.Single(p => p.X == x && p.Y == y);
                    Position newPosition = newPuzzle.Positions.Single(p => p.X == x && p.Y == y);
                    if (currentPosition.Value != newPosition.Value)
                    {
                        currentPosition.Value = newPosition.Value; 
                    }
                }
            }
        }

        public void GetBlankPuzzle()
        {
            Positions = new List<Position>();
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    Position position = new Position(this, x, y);
                    this.Positions.Add(position);
                }
            }
        }

        public void SolvePuzzle()
        {
            Node node = new Node(this);
            var result = BacktrackingSearch.Search(node);

            SyncPuzzle(result.GetModel());
        }

        public bool IsSolved()
        {
            Node node = new Node(this);

            if (node.IsGoalNode())
                return true;
            else
                return false;
        }

        public bool IsValid()
        {
            if (AreRowsValid() && AreColumnsValid() && AreBlocksValid())
                return true;
            else
                return false;
        }

        private bool AreRowsValid()
        {
            for (int x = 0; x < SizeX; x++)
            {
                List<int> values = new List<int>();

                for (int y = 0; y < SizeY; y++)
                {
                    if (Positions[x * SizeX + y].Value.HasValue)
                        values.Add(Positions[x * SizeX + y].Value.Value);
                }

                if (DuplicatesInList(values))
                    return false;
            }

            return true;
        }

        private bool AreColumnsValid()
        {
            for (int y = 0; y < SizeY; y++)
            {
                List<int> values = new List<int>();

                for (int x = 0; x < SizeX; x++)
                {
                    if (Positions[x * SizeX + y].Value.HasValue)
                        values.Add(Positions[x * SizeX + y].Value.Value);
                }

                if (DuplicatesInList(values))
                    return false;
            }

            return true;
        }

        private bool AreBlocksValid()
        {
            for (int blockX = 0; blockX < (SizeX / BlockSize); blockX++)
            {
                for (int blockY = 0; blockY < (SizeY / BlockSize); blockY++)
                {
                    List<int> values = new List<int>();

                    for (int x = blockX * 3; x < (blockX + 1) * 3; x++)
                    {
                        for (int y = blockY * 3; y < (blockY + 1) * 3; y++)
                        {
                            if (Positions[x * SizeX + y].Value.HasValue)
                                values.Add(Positions[x * SizeX + y].Value.Value);
                        }
                    }

                    if (DuplicatesInList(values))
                        return false;
                }
            }

            return true;
        }

        private static bool DuplicatesInList(List<int> values)
        {
            return values.GroupBy(x => x).Any(x => x.Count() > 1);
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