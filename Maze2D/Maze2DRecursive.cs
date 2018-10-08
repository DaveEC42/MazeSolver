using System;
using System.IO;

namespace Maze2D
{
    public class Maze2DRecursive
    {
        public string Path { get; set; }        // Filepath of the maze
        public Maze Game { get; set; }          // The maze
        public int Direction { get; set; }      // Current direction to look ahead 
                                                // 0 = north, 1 = east, 2 = south, 3 = west
        public int CurRow { get; set; }         // Current Row
        public int CurCol { get; set; }         // Current Column


        public Maze2DRecursive(string path, int rows, int cols)
        {
            Path = path;
            Game = new Maze(rows, cols);
            Direction = 0;
        }

        public void RunGame()
        {
            ReadMaze();
            DisplayMaze();
            //var currentNode = new MazeNode();
            //PrintLocation();
            bool solved = Solve(CurRow, CurCol);
            //Console.WriteLine("Found the exit at: " + CurRow + ", " + CurCol);
        }

        private void PrintLocation()
        {
     //       Console.WriteLine("Currently at: " + CurRow + ", " + CurCol);
        }

        public bool Solve(int row, int col)
        {
            Console.WriteLine("Currently at: " + row + ", " + col);    
            bool solved = false;
            var node = Game.MazeNodes[row, col];
            if (node.HasBeenVisited)
            {
                return false;
            }
            node.HasBeenVisited = true;
            
            if (node.Value == 3)
            {
                Console.WriteLine("END");
                return true;
            }
            for (int i = 0; i < 4; i++)
            {
                if ((row > 0) && (Game.MazeNodes[row - 1, col].Value > 0 && Direction == 0))
                { // Check North
                    solved = Solve(row - 1, col);
                }
                else if ((col < Game.Cols - 1) && (Game.MazeNodes[row, col + 1].Value > 0 && Direction == 1))
                { // Check East
                    solved = Solve(row, col + 1);
                }
                else if ((row < Game.Rows - 1) && (Game.MazeNodes[row + 1, col].Value > 0 && Direction == 2))
                { // Check South
                    solved = Solve(row + 1, col);
                }
                else if ((col > 0) && (Game.MazeNodes[row, col - 1].Value > 0 && Direction == 3))
                { // Check West
                    solved = Solve(row, col - 1);
                }
                Direction++;
                if(Direction > 3)
                {
                    Direction = 0;
                }
                if (solved)
                {
                    return true;
                }
            }
            return false;
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
                        if (row[j].Equals("2")) // 2 marks the Start location
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

        static void Main(string[] args)
        {
            var game = new Maze2DRecursive("maze.csv", 25,25) ;
            game.RunGame();
            Console.ReadKey();
        }
    }
}
