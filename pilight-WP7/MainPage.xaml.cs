using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WebSocket4Net;
using System.IO;
using System.Text;
using pilight.Model;

namespace pilight_WP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        //private LocationRepository locations;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //locations = new LocationRepository();
            // Connect();
        }

    }
}