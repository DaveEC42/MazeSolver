using System;
using System.IO;

namespace Maze2D
{
    class Maze2DGame
    {
        public string Path { get; set; }        // Filepath of the maze
        public Maze Game { get; set; }          // The maze
        public int Direction { get; set; }      // Current direction to look ahead 
                                                    // 0 = north, 1 = east, 2 = south, 3 = west
        public int CurRow { get; set; }         // Current Row
        public int CurCol { get; set; }         // Current Column


        public Maze2DGame(string path, int rows, int cols)
        {
            Path = path;
            Game = new Maze(rows, cols);
            Direction = 0;
        }

        public void RunGame()
        {
            ReadMaze();
            DisplayMaze();
            var currentNode = new MazeNode();
            do
            {
                PrintLocation();
                MakeMove();
                currentNode = Game.MazeNodes[CurRow, CurCol]; //if this is 3, the exit has been found

                
            } while (!(currentNode.Value == 3));
            Console.WriteLine("Found the exit at: " + CurRow + ", " + CurCol);
        }

        private void PrintLocation()
        {
            Console.WriteLine("Currently at: " + CurRow + ", " + CurCol);
        }

        private void MakeMove()
        {
            Game.MazeNodes[CurRow, CurCol].HasBeenVisited = true; // mark the node as visited
            var neighborNodes = new MazeNode[4]; // Used for the north, east, south, and west nodes
            if (CurRow > 0) // check north boundary
            {
                neighborNodes[0] = Game.MazeNodes[CurRow - 1, CurCol];
            }
            if (CurCol < Game.Cols - 1) // check east boundary
            {
                neighborNodes[1] = Game.MazeNodes[CurRow, CurCol + 1];
            }
            if (CurRow < Game.Rows - 1) // check south boundary
            {
                neighborNodes[2] = Game.MazeNodes[CurRow + 1, CurCol];
            }
            if (CurCol > 0) // check west boundary
            {
                neighborNodes[3] = Game.MazeNodes[CurRow, CurCol - 1];
            }
            for (int i = 0; i < 4; i++)
            {
                // If there is a neighbor node, and it hasn't been visited, and it has a value > 0 (zero is a wall)
                if(neighborNodes[Direction] != null && (!neighborNodes[Direction].HasBeenVisited 
                    && neighborNodes[Direction].Value > 0))
                {
                    // which way to move
                    switch (Direction)
                    {
                        case 0:
                            CurRow--;
                            break;
                        case 1:
                            CurCol++;
                            break;
                        case 2:
                            CurRow++;
                            break;
                        case 3:
                            CurCol--;
                            break;
                    }
                    return;
                }
                Direction++;
                if (Direction > 3)
                {
                    Direction = 0;
                }
            }
            // If after looking all four ways, nothing is found, mark all nodes as unvisited, so 
            //      to backtrack
            for (int i = 0; i < 4; i++)
            {
                if(neighborNodes[i] != null)
                {
                    neighborNodes[i].HasBeenVisited = false;
                }
            }
        }

        private void ReadMaze()
        {
            using (var input = new StreamReader(Path)) 
            {
                for (int i = 0; i < Game.Rows; i++)
                {
                    string line = input.ReadLine();
                    string[] row = line.Split(',');  // Comma Separated 
                    for (int j = 0; j < Game.Cols; j++)
                    {
                        Game.MazeNodes[i, j].Value = int.Parse(row[j]);
                        if(row[j].Equals("2")) // 2 marks the Start location
                        {
                            CurRow = i;
                            CurCol = j;
                        }
                    }
                }
            }
        }

        private void DisplayMaze()
        {
            for (int i = 0; i < Game.Rows; i++)
            {
                for (int j = 0; j < Game.Cols; j++)
                {
                    Console.Write(Game.MazeNodes[i, j].Value);
                }
                Console.WriteLine();
            }          
        }

        //static void Main(string[] args)
        //{
        //    var game = new Maze2DGame("maze.csv", 25, 25);
         //   game.RunGame();
          //  Console.ReadKey();
       // }
    }
//----------------------------------------------------------------
    public class Maze
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public MazeNode[,] MazeNodes { get; set; }

        public Maze(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            // initialize the array and all of the nodes within.
            MazeNodes = new MazeNode[Rows, Cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    MazeNodes[i, j] = new MazeNode();
                }
            }
        }
    }
//----------------------------------------------------------------
    public class MazeNode
    {
        public bool HasBeenVisited { get; set; }
        public int Value { get; set; } // 0 = wall, 1 = path, 2 = start, 3 = end
    }


}
