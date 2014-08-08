using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifteenPuzzleLib
{
    public class DefaultSolver : IPuzzleSolver
    {
        public string Name
        {
            get { return "DefaultSolver"; }
        }

        public IEnumerable<Direction> Solve(IPuzzle puzzle)
        {
            Direction[] dirs = { Direction.Up, Direction.Left, Direction.Down, Direction.Right };
            return dirs;
        }
    }
}
