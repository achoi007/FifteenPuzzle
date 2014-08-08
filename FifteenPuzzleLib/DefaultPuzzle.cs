using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifteenPuzzleLib
{
    public class DefaultPuzzle : IPuzzle
    {
        private int[,] mCells;
        private int mZeroRow, mZeroCol;

        public int Width
        {
            get { return mCells.GetLength(1); }
        }

        public int Height
        {
            get { return mCells.GetLength(0); }
        }

        public bool IsSolved
        {
            get
            {
                int i = 0;
                foreach (var cell in mCells)
                {
                    if (cell != i)
                    {
                        return false;
                    }
                    ++i;
                }

                return true;
            }
        }

        public IEnumerable<int> Cells
        {
            get
            {
                foreach (var cell in mCells)
                {
                    yield return cell;
                }
            }
        }

        public int this[int row, int col]
        {
            get { return mCells[row, col]; }
        }

        public void Init(int width, int height)
        {
            mCells = new int[height, width];
            Set(Enumerable.Range(0, width * height));
        }

        public void Set(IEnumerable<int> cells)
        {
            int height = Height;
            int width = Width;

            var cellIter = cells.GetEnumerator();
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    // Update cell value
                    cellIter.MoveNext();
                    int value = cellIter.Current;
                    mCells[row, col] = value;

                    // Set zero cell position
                    if (value == 0)
                    {
                        mZeroRow = row;
                        mZeroCol = col;
                    }

                    // Generate event 
                    CellChanged(this, row, col, value);
                }
            }
        }

        public void Move(Direction dir)
        {
            int swapRow, swapCol;

            // Calculate swap cell
            switch (dir)
            {
                case Direction.Up:
                    if (mZeroRow == 0)
                    {
                        return;
                    }
                    swapRow = mZeroRow - 1;
                    swapCol = mZeroCol;
                    break;

                case Direction.Down:
                    if (mZeroRow == Height - 1)
                    {
                        return;
                    }
                    swapRow = mZeroRow + 1;
                    swapCol = mZeroCol;
                    break;

                case Direction.Left:
                    if (mZeroCol == 0)
                    {
                        return;
                    }
                    swapRow = mZeroRow;
                    swapCol = mZeroCol - 1;
                    break;

                case Direction.Right:
                    if (mZeroCol == Width - 1)
                    {
                        return;
                    }
                    swapRow = mZeroRow;
                    swapCol = mZeroCol + 1;
                    break;

                default:
                    throw new ArgumentException("Illegal move: " + dir);
            }

            // Swap empty cell with swap cell
            int tmpVal = mCells[swapRow, swapCol];
            mCells[swapRow, swapCol] = mCells[mZeroRow, mZeroCol];
            mCells[mZeroRow, mZeroCol] = tmpVal;

            // Generate event
            CellChanged(this, swapRow, swapCol, 0);
            CellChanged(this, mZeroRow, mZeroCol, tmpVal);

            // Set position of empty cell
            mZeroRow = swapRow;
            mZeroCol = swapCol;
        }

        public event Action<IPuzzle, int, int, int> CellChanged = delegate { };
    }
}
