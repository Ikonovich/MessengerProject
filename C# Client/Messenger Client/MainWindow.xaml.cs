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
using System.Windows.Navigation;

namespace Messenger_Client
{

    /// <summary>
    /// MainWindow is the surrounding class for the rest of the GUI. 
    /// It receives events from the Controller to determine what elements to display and when.
    /// 
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private int DebugMask = 64;

        //////////// ------------ Bound properties ---------------/////////

        // Bindings for popup messages

        // Selection popup.
        // The way these are handles is super clunky and needs to be seriously updated. Maybe a special class for 
        // option events.

        private bool isSelectionPopupOpen;

        public bool IsSelectionPopupOpen
        {
            get
            {
                return isSelectionPopupOpen;
            }
            set
            {
                isSelectionPopupOpen = value;
                OnPropertyChanged(nameof(IsSelectionPopupOpen));
            }
        }

        private string selectionPopupMessage = "";
        public string SelectionPopupMessage
        {
            get
            {
                return selectionPopupMessage;
            }
            set
            {
                selectionPopupMessage = value;
                OnPropertyChanged(nameof(SelectionPopupMessage));
            }
        }

        private string optionOneText = "";
        public string OptionOneText
        {
            get
            {
                return optionOneText;
            }
            set
            {
                optionOneText = value;
                OnPropertyChanged(OptionOneText);
            }
        }
        private string optionTwoText = "";
        public string OptionTwoText
        {
            get
            {
                return optionTwoText;
            }
            set
            {
                optionTwoText = value;
                OnPropertyChanged(OptionTwoText);
            }
        }

        private string optionOneTag = "";
        public string OptionOneTag
        {
            get
            {
                return optionOneTag;
            }
            set
            {
                optionOneTag = value;
                OnPropertyChanged(OptionOneTag);
            }
        }

        private string optionTwoTag = "";
        public string OptionTwoTag
        {
            get
            {
                return optionTwoTag;
            }
            set
            {
                optionTwoTag = value;
                OnPropertyChanged(OptionTwoTag);
            }
        }

        private string SelectionParameterOne = "";

        private string SelectionParameterTwo = "";

        // Notification popup
        private bool isNotificationPopupOpen;

        public bool IsNotificationPopupOpen
        {
            get
            {
                return isNotificationPopupOpen;
            }
            set
            {
                isNotificationPopupOpen = value;
                OnPropertyChanged(nameof(IsNotificationPopupOpen));
            }
        }

        private string notificationPopupMessage = "";
        public string NotificationPopupMessage
        {
            get
            {
                return notificationPopupMessage;
            }
            set
            {
                notificationPopupMessage = value;
                OnPropertyChanged(nameof(NotificationPopupMessage));
            }
        }

        // Binding for requests link visibility


        private Visibility requestVisibility = Visibility.Collapsed;

        public Visibility RequestVisibility
        {
            get
            {
                return requestVisibility;
            }
            set
            {
                requestVisibility = value;
                OnPropertyChanged(nameof(RequestVisibility));
            }
        }



        // Bindings for current view options dropdown

        // Visibility

        private Visibility isCurrentViewVisible = Visibility.Hidden;

        public Visibility IsCurrentViewVisible
        {
            get
            {
                return isCurrentViewVisible;
            }
            set
            {
                isCurrentViewVisible = value;
                OnPropertyChanged(nameof(IsCurrentViewVisible));
            }
        }

        private bool isOptionsDropdownVisible = false;

        public bool IsOptionsDropdownVisible
        {
            get
            {
                return isOptionsDropdownVisible;
            }
            set
            {
                isOptionsDropdownVisible = value;
                OnPropertyChanged(nameof(IsOptionsDropdownVisible));
            }
        }

        // Used by the drop down panel to show the current view.

        private string currentLeftView = "N/A";
        public string CurrentLeftView
        {
            get
            {
                return currentLeftView;
            }
            set
            {
                currentLeftView = value;
                OnPropertyChanged(nameof(CurrentLeftView));
            }
        }


        // Bindings for username display

        private string displayText = "Not logged in";
        public string DisplayText
        {
            get
            {
                return displayText;
            }
            set
            {
                displayText = value;
                OnPropertyChanged(nameof(DisplayText));
            }
        }

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
            }
        }

        // Used to summon the username dropdown

        private bool isUserNameDropdownVisible = false;

        public bool IsUserNameDropdownVisible
        {
            get
            {
                return isUserNameDropdownVisible;
            }
            set
            {
                isUserNameDropdownVisible = value;
                OnPropertyChanged(nameof(IsUserNameDropdownVisible));
            }
        }


        // Bindings for content display

        private UserControl sidebarSegment;

        public UserControl SidebarSegment
        {
            get
            {
                return sidebarSegment;
            }
            set
            {
                sidebarSegment = value;
                OnPropertyChanged(nameof(SidebarSegment));
            }
        }

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
                OnPropertyChanged(nameof(RightSegment));
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
                OnPropertyChanged(nameof(LeftSegment));
            }
        }


        // Binding to orient the position of the main panel. Primarily used before login to center the login/registration
        // panels. 

        private HorizontalAlignment rightSegmentAlignment = HorizontalAlignment.Center;

        public HorizontalAlignment RightSegmentAlignment
        {
            get
            {
                return rightSegmentAlignment;
            }
            set
            {
                rightSegmentAlignment = value;
                OnPropertyChanged("RightSegmentAlignment");
            }
        }

        //////////// --------- End bound properties ---------- ///////////////


        // Singletons
        private Controller Controller;

        public LoginControl LoginControl;
        public FriendsControl FriendsControl;
        public FindFriendsControl FindFriendsControl;
        public MessageControl MessageControl; 
        public RegistrationControl RegistrationControl;
        public ChatsViewControl ChatsViewControl;
        public RequestsControl RequestsControl;
        public BlankControl BlankControl;
        public Sidebar Sidebar;


        public MainWindow()
        {

            MessageControl = new MessageControl();
            LoginControl = new LoginControl();
            RegistrationControl = new RegistrationControl();
            FriendsControl = new FriendsControl();
            FindFriendsControl = new FindFriendsControl();
            ChatsViewControl = new ChatsViewControl();
            RequestsControl = new RequestsControl();
            BlankControl = new BlankControl();
            Sidebar = new Sidebar();

            Controller = Controller.ControllerInstance;


            InitializeComponent();



            // Subscribing to events

            MouseDown += OnMouseDown;
            KeyDown += OnKeyDown;

            Controller.SelectionPopupEvent += OnSelectionPopupEvent;
            Controller.NotificationPopupEvent += OnNotificationPopupEvent;
            Controller.ChangeViewEvent += OnChangeView;
            Controller.ChangeUsernameEvent += OnChangeUsername;
            Controller.UpdateRequestsEvent += OnUpdateRequests;


            //// Setting the default view mode.
            Controller.RaiseChangeViewEvent(Segment.Right, ViewType.LoginView);
            SidebarSegment = Sidebar;

            Controller.Connect();
        }


        // -------- Begin Event Handling ---------- //


        // Signalled by controller events to update the content of the various panels
        private void OnChangeView(object sender, ChangeViewEventArgs e)
        {

            ViewType view = e.ViewType;
            Segment segment = e.Segment;

            IsOptionsDropdownVisible = false;

            Debug.WriteLine("On change view called for " + e.ViewType + " and " + e.Segment + "\n");
            Debug.WriteLine("On change view called");
            UserControl control = MessageControl;

            string newView = "";

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
                    newView = "Friends List";
                    break;
                case ViewType.FindFriendsView:
                    control = FindFriendsControl;
                    newView = "Find Friends";
                    break;
                case ViewType.ChatsView:
                    control = ChatsViewControl;
                    newView = "Chats";
                    break;
                case ViewType.RequestsView:
                    control = RequestsControl;
                    break;
                case ViewType.Blank:
                    control = BlankControl;
                    break;
                default:
                    //Defaults to message view
                    break;
            }

            if (segment == Segment.Left)
            {
                Debug.WriteLine("Setting left segment.\n");
                CurrentLeftView = newView;
                LeftSegment = control;
            }
            else
            {
                Debug.WriteLine("Setting right segment.\n");
                RightSegment = control;
            }

        }


        // Signalled by left side drop down menu.

        private void OnLeftViewSelect(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Left view selected");

            FrameworkElement element = sender as FrameworkElement;

            string parameter = element.Name;

            if (parameter == "FriendsListView")
            {
                Controller.RaiseChangeViewEvent(Segment.Left, ViewType.FriendsView);
            }
            else if (parameter == "FindFriendsView")
            {
                Controller.RaiseChangeViewEvent(Segment.Left, ViewType.FindFriendsView);
            }
            else if (parameter == "ChatsView")
            {
                Controller.RaiseChangeViewEvent(Segment.Left, ViewType.ChatsView);
            }
        }



        // Used to transition into the requests view.

        private void OnRequestsSelected(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Requests selected");
            Debugger.Record("Request view selected.", DebugMask);
            Controller.RaiseChangeViewEvent(Segment.Right, ViewType.RequestsView);
        }



        // Signalled by controller events to display a selection popup.
        private void OnSelectionPopupEvent(object sender, SelectionPopupEventArgs e)
        {



            SelectionPopupMessage = e.Message;

            OptionOneText = e.OptionOneText;
            OptionTwoText = e.OptionTwoText;
            OptionOneTag = e.OptionOneTag;
            OptionTwoTag = e.OptionTwoTag;
            SelectionParameterOne = e.ParameterOne;
            SelectionParameterTwo = e.ParameterTwo;

            Debug.WriteLine("Selection Pop up event detected in Main. Option one: " + OptionOneText + OptionOneTag + " Option Two: " + OptionTwoText + OptionTwoTag + " Parameter one: " + SelectionParameterOne + " Parameter two: " + SelectionParameterTwo);

            IsSelectionPopupOpen = true;
        }

        // Used to take input from a popup selection.
        private void OnPopupSelection(object sender, RoutedEventArgs args)
        {
            Debug.WriteLine("Popup selection made.");

            Button button = sender as Button;

            string tag = "";
            string text = "";

            if (button.Name == "ButtonOne")
            {
                Controller.SendFriendRequest(SelectionParameterOne, OptionOneTag);
            }

            IsSelectionPopupOpen = false;


            Debug.WriteLine("Tag and Text of button selected: " + tag + "  " + text);
        }


        // Signalled by controller events to display a notification popup.
        private void OnNotificationPopupEvent(object sender, NotificationPopupEventArgs args)
        {

            Debug.WriteLine("Pop up event detected: " + args.Message);

            NotificationPopupMessage = args.Message;
            IsNotificationPopupOpen = true;
        }

        // Used to close any popup
        
        private void OnClosePopup(object sender, RoutedEventArgs args)
        {
            IsNotificationPopupOpen = false;
            IsSelectionPopupOpen = false;
        }

        // 

        // Used to change the username display
        private void OnChangeUsername(object sender, MessageEventArgs args)
        {


            Username = args.Message;

            if (Username != "NONE")
            {
                IsCurrentViewVisible = Visibility.Visible; ;
                DisplayText = "Logged in as " + Username;

                // Reposition the right segment panel to the right side, rather than in the center as it's kept for
                // login/registration.
                RightSegmentAlignment = HorizontalAlignment.Right;
            }
            else
            {
                DisplayText = "Not logged in";
                IsCurrentViewVisible = Visibility.Hidden;

                // Reposition the right segment panel to the right side, rather than in the center as it's kept for
                // login/registration.
                RightSegmentAlignment = HorizontalAlignment.Center;
            }
        }


        // Used to implement click-and-drag of the window.
        private void OnMouseDown(object sender, MouseButtonEventArgs args)
        {

            if (args.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }


        // Used to detect when the escape key is being pressed, which closes the application.
        private void OnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }


        private void OnExitButton(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

        private void OnMinimizeButton(object sender, RoutedEventArgs args)
        {
            WindowState = WindowState.Minimized;
        }

        public void OnUpdateRequests(object sender, UpdateRequestsEventArgs args)
        {

            if (args.Requests.Count > 0)
            {
                RequestVisibility = Visibility.Visible;
            }
            else
            {
                RequestVisibility = Visibility.Collapsed;
            }
        }

        // Activated when the username at the top left of the window is clicked.
        private void OnUserNameDropdown(object sender, RoutedEventArgs args)
        {
            if (Controller.SessionID != "")
            {
                IsUserNameDropdownVisible = true;
            }
        }

        // Activated when the an option is selected from the user options dropdown.
        private void OnUserDropdownSelect(object sender, RoutedEventArgs args)
        {
            if (Controller.SessionID != "")
            {
                Button button = sender as Button;
                if (button.Name == "LogoutButton")
                {
                    Controller.Logout();
                }
            }
            IsUserNameDropdownVisible = false;
        }

        // Activated when the top-level left panel view is clicked.
        private void OnLeftViewDropdown(object sender, RoutedEventArgs args)
        {
            IsOptionsDropdownVisible = true;
        }




        //INotifyPropertyChanged members, needed to call popups.
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
