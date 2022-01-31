using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            Debug.WriteLine("Making registration attempt");


            string username = RegistrationUsername.Text;
            string passwordOne = RegistrationPasswordOne.Text;
            string passwordTwo = RegistrationPasswordTwo.Text;

            // Parameter checking //
            if (username.Length < 8)
            {
                Controller.RaiseNotificationPopupEvent("Username must be at least 8 characters.");
                return;
            }
            else if (username.Length > 32)
            {
                Controller.RaiseNotificationPopupEvent("Username must be no more than 32 characters.");
            }

            // Performs a regex match to ensure only underscores and alphanumeric characters are in the username.
            Regex reg = new Regex("^A - Za - z\\d_", RegexOptions.None);

            if (reg.Match(username).Success == true)
            {
                Controller.RaiseNotificationPopupEvent("Username must contain only Alphanumeric characters or underscores.");
                return;
            }

            if (passwordOne.Length < 8)
            {
                Controller.RaiseNotificationPopupEvent("Password must be at least 8 characters.");
                return;
            }
            if (passwordOne.Length > 128)
            {
                Controller.RaiseNotificationPopupEvent("Password must be 128 characters or less.");
                return;
            }
            if (!string.Equals(passwordOne, passwordTwo))
            {
                Controller.RaiseNotificationPopupEvent("Password fields must match.");
                return;
            }

            // End parameter checks // 

            // Provided parameters are acceptable, send them to the controller.
            Controller.Register(username, passwordOne);

        }

        public void GoToLogin(object sender, RequestNavigateEventArgs e)
        {
            Controller.RaiseChangeViewEvent(Segment.Right, ViewType.LoginView);
        }

        public void GoToRegistrationAttempt(object sender, RequestNavigateEventArgs e)
        {
            RegistrationAttempt();
        }


    }
}
