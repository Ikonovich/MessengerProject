using Messenger_Client.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Messenger_Client
{

    /// <summary>
    /// Interaction logic for FriendsControl.xaml
    /// </summary>
    public partial class FriendsControl : UserControl, INotifyPropertyChanged
    {

        private int DebugMask = 64;

        // Determines whether or not the sort options dropdown is currently visible.

        private Visibility searchPanelVisibility = Visibility.Collapsed;

        public Visibility SearchPanelVisibility
        {
            get
            {
                return searchPanelVisibility;
            }
            set
            {
                searchPanelVisibility = value;
                OnPropertyChanged(nameof(SearchPanelVisibility));
            }
        }

        private string currentSort = "Sort"; // Stores the currently selected sort selection for display.
        public string CurrentSort
        {
            get
            {
                return currentSort;
            }
            set
            {
                currentSort = value;
                OnPropertyChanged(nameof(CurrentSort));
            }
        }



        private Controller Controller;

        public List<FriendUser> FriendList; // Stores all friends.

        public List<FriendUser> VisibleFriendList; // Stores what friends are visible at the current time.


        public FriendsControl()
        {

            InitializeComponent();

            Controller = Controller.ControllerInstance;
            FriendList = new();

            // Subscribing to events

            Controller.UpdateFriendsEvent += OnUpdateFriends;
        }

        private void PopulateFriendsList(List<FriendUser> friends)
        {

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                VisibleFriendList = friends;
                FriendsDisplay.ItemsSource = VisibleFriendList;
            });
        }

    


        private void FriendSort(string parameter)
        {

            List<FriendUser> sortedFriends = FriendList;

            if (parameter == "ActiveSort")
            {
                sortedFriends.Sort(ActiveCompare);
                CurrentSort = "Active";
            }
            else if (parameter == "AlphabeticalSort")
            {
                sortedFriends.Sort(AlphabeticalCompare);
                CurrentSort = "Alpha";

            }
            else if (parameter == "RecentSort")
            {
                sortedFriends.Sort(RecentCompare);
                CurrentSort = "Recent";
            }

            else if (parameter == "FavoriteSort")
            {
                sortedFriends.Sort(FavoriteCompare);

                CurrentSort = "Favorite";
            }

            FriendList = sortedFriends;
            PopulateFriendsList(FriendList);

        }

        private int AlphabeticalCompare(FriendUser itemOne, FriendUser itemTwo)
        {
            string stringOne = itemOne.UserName;
            string stringTwo = itemOne.UserName;
            return string.Compare(stringOne, stringTwo);
        }

        private int ActiveCompare(FriendUser itemOne, FriendUser itemTwo)
        {
            Messenger_Client.Activity friendOneActivity = itemOne.Activity;
            Messenger_Client.Activity friendTwoActivity = itemTwo.Activity;

            if (friendOneActivity > friendTwoActivity)
            {
                return 1;
            }
            else if (friendOneActivity < friendTwoActivity)
            {

                return -1;
            }
            return 0;

        }

        private int RecentCompare(FriendUser itemOne, FriendUser itemTwo)
        {

            //float friendOneTime = itemOne.Item4;
            //float friendTwoTime = itemTwo.Item4;

            //if (friendOneTime < friendTwoTime)
            //{
            //    return 1;
            //}
            //else if (friendOneTime > friendTwoTime)
            //{

            //    return -1;
            //}
            return 0;
        }


        private int FavoriteCompare(FriendUser itemOne, FriendUser itemTwo)
        {
            float friendOneRank = itemOne.FriendRanking;
            float friendTwoRank = itemTwo.FriendRanking;

            if (friendOneRank < friendTwoRank)
            {
                return 1;
            }
            else if (friendOneRank > friendTwoRank)
            {

                return -1;
            }
            return 0;
        }

        // End sort handling functionality.


        // Begin event handlers

        private void OnCancelSearch(object sender, RoutedEventArgs args)
        {
            SearchPanelVisibility = Visibility.Collapsed;
        }


        private void OnOpenSearch(object sender, RoutedEventArgs args)
        {
            SearchPanelVisibility = Visibility.Visible;
            SearchBox.Text = "Search";
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

            PopulateFriendsList(Controller.FriendsList);
        }

        private void OnSearch(object sender, TextChangedEventArgs args)
        {
            TextBox searchBox = sender as TextBox;

            string searchText = searchBox.Text.ToLower();

            List<FriendUser> searchResults = Controller.FriendSearch(searchText);



            if (searchResults.Count > 0)
            {
                PopulateFriendsList(searchResults);
            }
            else
            {
                PopulateFriendsList(Controller.FriendsList);
            }
        }


        private void OnUpdateFriends(object sender, UpdateFriendsEventArgs args)
        {

            Debug.WriteLine("OnUpdateFriends activated in friends control.");

            PopulateFriendsList(Controller.FriendsList);
        }

        // ---------- BEGIN EVENT DELEGATES --------- //


        private void OnFriendSelected(object sender, RoutedEventArgs args)
        {
            Button button = sender as Button;

            int friendID = (int)button.Tag;

            Controller.FriendSelected(friendID);
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
