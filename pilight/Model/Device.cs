using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pilight.Model
{
    public class Device : ViewModelBase
    {
        public String Name { get; set; }
        public DeviceType Type { get; set; }

        private double _temperature;
        private double _humidity;
        private bool _state;

        public String Key
        {
            get;
            set;
        }

        public Dictionary<String, String> Id
        {
            get;
            set;
        }
        public List<String> Protocol
        {
            get;
            set;
        }
        public bool State
        {
            get
            {
                return _state;
            }

            set
            {
                if (_state == value)
                {
                    return;
                }

                var oldValue = _state;
                _state = value;

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    RaisePropertyChanged("State", oldValue, value, true)
                );
            }
        }

        public double Temperature
        {
            get
            {
                // TODO: Make this better!
                if (this.settings.decimals == 1)
                    return _temperature / 10;
                else if (this.settings.decimals == 2)
                    return _temperature / 100;
                else if (this.settings.decimals == 3)
                    return _temperature / 1000;
                else
                    return _temperature;
            }

            set
            {
                if (_temperature == value)
                {
                    return;
                }

                var oldValue = _temperature;
                _temperature = value;

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    RaisePropertyChanged("Temperature", oldValue, value, true)
                );
            }

        }
        public double Humidity
        {
           get{
               // TODO: Make this better!
                if (this.settings.decimals == 1)
                    return _humidity / 10;
                else if (this.settings.decimals == 2)
                    return _humidity / 100;
                else if (this.settings.decimals == 3)
                    return _humidity / 1000;
                else
                    return _humidity;
            }

            set
            {
                if (_humidity == value)
                {
                    return;
                }

                var oldValue = _humidity;
                _humidity = value;

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    RaisePropertyChanged("Humidity", oldValue, value, true)
                );
            }
        }
        public int Dimlevel { get; set; }

        public Settings settings { get; set; }
    }

    public enum DeviceType
    {  
        RAW = 0,
        SWITCH = 1,
        DIMMER = 2,
        WEATHER = 3,
        RELAY = 4
    }
}
