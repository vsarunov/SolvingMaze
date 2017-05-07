using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolvingMaze
{
    public class Program
    {
        static void Main(string[] args)
        {
            // read file path input
            Console.WriteLine("Please enter maze txt file path");
            string path = Console.ReadLine();

            /*
             * Separated the process into two classes, As providing the maze and its information
             * is a responsibility of one and solving it of another
             */
            Stopwatch stopWatch = new Stopwatch();
            MazeProvider provider = new MazeProvider(path);
            stopWatch.Start();
            MazeSolver mazeSolver = new MazeSolver(provider.Maze, provider.StartingPoints, provider.FinishPoints);
            mazeSolver.SolveMaze();
            mazeSolver.DisplayResult();
            stopWatch.Stop();

            // Time Elapse
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }



    }
}
