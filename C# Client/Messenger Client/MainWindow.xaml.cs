using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using System.Windows.Documents;


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
    public partial class MainWindow : Window
    {

        ConnectionHandler ConnectionHandler;

        LoginControl LoginControl = new LoginControl();
        FriendsControl FriendsControl = new FriendsControl();
        MessageControl MessageControl;

        public MainWindow()
        {

            MessageControl = new MessageControl();


            InitializeComponent();

            MouseDown += OnMouseDown;
            KeyDown += OnKeyDown;

            Bold username = new Bold(new Run("Logged out"));

            UsernameHolder.Inlines.Add(username);

            ContentLeft.Content = LoginControl;
            ContentRight.Content = MessageControl;

            // Getting the singleton that handles the server connection.

            ConnectionHandler = ConnectionHandler.HandlerInstance;
            if (ConnectionHandler.Login("TestUsername", "TestPassword"))
            {

                LoginSuccessful();
            }

        }

        public void LoginSuccessful()
        {

            Bold username = new Bold(new Run(ConnectionHandler.GetUsername()));
            UsernameHolder.Inlines.Clear();
            UsernameHolder.Inlines.Add(username);

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
    }
}
