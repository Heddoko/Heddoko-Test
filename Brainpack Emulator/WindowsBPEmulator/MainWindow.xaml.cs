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
using MahApps.Metro.Controls;

namespace WindowsBPEmulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ServerListener mListener;
        private ProtobuffDispatch mDispatcher;
        public ConsoleTextBlockController ConsoleTextBlockController;
        private BrainpackAdvertiser BrainpackAdvertiser;
        public MainWindow()
        {
            InitializeComponent();
         //   ConsoleTextBlockController = new ConsoleTextBlockController(ConsoleTextblock);
            mListener = new ServerListener(ConsoleTextBlockController);
            mListener.StartServer();
            BrainpackAdvertiser  = new BrainpackAdvertiser();
            BrainpackAdvertiser.ConsoleTextBlockController = ConsoleTextBlockController;
            BrainpackAdvertiser.StartAdvertising(6668,5000);
            mDispatcher = new ProtobuffDispatch(mListener);

            ConsoleTextBlockController.AddMsg("Start");
     //       ClearButton.Click += ConsoleTextBlockController.Clear;
        
        }

        private void CloseCustomDialog(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void LaunchMahAppsOnGitHub(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void LaunchVisualStudioDemo(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void LaunchFlyoutDemo(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void LaunchIcons(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void LauchCleanDemo(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void InteropDemo(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void LaunchNavigationDemo(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowInputDialog(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowLoginDialog(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowLoginDialogPasswordPreview(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowLoginDialogWithRememberCheckBox(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowLoginDialogOnlyPassword(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowMessageDialog(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowLimitedMessageDialog(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowProgressDialog(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowCustomDialog(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowAwaitCustomDialog(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowInputDialogOutside(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowLoginDialogOutside(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowMessageDialogOutside(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void ShowDialogOutside(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void MenuWindowWithoutBorderOnClick(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void MenuWindowWithBorderOnClick(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void MenuWindowWithGlowOnClick(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void MenuWindowWithShadowOnClick(object vSender, RoutedEventArgs vE)
        {
            throw new NotImplementedException();
        }

        private void LaunchSizeToContentDemo(object vSender, RoutedEventArgs vE)
        {
            

        }
 
        public enum MultiFrameImageMode
        {
            ScaleDownLargerFrame,
            NoScaleSmallerFrame,
        }
 
    }

}
