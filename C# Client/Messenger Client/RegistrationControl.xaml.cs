using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class RegistrationControl : UserControl
    {

        MainWindow MainWindow;
        Controller Controller;

        public RegistrationControl()
        {
            InitializeComponent();

            MainWindow = Application.Current.MainWindow as MainWindow;
            Controller = Controller.ControllerInstance;
        }

        public void RegistrationAttempt()
        {
            string username = RegistrationUsername.Text;
            string passwordOne = RegistrationPasswordOne.Text;
            string passwordTwo = RegistrationPasswordTwo.Text;

            // Parameter checking //
            if (username.Length < 8 || username.Length > 32)
            {
                return;
            }
                // Performs a regex match to ensure only underscores and alphanumeric characters are in the username.
            Regex reg = new Regex("^A - Za - z\\d_", RegexOptions.None);

            if (reg.Match(username).Success == true)
            {
                return;
            }

            else if (passwordOne.Length < 8 || passwordOne.Length > 128)
            {
                return;
            }
            else if (!string.Equals(passwordOne, passwordTwo))
            {
                return;
            }

            // End parameter checks // 

            // Provided parameters are acceptable, send them to the controller.
            Controller.Register(username, passwordOne);
            
        }

        public void GoToLogin(object sender, RequestNavigateEventArgs e)
        {
            MainWindow.LoginView();
        }

        public void GoToRegistrationAttempt(object sender, RequestNavigateEventArgs e)
        {
            RegistrationAttempt();
        }


    }
}
