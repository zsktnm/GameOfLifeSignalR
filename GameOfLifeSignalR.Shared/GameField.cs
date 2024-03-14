namespace GameOfLifeSignalR.Shared
{
    public enum CellState { Empty, Filled }


    public class GameField
    {
        private CellState[,] field;
        private int[,] scores;
        private List<Cell> selected = new List<Cell>();


        public bool this[int i, int j] => field[i, j] == CellState.Empty ? false : true;

        public CellState[,] Field => field;
        public Cell[] SelectedCells => selected.ToArray();

        public int Rows => field.GetLength(0);
        public int Columns => field.GetLength(1);


        public GameField(int rows, int coluns)
        {
            field = new CellState[rows, coluns];
            scores = new int[rows, coluns];
        }


        public GameField(bool[][] cells)
        {
            // TODO: check lengths
            int rows = cells.Length;
            int cols = cells[0].Length;
            field = new CellState[rows, cols];
            scores = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    field[i, j] = cells[i][j] ? CellState.Filled : CellState.Empty;
                }
            }
        }

        public void DrawPoints(Cell[] points)
        {
            foreach (Cell point in points)
            {
                if (point.Row < 0 ||
                    point.Col < 0 ||
                    point.Row >= Rows ||
                    point.Col >= Columns)
                {
                    continue;
                }
                field[point.Row, point.Col] = CellState.Filled;
            }
        }

        private int GetCellScore(int row, int col)
        {
            if (row < 0 ||
                col < 0 ||
                row >= field.GetLength(0) ||
                col >= field.GetLength(1))
            {
                return 0;
            }
            return (int)field[row, col];
        }

        private int CountOfNeighbors(int row, int column)
        {
            int score = 0;
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = column - 1; j <= column + 1; j++)
                {
                    score += GetCellScore(i, j);
                }
            }
            return score - GetCellScore(row, column);
        }

        private CellState GetNewState(CellState currentState, int score) => (score, currentState) switch
        {
            (3, CellState.Empty) => CellState.Filled,
            (2 or 3, CellState.Filled) => CellState.Filled,
            _ => CellState.Empty
        };


        public void EvolutionStep()
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    scores[i, j] = CountOfNeighbors(i, j);
                }
            }

            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[i, j] = GetNewState(field[i, j], scores[i, j]);
                }
            }
        }

        public void SelectCell(int i, int j)
        {
            selected.Add(new Cell(i, j));
        }

        public bool IsSelected(int i, int j) => selected.Any(c => c.Row == i && c.Col == j);

        public void DeselectAll()
        {
            selected.Clear();
        }
    }
}
