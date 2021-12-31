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

namespace Messenger_Client
{



    /// <summary>
    /// Interaction logic for MessageControl.xaml
    /// </summary>
    /// 

    public partial class MessageControl : UserControl
    {

        private int DebugMask = 32;

        private Controller Controller;

        private int ChatID; // Stores the currently active chat ID.



        private MainWindow MainWindow;


        public MessageControl()
        {

            MainWindow = Application.Current.MainWindow as MainWindow;

            Debug.WriteLine("Event subscribed to");


            InitializeComponent();

            Controller = Controller.ControllerInstance;

            MessageEntry.KeyDown += OnMessageKey;
            Controller.UpdateChatEvent += OnUpdateChatEvent;

        }

        
        public void OnUpdateChatEvent(object sender, UpdateChatEventArgs e)
        {

            PopulateMessages();
        }

        public void OnMessageEvent(object sender, MessageEventArgs e)
        {
            Debug.WriteLine("Message Event: " + e.Message + "\n");

            PopulateMessages();
        }



        private void PopulateMessages()
        {
            Chat chat = Controller.ActiveChat;
            List <Dictionary<String, String>> messageList = chat.RetrieveMessages();

            Debugger.Record("Populating messages.", DebugMask);

            Dispatcher.BeginInvoke(new ThreadStart(() => {

                if (chat.ChatID != ChatID)
                {
                    MessagePanel.Children.Clear();
                    ChatID = chat.ChatID;
                }

                for (int i = 0; i < messageList.Count; i++)
                {
                    NewMessage(messageList[i]);
                }
            }));
        }


        // Takes a tuple containing a new message.
        // Item 1: User who sent message.
        // Item 2: Time and date of message.
        // Item 3: The message itself.
        // Sends it to be added to the display via NewMessage,
        // then adds the SM header code and receiving user to the beginning and sends it to the
        // connection handler.

        private void SendMessage(Dictionary<string, string> message)
        {

            // Gets sender username with filler asterisks.


            NewMessage(message);


        }


        /// <summary> 
        /// Adds a new message to the interface. Not optimized for XAML binding, since it relies on code behind to set th emessage.
        /// </summary>  
        /// <param name="message">A dictionary containg message components: CreateTimestamp, SenderID, Message (Indicating message body)</param>

        public void NewMessage(Dictionary<string, string> message)
        {




            string timestamp = message["CreateTimestamp"];
            string senderID = message["SenderID"];
            string body = message["Message"];


            Debugger.Record("Inside dictionary NewMessage function. Message: " + body, DebugMask);

            string newMessage = timestamp + ": " + senderID + ": " + body;

            RichTextBox newBox = new RichTextBox();
            FlowDocument document = new FlowDocument();
            Paragraph paragraph = new Paragraph();

            paragraph.Inlines.Add(newMessage);

            document.Blocks.Add(paragraph);
            newBox.Document = document;

            MessagePanel.Children.Add(newBox);

        }

     

        public void NewMessage(Tuple<string, string, string> message)
        {

            string newMessage = message.Item1 + " " + message.Item2 + ": " + message.Item3;

            RichTextBox newBox = new RichTextBox();
            FlowDocument document = new FlowDocument();
            Paragraph paragraph = new Paragraph();

            paragraph.Inlines.Add(newMessage);

            document.Blocks.Add(paragraph);
            newBox.Document = document;

            MessagePanel.Children.Add(newBox);

        }



        private string GetDateTimeString()
        {
            DateTime currentTime = DateTime.Now;

            return currentTime.ToString();

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
                else if (newMessage.Length > 0)
                {

                    string dateTime = GetDateTimeString();

                    MessageEntry.Text = "";
                }
            }
        }


    }
}
