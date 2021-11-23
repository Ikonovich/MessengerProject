using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using System.Windows.Documents;
using System.ComponentModel;

namespace Messenger_Client
{

    public enum Activity
    {
        Active = 0,
        Busy = 1,
        Offline = 2
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        // Singletons
        Controller Controller;

        LoginControl LoginControl = new LoginControl();
        FriendsControl FriendsControl = new FriendsControl();
        MessageControl MessageControl = new MessageControl();
        RegistrationControl RegistrationControl = new RegistrationControl();

        private bool isPopupOpen = false;
        public bool IsPopupOpen
        {
            get 
            { 
                return isPopupOpen; 
            }
            set
            {
                isPopupOpen = value;
                OnPropertyChanged("IsPopupOpen");
            }
        }

        private string username = "Not Logged In";
        public string Username
        {
            get 
            { 
                return username; 
            }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        public MainWindow()
        {

            MessageControl = new MessageControl();
            LoginControl = new LoginControl();
            RegistrationControl = new RegistrationControl();

            InitializeComponent();

            MouseDown += OnMouseDown;
            KeyDown += OnKeyDown;

            ContentRight.Content = LoginControl;

        }

        public void LoginSuccessful(string username)
        {
            Username = username;
            ContentLeft.Content = FriendsControl;

        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        private void OnExitButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void RegistrationView()
        {
            ContentRight.Content = RegistrationControl;
        }

        public void LoginView()
        {
            ContentRight.Content = LoginControl;
        }


        public void ShowPopup(string message)
        {
            Run run = new Run(message);
            Bold text = new Bold(run);
            PopupText.Inlines.Add(text);
            IsPopupOpen = true;
        }


        //INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
