using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public class DecoyAI
    {
        public GameObject PointB { get; set; }      // Punkt w którym ma się postać zatrzymać
        public GameObject PointC { get; set; }      // Punkt do którego ma spitolić
        public bool BIncluded { get; set; }         // Czy byłeś już w B?
        public bool CIncluded { get; set; }         // Czy byłeś już w C?

        public DecoyAI(GameObject _b, GameObject _c)
        {
            PointB = _b;
            PointC = _c;
            BIncluded = false;
            CIncluded = false;
        }


    }
}
