using System;
using System.Collections.Generic;
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
    public partial class FriendsControl : UserControl
    {

        List<Tuple<string, Messenger_Client.Activity, float, float>> Friends = new();

        public FriendsControl()
        {


            InitializeComponent();


            ActiveSort.Selected += OnSortSelect;
            AlphabeticalSort.Selected += OnSortSelect;
            RecentSort.Selected += OnSortSelect;
            FavoriteSort.Selected += OnSortSelect;

            SearchBox.TextChanged += OnSearch;

            Friends.Add(new Tuple<string, Messenger_Client.Activity, float, float>("FriendOne", Activity.Active, 12, 0));
            Friends.Add(new Tuple<string, Messenger_Client.Activity, float, float>("FriendTwo", Activity.Offline, 5, 6));
            Friends.Add(new Tuple<string, Messenger_Client.Activity, float, float>("FriendThree", Activity.Offline, 200, 3));
            Friends.Add(new Tuple<string, Messenger_Client.Activity, float, float>("FriendFour", Activity.Active, 36, 4));
            Friends.Add(new Tuple<string, Messenger_Client.Activity, float, float>("FriendFive", Activity.Busy, 12, 1));


            FriendSort("ActiveSort");
        }

        private void PopulateFriendsList(List<Tuple<string, Messenger_Client.Activity, float, float>> friends)
        {

            FriendsPanel.Children.Clear();

            for (int i = 0; i < friends.Count; i++)
            {
                string friendName = friends[i].Item1;
                TextBlock newBlock = new TextBlock();
                newBlock.Text = friendName;
                newBlock.Name = friendName;
                newBlock.Margin = new Thickness(1, 1, 1, 1);
                newBlock.Background = new LinearGradientBrush(Colors.LightBlue, Colors.SlateBlue, 90);
                newBlock.MouseLeftButtonDown += new MouseButtonEventHandler(FriendSelected);
                FriendsPanel.Children.Add(newBlock);
            }

        }

        private void FriendSelected(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Friend selected");
            TextBlock senderBlock = sender as TextBlock;

            PointAnimation gradientAnim = new PointAnimation();
            gradientAnim.Duration = TimeSpan.FromSeconds(1.3);
            gradientAnim.From = new Point(0, 1);
            gradientAnim.To = new Point(1, 0);
            gradientAnim.AutoReverse = true;
            senderBlock.Background.BeginAnimation(LinearGradientBrush.EndPointProperty, gradientAnim);
        }

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

            List<Tuple<string, Messenger_Client.Activity, float, float>> sortedFriends = Friends;

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

            Friends = sortedFriends;
            PopulateFriendsList(Friends);
        }

        private int AlphabeticalCompare(Tuple<string, Messenger_Client.Activity, float, float> itemOne, Tuple<string, Messenger_Client.Activity, float, float> itemTwo)
        {

            string stringOne = itemOne.Item1;
            string stringTwo = itemOne.Item1;
            return string.Compare(stringOne, stringTwo);
        }

        private int ActiveCompare(Tuple<string, Messenger_Client.Activity, float, float> itemOne, Tuple<string, Messenger_Client.Activity, float, float> itemTwo)
        {
            Messenger_Client.Activity friendOneActivity = itemOne.Item2;
            Messenger_Client.Activity friendTwoActivity = itemTwo.Item2;

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

        private int RecentCompare(Tuple<string, Messenger_Client.Activity, float, float> itemOne, Tuple<string, Messenger_Client.Activity, float, float> itemTwo)
        {

            float friendOneTime = itemOne.Item3;
            float friendTwoTime = itemTwo.Item3;

            if (friendOneTime < friendTwoTime)
            {
                return 1;
            }
            else if (friendOneTime > friendTwoTime)
            {

                return -1;
            }
            return 0;
        }


        private int FavoriteCompare(Tuple<string, Messenger_Client.Activity, float, float> itemOne, Tuple<string, Messenger_Client.Activity, float, float> itemTwo)
        {
            float friendOneRank = itemOne.Item4;
            float friendTwoRank = itemTwo.Item4;

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

        private void OnSearch(object sender, TextChangedEventArgs e)
        {
            TextBox searchBox = sender as TextBox;

            string searchText = searchBox.Text;

            List<Tuple<string, Messenger_Client.Activity, float, float>> foundItems = new List<Tuple<string, Messenger_Client.Activity, float, float>>();

            for (int i = 0; i < Friends.Count; i++)
            {
                string nameString = Friends[i].Item1;

                if (nameString.Contains(searchText))
                {
                    foundItems.Add(Friends[i]);
                }
            }
            PopulateFriendsList(foundItems);
        }
    }
}
