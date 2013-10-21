
using System;
using System.Collections.Generic;
using System.Linq;

using GalaSoft.MvvmLight;
using pilight.Model;
using System.Collections.ObjectModel;

using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SuperSocket.ClientEngine;
using WebSocket4Net;
using GalaSoft.MvvmLight.Threading;
using System.Windows.Threading;
using System.Windows;

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

        private WebSocket websocket;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Connect();
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
            var result = JsonConvert.DeserializeObject(p);
            var json = JObject.Parse(p);

            if (json["config"] != null)
            {
                createGui(json["config"]);
            }
            else
            {

                var x = JsonConvert.DeserializeObject<Update>(p);


                /// OK!
                var resultTemp = (float)json["values"]["temperature"] / 1000;

                foreach (JProperty device in json["devices"])
                {
                    var deviceName = device.Name;
                    var deviceKey = (String)(device.Value.First);

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                       {
                           var loc = _locations.First(l => l.Name == deviceName);

                           var dev = loc.Devices.FirstOrDefault(d => d.Key == deviceKey);
                           if (dev == null)
                           {
                               dev = new Device() { Key = deviceKey, Temperature = resultTemp };
                               loc.Devices.Add(dev);
                           }
                           else
                           {
                               dev.Temperature = resultTemp;
                           }
                       });
                }
            }

        }

        private void createGui(JToken config)
        {
            foreach (JProperty location in config.Children())
            {
                var loc = new Location() { Name = location.Name, Devices = new ObservableCollection<Device>() };

                foreach (var x in location.First().Last())
                {
                    var device = new Device()
                    {
                        Key = "temperature",
                        Name = (String)x["name"],
                        Id = (String)((JValue)(((JProperty)((x["id"].First).First)).Value)).Value,
                        Protocol = (String)((JValue)((x["protocol"]).First)).Value
                    };

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        loc.Devices.Add(device);
                    });
                }
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _locations.Add(loc);
                });
            }
        }

        private void Connect()
        {
            websocket = new WebSocket("ws://home.robertdeveen.com:5001/");

            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += websocket_MessageReceived;

            websocket.Open();
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            // websocket.Send("{\"message\": \"client receiver\"}");
            websocket.Send("{\"message\": \"request config\"}");
        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            ParseResult(e.Message);
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            // Nothing to do here.
        }

        private void websocket_Error(object sender, ErrorEventArgs e)
        {
            // Nothing to do here.
        }
    }
}