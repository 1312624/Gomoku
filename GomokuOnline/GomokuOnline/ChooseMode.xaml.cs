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

namespace Gomoku
{
    /// <summary>
    /// Interaction logic for ChooseMode.xaml
    /// </summary>
    public partial class ChooseMode : Window
    {
        public ChooseMode()
        {
            InitializeComponent();
        }

        private void Mode_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            if (bt.Name == "btPlayer")
            {
                GlobalVariable.Mode = 1;
            }
            else if (bt.Name == "btAI")
            {
                GlobalVariable.Mode = 2;
            }
            else if (bt.Name == "btPlayerOnl")
            {
                GlobalVariable.Mode = 3;
            }
            else GlobalVariable.Mode = 4;
            Close();
        }
    }
}
