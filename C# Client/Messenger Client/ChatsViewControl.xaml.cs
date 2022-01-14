using Messenger_Client.SupportClasses;
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
    /// Interaction logic for ChatsViewControl.xaml
    /// </summary>
    public partial class ChatsViewControl : UserControl
    {

        private Controller Controller;

        private List<Chat> ChatList;

        public ChatsViewControl()
        {
            InitializeComponent();

            Controller = Controller.ControllerInstance;

            Controller.UpdateChatEvent += OnUpdateChats;
        }

        private void PopulateChatsList(List<Chat> chats)
        {

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ChatList = chats;
                ChatsDisplay.ItemsSource = ChatList;
            });
        }

        private void OnChatSelected(object sender, RoutedEventArgs args)
        {
            
            Button button = sender as Button;

            int chatID = (int)button.Tag;
            string chatName = button.Content as string;

            Controller.ChatSelected(chatID, chatName);
        }


        private void OnUpdateChats(object sender, UpdateChatEventArgs args)
        {

            ChatList = args.ChatList;
            PopulateChatsList(ChatList);
        }
    }
}
