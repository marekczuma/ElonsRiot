using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
//komentuje
namespace ElonsRiot.Dialogues
{
    [Serializable]
     public class Statement
    {
        [XmlElement("dialogLines")]
        public DialogLine dialogLines { get; set; } //linie wypowiedzi
        [XmlElement("dialogTimes")]
        public DialogTime dialogTimes { get; set; } //linie wypowiedzi
        [XmlElement("place")]
        public string placeToShow {get;set;} //miejsce wyswietlenia wypowiedzi

        public Statement() { }
        public Statement(DialogLine _dialogLines, string _placeToShow)
        {
            placeToShow = _placeToShow;
            dialogLines = _dialogLines;
        }
    }
}
