using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Messenger_Client
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {

        MainWindow MainWindow;
        Controller Controller;

        public LoginControl()
        {
            InitializeComponent();

            MainWindow = Application.Current.MainWindow as MainWindow;
            Controller = Controller.ControllerInstance;

        }

        public void GoToRegistration(object sender, RequestNavigateEventArgs e)
        {
            Debug.WriteLine("Registration event fired.");
            Controller.RaiseChangeViewEvent(Segment.Right, ViewType.RegisterView);
        }


        public void LoginAttempt()
        {

            string username = LoginUsername.Text;
            string password = LoginPassword.Text;

            Debug.WriteLine("Asking controller to login.");

            Controller.Login(username, password);

        }

        private void OnLoginButton(object sender, RequestNavigateEventArgs e)
        {
            LoginAttempt();
        }

        private void OnMessageKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginAttempt();
            }
        }
    }
}
