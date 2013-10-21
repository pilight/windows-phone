using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace pilight.Model
{
    public class Location
    {
        public String Name
        {
            get;
            set;
        }

        public ObservableCollection<Device> Devices
        {
            get;
            set;
        }
    }

}
