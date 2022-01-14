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

    // A struct that stores the data about individual messages for display.

    public class Message
    {
        public string MessageID { get; private set; }
        public string SenderID { get; private set; }
        public string SenderName { get; private set; }
        public string CreateTimestamp { get; private set; }
        public string Body { get; private set; }

        public Message(Dictionary<string, string> messageDict)
        {
            MessageID = messageDict["MessageID"];
            SenderID = messageDict["SenderID"];
            SenderName = messageDict["SenderName"];
            CreateTimestamp = messageDict["CreateTimestamp"];
            Body = messageDict["Message"];
        }

    }


    /// <summary>
    /// Interaction logic for MessageControl.xaml
    /// </summary>
    /// 

    public partial class MessageControl : UserControl
    {

        private int DebugMask = 32;

        private Controller Controller;

        private int ChatID; // Stores the currently active chat ID.

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

            Chat chat = Controller.ActiveChat;
            Debugger.Record("ChatID is: " + ChatID + " . New chat ID is: " + chat.ChatID, DebugMask);
            MessageList = chat.RetrieveAll();

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
        MessagePanel.UpdateLayout();

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
                Controller.SendMessage(newMessage);
                MessageEntry.Text = "";
            }
        }
    }


}
}
