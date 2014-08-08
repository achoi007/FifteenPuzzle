using FifteenPuzzleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FifteenPuzzle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMvvmMediator<Dimension>
    {
        private IPuzzleMVVM mPuzzleModel;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize puzzle
            var puzzle = new DefaultPuzzle();
            puzzle.Init(4, 4);

            // Initialize MVVM
            mPuzzleModel = new DefaultPuzzleMVVM() { Puzzle = puzzle };
            mPuzzleModel.ChangeSizeMediator = this;
            DataContext = mPuzzleModel;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// ChangeSize mediator used by MVVM
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Query(string prompt, out Dimension value)
        {
            value = new Dimension();
            // Prompt for user input
            var promptWin = new PromptWindow() { Prompt = prompt };
            if (promptWin.ShowDialog() != true)
            {
                return false;
            }

            // Process user input
            var userInput = promptWin.InputString.Split(' ', ',');
            int width, height;

            if (int.TryParse(userInput[1], out width))
            {
                value.Width = width;
            }
            else
            {
                return false;
            }

            if (int.TryParse(userInput[0], out height))
            {
                value.Height = height;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
