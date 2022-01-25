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

        private bool isSortDropdownVisible = false;

        public bool IsSortDropdownVisible
        {
            get
            {
                return isSortDropdownVisible;
            }
            set
            {
                isSortDropdownVisible = value;
                OnPropertyChanged(nameof(IsSortDropdownVisible));
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

        //private void OnFriendSelected(object sender, MouseButtonEventArgs e)
        //{
        //    Console.WriteLine("Friend selected");
        //    TextBlock senderBlock = sender as TextBlock;

        //    PointAnimation gradientAnim = new PointAnimation();
        //    gradientAnim.Duration = TimeSpan.FromSeconds(1.3);
        //    gradientAnim.From = new Point(0, 1);
        //    gradientAnim.To = new Point(1, 0);
        //    gradientAnim.AutoReverse = true;
        //    senderBlock.Background.BeginAnimation(LinearGradientBrush.EndPointProperty, gradientAnim);
        //}

        // Begin sort handling functionality.

        private void OnSortSelect(object sender, RoutedEventArgs e)
        {

            if (IsSortDropdownVisible == false)
            {
                IsSortDropdownVisible = true;
            }
            else
            {
                FrameworkElement element = sender as FrameworkElement;
                string parameter = element.Name;

                FriendSort(parameter);
                IsSortDropdownVisible = false;
            }
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


        private void OnSearchSelect(object sender, RoutedEventArgs e)
        {
            TextBox searchBox = sender as TextBox;
            searchBox.Clear();
        }

        private void OnSearchDeselect(object sender, RoutedEventArgs e)
        {
            TextBox searchBox = sender as TextBox;
            searchBox.Text = "Search";

            PopulateFriendsList(FriendList);
        }

        private void OnSearch(object sender, TextChangedEventArgs e)
        {
            TextBox searchBox = sender as TextBox;

            string searchText = searchBox.Text;

            List<FriendUser> foundItems = new List<FriendUser>();

            if (searchText != "")
            {
                for (int i = 0; i < FriendList.Count; i++)
                {
                    string nameString = FriendList[i].UserName;

                    if (nameString.Contains(searchText))
                    {
                        foundItems.Add(FriendList[i]);
                    }
                }
                PopulateFriendsList(foundItems);
            }
            else
            {
                PopulateFriendsList(FriendList);
            }
        }


        private void OnUpdateFriends(object sender, UpdateFriendsEventArgs e)
        {

            Debug.WriteLine("OnUpdateFriends activated in friends control.");

            FriendList = e.Friends;
            PopulateFriendsList(FriendList);
        }

        // ---------- BEGIN EVENT DELEGATES --------- //


        private void OnFriendSelected(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            int chatID = (int)button.Tag;
            string chatName = (string)button.Content;

            Controller.ChatSelected(chatID, chatName);
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
