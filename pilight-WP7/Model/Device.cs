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
        //public int Order { get; set; }
        //public Dictionary<String, List<String>> settings = new Dictionary<String, List<String>>();

        private double _temperature;
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
        public int Dimlevel { get; set; }
    }
}
