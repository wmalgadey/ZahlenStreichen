
namespace ZahlenStreichen
{
    public class SolutionMarker
    {
        private int _firstRow;
        private int _firstColumn;

        private int _secondRow;
        private int _secondColumn;

        private Solutions _solution;


        public SolutionMarker(Number first, Number second, Solutions solution)
        {
            _firstRow = first.Row;
            _firstColumn = first.Column;

            _secondRow = second.Row;
            _secondColumn = second.Column;

            _solution = solution;
        }


        public bool IsNumberToMark(Number number)
        {
            if (number.Column == _firstColumn && number.Row == _firstRow)
                return true;

            return false;
        }

        public void SetSolution(Number number)
        {
            number.SetSolutionToSolve(_solution);
        }


        public override bool Equals(object obj)
        {
            var isMarker = obj as SolutionMarker;

            if (isMarker != null)
                return Equals(isMarker);

            return base.Equals(obj);
        }

        public bool Equals(SolutionMarker marker)
        {
            if (marker._firstColumn == _firstColumn && marker._firstRow == _firstRow &&
                marker._secondColumn == _secondColumn && marker._secondRow == _secondRow)
                return true;

            if (marker._firstColumn == _secondColumn && marker._firstRow == _secondRow &&
                marker._secondColumn == _firstColumn && marker._secondRow == _firstRow)
                return true;

            if (_secondColumn == -1 && _secondRow == -1 &&
                ((marker._firstColumn == _firstColumn && marker._firstRow == _firstRow) ||
                (marker._secondColumn == _firstColumn && marker._secondRow == _firstRow)))
                return true;

            return false;
        }


        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return string.Format("Solved Cell({0},{1}) and Cell({2},{3}) with Solution {4}",
                _firstRow, _firstColumn, _secondRow, _secondColumn, _solution);
        }

        public string ToString(bool shortText)
        {
            if (shortText)
                return string.Format("({0},{1})({2},{3})",
                    _firstRow, _firstColumn, _secondRow, _secondColumn);

            return ToString();
        }
    }
}
