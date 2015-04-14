using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ElonsRiot
{
    [Serializable]
    [XmlRoot("Scene")]
    public class XMLScene
    {
        [XmlElement("GameObject")]
        public List<GameObject> GameObjects { get; set; }
        //public List<XMLGameObject> GameObjects { get; set; }
        public XMLScene()
        {
            GameObjects = new List<GameObject>();
            //GameObjects = new List<XMLGameObject>();
        }
    }
}
