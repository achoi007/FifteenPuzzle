using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifteenPuzzleLib
{
    /// <summary>
    /// Puzzle interface
    /// 
    /// A puzzle with height x width cells.  The empty slot has a cell value of 0.
    /// </summary>
    public interface IPuzzle
    {
        /// <summary>
        /// Width - number of columns
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Height - number of rows
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Return true if puzzle is solved.  Empty cell should be at upper left hand
        /// corner (0, 0)
        /// </summary>
        bool IsSolved { get; }

        /// <summary>
        /// Row-majored ordered of cell values.  Empty cell has value of 0
        /// </summary>
        IEnumerable<int> Cells { get; }

        /// <summary>
        /// Get cell value at given row and column.  Row/Column starts at 0.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        int this[int row, int col]
        {
            get;
        }

        /// <summary>
        /// Init puzzle to given width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void Init(int width, int height);

        /// <summary>
        /// Set cells to given row-major ordered values.  Count(cells) == Width * Height.
        /// </summary>
        /// <param name="cells"></param>
        void Set(IEnumerable<int> cells);

        /// <summary>
        /// Move empty cell to given direction.  CellChanged event will be generated
        /// if move is legal.
        /// </summary>
        /// <param name="dir"></param>
        void Move(Direction dir);

        /// <summary>
        /// Cell changed due to move - CellChanged(this, row, col, cellValue)
        /// </summary>
        event Action<IPuzzle, int, int, int> CellChanged;
    }
}
