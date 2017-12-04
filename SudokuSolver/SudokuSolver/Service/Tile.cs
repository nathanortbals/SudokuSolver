using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.ServiceLayer
{
    public class Tile
    {
        public Position Position;
        public List<int> Domain;

        public Tile()
        {
            
        }

        public Tile(Position position)
        {
            this.Position = position;
            Domain = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }

        //Copying function
        public Tile(Tile tile)
        {
            this.Position = tile.Position;
            this.Domain = new List<int>(tile.Domain);
        }

        public int GetMinimumRemainingValue()
        {
            return Domain.Count();
        }

        //Get number of values that can be affected
        public int GetDegree(Node node)
        {
            List<Tile> constrainedCells = new List<Tile>();
            constrainedCells.AddRange(node.GetRow(this));
            constrainedCells.AddRange(node.GetColumn(this).Where(t => !constrainedCells.Contains(t)));
            constrainedCells.AddRange(node.GetSection(this).Where(t => !constrainedCells.Contains(t)));

            return constrainedCells.Where(t => t.Domain.Count() > 1).Count();
        }

        public void RemoveFromDomain(int toRemove)
        {
            Domain.Remove(toRemove);
        }

        //Set domain to just one value
        public void AssignValue(int toAssign)
        {
            this.Domain = new List<int> { toAssign };
        }

        public bool IsAssigned()
        {
            if (Domain.Count == 1)
                return true;
            else
                return false;
        }

        public bool IsEmpty()
        {
            if (Domain.Count == 0)
                return true;
            else
                return false;
        }
    }
}
