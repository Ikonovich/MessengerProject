using System;
using System.Collections.Generic;
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
    /// Interaction logic for Sidebar.xaml
    /// </summary>

    public partial class Sidebar : UserControl
    {


        private Controller Controller;

        public Sidebar()
        {

            InitializeComponent();

            Controller = Controller.ControllerInstance;
        }


        private void OnFriendsClick(object sender, RoutedEventArgs args)
        {
            Controller.RaiseChangeViewEvent(Segment.Left, ViewType.FriendsView);
        }

        private void OnChatsClick(object sender, RoutedEventArgs args)
        {
            Controller.RaiseChangeViewEvent(Segment.Left, ViewType.ChatsView);
        }

        private void OnFindFriendsClick(object sender, RoutedEventArgs args)
        {
            Controller.RaiseChangeViewEvent(Segment.Right, ViewType.FindFriendsView);
        }
    }
}
