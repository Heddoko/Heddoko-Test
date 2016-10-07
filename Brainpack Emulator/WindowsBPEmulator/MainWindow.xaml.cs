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
using WindowsBPEmulator.Communication;
using WindowsBPEmulator.Controller;

namespace WindowsBPEmulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServerListener mListener;
        private ProtobuffDispatch mDispatcher;
        public ConsoleTextBlockController ConsoleTextBlockController;
        public MainWindow()
        {
            InitializeComponent();
            ConsoleTextBlockController = new ConsoleTextBlockController(ConsoleTextblock);
            mListener = new ServerListener(ConsoleTextBlockController);

            mDispatcher = new ProtobuffDispatch(mListener);

            ConsoleTextBlockController.AddMsg("Start");
            ClearButton.Click += ConsoleTextBlockController.Clear;
            mListener.StartServer();
        }
    }
}
