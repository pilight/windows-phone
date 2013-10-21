using System;
using Microsoft.Phone.Controls;
using WebSocket4Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperSocket.ClientEngine;

namespace pilight
{
    public partial class MainPage : PhoneApplicationPage
    {
        private WebSocket websocket;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Connect();
        }

        private void ParseResult(string p)
        {
            var result = JsonConvert.DeserializeObject(p);
            JObject json = JObject.Parse(p);

            var resultTemp = (float)json["values"]["temperature"]/1000;

            Dispatcher.BeginInvoke(() =>
                temperature.Text = String.Format("{0} °C", resultTemp)
            );
        }

        private void Connect()
        {
            websocket = new WebSocket("ws://192.168.178.29:5001/");

            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += websocket_MessageReceived;

            websocket.Open();
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            websocket.Send("{\"message\": \"client receiver\"}");
        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            ParseResult(e.Message);
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            // Nothing to do here.
            Connect();
        }

        private void websocket_Error(object sender, ErrorEventArgs e)
        {
            // Nothing to do here.
        }
    }
}