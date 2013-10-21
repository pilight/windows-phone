using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pilight.Model
{
    public class Device: ViewModelBase
    {
        private double _temperature;

        public String Key
        {
            get;
            set;
        }
        public String Name
        {
            get;
            set;
        }
        public String Id
        {
            get;
            set;
        }
        public String Protocol
        {
            get;
            set;
        }
        public Boolean State
        {
            get;
            set;
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
                    RaisePropertyChanged("Temperature", oldValue, value, true));
            }

        }
        public int Dimlevel
        {
            get;
            set;
        }        
    }
}
