using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ElonsRiot
{
     [Serializable]
    public class DialogTime
    {
        [XmlElement("time")]
        public List<float> Times { get; set; }
    }
}
