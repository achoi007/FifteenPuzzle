using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FifteenPuzzleLib
{
    public class DefaultPuzzleMVVM : IPuzzleMVVM
    {
        private IPuzzle mPuzzle;
        private int mWidth, mHeight;
        private bool mIsSolved;
        private string mStatus;
        private ObservableCollection<string> mCells;
        private CancellationTokenSource mCxlSrc;

        private void NotifyChange([CallerMemberName] string propName = null)
        {
            PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propName));
        }

        private bool IsNotActive(object o)
        {
            return mCxlSrc == null;
        }

        private void ResetCxlSrc()
        {
            mCxlSrc = null;
            IsSolved = Puzzle.IsSolved;
            CommandManager.InvalidateRequerySuggested();
        }

        public DefaultPuzzleMVVM()
        {
            // Member vars
            MoveDelay = 100;

            // Initialize puzzle
            var puzzle = new DefaultPuzzle();
            puzzle.Init(4, 4);
            Puzzle = puzzle;

            // Initialize solver
            Solver = new DefaultSolver();

            // Create new game command
            NewGameCmd = new RelayCommand(o =>
            {
                SetCells(Enumerable.Range(0, Puzzle.Width * Puzzle.Height));
            },
            IsNotActive);

            // Create move command
            MoveCmd = new RelayCommand(o =>
            {
                if (o is Direction)
                {
                    Puzzle.Move((Direction)o);
                }
                else
                {
                    var dir = Enum.Parse(typeof(Direction), o.ToString());
                    Puzzle.Move((Direction)dir);
                }
            },
            IsNotActive);
            
            // Create start randomization command
            StartRandomizeCmd = new RelayCommand(async o =>
            {
                Status = "Randomizing";

                // Set up cancellation support
                mCxlSrc = new CancellationTokenSource();
                var cxlToken = mCxlSrc.Token;

                // List of legal moves
                Direction[] legalMoves = { Direction.Down, Direction.Left, Direction.Right, Direction.Up };
                Random rnd = new Random();

                while (!cxlToken.IsCancellationRequested)
                {
                    var dir = legalMoves[rnd.Next(0, legalMoves.Length)];
                    Puzzle.Move(dir);
                    try
                    {
                        await Task.Delay(MoveDelay, cxlToken);
                    }
                    catch (Exception)
                    {

                    }
                }

                ResetCxlSrc();
            },
            IsNotActive);

            // Create command to solve puzzle
            StartSolveCmd = new RelayCommand(async o =>
            {
                Status = "Solving";

                // Set up cancellation support
                mCxlSrc = new CancellationTokenSource();
                var cxlToken = mCxlSrc.Token;

                // List of moves
                var moves = Solver.Solve(Puzzle);
                var moveIter = moves.GetEnumerator();

                while (moveIter.MoveNext() && !cxlToken.IsCancellationRequested)
                {
                    var dir = moveIter.Current;
                    Puzzle.Move(dir);
                    try
                    {
                        await Task.Delay(MoveDelay, cxlToken);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                ResetCxlSrc();
                Status = "OK";
            },
            IsNotActive);

            // Create stop command
            StopCmd = new RelayCommand(o =>
            {
                mCxlSrc.Cancel();
                ResetCxlSrc();
                Status = "OK";
            }, 
            o => !IsNotActive(o));

            // Change puzzle size
            ChangeSizeCmd = new RelayCommand(o =>
            {
                // Get new dimension
                Dimension dimension;
                if (!ChangeSizeMediator.Query("number of rows and number of columns separated by space", out dimension))
                {
                    return;
                }

                // Ignore if size is the same
                if (Puzzle.Width == dimension.Width && Puzzle.Height == dimension.Height)
                {
                    return;
                }

                // Change size
                var newPuzzle = new DefaultPuzzle();
                newPuzzle.Init(dimension.Width, dimension.Height);
                Puzzle = newPuzzle;
            },
            o => IsNotActive(o) && ChangeSizeMediator != null);
        }

        public IMvvmMediator<Dimension> ChangeSizeMediator { get; set; }

        public IPuzzle Puzzle
        {
            get
            {
                return mPuzzle;
            }

            set
            {
                // Unhook event processing from old puzzle
                if (mPuzzle != null)
                {
                    mPuzzle.CellChanged -= mPuzzle_CellChanged;                    
                }

                // Hook up event processing for new puzzle
                mPuzzle = value;
                mPuzzle.CellChanged += mPuzzle_CellChanged;

                // Initialize various dynamic attributes
                Cells = new ObservableCollection<string>(mPuzzle.Cells.Select(c => c == 0 ? " " : c.ToString()));
                Width = mPuzzle.Width;
                Height = mPuzzle.Height;
                IsSolved = mPuzzle.IsSolved;
                Status = "OK";
            }
        }

        private void mPuzzle_CellChanged(IPuzzle puzzle, int row, int col, int value)
        {
            Cells[row * Width + col] = value == 0 ? " " : value.ToString();
        }

        public IPuzzleSolver Solver
        {
            get;
            set;
        }

        public int Width
        {
            get { return mWidth; }
            set { mWidth = value; NotifyChange(); }
        }

        public int Height
        {
            get { return mHeight; }
            set { mHeight = value; NotifyChange(); }
        }

        public bool IsSolved
        {
            get { return mIsSolved; }
            set { mIsSolved = value; NotifyChange(); }
        }

        public string Status 
        {
            get { return mStatus; }
            set { mStatus = value; NotifyChange(); }
        }

        public int MoveDelay { get; set; }

        public System.Collections.ObjectModel.ObservableCollection<string> Cells
        {
            get { return mCells; }
            set { mCells = value; NotifyChange(); }
        }

        public System.Windows.Input.ICommand NewGameCmd
        {
            get;
            private set;
        }

        public System.Windows.Input.ICommand MoveCmd
        {
            get;
            private set;
        }

        public System.Windows.Input.ICommand StartRandomizeCmd
        {
            get;
            private set;
        }

        public System.Windows.Input.ICommand StartSolveCmd
        {
            get;
            private set;
        }

        public System.Windows.Input.ICommand StopCmd
        {
            get;
            private set;
        }

        public System.Windows.Input.ICommand ChangeSizeCmd
        {
            get;
            private set;
        }

        public void SetCells(IEnumerable<int> cells)
        {
            Puzzle.Set(cells);
            IsSolved = Puzzle.IsSolved;
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged = delegate { };



    }
}
