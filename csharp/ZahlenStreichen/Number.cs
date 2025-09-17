using System;
using System.Collections.Generic;
using System.Linq;

namespace ZahlenStreichen
{
    public class Number
    {
        private Solutions _solutionToSolve;

        public Solutions GetSolutionToSolve()
        {
            return _solutionToSolve;
        }
        public void SetSolutionToSolve(Solutions solutionToSolve, bool setOther = true)
        {
            if (Solved)
                return;

            _solutionToSolve = solutionToSolve;

            if (!setOther)
                return;

            switch (_solutionToSolve)
            {
                case Solutions.EqualTop:
                    Top.SetSolutionToSolve(Solutions.EqualDown, false);
                    break;

                case Solutions.EqualDown:
                    Down.SetSolutionToSolve(Solutions.EqualTop, false);
                    break;

                case Solutions.EqualNext:
                    Next.SetSolutionToSolve(Solutions.EqualPrevious, false);
                    break;

                case Solutions.EqualPrevious:
                    Previous.SetSolutionToSolve(Solutions.EqualNext, false);
                    break;

                case Solutions.TenTop:
                    Top.SetSolutionToSolve(Solutions.TenDown, false);
                    break;

                case Solutions.TenDown:
                    Down.SetSolutionToSolve(Solutions.TenTop, false);
                    break;

                case Solutions.TenNext:
                    Next.SetSolutionToSolve(Solutions.TenPrevious, false);
                    break;
                    
                case Solutions.TenPrevious:
                    Previous.SetSolutionToSolve(Solutions.TenNext, false);
                    break;
            }
        }

        public bool Solved
        {
            get { return _solutionToSolve != Solutions.None; }
        }

        public Solutions Solutions
        {
            get
            {
                if (Solved)
                    return _solutionToSolve;

                var solutions = Solutions.None;

                if (Next != null && !Next.Solved)
                {
                    if (Value + Next.Value == 10)
                        solutions |= Solutions.TenNext;
                    if (Value == Next.Value)
                        solutions |= Solutions.EqualNext;
                }

                if (Previous != null && !Previous.Solved)
                {
                    if (Value + Previous.Value == 10)
                        solutions |= Solutions.TenPrevious;
                    if (Value == Previous.Value)
                        solutions |= Solutions.EqualPrevious;
                }

                if (Top != null && !Top.Solved)
                {
                    if (Value + Top.Value == 10)
                        solutions |= Solutions.TenTop;
                    if (Value == Top.Value)
                        solutions |= Solutions.EqualTop;
                }

                if (Down != null && !Down.Solved)
                {
                    if (Value + Down.Value == 10)
                        solutions |= Solutions.TenDown;
                    if (Value == Down.Value)
                        solutions |= Solutions.EqualDown;
                }

                if (solutions > Solutions.None)
                    solutions &= ~Solutions.None;

                return solutions;
            }
        }


        public Number(int value, Number previous)
        {
            Value = value;

            if (previous == null)
            {
                Row = 1;
                Column = 1;
            }
            else
            {
                Previous = previous;
                previous.Next = this;

                Column = previous.Column;
                Column++;

                Row = previous.Row;

                if (Column == 10)
                {
                    Column = 1;
                    Row++;
                }

                var allPrevious = GetAllPrevious();

                Top = allPrevious
                    .Where(e => e.Column == Column && e.Row == Row - 1)
                    .FirstOrDefault();

                if (Top != null)
                    Top.Down = this;
            }
        }

        public override string ToString()
        {
            return string.Format("Value: {0}, Row: {1}, Column: {2}, Solutions: {3}, Solved: {4}",
                Value, Row, Column, Solutions,
                Solved ? String.Format("true({0})", _solutionToSolve) : "false");
        }

        public string ToString(bool inBoard)
        {
            if (!inBoard)
                return ToString();

            var output = string.Empty;
            if (Solved)
                output = "\tx";
            else
                output = string.Format("\t{0}({1})", Value, (int)Solutions);

            if (Column == 9)
                output += "\n";

            return output;
        }


        public IEnumerable<Number> GetAllPrevious(Func<Number, bool> filter = null)
        {
            if (_previous == null)
                yield break;

            if (filter != null && !filter(_previous))
            {
                foreach (var previous in _previous.GetAllPrevious(filter))
                    yield return previous;

                yield break;
            }


            yield return _previous;

            foreach (var previous in _previous.GetAllPrevious(filter))
                yield return previous;
        }

        public IEnumerable<Number> GetAllNext(Func<Number, bool> filter = null)
        {
            if (_next == null)
                yield break;

            if (filter != null && !filter(_next))
            {
                foreach (var next in _next.GetAllNext(filter))
                    yield return next;

                yield break;
            }

            yield return _next;

            foreach (var next in _next.GetAllNext(filter))
                yield return next;
        }

        public IEnumerable<Number> GetAllTop(Func<Number, bool> filter = null)
        {
            if (_top == null)
                yield break;

            if (filter != null && !filter(_top))
            {
                foreach (var top in _top.GetAllTop(filter))
                    yield return top;

                yield break;
            }

            yield return _top;

            foreach (var top in _top.GetAllTop(filter))
                yield return top;
        }

        public IEnumerable<Number> GetAllDown(Func<Number, bool> filter = null)
        {
            if (_down == null)
                yield break;

            if (filter != null && !filter(_down))
            {
                foreach (var down in _down.GetAllDown(filter))
                    yield return down;

                yield break;
            }

            yield return _down;

            foreach (var down in _down.GetAllDown(filter))
                yield return down;
        }


        public int Row { get; set; }
        public int Column { get; set; }

        public int Value { get; set; }


        private Number _next;
        public Number Next
        {
            get
            {
                if (_next != null && _next.Solved)
                    _next = GetAllNext(n => !n.Solved).FirstOrDefault();

                return _next;
            }
            set { _next = value; }
        }

        private Number _previous;
        public Number Previous
        {
            get
            {
                if (_previous != null && _previous.Solved)
                    _previous = GetAllPrevious(n => !n.Solved).FirstOrDefault();

                return _previous;
            }
            set { _previous = value; }
        }

        private Number _top;
        public Number Top
        {
            get
            {
                if (_top != null && _top.Solved)
                    _top = GetAllTop(n => !n.Solved).FirstOrDefault();

                return _top;
            }
            set { _top = value; }
        }

        private Number _down;
        public Number Down
        {
            get
            {
                if (_down != null && _down.Solved)
                    _down = GetAllDown(n => !n.Solved).FirstOrDefault();

                return _down;
            }
            set { _down = value; }
        }

        public IEnumerable<SolutionMarker> GetMarker()
        {
            foreach(var solution in Solutions.GetIndividualFlags().Cast<Solutions>())
                switch (solution)
                {
                    case Solutions.EqualTop:
                    case Solutions.TenTop:
                        yield return new SolutionMarker(this, Top, solution);
                        break;

                    case Solutions.EqualDown:
                    case Solutions.TenDown:
                        yield return new SolutionMarker(this, Down, solution);
                        break;

                    case Solutions.EqualNext:
                    case Solutions.TenNext:
                        yield return new SolutionMarker(this, Next, solution);
                        break;

                    case Solutions.EqualPrevious:
                    case Solutions.TenPrevious:
                        yield return new SolutionMarker(this, Previous, solution);
                        break;
                }
        }
    }
}
