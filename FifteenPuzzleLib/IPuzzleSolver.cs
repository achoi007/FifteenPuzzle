using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifteenPuzzleLib
{
    public interface IPuzzleSolver
    {
        /// <summary>
        /// Name of puzzle solver algorithm
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Attempt to solve puzzle and return moves.  Throws exception
        /// if puzzle is unsolvable.
        /// </summary>
        /// <param name="puzzle"></param>
        /// <returns></returns>
        IEnumerable<Direction> Solve(IPuzzle puzzle);
    }
}
