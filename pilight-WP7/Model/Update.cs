using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pilight.Model
{
    /*
     * 
    {
	"origin": "config",
	"type": 3,
	"devices": {
		"Woonkamer": ["temperature"],
		"Keuken": ["temperature"]
	},
	"values": {
		"temperature": "18812"
	}
}
     * */

        public class Devices
        {
            public List<string> Woonkamer { get; set; }
            public List<string> Keuken { get; set; }
        }

        public class Values
        {
            public string temperature { get; set; }
        }

        public class Update
        {
            public string origin { get; set; }
            public int type { get; set; }
            public Devices devices { get; set; }
            public Values values { get; set; }
        }

}
