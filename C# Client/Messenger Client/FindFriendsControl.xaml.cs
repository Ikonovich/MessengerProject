using Messenger_Client.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for FindFriendsControl.xaml
    /// </summary>
    public partial class FindFriendsControl : UserControl
    {
        private int DebugMask = 64;

        private Controller Controller;

        private List<Tuple<string, string>> UserResults;


        // Binding for request confirmation popup
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
                OnPropertyChanged(nameof(IsPopupOpen));
            }
        }

        private string popupMessage = "";
        public string PopupMessage
        {
            get
            {
                return popupMessage;
            }
            set
            {
                popupMessage = value;
                OnPropertyChanged(nameof(PopupMessage));
            }
        }


        public FindFriendsControl()
        {
            UserResults = new List<Tuple<string, string>>();


            InitializeComponent();

            Controller = Controller.ControllerInstance;
            
            Controller.UpdateUserSearchEvent += OnUpdateUserSearchResults;
            SearchBox.TextChanged += OnSearch;
            SearchBox.GotFocus += OnSearchSelect;
            //SearchBox.LostFocus += OnSearchDeselect;


        }
        private void OnSearchSelect(object sender, RoutedEventArgs args)
        {
            TextBox searchBox = sender as TextBox;
            searchBox.Clear();
        }

        private void OnSearchDeselect(object sender, RoutedEventArgs args)
        {
            TextBox searchBox = sender as TextBox;
            searchBox.Text = "Search";
        }

        private void ShowResults(List<Tuple<string, string>> results)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                UserResultsDisplay.ItemsSource = UserResults;
            });
        }

        private void OnSearch(object sender, TextChangedEventArgs args)
        {
            TextBox searchBox = sender as TextBox;

            string searchText = searchBox.Text;

            if ((searchText == "") || (searchText== "Search"))
            {
                UserResults = new List<Tuple<string, string>>();
                ShowResults(UserResults);
                return;
            }
            else if (searchText.Length > 32)
            {
                searchBox.Text = searchText.Substring(0, 32);
            }
            else
            {
                Debug.WriteLine("Searching for new friend with text: " + searchText);

                Controller.UserSearch(searchText);
            }

            UserResults = new List<Tuple<string, string>>();
            ShowResults(UserResults);
        
        }

        private void OnUpdateUserSearchResults(object sender, UpdateUserSearchEventArgs args)
        {
            List<Dictionary<string, string>> resultsList = args.Results;

            UserResults = new List<Tuple<string, string>>();


            for (int i = 0; i < resultsList.Count; i++)
            {
                UserResults.Add(new Tuple<string, string>(resultsList[i]["UserName"], resultsList[i]["UserID"]));
            }
            ShowResults(UserResults);

        }


        private void OnUserSelected(object sender, RoutedEventArgs args)
        {

            Button button = sender as Button;
            string username = button.Content as string;
            string userID = button.Tag as string;


            Debug.WriteLine("On User Selected has been called. Tag is: " + userID);

            int userIDint; 

            try
            {
                userIDint = int.Parse(userID);
            }
            catch (ArgumentNullException ex)
            {
                Debugger.Record("Argument null exception, OnUserSelected, FindFriendsControl.", DebugMask + 1);
                return;
            }
            catch (FormatException ex)
            {
                Debugger.Record("Format exception, OnUserSelected, FindFriendsControl.", DebugMask + 1);
                return;
            }

            catch (OverflowException ex)
            {
                Debugger.Record("Overflow exception, OnUserSelected, FindFriendsControl.", DebugMask + 1);
                return;
            }

            List<FriendUser> friendList = new();
            FriendUser friend;

            // Checks to see if the user selected is already a friend, then goes to that friend's chat if it is.
            for (int i = 0; i < friendList.Count; i++)
            {
                friend = friendList[i];

                if (friend.UserID == userIDint)
                {
                    Controller.ChatSelected(friend.ChatID, friend.UserName);
                    return;
                }

            }

            string message = "Would you like to send a friend request to " + username + "?";

            Controller.RaiseSelectionPopupEvent(message, "Yes", userID, "No", "None", username, "True");
        }


        //INotifyPropertyChanged members, needed to call popups.

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
