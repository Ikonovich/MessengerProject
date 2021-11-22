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

        ConnectionHandler ConnectionHandler;
        public LoginControl()
        {
            InitializeComponent();

            MainWindow = Application.Current.MainWindow as MainWindow;

        }


        public void LoginAttempt()
        {


        }

        public void GoToRegistration(object sender, RequestNavigateEventArgs e)
        {
            Debug.WriteLine("Registration event fired.");
            MainWindow.RegistrationView();
        }


        public void LoginAttempt(object sender, RequestNavigateEventArgs e)
        {
            string username = LoginUsername.Text;
            string password = LoginPassword.Text;

            if (username.Length < 8 || username.Length > 32)
            {


            }
            else if (password.Length < 8 || password.Length > 128)
            {

            }

        }
    }
}
