using Microsoft.Xna.Framework;
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
/*
        public bool CheckCollision(Player player, List<GameObject> gameObjects, int mode)
        {
            foreach (GameObject gameObject in gameObjects)
            {

                if (gameObject.AAbox.corners != null && gameObject.Name != player.Name && mode == 0)
                {

                    if ((player.AAbox.max.X > gameObject.AAbox.min.X) || (player.AAbox.min.X > gameObject.AAbox.max.X)) return Testcenter2sDistance(player, gameObject);
                    if ((player.AAbox.max.Y < gameObject.AAbox.min.Y) || (player.AAbox.min.Y > gameObject.AAbox.max.Y)) return Testcenter2sDistance(player, gameObject);

                }
                else if (gameObject.AAbox.corners != null && gameObject.Name != player.Name && mode == 1)
                {

                    if ((player.AAbox.max.X < gameObject.AAbox.min.X) || (player.AAbox.min.X < gameObject.AAbox.max.X)) return Testcenter2sDistance(player, gameObject);
                    if ((player.AAbox.max.Y < gameObject.AAbox.min.Y) || (player.AAbox.min.Y > gameObject.AAbox.max.Y)) return Testcenter2sDistance(player, gameObject);

                }
                else if(gameObject.AAbox.corners != null && gameObject.Name != player.Name && mode ==2 )
                {
                  if ((player.AAbox.max.Z > gameObject.AAbox.min.Z) || (player.AAbox.min.Z > gameObject.AAbox.max.Z)) return Testcenter2sDistance(player, gameObject);
                }
                else if(gameObject.AAbox.corners != null && gameObject.Name != player.Name && mode ==3 )
                {
                    if ((player.AAbox.max.Z < gameObject.AAbox.min.Z) || (player.AAbox.min.Z < gameObject.AAbox.max.Z)) return Testcenter2sDistance(player, gameObject);
                }

            }
            return false;
        }
        private bool Testcenter2sDistance(Player player, GameObject obj)
        {
            double distance0, distance2;
            double distance1 = Math.Sqrt(Math.Pow((player.AAbox.center2.X - obj.AAbox.center2.X), 2.0) + Math.Pow((player.AAbox.center2.Y - obj.AAbox.center2.Y), 2.0)
                + Math.Pow((player.AAbox.center2.Z - obj.AAbox.center2.Z), 2.0));

            for (int i = 0; i < player.AAbox.actualRadiuses.Count; i++)
            {
                distance0 = Math.Sqrt(Math.Pow(System.Convert.ToDouble(player.AAbox.center2.X - player.AAbox.actualRadiuses[i].X), 2.0) +
                    Math.Pow(System.Convert.ToDouble(player.AAbox.center2.Y - player.AAbox.actualRadiuses[i].Y), 2.0) +
                    Math.Pow(System.Convert.ToDouble(player.AAbox.center2.Z - player.AAbox.actualRadiuses[i].Z), 2.0));
                distance2 = Math.Sqrt(Math.Pow(System.Convert.ToDouble(obj.AAbox.center2.X - obj.AAbox.actualRadiuses[i].X), 2.0) +
                    Math.Pow(System.Convert.ToDouble(obj.AAbox.center2.Y - obj.AAbox.actualRadiuses[i].Y), 2.0) +
                    Math.Pow(System.Convert.ToDouble(obj.AAbox.center2.Z - obj.AAbox.actualRadiuses[i].Z), 2.0));

                if (distance1 < (distance0 + distance2))
                { return true; }
            }
            return false;
        }*/
        public bool TestAABBAABB(Player player, GameObject gameObjects)
        {
                float r;
                int couter = 0;
                if (gameObjects.Name != player.Name && gameObjects.ObjectPath != "3D/Ziemia/bigFloor")
                {
                    r = (int)(player.AAbox.radiuses[0] + gameObjects.AAbox.radiuses[0]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.X - gameObjects.AAbox.center.X, 2.0)) > r) { couter++; gameObjects.collisionCommunicat = "x"; return false; };
                    r = (int)(player.AAbox.radiuses[1] + gameObjects.AAbox.radiuses[1]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.Y - gameObjects.AAbox.center.Y, 2.0)) > r) { couter++; gameObjects.collisionCommunicat = "y"; return false; };
                    r = (int)(player.AAbox.radiuses[2] + gameObjects.AAbox.radiuses[2]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.Z - gameObjects.AAbox.center.Z, 2.0)) > r) { couter++; gameObjects.collisionCommunicat = "z"; return false; };
                   if(couter > 0)
                   {
                       return false;
                   }
                    return true;
                }  
           
                return false;
        }
        public bool TestAABBAABB(GameObject player, GameObject gameObjects)
        {
            float r;
            int couter = 0;
            gameObjects.collisionCommunicat = "";
            if (gameObjects.Name != player.Name && gameObjects.ObjectPath != "3D/Ziemia/bigFloor")
            {
                r = (int)(player.AAbox.radiuses[0] + gameObjects.AAbox.radiuses[0]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.X - gameObjects.AAbox.center.X, 2.0)) > r) { couter++; gameObjects.collisionCommunicat += "x";};
                r = (int)(player.AAbox.radiuses[1] + gameObjects.AAbox.radiuses[1]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.Y - gameObjects.AAbox.center.Y, 2.0)) > r) { couter++; gameObjects.collisionCommunicat += "y";};
                r = (int)(player.AAbox.radiuses[2] + gameObjects.AAbox.radiuses[2]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.Z - gameObjects.AAbox.center.Z, 2.0)) > r) { couter++; gameObjects.collisionCommunicat += "z";};
                if (couter > 0)
                {
                    return false;
                }
                return true;
            }

            return false;
        }
       
        public bool TestAABBAABB(Player player, Box box, GameObject gameObjects)
        {
            float r;
            int couter = 0;
            if (gameObjects.Name != player.Name && gameObjects.ObjectPath != "3D/Ziemia/bigFloor")
            {
                r = (int)(player.AAbox.radiuses[0] + box.radiuses[0]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.X - box.center.X, 2.0)) > r) couter++;
                r = (int)(player.AAbox.radiuses[1] + box.radiuses[1]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.Y - box.center.Y, 2.0)) > r) couter++;
                r = (int)(player.AAbox.radiuses[2] + box.radiuses[2]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.Z - box.center.Z, 2.0)) > r) couter++;
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
            Vector3 e =  player.AAbox.max -c ; // Compute positive extents
            // Compute the projection interval radius of b onto L(t) = b.c + t * p.n
            float r = e.X * Math.Abs(p.Normal.X) + e.Y * Math.Abs(p.Normal.Y) + e.Z * Math.Abs(p.Normal.Z);
            // Compute distance of box center from plane
            r += 5;
            float s = Vector3.Dot(p.Normal, c) - p.D;
            // Intersection occurs when distance s falls within [-r,+r] interval
            return Math.Abs(s) <= r;
        }

        public bool TestAABBAABBTMP(Player player, GameObject gameObjects)
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

            float r;
            int couter = 0;
            if (gameObjects.Name != player.Name && gameObjects.ObjectPath != "3D/Ziemia/bigFloor")
            {
                r = (int)(gameObjects.AAbox.radiuses[0] + radiuses[0]); if ((int)Math.Sqrt(Math.Pow(center2.X - gameObjects.AAbox.center.X, 2.0)) > r) couter++;
                r = (int)(gameObjects.AAbox.radiuses[1] + radiuses[1]); if ((int)Math.Sqrt(Math.Pow(center2.Y - gameObjects.AAbox.center.Y, 2.0)) > r) couter++;
                r = (int)(gameObjects.AAbox.radiuses[2] + radiuses[2]); if ((int)Math.Sqrt(Math.Pow(center2.Z - gameObjects.AAbox.center.Z, 2.0)) > r) couter++;
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
