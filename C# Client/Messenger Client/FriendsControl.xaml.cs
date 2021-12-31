using Messenger_Client.SupportClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messenger_Client
{

    /// <summary>
    /// Interaction logic for FriendsControl.xaml
    /// </summary>
    public partial class FriendsControl : UserControl, INotifyPropertyChanged
    {

        private Controller Controller;

        public List<FriendUser> FriendList;

        private int DebugMask = 64;

        public FriendsControl()
        {

            InitializeComponent();

            Controller = Controller.ControllerInstance;
            FriendList = new();

            // Subscribing to events

            Controller.UpdateFriendsEvent += OnUpdateFriends;

            ActiveSort.Selected += OnSortSelect;
            AlphabeticalSort.Selected += OnSortSelect;
            RecentSort.Selected += OnSortSelect;
            FavoriteSort.Selected += OnSortSelect;

            SearchBox.TextChanged += OnSearch;
            
        }

        private void PopulateFriendsList(List<FriendUser> friends)
        {

            Debug.WriteLine("Populating friends list.");
            
    
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                FriendList = friends;
                FriendsDisplay.ItemsSource = FriendList;
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
            Debug.WriteLine("Sort selected");
            FrameworkElement element = sender as FrameworkElement;
            string parameter = element.Name;

            FriendSort(parameter);
        }

        private void FriendSort(string parameter)
        {

            List<FriendUser> sortedFriends = FriendList;

            if (parameter == "ActiveSort")
            {
                sortedFriends.Sort(ActiveCompare);
            }
            else if (parameter == "AlphabeticalSort")
            {
                sortedFriends.Sort(AlphabeticalCompare);
            }
            else if (parameter == "RecentSort")
            {
                sortedFriends.Sort(RecentCompare);

            }

            else if (parameter == "FavoriteSort")
            {
                sortedFriends.Sort(FavoriteCompare);
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


        private void OnSearch(object sender, TextChangedEventArgs e)
        {
            TextBox searchBox = sender as TextBox;

            string searchText = searchBox.Text;

            List<FriendUser> foundItems = new List<FriendUser>();

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


        private void OnUpdateFriends(object sender, UpdateFriendsEventArgs e)
        {

            Debug.WriteLine("OnUpdateFriends activated in friends control.");

            PopulateFriendsList(e.Friends);
        }

        // ---------- BEGIN EVENT DELEGATES --------- //


        private void OnFriendSelected(object sender, RoutedEventArgs e)
        {
            Button friendButton = sender as Button;

            int chatID = (int)friendButton.Tag;

            Controller.ChatSelected(chatID);
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
