using Messenger_Client.SupportClasses;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messenger_Client
{

    /// <summary>
    /// Interaction logic for RequestsControl.xaml
    /// </summary>
    public partial class RequestsControl : UserControl, INotifyPropertyChanged
    {

        private static readonly int DebugMask = 64;

        //////// BOUND PROPERTIES ///////



        // Binding for request confirmation popup
        private Visibility confirmationPopupVisibility = Visibility.Collapsed;
        public Visibility ConfirmationPopupVisibility
        {
            get
            {
                return confirmationPopupVisibility;
            }
            set
            {
                confirmationPopupVisibility = value;
                OnPropertyChanged(nameof(ConfirmationPopupVisibility));
            }
        }


        private string popupMessage = "";
        public string PopupMessage
        {
            get
            {
                return popupMessage;
            }
            set
            {
                popupMessage = value;
                OnPropertyChanged(nameof(PopupMessage));
            }
        }

        // Stores the ID of any currently selected request
        int SelectedRequestID;

        // Contains all requests
        private List<Request> RequestList;


        // Contains the display items specifically for incoming requests.
        private List<Request> Inbox;


        // Contains the display items specifically for outgoing requests.
        private List<Request> Outbox;

        //////// END BOUND PROPERTIES ///////

        private Controller Controller;


        public RequestsControl()
        {
            RequestList = new List<Request>();
            InitializeComponent();

            Controller = Controller.ControllerInstance;

            Controller.UpdateRequestsEvent += OnUpdateRequests;
        }


        private void PopulateRequests()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
               InboxDisplay.ItemsSource = Inbox;

                if (Inbox.Count < 1)
                {
                    InboxMessage.Text = "You have 0 requests waiting.";
                }
                else if (Inbox.Count == 1)
                {
                    InboxMessage.Text = "You have 1 request waiting.";
                }
                else
                {
                    InboxMessage.Text = "You have " + RequestList.Count + " requests waiting.";
                }

            });
        }

        public void OnUpdateRequests(object sender, UpdateRequestsEventArgs args)
        {

            Debug.WriteLine("Requests updating. Request count: " + args.Requests.Count);


           Debugger.Record("Requests updating. Request count: " + args.Requests.Count, DebugMask);

            RequestList = new List<Request>();
            Inbox = new List<Request>();
            Outbox = new List<Request>();

            for (int i = 0; i < args.Requests.Count; i++)
            {
                Dictionary<string, string> requestDict = args.Requests[i];

                Request newRequest = new Request(requestDict);

                RequestList.Add(newRequest);

                if (newRequest.IsInbox == true)
                {
                    Inbox.Add(newRequest);
                }
                else
                {
                    Outbox.Add(newRequest);
                }
            }

            PopulateRequests();
        }


      
        /// <summary> 
        /// Responsible for displaying a selected request.
        /// </summary>
        public void OnRequestClick(object sender, RoutedEventArgs e)
        {


            FrameworkElement source = sender as FrameworkElement;
            int requestID = (int)source.Tag;


            Debug.WriteLine("ID of selected request: " + requestID);


            SelectedRequestID = requestID;
            Request requestActual = RequestList[0];

            for (int i = 0; i < RequestList.Count; i++)
            {

                if (RequestList[i].RequestID == SelectedRequestID)
                {
                    requestActual = RequestList[i];
                    break;
                }
            }

            if (requestActual.RequestType == "INVITE")
            {
                ConfirmationText.Text = "Accept the invite to " + requestActual.ChatName + "?";
            }

            else if (requestActual.RequestType == "FRIEND")
            {
                ConfirmationText.Text = "Accept the friend request from " + requestActual.SenderName + "?";
            }

            ConfirmationPopupVisibility = Visibility.Visible;
        }



        /// <summary> 
        /// Responsible for approving a selected request.
        /// </summary>
        public void OnRequestApprove(object sender, RoutedEventArgs e)
        {

            Debug.WriteLine("ID of selected request: " + SelectedRequestID);

            ConfirmationPopupVisibility = Visibility.Collapsed;

            Controller.ApproveRequest(SelectedRequestID);

            SelectedRequestID = 0;
        }


        /// <summary> 
        /// Responsible for declining a selected request.
        /// </summary>
        public void OnRequestDecline(object sender, RoutedEventArgs e)
        {
            FrameworkElement source = sender as FrameworkElement;
            int requestID = (int)source.Tag;


            Debug.WriteLine("ID of selected request: " + SelectedRequestID);

            SelectedRequestID = 0;
            ConfirmationPopupVisibility = Visibility.Collapsed;

        }


        //INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {

            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
