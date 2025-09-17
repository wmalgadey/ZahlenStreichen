using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ZahlenStreichen
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();

            var runs = 0;
            var currentSolutions = new List<NumberBoard>() { new NumberBoard() };
            do
            {
                stopwatch.Start();

                var solutionsToProcess = currentSolutions.ToList();

                var solutions = solutionsToProcess
                    .AsParallel()
                    .SelectMany(currentBoard =>
                    {
                        var possibleMarker = currentBoard
                            .GetPossibleMarker();

                        if (!possibleMarker.Any())
                        {
                            // we found our solution
                            currentBoard.PrintBoard();
                            return new List<NumberBoard>();
                        }

                        var newSolutions = new List<NumberBoard>();
                        foreach (var marker in possibleMarker.Skip(1))
                        {
                            var newBoard = NumberBoard.Copy(currentBoard);
                            newBoard.SetMarker(marker);

                            newSolutions.Add(newBoard);
                        }

                        currentBoard.SetMarker(possibleMarker.First());

                        return newSolutions;
                    });

                var addSolutions = solutions.ToList().Concat(currentSolutions.ToList());
                //var difficulty = addSolutions.Sum(g => g.Difficulty) / addSolutions.Count();


                currentSolutions = addSolutions
                    .Where(game => game.Rows <= 8)// && game.GetSolutionMarker().Take(_bestGameMarkers.Count/2).All(gg => _bestGameMarkers.Contains(gg))) // && game.Difficulty <= difficulty)
                    .Distinct()
                    .ToList();

                var minDifficulty = addSolutions.Min(g => g.Difficulty);

                stopwatch.Stop();

                Console.WriteLine(String.Format("{2,4:N0} run - {0,5:N0} games in {1,10:N0}ms - Min. Solutions {3,4:N0}", 
                    currentSolutions.Count,
                    stopwatch.ElapsedMilliseconds, ++runs, minDifficulty));
            } while (true);

            Console.ReadKey();
        }

        //private static List<SolutionMarker> _bestGameMarkers = new List<SolutionMarker>()
        //{
        //    SolutionMarker.Create(1,1),
        //    SolutionMarker.Create(1,8),
        //    SolutionMarker.Create(1,9),
        //    SolutionMarker.Create(2,1),
        //    SolutionMarker.Create(2,2),
        //    SolutionMarker.Create(2,3),
        //    SolutionMarker.Create(2,4),
        //    SolutionMarker.Create(2,9),
        //    SolutionMarker.Create(3,8),
        //    SolutionMarker.Create(3,9),
        //};
    }
}