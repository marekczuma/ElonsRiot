using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public class BoxMovementAI
    {
        public GameObject Cube { get; set; }
        public Vector3 PointA { get; set; }      // Punkt za beczką
        public Vector3 PointB { get; set; }      // Punkt do którego ma przesunąć skrzynkę
        public bool AIncluded { get; set; }         // Czy byłeś już w A?
        public bool BIncluded { get; set; }         // Czy byłeś już w B?
        public bool IsFinished { get; set; }
        public float CubeMass { get; set; }
        public BoxMovementAI(Vector3 _b)
        {
            PointB = _b;
            AIncluded = false;
            BIncluded = false;
            IsFinished = false;
        }
    }
}
