using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Diagnostics;
using Messenger_Client.SupportClasses;
using System.ComponentModel;

namespace Messenger_Client
{



    /// <summary>
    /// Interaction logic for MessageControl.xaml
    /// </summary>
    /// 

    public partial class MessageControl : UserControl, INotifyPropertyChanged
    {


        private int DebugMask = 32;

        // Binding for displaying the chat title
        private string chatTitle;
        public string ChatTitle
        {
            get
            {
                return chatTitle;
            }
            set
            {
                chatTitle = value;
                OnPropertyChanged(nameof(ChatTitle));
            }
        }

        private bool IsEditing = false;

        private int EditMessageID = 0;

        string SelectedMessageID = ""; // Stores the ID of the currently selected message, or 0.

        private Controller Controller;

        private Chat ActiveChat; // Stores the currently active chat.

        public List<Message> MessageList { get; private set; }

        private MainWindow MainWindow;

        public MessageControl()
        {

            MessageList = new();

            MainWindow = Application.Current.MainWindow as MainWindow;

            InitializeComponent();

            MessageList = new();


            Controller = Controller.ControllerInstance;

            MessageEntry.KeyDown += OnMessageKey;
            Controller.ChangeChatEvent += OnChangeChatEvent;

        }


        public void OnChangeChatEvent(object sender, ChangeChatEventArgs e)
        {
            ActiveChat = Controller.ActiveChat;
            Debugger.Record("ChatName is: " + ActiveChat.ChatName, DebugMask);
            MessageList = ActiveChat.RetrieveAll();

            ChatTitle = "Chat with " + ActiveChat.ChatName;

            PopulateMessages();
        }


        private void PopulateMessages()
        {
            try
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    MessageDisplay.ItemsSource = MessageList;
                });
            }
            catch (Exception e)
            {
                Debugger.Record("An error occurred attempting to populate messages: " + e.Message + e.StackTrace, DebugMask);
            }

        }


        private string GetDateTimeString()
        { 
            return DateTime.Now.ToString();
        }
        
        private Message GetMessage(string messageID)
        {

            for (int i = 0; i < MessageList.Count; i++)
            {
                Message tempMessage = MessageList[i];

                if (tempMessage.MessageID == messageID)
                {
                    return tempMessage;
                }
            }

            throw new IndexOutOfRangeException("MessageControl.GetMessage(): Did not find messageID " + messageID);
        }

        private void OnMessageKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string newMessage = MessageEntry.Text;

                if (Keyboard.IsKeyDown(Key.RightCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    MessageEntry.Text = newMessage + "\n";
                    MessageEntry.Select(MessageEntry.Text.Length, MessageEntry.Text.Length);
                }
                else if (IsEditing == true)
                {
                    Controller.EditMessage(EditMessageID, newMessage);
                    MessageEntry.Text = "";
                    IsEditing = false;
                    EditMessageID = 0;
                }
                else if (newMessage.Length > 0)
                {
                    Controller.SendMessage(newMessage);
                    MessageEntry.Text = "";
                }
            }
        }

        private void OnMessageRightClick(object sender, RoutedEventArgs args)
        {

            Debugger.Record("On Message Right Click in message control", DebugMask);

            Grid currentItem = sender as Grid;

            SelectedMessageID = (string)currentItem.Tag;


            ButtonMenu.PlacementTarget = currentItem;

            RestrictButton.Visibility = Visibility.Visible;
            ButtonMenu.IsOpen = true;

            string senderID = "0";

            for (int i = 0; i < MessageList.Count; i++)
            {
                Message tempMessage = MessageList[i];
                
                if (tempMessage.MessageID == SelectedMessageID)
                {
                    senderID = tempMessage.SenderID;
                    Debugger.Record("Sender ID: " + senderID + " Assigned to message: " + SelectedMessageID, DebugMask);
                    break;
                }
            }

            /// <summary> 
            /// This section of code controls the visibility of the Button Menu buttons.
            /// </summary>
            /// 

            EditButton.Visibility = Visibility.Collapsed;
            DeleteButton.Visibility = Visibility.Collapsed;
            RestrictButton.Visibility = Visibility.Collapsed;
            MuteButton.Visibility = Visibility.Collapsed;
            BanButton.Visibility = Visibility.Collapsed;
            NoOptionsIndicator.Visibility = Visibility.Collapsed;

            bool isEmpty = true;

            if (senderID == Controller.UserID.ToString())
            {
                EditButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
                isEmpty = false;
            }

            if ((ActiveChat.PermissionMask & (int)Permissions.Delete) == (int)Permissions.Delete)
            {
                DeleteButton.Visibility = Visibility.Visible;
                isEmpty = false;
            }

            if ((ActiveChat.PermissionMask & (int)Permissions.Restrict) == (int)Permissions.Restrict)
            {
                RestrictButton.Visibility = Visibility.Visible;
                isEmpty = false;
            }

            if ((ActiveChat.PermissionMask & (int)Permissions.Mute) == (int)Permissions.Mute)
            {
                MuteButton.Visibility = Visibility.Visible;
                isEmpty = false;
            }

            if ((ActiveChat.PermissionMask & (int)Permissions.Ban) == (int)Permissions.Ban)
            {
                BanButton.Visibility = Visibility.Visible;
                isEmpty = false;
            }

            if (isEmpty == true)
            {
                NoOptionsIndicator.Visibility = Visibility.Visible;
            }
        }


        private void OnMouseLeaveButtonMenu(object sender, RoutedEventArgs args)
        {
            ButtonMenu.IsOpen = false;
            SelectedMessageID = "0";
        }


        /// <summary> 
        /// Fires when the edit button is selected in the dropdown menu accessed when right clicking a message.
        /// </summary> 
        /// <param name="sender">Should be the button named EditButton.</param>
        /// <param name="args">The args sent by Button.Click</param>

        private void OnEditClick(object sender, RoutedEventArgs args)
        {

            Control control = sender as Control;
            Message tempMessage;
            try
            {
                tempMessage = GetMessage(SelectedMessageID);
                EditMessageID = int.Parse(tempMessage.MessageID);
                MessageEntry.Text = tempMessage.Body;
                IsEditing = true;
                ButtonMenu.IsOpen = false;
            }
            catch (FormatException e)
            {
                Debugger.Record(e.Message, DebugMask);
            }
            catch (IndexOutOfRangeException e)
            {
                Debugger.Record(e.Message, DebugMask);
            }
        }


        /// <summary> 
        /// Fires when the delete button is selected in the dropdown menu accessed when right clicking a message.
        /// </summary> 
        /// <param name="sender">Should be the button named DeleteButton.</param>
        /// <param name="args">The args sent by Button.Click</param>

        private void OnDeleteClick(object sender, RoutedEventArgs args)
        {
            Control Control = sender as Control;

            Controller.DeleteMessage(SelectedMessageID);
            ButtonMenu.IsOpen = false;
        }

        /// <summary> 
        /// Fires when the restrict button is selected in the dropdown menu accessed when right clicking a message.
        /// </summary> 
        /// <param name="sender">Should be the button named MuteButton.</param>
        /// <param name="args">The args sent by Button.Click</param>
        private void OnRestrictClick(object sender, RoutedEventArgs args)
        {
            Controller.RaiseNotificationPopupEvent("Restricting uploads has not yet been implemented.");
            ButtonMenu.IsOpen = false;
        }

        /// <summary> 
        /// Fires when the mute button is selected in the dropdown menu accessed when right clicking a message.
        /// </summary> 
        /// <param name="sender">Should be the button named MuteButton.</param>
        /// <param name="args">The args sent by Button.Click</param>
        private void OnMuteClick(object sender, RoutedEventArgs args)
        {
            Controller.RaiseNotificationPopupEvent("Muting users has not yet been implemented.");
            ButtonMenu.IsOpen = false;
        }

        private void OnBanClick(object sender, RoutedEventArgs args)
        {
            Controller.RaiseNotificationPopupEvent("Banning users has not yet been implemented.");
            ButtonMenu.IsOpen = false;
        }

        //INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            Debugger.Record("Property change called for: " + propertyName, 2);
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
