using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    class Physic
    {
        List<GameObject> gameObject;
        public Physic(List<GameObject> gobj)
        {
            this.gameObject = gobj;
        }
        public bool DetectCollision(Player player)
        {
            foreach (GameObject gObj in gameObject)
            {
                if (gObj.ObjectPath != "3D/ludzik/elon")
                {
                    if (gObj.ObjectPath != "3D/Ziemia/bigFloor")
                    {
                        if (player.boundingBox.Intersects(gObj.boundingBox))
                        {
                            Debug.WriteLine("collision");
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
