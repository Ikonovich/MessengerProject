using Messenger_Client.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class ChatsViewControl : UserControl, INotifyPropertyChanged
    {
        // Binding for showing the create chat dialog
        private Visibility createChatVisibility = Visibility.Collapsed;

        public Visibility CreateChatVisibility
        {
            get
            {
                return createChatVisibility;
            }
            set
            {
                createChatVisibility = value;
                OnPropertyChanged(nameof(CreateChatVisibility));
            }
        }



        private Controller Controller;

        // Stores all chats that have an associated user-chat-pair for this user.
        private List<Chat> ChatList;

        // Stores all chats that have an associated user-chat-pair for this user AND where where IsMember == true.
        private List<Chat> VisibleChatList;


        public ChatsViewControl()
        {
            InitializeComponent();

            Controller = Controller.ControllerInstance;

            Controller.UpdateChatEvent += OnUpdateChats;
        }


        /// <summary> 
        /// Receives a list of chats and displays each chat where the user is a member. 
        /// </summary>  
        /// <param name="Chats">The list of chats to display if allowable</param>


        private void PopulateChatsList(List<Chat> chats)
        {

            List<Chat> tempList = new List<Chat>();

            for (int i = 0; i < chats.Count; i++)
            {
                Chat tempChat = chats[i];

                if (tempChat.IsMember == true)
                {
                    tempList.Add(tempChat);
                }

            }

            VisibleChatList = tempList;

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                VisibleChatList = chats;
                ChatsDisplay.ItemsSource = VisibleChatList;
            });
        }


        private void OnOpenCreateChat(object sender, RoutedEventArgs args)
        {
            CreateChatVisibility = Visibility.Visible;
        }

        private void OnCancelCreateChat(object sender, RoutedEventArgs args)
        {
            CreateChatVisibility = Visibility.Collapsed;
        }

        private void OnCreateChat(object sender, RoutedEventArgs args)
        {
            string chatName = ChatTitleEntry.Text;

            if (Controller.CreateChat(chatName) == true)
            {
                CreateChatVisibility = Visibility.Collapsed;
            }
        }

        private void OnChatSelected(object sender, RoutedEventArgs args)
        {

            Button button = sender as Button;

            int chatID = (int)button.Tag;
            string chatName = button.Content as string;

            Controller.ChatSelected(chatID);
        }


        private void OnUpdateChats(object sender, UpdateChatEventArgs args)
        {

            ChatList = args.ChatList;

            PopulateChatsList(ChatList);
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
