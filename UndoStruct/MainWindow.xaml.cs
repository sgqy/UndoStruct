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

namespace UndoStruct
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            refresh();
        }

        UndoPacker<string> ups = new UndoPacker<string>(2);

        void refresh()
        {
            listBox.Items.Clear();
            btnRedo.IsEnabled = btnUndo.IsEnabled = false;
            if ((ups.state() & UndoPacker<string>.RedoAvaliable) != 0) btnRedo.IsEnabled = true;
            if ((ups.state() & UndoPacker<string>.UndoAvaliable) != 0) btnUndo.IsEnabled = true;

            var sl = ups.content();
            foreach (var s in sl)
            {
                listBox.Items.Add(s);
            }
        }

        private void btnPush_Click(object sender, RoutedEventArgs e)
        {
            ups.push(textBox.Text);
            refresh();
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text = ups.undo();
            refresh();
        }

        private void btnRedo_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text = ups.redo();
            refresh();
        }
    }
}
