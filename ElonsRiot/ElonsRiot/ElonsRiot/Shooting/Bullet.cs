using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Shooting
{
    public class Bullet : GameObject
    {
        public Scene Scene { get; set; }
        public GameObject targetGO { get; set; }
        public GameObject Shooter { get; set; }
        private float velocity = 1.0f;

        
        public Bullet()
        {
            ObjectPath = "3D/Placeholders/Bullet";
            targetGO = null;
        }
        public void Shoot()
        {
            ChangePosition(new Vector3(0, 0, -velocity));
        }
        public bool IsCollision()
        {
            foreach(var element in Scene.GameObjects)
            {
                if (element != Shooter)
                {
                    if (CalculateDistance(element.Position) <= 1)
                    {
                        targetGO = element;
                        return true;
                    }
                }
            }
            return false;
        }
        public float CalculateDistance(Vector3 _target)
        {
            Vector3 posTarget = _target;
            posTarget.Y = 0;
            Vector3 posMy = this.Position;
            posMy.Y = 0;
            return Vector3.Distance(posMy, posTarget);
        }
    }
}
