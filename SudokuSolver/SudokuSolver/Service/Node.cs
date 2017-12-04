using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SudokuSolver.Models;

namespace SudokuSolver.ServiceLayer
{
    public class Node
    {
        public int depth;
        public Node parent;
        public List<Tile> Board;
        public Position positionChanged;
        public string output;


        public Node()
        {
            
        }

        //This function is for converting from the view model
        public Node(Puzzle model)
        {
            this.parent = null;
            this.depth = 0;

            this.InitializeBoard();
            for (int x = 0; x < Puzzle.SizeX; x++)
            {
                for (int y = 0; y < Puzzle.SizeY; y++)
                {
                    int? value = model.Positions[x * Puzzle.SizeX + y].Value;
                    if (value.HasValue)
                        Board.Single(t => t.Position.x == x && t.Position.y == y).AssignValue(value.Value);
                }
            }
        }

        //This function is for getting the view model after solving puzzle
        public Puzzle GetModel()
        {
            Puzzle model = new Puzzle();
            model.GetBlankPuzzle();

            foreach (Tile tile in Board.Where(t => t.IsAssigned()))
            {
                model.Positions[tile.Position.x * Puzzle.SizeX + tile.Position.y].Value = tile.Domain.First();
            }

            return model;
        }

        //This is for copying a child node
        public Node(Node node)
        {
            this.parent = node;
            this.depth = node.depth++;

            this.Board = new List<Tile>();

            //Making sure tile objects are copies
            foreach (Tile tile in node.Board)
                this.Board.Add(new Tile(tile));
        }

        //For loading examples
        public static Node FromJsonFile(string filePath)
        {
            Node node = null;

            using (StreamReader file = System.IO.File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                node = (Node)serializer.Deserialize(file, typeof(Node));
            }

            return node;
        }

        public List<Node> GetChildren()
        {
            List<Node> children = new List<Node>();

            //Get tile with minimum remaining value, tie breaking with degree
            //This is the heuristic
            Tile tile = Board
                .Where(t => !t.IsAssigned())
                .OrderBy(t => t.GetMinimumRemainingValue())
                .ThenBy(t => t.GetDegree(this))
                .First();

            //Create a child node for each possible value in domain
            foreach(int tileValue in tile.Domain)
            {
                Node node = CreateChildNode(tile, tileValue);
                if(!node.IsTerminalNode())
                    children.Add(node);
            }
               
            return children;
        }

        //Pretty simple goal checking, checks if all domain counts are 1
        public bool IsGoalNode()
        {
            if (Board.Any(t => !t.IsAssigned()))
                return false;
            else
                return true;
        }

        //Checks if any domain counts are 0
        public bool IsTerminalNode()
        {
            if (Board.Any(t => t.GetMinimumRemainingValue() == 0))
                return true;
            else
                return false;
        }

        //Initialize board with all full domains
        private void InitializeBoard()
        {
            this.Board = new List<Tile>();
            foreach (Position position in Position.GetAllPositions())
                Board.Add(new Tile(position));
        }

        //Child creator, makeing sure to copy objects
        private Node CreateChildNode(Tile tile, int value)
        {
            Node child = new Node(this);
            child.Board.Single(t => t.Position == tile.Position).AssignValue(value);
            child.positionChanged = tile.Position;
            child.ApplyConstraints();
            child.output =
                 "Tile Changed: [" + tile.Position.x + "," + tile.Position.y + "], " +
                 " Domain Size: " + Board.Single(t => t.Position == tile.Position).GetMinimumRemainingValue() +
                 ", Value Chosen: " + value +
                     ", Degree: " + Board.Single(t => t.Position == tile.Position).GetDegree(this);

            return child;
        }

        //Constraint application below this point
        public void ApplyConstraints()
        {
            foreach(Tile tile in Board.Where(x => x.IsAssigned()))
            {
                ApplyHorizontalContstraints(tile);
                ApplyVerticalContrains(tile);
                ApplySectionConstraints(tile);
            }
        }

        private void ApplyHorizontalContstraints(Tile tile)
        {
            int value = tile.Domain.First();

            foreach (Tile rowItem in GetRow(tile))
                rowItem.RemoveFromDomain(value);
        }

        public List<Tile> GetRow(Tile tile)
        {
            return Board
                .Where(t => t.Position.x == tile.Position.x && t != tile)
                .ToList();
        }

        private void ApplyVerticalContrains(Tile tile)
        {
            int value = tile.Domain.First();

            foreach (Tile columnItem in GetColumn(tile))
                columnItem.RemoveFromDomain(value);
        }

        public List<Tile> GetColumn(Tile tile)
        {
            return Board
                .Where(t => t.Position.y == tile.Position.y && t != tile)
                .ToList();
        }

        private void ApplySectionConstraints(Tile tile)
        {
            int value = tile.Domain.First();

            foreach (Tile columnItem in GetSection(tile))
                columnItem.RemoveFromDomain(value);
        }

        public List<Tile> GetSection(Tile tile)
        {
            return Board
                .Where(t => t.Position.GetSection() == tile.Position.GetSection() && t != tile)
                .ToList();
        }
    }
}
