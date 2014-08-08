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
using System.Windows.Shapes;

namespace FifteenPuzzle
{
    /// <summary>
    /// Interaction logic for PromptWindow.xaml
    /// </summary>
    public partial class PromptWindow : Window
    {
        private Type[] mInputTypes;

        public PromptWindow()
        {
            InitializeComponent();
        }

        public string Prompt
        {
            get { return txtPrompt.Text; }
            set { txtPrompt.Text = value; }
        }

        public string InputString
        {
            get { return txtInput.Text; }
            set { txtInput.Text = value; }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

    }
}
