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
    public partial class RequestsControl : UserControl
    {

        //////// BOUND PROPERTIES ///////

        // List of requests that will be displayed
        private List<Tuple<string, string>> RequestList;



        //////// END BOUND PROPERTIES ///////

        private Controller Controller;


        public RequestsControl()
        {
            RequestList = new List<Tuple<string, string>>();
            InitializeComponent();

            Controller = Controller.ControllerInstance;

            Controller.UpdateRequestsEvent += OnUpdateRequests;
        }


        private void PopulateRequests()
        {

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
               RequestsDisplay.ItemsSource = RequestList;

               if (RequestList.Count < 1)
               {
                    DisplayMessage.Text = "You have 0 requests waiting.";
               }
               else if (RequestList.Count == 1)
               {
                    DisplayMessage.Text = "You have 1 request waiting.";
               }
               else
               { 
                    DisplayMessage.Text = "You have " + RequestList.Count + " requests waiting.";
               }


            });
        }

        public void OnUpdateRequests(object sender, UpdateRequestsEventArgs e)
        {
            RequestList = new List<Tuple<string, string>>();

            for (int i = 0; i < e.Requests.Count; i++)
            {
                Dictionary<string, string> requestDict = e.Requests[i];
                Tuple<string, string> request = new Tuple<string, string>(requestDict["UserName"], requestDict["UserID"]);
                RequestList.Add(request);
            }

            PopulateRequests();
        }

        public void OnRequestSelected(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;
            string username = button.Content as string;
            string userID = button.Tag as string;


            Debug.WriteLine("On Request Selected has been called. Button tag is: " + userID);

            string message = "Would you like to add " + username + " as a friend?";

            Controller.RaiseSelectionPopupEvent(message, "Yes", userID, "No", "None", username, "False");
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
