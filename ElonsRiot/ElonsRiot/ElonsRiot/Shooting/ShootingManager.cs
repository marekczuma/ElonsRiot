using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;

namespace ElonsRiot.Shooting
{
    public class ShootingManager
    {
        public Scene Scene { get; set; }
        public List<Bullet> Bullets { get; set; }
        public float timer = 0;
        public ShootingManager()
        {
            Bullets = new List<Bullet>();
        }

        public void Shot(GameObject Shooter, Quaternion StartQuaternion)
        {
            
            if (timer <= 0)
            {
                Vector3 BulletPosition = Shooter.Position;
                BulletPosition.Y += 6;
                Bullet tmpBullet = new Bullet { Scene = Scene, Position = BulletPosition, Rotation = new Vector3(0, 0, 0), Scale = new Vector3(1, 1, 1), Name = "Kuleczka", MatrixWorld = Scene.PlayerObject.MatrixWorld, RotationQ = StartQuaternion, Shooter = Shooter};
                Matrix orbit = Matrix.CreateFromAxisAngle(new Vector3(0, 1, 0), 90);
                Bullets.Add(tmpBullet);
                Scene.GameObjects.Add(tmpBullet);
                tmpBullet.LoadModels(Scene.ContentManager);
                tmpBullet.Shoot();
                timer = 500; //Strzelać można co pół sekundy
            }
        }

        public void BulletsMovement()
        {
            
            float timeInMS = Scene.time.ElapsedGameTime.Milliseconds;
            timer -= timeInMS;
            foreach(var element in Bullets)
            {
                element.Shoot();
                if(element.IsCollision())
                {
                    Interaction(element.targetGO);
                    Scene.GameObjects.Remove(element);
                }
            }
        }

        public void Interaction(GameObject target)
        {
            if(target.Tag == "guard")
            {
                for (int i = 0; i < Scene.GameObjects.Count; i++)
                {
                    if (Scene.GameObjects[i] == target)
                    {
                        Guard tmpGuard = (Guard)target;
                        tmpGuard.Die();
                    }
                }
            }else if(target.Tag == "Tin")
            {
                for (int i = 0; i < Scene.GameObjects.Count; i++)
                {
                    if (Scene.GameObjects[i] == target)
                    {
                        Learning.Tin tmpTin = (Learning.Tin)target;
                        tmpTin.Destroy();
                    }
                }
            }

        }
        
    }
}
