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

        public bool CheckCollision(Player player, List<GameObject>gameObjects, int lenght)
        {
            foreach(GameObject gameObject in gameObjects)
            {
                if (gameObject.AAbox.corners != null && gameObject.Name != player.Name )
                {
                    if ((player.AAbox.corners[7].X < gameObject.AAbox.corners[1].X) && (player.AAbox.corners[1].X > gameObject.AAbox.corners[7].X)) return true;
                    if ((player.AAbox.corners[7].Y < gameObject.AAbox.corners[1].Y) && (player.AAbox.corners[7].Y > gameObject.AAbox.corners[1].Y)) return true;
                    if ((player.AAbox.corners[7].Z < gameObject.AAbox.corners[1].Z) && (player.AAbox.corners[7].Z > gameObject.AAbox.corners[1].Z)) return true;
                }
               
            }
            return false;
        }
       
        public int SphereToPlaneCollision( Plane plane, BoundingSphere sphere)
        {
            float dot = Vector3.Dot(plane.Normal,sphere.Center) - plane.D;
            if(dot > sphere.Radius)
                return 1; // The sphere is in front of the plane
            else if(dot < -sphere.Radius)
                return 2; // The sphere is behind the plane
 
            return 3; // The sphere collides/straddles with the plane
        }
        public int AabbToPlaneCollision( Plane plane,Box aabb)
        {
            // Get the Extense vector
            Vector3 E = (aabb.max - aabb.min)/2.0f;
    
            // Get the center of the Box
            Vector3 center = aabb.min + E;
 
            Vector3 N = plane.Normal;
 
            // Dot Product between the plane normal and the center of the Axis Aligned Box
            // using absolute values
            float fRadius = Math.Abs(N.X*E.X) + Math.Abs(N.Y*E.Y) + Math.Abs(N.Z*E.Z);
 
            BoundingSphere sphere;
            sphere.Center = center;
            sphere.Radius = fRadius;
 
            return SphereToPlaneCollision( plane,sphere );
        }

    }
}
