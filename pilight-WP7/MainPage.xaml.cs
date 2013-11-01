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
using pilight.ViewModel;

namespace pilight_WP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            MainViewModel mvm = DataContext as MainViewModel;

            Device deviceToChange = (Device)((ToggleSwitch)sender).Header;

            foreach (Location loc in mvm.Locations)
            {
                foreach (Device dev in loc.Devices.Where(d => d.Key == deviceToChange.Key))
                {
                    var s = "{\"message\": \"send\",\"code\": {\"location\": \"" + loc.Name + "\",\"device\": \"" + dev.Key + "\",\"state\": \"on\"}}";
                    mvm.websocket.Send(s);
                }
            }
        }

        private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            MainViewModel mvm = DataContext as MainViewModel;

            Device deviceToChange = (Device)((ToggleSwitch)sender).Header;

            foreach (Location loc in mvm.Locations)
            {
                foreach (Device dev in loc.Devices.Where(d => d.Key == deviceToChange.Key))
                {
                    var s = "{\"message\": \"send\",\"code\": {\"location\": \"" + loc.Name + "\",\"device\": \"" + dev.Key + "\",\"state\": \"off\"}}";
                    mvm.websocket.Send(s);
                }
            }

        }
    }
}