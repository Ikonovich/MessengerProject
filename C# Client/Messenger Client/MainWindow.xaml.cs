using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using System.Windows.Documents;
using System.ComponentModel;
using System.Diagnostics;

namespace Messenger_Client
{

    /// <summary>
    /// MainWindow is the surrounding class for the rest of the GUI. 
    /// It receives events from the Controller to determine what elements to display and when.
    /// 
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        //////////// ------------ Bound properties ---------------/////////

        // Bindings for popup messages
        private bool isPopupOpen;

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

        private string popupMessage = "No Username";
        public string PopupMessage
        {
            get
            {
                return popupMessage;
            }
            set
            {
                popupMessage = value;
                OnPropertyChanged("PopupMessage");
            }
        }

        // Bindings for username display

        private string username = "No Username";
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

        // Bindings for content display

        private UserControl rightSegment;

        public UserControl RightSegment
        {
            get
            {
                return rightSegment;
            }
            set
            {
                rightSegment = value;
                OnPropertyChanged("RightSegment");
            }
        }


        private UserControl leftSegment;

        public UserControl LeftSegment
        {
            get
            {
                return leftSegment;
            }
            set
            {
                leftSegment = value;
                OnPropertyChanged("LeftSegment");
            }
        }


        //////////// --------- End bound properties ---------- ///////////////


        // Singletons
        private Controller Controller;

        private LoginControl LoginControl = new LoginControl();
        private FriendsControl FriendsControl = new FriendsControl();
        private MessageControl MessageControl = new MessageControl();
        private RegistrationControl RegistrationControl = new RegistrationControl();

        public MainWindow()
        {

            MessageControl = new MessageControl();
            LoginControl = new LoginControl();
            RegistrationControl = new RegistrationControl();

            Controller = Controller.ControllerInstance;


            InitializeComponent();



            // Subscribing to events

            MouseDown += OnMouseDown;
            KeyDown += OnKeyDown;


            Controller.PopupEvent += OnPopupEvent;
            Controller.ChangeViewEvent += OnChangeView;
            Controller.ChangeUsernameEvent += OnChangeUsername;


            // Setting the default view mode.
            LoginView();
        }


        // Called by other windows to switch to the registration view.

        public void RegistrationView()
        {
            RightSegment = RegistrationControl;
        }

        public void MessageView()
        {
            RightSegment = MessageControl;
            LeftSegment = FriendsControl;
        }

        // Called by other windows to switch to the login view.

        public void LoginView()
        {
            RightSegment= LoginControl;
        }



        // -------- Begin Event Handling ---------- //


        // Signalled by controller events to update the content of the various panels
        private void OnChangeView(object sender, ChangeViewEventArgs e)
        {

            ViewType view = e.ViewType;
            Segment segment = e.Segment;


            Debug.WriteLine("On change view called for " + e.ViewType + " and " + e.Segment + "\n");
            Debug.WriteLine("On change view called");
            UserControl control = MessageControl;

            switch (view)
            {
                case ViewType.LoginView:
                    control = LoginControl;
                    break;
                case ViewType.RegisterView:
                    control = RegistrationControl;
                    break;
                case ViewType.FriendsView:
                    control = FriendsControl;
                    break;
                default:
                    //Defaults to message view
                    break;
            }

            if (segment == Segment.Left)
            {
                Debug.WriteLine("Setting left segment.\n");
                LeftSegment = control;
            }
            else
            {
                Debug.WriteLine("Setting right segment.\n");
                RightSegment = control;
            }

        }


        // Signalled by controller events to display a popup.
        private void OnPopupEvent(object sender, PopupEventArgs e)
        {

            Debug.WriteLine("Pop up event detected: " + e.Message);

            PopupMessage = e.Message;
            IsPopupOpen = true;
        }

        // Used to change the username display

        private void OnChangeUsername(object sender, MessageEventArgs e)
        {
            Username = e.Message;

        }


        // Used to implement click-and-drag of the window.
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }


        // Used to detect when the escape key is being pressed, which closes the application.
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



        //INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {

            Debug.WriteLine("Property change called for property " + propertyName);
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
