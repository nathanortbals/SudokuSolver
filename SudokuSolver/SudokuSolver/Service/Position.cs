using System;
using System.Collections.Generic;

namespace SudokuSolver.ServiceLayer
{
    public class Position
    {
        public int x;
        public int y;

        public Position()
        {
            
        }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        //Return 9x9 grid of positions
        public static List<Position> GetAllPositions()
        {
            List<Position> positions = new List<Position>();

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    Position position = new Position(x, y);
                    positions.Add(position);
                }
            }

            return positions;
        }

        //Fast function for determining section number on board (1-9)
        public int GetSection()
        {
            switch(this.x / 3)
            {
                case 0:
                    switch (this.y / 3)
                    {
                        case 0:
                            return 1;
                        case 1:
                            return 2;
                        case 2:
                            return 3;
                        default:
                            return 0;
                    }
                case 1:
                    switch (this.y / 3)
                    {
                        case 0:
                            return 4;
                        case 1:
                            return 5;
                        case 2:
                            return 6;
                        default:
                            return 0;
                    }
                case 2:
                    switch (this.y / 3)
                    {
                        case 0:
                            return 7;
                        case 1:
                            return 8;
                        case 2:
                            return 9;
                        default:
                            return 0;
                    }
                default:
                    return 0;
            }
        }
    }
}
