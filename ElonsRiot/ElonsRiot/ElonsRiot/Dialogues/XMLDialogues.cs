using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ElonsRiot.Dialogues
{
    [Serializable]
    [XmlRoot("Dialogues")]
    public class XMLDialogues
    {
        [XmlElement("statement")]
        public List<Statement> statements { get; set; }
        public XMLDialogues()
        {
            statements = new List<Statement>();
        }
    }
}
