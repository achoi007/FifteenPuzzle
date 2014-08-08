using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FifteenPuzzleLib
{
    public interface IPuzzleMVVM : INotifyPropertyChanged
    {
        IMvvmMediator<Dimension> ChangeSizeMediator { get; set; }

        IPuzzle Puzzle { get; set; }

        IPuzzleSolver Solver { get; set; }

        int Width { get; }

        int Height { get; }

        bool IsSolved { get; }

        string Status { get; }

        /// <summary>
        /// How long to wait (in milliseconds) between each move in either solvecmd or randomcmd.
        /// </summary>
        int MoveDelay { get; set; }

        /// <summary>
        /// Row-major ordered cell values.
        /// </summary>
        ObservableCollection<string> Cells { get; }

        ICommand NewGameCmd { get; }

        /// <summary>
        /// Start moving cells randomly until StopCmd is issued
        /// </summary>
        ICommand StartRandomizeCmd { get; }

        /// <summary>
        /// Parameter should be stringify Direction
        /// </summary>
        ICommand MoveCmd { get; }

        /// <summary>
        /// Start solving puzzle until StopCmd is issued.
        /// </summary>
        ICommand StartSolveCmd { get; }

        /// <summary>
        /// Stop currently started action (e.g. StartSolveCmd or StartRandomizeCmd)
        /// </summary>
        ICommand StopCmd { get; }

        /// <summary>
        /// Change size of puzzle
        /// </summary>
        ICommand ChangeSizeCmd { get; }

        /// <summary>
        /// Set cells of puzzle in row-major ordered to given cells.
        /// </summary>
        /// <param name="cells"></param>
        void SetCells(IEnumerable<int> cells);
    }
}
