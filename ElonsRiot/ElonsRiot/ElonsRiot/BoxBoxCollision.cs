using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    class BoxBoxCollision
    {
        public BoxBoxCollision() { }

        public bool CheckCollision(Box player, Box Obj, int lenght)
        {
            if ((player.corners[7].X < Obj.corners[1].X) && (player.corners[1].X > Obj.corners[7].X)) return true;
            if ((player.corners[7].Y < Obj.corners[1].Y) && (player.corners[7].Y > Obj.corners[1].Y)) return true;
            if ((player.corners[7].Z < Obj.corners[1].Z) && (player.corners[7].Z > Obj.corners[1].Z)) return true;
            return false;
        }
    }
}
