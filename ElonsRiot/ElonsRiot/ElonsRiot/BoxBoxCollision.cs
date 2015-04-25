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

        public bool CheckCollision(Player player, List<GameObject> gameObjects, bool isPositive)
        {
            foreach (GameObject gameObject in gameObjects)
            {

                if (gameObject.AAbox.corners != null && gameObject.Name != player.Name && isPositive == true)
                {

                    if ((player.AAbox.max.X > gameObject.AAbox.min.X) || (player.AAbox.min.X > gameObject.AAbox.max.X)) return Testcenter2sDistance(player, gameObject);
                    if ((player.AAbox.max.Y < gameObject.AAbox.min.Y) || (player.AAbox.min.Y > gameObject.AAbox.max.Y)) return Testcenter2sDistance(player, gameObject);
             //       if ((player.AAbox.max.Z > gameObject.AAbox.min.Z) || (player.AAbox.min.Z < gameObject.AAbox.max.Z)) return Testcenter2sDistance(player, gameObject);

                }
                else if (gameObject.AAbox.corners != null && gameObject.Name != player.Name && isPositive == false)
                {

                    if ((player.AAbox.max.X < gameObject.AAbox.min.X) || (player.AAbox.min.X < gameObject.AAbox.max.X)) return Testcenter2sDistance(player, gameObject);
                    if ((player.AAbox.max.Y < gameObject.AAbox.min.Y) || (player.AAbox.min.Y > gameObject.AAbox.max.Y)) return Testcenter2sDistance(player, gameObject);
            //        if ((player.AAbox.max.Z > gameObject.AAbox.min.Z) || (player.AAbox.min.Z < gameObject.AAbox.max.Z)) return Testcenter2sDistance(player, gameObject);

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

                if (distance1 < (distance0 + distance2)) return true;
            }
            return false;
        }


        public int SphereToPlaneCollision(Plane plane, BoundingSphere sphere)
        {
            float dot = Vector3.Dot(plane.Normal, sphere.Center) - plane.D;
            if (dot > sphere.Radius)
                return 1; // The sphere is in front of the plane
            else if (dot < -sphere.Radius)
                return 2; // The sphere is behind the plane

            return 3; // The sphere collides/straddles with the plane
        }
        public int AabbToPlaneCollision(Plane plane, Box aabb)
        {
            // Get the Extense vector
            Vector3 E = (aabb.max - aabb.min) / 2.0f;

            // Get the center2 of the Box
            Vector3 center = aabb.min + E;

            Vector3 N = plane.Normal;

            // Dot Product between the plane normal and the center2 of the Axis Aligned Box
            // using absolute values
            float fRadius = Math.Abs(N.X * E.X) + Math.Abs(N.Y * E.Y) + Math.Abs(N.Z * E.Z);

            BoundingSphere sphere;
            sphere.Center = center;
            sphere.Radius = fRadius;

            return SphereToPlaneCollision(plane, sphere);
        }

    }
}
