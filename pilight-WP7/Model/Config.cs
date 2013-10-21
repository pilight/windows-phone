using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pilight.Model
{
    public class Id
    {
        public string id { get; set; }
    }

    public class Settings
    {
        public int battery { get; set; }
        public int temperature { get; set; }
        public int humidity { get; set; }
        public int decimals { get; set; }
    }

    public class Temperature
    {
        public string name { get; set; }
        public int order { get; set; }
        public int type { get; set; }
        public List<string> protocol { get; set; }
        public List<Id> id { get; set; }
        public int temperature { get; set; }
        public Settings settings { get; set; }
    }

    public class Woonkamer
    {
        public string name { get; set; }
        public int order { get; set; }
        public Temperature temperature { get; set; }
    }

    public class Keuken
    {
        public string name { get; set; }
        public int order { get; set; }
        public Temperature temperature { get; set; }
    }

    public class Config
    {
        public Woonkamer Woonkamer { get; set; }
        public Keuken Keuken { get; set; }
    }
}
