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

        public bool CheckCollision(Player player, List<GameObject> gameObjects, int mode)
        {
            foreach (GameObject gameObject in gameObjects)
            {

                if (gameObject.AAbox.corners != null && gameObject.Name != player.Name && mode == 0)
                {

                    if ((player.AAbox.max.X > gameObject.AAbox.min.X) || (player.AAbox.min.X > gameObject.AAbox.max.X)) return Testcenter2sDistance(player, gameObject);
                    if ((player.AAbox.max.Y < gameObject.AAbox.min.Y) || (player.AAbox.min.Y > gameObject.AAbox.max.Y)) return Testcenter2sDistance(player, gameObject);
             //       if ((player.AAbox.max.Z > gameObject.AAbox.min.Z) || (player.AAbox.min.Z < gameObject.AAbox.max.Z)) return Testcenter2sDistance(player, gameObject);

                }
                else if (gameObject.AAbox.corners != null && gameObject.Name != player.Name && mode == 1)
                {

                    if ((player.AAbox.max.X < gameObject.AAbox.min.X) || (player.AAbox.min.X < gameObject.AAbox.max.X)) return Testcenter2sDistance(player, gameObject);
                    if ((player.AAbox.max.Y < gameObject.AAbox.min.Y) || (player.AAbox.min.Y > gameObject.AAbox.max.Y)) return Testcenter2sDistance(player, gameObject);
            //        if ((player.AAbox.max.Z > gameObject.AAbox.min.Z) || (player.AAbox.min.Z < gameObject.AAbox.max.Z)) return Testcenter2sDistance(player, gameObject);

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
        }
        public bool TestAABBAABB(Player player, GameObject gameObjects)
        {
                float r;
                int couter = 0;
           /* foreach(GameObject gameObj in gameObjects)
            {*/
                if (gameObjects.ObjectPath != player.ObjectPath && gameObjects.ObjectPath != "3D/Ziemia/bigFloor")
                {
                    r = (int)(player.AAbox.radiuses[0] + gameObjects.AAbox.radiuses[0]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.X - gameObjects.AAbox.center.X, 2.0)) > r) couter++;
                    r = (int)(player.AAbox.radiuses[1] + gameObjects.AAbox.radiuses[1]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.Y - gameObjects.AAbox.center.Y, 2.0)) > r) couter++;
                    r = (int)(player.AAbox.radiuses[2] + gameObjects.AAbox.radiuses[2]); if ((int)Math.Sqrt(Math.Pow(player.AAbox.center2.Z - gameObjects.AAbox.center.Z, 2.0)) > r) couter++;
                   if(couter > 0)
                   {
                       return false;
                   }
                    return true;
                }  
           /* } 
                return true;*/
                return false;
        }

        // Test if OBB b intersects plane p
   /*     public bool TestOBBPlane(GameObject player, Plane p)
        {
            Vector3[] u = new Vector3[3];
            Vector3[] corners = player.obbox.GetCorners();
            u[0] = corners[7] - corners[6]; //x
            u[1] = corners[7] - corners[4]; //y
            u[2] = corners[7] - corners[3]; //z
            float[] e = new float[3];
            e[0] = u[0].Length();
            e[1] = u[1].Length();
            e[2] = u[2].Length();
            Vector3 c = (player.AAbox.max + player.AAbox.min) * 0.5f; // Compute AABB center
            Vector3 e = player.AAbox.max - c; // Compute positive extents
            // Compute the projection interval radius of b onto L(t) = b.c + t * p.n
            float r = e.X* Math.Abs(Vector3.Dot(p.Normal,u[0])) +
            e.Y * Math.Abs(Vector3.Dot(p.Normal, u[1])) +
            e.Z * Math.Abs(Vector3.Dot(p.Normal,u[2]));
            // Compute distance of box center from plane
            float s = Vector3.Dot(p.Normal, player.AAbox.center2) - p.D;
            // Intersection occurs when distance s falls within [-r,+r] interval
            return Math.Abs(s) <= r;
        }*/

        // Test if AABB b intersects plane p
        public bool TestAABBPlane(GameObject player, Plane p)
        {
            // These two lines not necessary with a (center, extents) AABB representation
            Vector3 c = (player.AAbox.max + player.AAbox.min) * 0.5f; // Compute AABB center
            Vector3 e = player.AAbox.max - c; // Compute positive extents
            // Compute the projection interval radius of b onto L(t) = b.c + t * p.n
            float r = e.X * Math.Abs(p.Normal.X) + e.Y * Math.Abs(p.Normal.Y) + e.Z * Math.Abs(p.Normal.Z);
            // Compute distance of box center from plane
            float s = Vector3.Dot(p.Normal, c) - p.D;
            // Intersection occurs when distance s falls within [-r,+r] interval
            return Math.Abs(s) <= r;
        }
    }
}
