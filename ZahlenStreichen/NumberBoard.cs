using System;
using System.Collections.Generic;
using System.Linq;

namespace ZahlenStreichen
{
    class NumberBoard
    {
        private List<Number> _numbers;
        private IList<SolutionMarker> _solutionMarker;

        public int Rows
        {
            get
            {
                return _numbers.Max(e => e.Row);
            }
        }

        public double Difficulty
        {
            get
            {
                var count = _numbers.Count;
                var solved = _numbers.Where(e => e.Solved).Count();
                //var possible = _Numbers.Where(e => e.Solutions != Solutions.None && !e.Solved).Count();

                return (double)count - solved;
            }
        }


        public NumberBoard()
        {
            _solutionMarker = new List<SolutionMarker>();
            _numbers = new List<Number>();

            var currentNumber = new Number(1, null);
            _numbers.Add(currentNumber);

            foreach (var value in Enumerable.Range(2, 18)
                .Where(n => n != 10)
                .Select(n => n.ToString())
                .Aggregate((a, b) => a += b)
                .ToCharArray()
                .Select(c => (int)char.GetNumericValue(c)))
            {
                currentNumber = new Number(value, currentNumber);
                _numbers.Add(currentNumber);
            }
        }

        public NumberBoard(IList<SolutionMarker> solvedWithMarker)
            : this()
        {
            foreach (var marker in solvedWithMarker)
                SetMarker(marker);
        }


        public IList<SolutionMarker> GetPossibleMarker()
        {
            var marker = _numbers
                .Where(e => e.Solutions != Solutions.None && !e.Solved)
                .SelectMany(e => e.GetMarker())
                .Distinct();

            if (!marker.Any())
                ExtendBoard();

            return marker.ToList();
        }

        public void SetMarker(SolutionMarker marker)
        {
            var NumberToMark = _numbers
                .Where(marker.IsNumberToMark)
                .FirstOrDefault();

            marker.SetSolution(NumberToMark);

            _solutionMarker.Add(marker);

            if (!_numbers.Any(e => e.Solutions != Solutions.None && !e.Solved))
                ExtendBoard();
        }

        public IEnumerable<SolutionMarker> GetSolutionMarker()
        {
            foreach (var marker in _solutionMarker)
                yield return marker;
        }

        private void ExtendBoard()
        {
            //PrintBoard();

            var notSolved = _numbers
                .Where(e => !e.Solved)
                .Select(elem => elem.Value)
                .ToList();

            var lastNumber = _numbers.Last();
            foreach (var value in notSolved)
            {
                lastNumber = new Number((int)value, lastNumber);
                _numbers.Add(lastNumber);
            }
        }


        private static object sync = new object();

        public void PrintBoard()
        {
            lock (sync)
            {
                Console.WriteLine("\n--------------");
                Console.WriteLine(this);
                Console.WriteLine("--------------");

                var lastRow = 1;
                foreach (var Number in _numbers)
                {
                    if (lastRow != Number.Row)
                        Console.WriteLine("");

                    Console.Write(Number.ToString(inBoard: true));

                    lastRow = Number.Row;
                }

                Console.WriteLine("\n--------------");

                Console.ReadKey();
            }
        }
        


        public static NumberBoard Copy(NumberBoard source)
        {
            return new NumberBoard(source._solutionMarker);
        }
        



        public override bool Equals(object obj)
        {
            var board = obj as NumberBoard;

            if (board != null)
                return Equals(board);

            return base.Equals(obj);
        }

        public bool Equals(NumberBoard board)
        {
            if (_solutionMarker.Any(marker =>
                !board._solutionMarker.Contains(marker)))
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            var markerString = string.Empty;
            foreach (var marker in _solutionMarker)
                markerString += marker.ToString(true);

            return string.Format("{0} {1}", Difficulty, markerString);
        }
    }
}
