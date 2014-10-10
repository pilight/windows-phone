using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;
using pilight.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using WebSocket4Net;
using System.Linq;
using SuperSocket.ClientEngine;

namespace pilight.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<Location> _locations = new ObservableCollection<Location>();

        public WebSocket websocket;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            GlobalLoading.Instance.IsLoading = true;

            if (String.IsNullOrEmpty(new AppSettings().AddressSetting) || String.IsNullOrEmpty(new AppSettings().PortSetting))
            {
                MessageBox.Show("Please set settings.", "Connection error", MessageBoxButton.OK);
            }
            else
            {
                Connect();
            }
        }

        public bool IsLoaded
        {
            get;
            set;
        }

        public ObservableCollection<Location> Locations
        {
            get
            {
                return _locations;
            }
            private set { }
        }

        private void ParseResult(string p)
        {
            var json = JObject.Parse(p);

            if (json["config"] != null)
            {
                createGui(json["config"]);
            }
            else
            {
                updateGui(json["values"], json["devices"], json["settings"]);
            }

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                GlobalLoading.Instance.IsLoading = false;
            });
        }

        private void createGui(JToken config)
        {
            foreach (JProperty location in config.Children())
            {
                var loc = new Location() { Name = location.Name, Devices = new ObservableCollection<Device>() };

                foreach (JProperty property in location.First().Children())
                {
                    switch (property.Name)
                    {
                        case "name":
                            break;
                        case "order":
                            break;
                        default:
                            var device = new Device()
                            {
                                Key = (String)property.Name,
                                Name = (String)property.Value["name"],
                                Type = (DeviceType)Enum.Parse(typeof(DeviceType), (String)property.Value["type"], true),
                                Id = ParseId(property.Value["id"]),
                                Temperature = (property.Value["temperature"] != null) ? (double)property.Value["temperature"] : 0,
                                Humidity = (property.Value["humidity"] != null) ? (double)property.Value["humidity"] : 0,
                                State = (property.Value["state"] != null) ? (string)property.Value["state"] == "on" : false,
                                Protocol = ParseProtocol(property.Value["protocol"]),
                                devicedecimals = (property.Value["device-decimals"] != null) ? (int)property.Value["device-decimals"] : 0
                            };

                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                loc.Devices.Add(device);
                            });
                            break;
                    }
                }
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _locations.Add(loc);
                });
            }
        }
        private void updateGui(JToken jValues, JToken jDevices, JToken jSettings)
        {
            if (_locations.Count == 0)
                return;

            foreach (JProperty value in jValues)
            {
                double resultTemp = 0;
                bool resultState = false;

                switch (value.Name)
                {
                    case "temperature":
                        resultTemp = (double)jValues["temperature"];
                        break;

                    case "state":
                        resultState = ((string)jValues["state"] == "on");
                        break;

                    case "humidity":
                    case "battery":
                        break;
                }

                foreach (JProperty device in jDevices)
                {
                    var deviceName = device.Name;
                    var deviceKey = (String)(device.Value.First);

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        var loc = _locations.First(l => l.Name == deviceName);

                        var devicesToUpdate = from d in loc.Devices
                                              where d.Key == deviceKey
                                              select d;

                        foreach (var dev in devicesToUpdate)
                        {
                            dev.Temperature = resultTemp;
                            dev.State = resultState;
                        }
                    });
                }
            }
        }

        private Dictionary<String, String> ParseId(JToken ids)
        {
            Dictionary<String, String> returnValues = new Dictionary<String, String>();
            foreach (JProperty id in ids.Values())
            {
                returnValues.Add(id.Name, (String)id.Value);
            }
            return returnValues;
        }

        private List<String> ParseProtocol(JToken protocols)
        {
            return protocols.Values<String>().ToList();
        }

        private void Connect()
        {
            List<KeyValuePair<string, string>> Cookies = new List<KeyValuePair<string, string>>();

            string uri = String.Format("ws://{0}:{1}/", new AppSettings().AddressSetting, new AppSettings().PortSetting);
            websocket = new WebSocket(uri);

            websocket.Opened += websocket_Opened;
            websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);

            websocket.Open();
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            websocket.Send("{\"message\":\"request config\"}");
        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            ParseResult(e.Message);
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBoxResult result = MessageBox.Show("Try to reconnect?", "Connection closed", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    Connect();
                }
                else
                {
                    GlobalLoading.Instance.IsLoading = false;
                }
            });
        }

        private void websocket_Error(object sender, ErrorEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK);
            });
        }

        internal void SetState(string location, string device, string state)
        {
            var s = "{\"message\": \"send\",\"code\": {\"location\": \"" + location + "\",\"device\": \"" + device + "\",\"state\": \"" + state + "\"}}";
            this.websocket.Send(s);
        }
    }
}