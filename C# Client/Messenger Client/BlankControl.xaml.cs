using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for BlankControl.xaml
    /// </summary>
    public partial class BlankControl : UserControl
    {

        private Controller Controller;

        private string displayText = "To begin, click the drop down and select 'Find Friends'.";

        public string DisplayText
        {
            get
            {
                return displayText;
            }
            set
            {
                displayText = value;
                OnPropertyChanged("DisplayText");
            }
        }

        public BlankControl()
        {
            InitializeComponent();

            Controller = Controller.ControllerInstance;

            Controller.ChangeBlankDisplayEvent += OnChangeBlankDisplay;

        }


        private void OnChangeBlankDisplay(object sender, MessageEventArgs args)
        {
            DisplayText = args.Message;
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
