using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
//komentujes
namespace ElonsRiot.Dialogues
{

    [Serializable]
    public class DialogLine
    {
        [XmlElement("line")]
        public List<string> Line { get; set; }
        
    }
}
