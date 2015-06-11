using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Interaction
{
    public class Door : InteractiveGameObject
    {
        public Door()
        {
            Information = "Kliknij E zeby otworzyc drzwi";
        }
        public override void Interaction(Scene _scene)
        {
            ChangePosition(new Vector3(0.05f, 0f, 0.0f));
        }
    }
}
