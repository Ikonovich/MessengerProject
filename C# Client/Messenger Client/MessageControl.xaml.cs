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


namespace Messenger_Client
{

    /// <summary>
    /// Interaction logic for MessageControl.xaml
    /// </summary>
    /// 

    public partial class MessageControl : UserControl
    {

        List<Tuple<string, string, string>> MessageList = new();

        MainWindow MainWindow;

        // Stores the username of the user currently being spoken to.
        string ViewedFriend = "FriendlyUser";

        public MessageControl()
        {

            MainWindow = Application.Current.MainWindow as MainWindow;

            Debug.WriteLine("Event subscribed to");


            InitializeComponent();

            MessageEntry.KeyDown += OnMessageKey;

        }



        public void OnMessageEvent(object sender, MessageEventArgs e)
        {
            Debug.WriteLine("Message Event: " + e.Message + "\n");
            MessageList.Add(new Tuple<string, string, string>(GetDateTimeString(), "TestName", e.Message));
            PopulateMessages();
        }



        private void PopulateMessages()
        {

            Dispatcher.BeginInvoke(new ThreadStart(() => {

                MessagePanel.Children.Clear();

                for (int i = 0; i < MessageList.Count; i++)
                {
                    NewMessage(MessageList[i]);
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

        private void SendMessage(Tuple<string, string, string> message)
        {

            // Gets sender username with filler asterisks.


            NewMessage(message);


        }



        public void NewMessage(string message)
        {

            Debug.WriteLine("Calling single-parameter NewMessage function");


            string newMessage = message;

            RichTextBox newBox = new RichTextBox();
            FlowDocument document = new FlowDocument();
            Paragraph paragraph = new Paragraph();

            paragraph.Inlines.Add(newMessage);

            document.Blocks.Add(paragraph);
            newBox.Document = document;

            MessagePanel.Children.Add(newBox); 
            
        }

        // Takes a tuple containing a new message.
        // Item 1: User who sent message.
        // Item 2: Time and date of message.
        // Item 3: The message itself.

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
