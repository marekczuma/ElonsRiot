using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Interaction
{
    class Wall : InteractiveGameObject
    {
         public Wall()
        {
            Information = "Kliknij E zeby otworzyc drzwi";
        }
        public override void Interaction(Scene _scene)
        {
            ChangePosition(new Vector3(0, 0f, -0.05f));
        }
    }
}
