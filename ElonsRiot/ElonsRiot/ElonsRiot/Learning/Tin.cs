using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Learning
{
    
    public class Tin : GameObject
    {
        public Scene Scene { get; set; }
        public float Weight { get; set; }
        public bool IsPlayer { get; set; }
        public bool explode;
        
        public Tin()
        {
            explode = false;
        }

        public void Destroy()
        {
            foreach(var element in Scene.GameObjects)
            {
                if(element.Name == Name)
                {
                    Scene.currentTinPos = this.Position;
                    Scene.tinExplosionTime = 0.1f;
                    Scene.GameObjects.Remove(element);
                    Scene.PlayerObject.showTinExplosion = true;
                    break;
                }
            }
            Scene.PaloObject.Skills.Shooting += 5;
            Console.WriteLine(Scene.PaloObject.Skills.Shooting);
        }

        
    }
}
