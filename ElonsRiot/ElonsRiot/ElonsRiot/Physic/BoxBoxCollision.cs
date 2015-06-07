using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    class BoxBoxCollision
    {
        public BoxBoxCollision() { }

        public bool TestAABBAABB(GameObject player, GameObject gameObjects)
        {
            float r;
            int couter = 0;
            gameObjects.collisionCommunicat = "";
            if ( gameObjects.ObjectPath != "3D/Ziemia/bigFloor")
            {
                r = (int)(player.AAbox.radiuses[2] + gameObjects.AAbox.radiuses[2]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.Z - gameObjects.AAbox.center2.Z, 2.0)) > r) {couter++;};
                r = (int)(player.AAbox.radiuses[0] + gameObjects.AAbox.radiuses[0]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.X - gameObjects.AAbox.center2.X, 2.0)) > r) { couter++;};
                r = (int)(player.AAbox.radiuses[1] + gameObjects.AAbox.radiuses[1]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.Y - gameObjects.AAbox.center2.Y, 2.0)) > r) { couter++;};

                if (couter > 0)
                {
                    return false;
                }
                return true;
              
            }

            return false;
        }
       
      
        /// <summary>
        /// Potrzebne gdy bohaterowie przesuwają paczki. Zawsze po zatrzymaniu będą 2 punkty dalej od paczki wiec trzeba wziąć to pod
        /// uwagę w czasie wyliczania czy mogą się wspiąć na paczkę. 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gameObjects"></param>
        /// <returns></returns>
        public bool TestAABBAABBWithError(GameObject player, GameObject gameObjects)
        {
            float r;
            int couter = 0;
            gameObjects.collisionCommunicat = "";
            if (gameObjects.Name != player.Name && gameObjects.ObjectPath != "3D/Ziemia/bigFloor")
            {
                r = (int)(player.AAbox.radiuses[0] + gameObjects.AAbox.radiuses[0])+2; if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.X - gameObjects.AAbox.center2.X, 2.0)) > r) { couter++; gameObjects.collisionCommunicat += "x"; };
                r = (int)(player.AAbox.radiuses[1] + gameObjects.AAbox.radiuses[1])+ 2; if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.Y - gameObjects.AAbox.center2.Y, 2.0)) > r) { couter++; gameObjects.collisionCommunicat += "y"; };
                r = (int)(player.AAbox.radiuses[2] + gameObjects.AAbox.radiuses[2])+ 2; if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.Z - gameObjects.AAbox.center2.Z, 2.0)) > r) { couter++; gameObjects.collisionCommunicat += "z"; };
                if (couter > 0)
                {
                    return false;
                }
                return true;
            }

            return false;
        }
     
        // Test if AABB b intersects plane p
        public bool TestAABBPlane(GameObject player, Plane p)
        {
            
            // These two lines not necessary with a (center, extents) AABB representation
            Vector3 c = player.AAbox.center2; // Compute AABB center
            Vector3 e = player.boundingBox.Max - c; // Compute positive extents

            // Compute the projection interval radius of b onto L(t) = b.c + t * p.n
            float r = e.X * Math.Abs(p.Normal.X) + e.Y * Math.Abs(p.Normal.Y) + e.Z * Math.Abs(p.Normal.Z);
            // Compute distance of box center from plane
            float s = Vector3.Dot(p.Normal, c) - p.D;
            // Intersection occurs when distance s falls within [-r,+r] interval
            //return Math.Abs(s) <= r;
            return s <= r;
        }

        public bool TestAABBAABBTMP(GameObject player, GameObject gameObjects)
        {
            float[] radiuses = new float[3];
            Vector3 center2;
            Vector3[] corners = player.boxes[0].GetCorners();
            radiuses[0] = (float)Math.Sqrt(Math.Pow(Convert.ToDouble(corners[7].X - corners[6].X), 2) + Math.Pow(Convert.ToDouble(corners[7].Y - corners[6].Y), 2)
            + Math.Pow(Convert.ToDouble(corners[7].Z - corners[6].Z), 2));//x
            radiuses[1] = (float)Math.Sqrt(Math.Pow(Convert.ToDouble(corners[7].X - corners[4].X), 2) + Math.Pow(Convert.ToDouble(corners[7].Y - corners[4].Y), 2)
             + Math.Pow(Convert.ToDouble(corners[7].Z - corners[4].Z), 2));//y
            radiuses[2] = (float)Math.Sqrt(Math.Pow(Convert.ToDouble(corners[7].X - corners[3].X), 2) + Math.Pow(Convert.ToDouble(corners[7].Y - corners[3].Y), 2)
             + Math.Pow(Convert.ToDouble(corners[7].Z - corners[3].Z), 2));//z
            radiuses[0] = radiuses[0] / 2;
            radiuses[1] = radiuses[1] / 2;
            radiuses[2] = radiuses[2] / 2;
            center2.X = (corners[1].X + corners[7].X) / 2;
            center2.Y = (corners[1].Y + corners[7].Y) / 2;
            center2.Z = (corners[1].Z + corners[7].Z) / 2;
            gameObjects.collisionCommunicat = "";
            float r;
            int couter = 0;
            if (gameObjects.Name != player.Name && gameObjects.ObjectPath != "3D/Ziemia/bigFloor")
            {
                r = (int)(gameObjects.AAbox.radiuses[0] + radiuses[0]); if ((int)Math.Sqrt(Math.Pow(center2.X - gameObjects.AAbox.center2.X, 2.0)) > r) { couter++; gameObjects.collisionCommunicat += "x"; }
                r = (int)(gameObjects.AAbox.radiuses[1] + radiuses[1]); if ((int)Math.Sqrt(Math.Pow(center2.Y - gameObjects.AAbox.center2.Y, 2.0)) > r) {couter++; gameObjects.collisionCommunicat += "y";}
                r = (int)(gameObjects.AAbox.radiuses[2] + radiuses[2]); if ((int)Math.Sqrt(Math.Pow(center2.Z - gameObjects.AAbox.center2.Z, 2.0)) > r) {couter++; gameObjects.collisionCommunicat += "z";}
                if (couter > 0)
                {
                    return false;
                }
                return true;
            }

            return false;
        }
       
    }
}
