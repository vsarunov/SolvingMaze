using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolvingMaze
{
    public class MazeProvider
    {
        public string[] FileText { get; private set; }
        public string[,] Maze { get; private set; }
        public Tuple<int, int> Size { get; private set; }
        public Tuple<int, int> StartingPoints { get; private set; }
        public Tuple<int, int> FinishPoints { get; private set; }

        public MazeProvider(string path)
        {
            this.FileText = this.ReadFile(path);
            this.Size = this.GetMazeInformationByIndex(0);
            this.StartingPoints = this.GetMazeInformationByIndex(1);
            this.FinishPoints = this.GetMazeInformationByIndex(2);
            this.Maze = this.GetMazeFromText();
        }

        private string[,] GetMazeFromText()
        {
            string[,] maze = new string[this.Size.Item1, this.Size.Item2];
            for (int i = 3; i < this.FileText.Length; i++)
            {
                var line = this.FileText[i].Split(' ');
                for (int y = 0; y < line.Length; y++)
                {
                    maze[i - 3, y] = line[y];
                }
            }
            return maze;
        }

        private Tuple<int, int> GetMazeInformationByIndex(int informationIndex)
        {
            string[] sizePoint = this.FileText[informationIndex].Split(' ');
            return new Tuple<int, int>(int.Parse(sizePoint[1]), int.Parse(sizePoint[0]));
        }

        private string[] ReadFile(string path)
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
