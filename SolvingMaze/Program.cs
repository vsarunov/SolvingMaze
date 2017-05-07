using System;
using System.Collections.Generic;
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
            Console.WriteLine("Please enter maze txt file path");
            string path = Console.ReadLine();
            string[] fileText = ReadFile(path);
            MazeSolver mazeSolver = new MazeSolver(fileText);
            mazeSolver.DisplayResult();
            Console.ReadLine();
        }

        private static string[] ReadFile(string path)
        {
            string[] lineList = null;
            try
            {
                lineList = File.ReadAllLines(@path);
            }
            catch
            {
                Console.WriteLine("Wrong file path - Please try again..");
                string newPath = Console.ReadLine();
                return ReadFile(newPath);
            }
            return lineList;
        }

    }
}
