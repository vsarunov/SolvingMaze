using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolvingMaze
{
    public class MazeSolver
    {
        private string[] FileText;
        private string[,] Maze;
        private Tuple<int, int> StartingPoints;
        private Tuple<int, int> FinishPoints;
        private Tuple<int, int> MazeSizePoints;

        public MazeSolver(string[] fileText)
        {
            this.FileText = fileText;
            this.MazeSizePoints = this.GetMazeInformationByIndex(0);
            this.StartingPoints = this.GetMazeInformationByIndex(1);
            this.FinishPoints = this.GetMazeInformationByIndex(2);
            this.Maze = this.GetMazeFromText();
            Dictionary<Tuple<int, int>, bool> alreadySearched = new Dictionary<Tuple<int, int>, bool>();
            var shortestPathCoordinates = this.GetShortestPathCoordinates(this.StartingPoints.Item1, this.StartingPoints.Item2, alreadySearched);
            this.ReplaceCoordinatesToShortestPath(shortestPathCoordinates);
            this.SetMazeStartAndFinish();
            this.SetMazeWallsToHashTag();
        }

        private List<Tuple<int, int>> GetShortestPathCoordinates(int startX, int startY, Dictionary<Tuple<int, int>, bool> alreadySearched, List<Tuple<int, int>> coordinates = null)
        {
            List<Tuple<int, int>> leftCoordinates = new List<Tuple<int, int>>(coordinates ?? new List<Tuple<int, int>>());
            List<Tuple<int, int>> rightCoordinates = new List<Tuple<int, int>>(coordinates ?? new List<Tuple<int, int>>());
            List<Tuple<int, int>> downCoordinates = new List<Tuple<int, int>>(coordinates ?? new List<Tuple<int, int>>());
            List<Tuple<int, int>> upCoordinates = new List<Tuple<int, int>>(coordinates ?? new List<Tuple<int, int>>());

            bool searchResult;
            Tuple<int, int> newTuple = new Tuple<int, int>(startX, startY + 1);
            alreadySearched.TryGetValue(newTuple, out searchResult);
            if (!this.Maze[startX, startY + 1].Equals("1") && (startX != this.FinishPoints.Item1 || startY + 1 != this.FinishPoints.Item2) && !searchResult)
            {
                rightCoordinates.Add(newTuple);
                alreadySearched[newTuple] = true;
                rightCoordinates = this.GetShortestPathCoordinates(startX, startY + 1, alreadySearched, rightCoordinates);
            }
            else if (this.Maze[startX, startY + 1].Equals("1") || searchResult)
            {
                rightCoordinates = null;
            }

            newTuple = new Tuple<int, int>(startX + 1, startY);
            alreadySearched.TryGetValue(newTuple, out searchResult);
            if (!this.Maze[startX + 1, startY].Equals("1") && (startX + 1 != this.FinishPoints.Item1 || startY != this.FinishPoints.Item2) && !searchResult)
            {
                downCoordinates.Add(newTuple);
                alreadySearched[newTuple] = true;
                downCoordinates = this.GetShortestPathCoordinates(startX + 1, startY, alreadySearched, downCoordinates);
            }
            else if (this.Maze[startX + 1, startY].Equals("1") || searchResult)
            {
                downCoordinates = null;
            }

            newTuple = new Tuple<int, int>(startX, startY - 1);
            alreadySearched.TryGetValue(newTuple, out searchResult);
            if (!this.Maze[startX, startY - 1].Equals("1") && (startX != this.FinishPoints.Item1 || startY - 1 != this.FinishPoints.Item2) && !searchResult)
            {
                leftCoordinates.Add(new Tuple<int, int>(startX, startY - 1));
                alreadySearched[newTuple] = true;
                leftCoordinates = this.GetShortestPathCoordinates(startX, startY - 1, alreadySearched, leftCoordinates);
            }
            else if (this.Maze[startX, startY - 1].Equals("1") || searchResult)
            {
                leftCoordinates = null;
            }

            newTuple = new Tuple<int, int>(startX - 1, startY);
            alreadySearched.TryGetValue(newTuple, out searchResult);
            if (!this.Maze[startX - 1, startY].Equals("1") && (startX - 1 != this.FinishPoints.Item1 || startY != this.FinishPoints.Item2) && !searchResult)
            {
                upCoordinates.Add(new Tuple<int, int>(startX - 1, startY));
                alreadySearched[newTuple] = true;
                upCoordinates = this.GetShortestPathCoordinates(startX - 1, startY, alreadySearched, upCoordinates);
            }
            else if (this.Maze[startX - 1, startY].Equals("1") || searchResult)
            {
                upCoordinates = null;
            }

            List<Tuple<int, int>> result = this.FindShortestPathCollection(new List<List<Tuple<int, int>>> { upCoordinates, downCoordinates, leftCoordinates, rightCoordinates });

            return result;
        }

        private bool checkIfSolutionWasFound()
        {
            return this.Maze[this.FinishPoints.Item1 - 1, FinishPoints.Item2] == "X" ? true :
                    this.Maze[this.FinishPoints.Item1 + 1, FinishPoints.Item2] == "X" ? true :
                    this.Maze[this.FinishPoints.Item1, FinishPoints.Item2 + 1] == "X" ? true :
                    this.Maze[this.FinishPoints.Item1, FinishPoints.Item2 - 1] == "X" ? true : false;
        }

        private List<Tuple<int, int>> FindShortestPathCollection(List<List<Tuple<int, int>>> listOfCoordinatesList)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            foreach (var item in listOfCoordinatesList)
            {
                if (item != null && item.Count != 0)
                {
                    var tempItem = result;
                    result = item;
                    if (tempItem.Count != 0 && tempItem.Count < result.Count)
                    {
                        result = tempItem;
                    }
                }
            }
            return result;
        }

        private void ReplaceCoordinatesToShortestPath(List<Tuple<int, int>> shortestPath)
        {
            foreach (var item in shortestPath)
            {
                this.Maze[item.Item1, item.Item2] = "X";
            }
        }

        private string[,] GetMazeFromText()
        {
            string[,] maze = new string[this.MazeSizePoints.Item1, this.MazeSizePoints.Item2];
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

        private void SetMazeStartAndFinish()
        {
            this.Maze[this.StartingPoints.Item1, this.StartingPoints.Item2] = "S";
            this.Maze[this.FinishPoints.Item1, this.FinishPoints.Item2] = "E";
        }

        private void SetMazeWallsToHashTag()
        {
            int rowLength = this.Maze.GetLength(0);
            int colLength = this.Maze.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int y = 0; y < colLength; y++)
                {
                    if (this.Maze[i, y].Equals("1"))
                    {
                        this.Maze[i, y] = "#";
                    }
                }
            }

        }

        public void DisplayResult()
        {
            bool solutionFound = this.checkIfSolutionWasFound();
            if (solutionFound)
            {
                int rowLength = this.Maze.GetLength(0);
                int colLength = this.Maze.GetLength(1);

                for (int i = 0; i < rowLength; i++)
                {
                    for (int y = 0; y < colLength; y++)
                    {
                        Console.Write(string.Format("{0}", this.Maze[i, y]));
                    }
                    Console.WriteLine();
                }
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("There was no solution");
            }
        }
    }
}
