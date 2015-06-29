using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Learning
{
    public class ShootingLearning
    {
        public Scene Scene { get; set; }
        public List<Tin> Tins {get; set;}

        public Vector3 PosA { get; set; } //Pozycja strzelecka Elona
        public Vector3 PosB { get; set; } //Pozycja strzelecka Pala

        public bool IsStarted { get; set; }
        public bool ElonShoot { get; set; }
        public bool ElonAfterFirst { get; set; }



        public ShootingLearning(Scene _scene)
        {
            PosA = new Vector3(55, 0, 43);
            PosB = new Vector3(65, 0, 43);
            Scene = _scene;
            Tins = new List<Tin>();
            IsStarted = false;
            ElonAfterFirst = true;
            ElonShoot = true;
        }

        public void FillTins()
        {
            Tin HighTin1 = new Tin { Scene = Scene, Position = new Vector3(55, 0, 80), Scale = new Vector3(0.3f,0.8f,0.3f), ObjectPath = "3D/Placeholders/Tin", Name = "asset Wysoka puszka1", id = "ABCDEF", IsPlayer = true, Tag = "Tin" };
            Tins.Add(HighTin1);
            Tin HighTin2 = new Tin { Scene = Scene, Position = new Vector3(65, 0, 80), Scale = new Vector3(0.3f, 0.8f, 0.3f), ObjectPath = "3D/Placeholders/Tin", Name = "asset Wysoka puszka2", id = "ABCDEF", IsPlayer = false, Tag = "Tin" };
            Tins.Add(HighTin2);
        }

        public void AddObjectsToScene()
        {
            FillTins();
            foreach(var element in Tins)
            {
                Scene.GameObjects.Add(element);
            }
        }
        public void MoveElonAndPalo()
        {
            Scene.PlayerObject.Position = PosA;
            Scene.PaloObject.Position = PosB;
            Scene.PaloObject.LookAt(Scene.PlayerObject.Position);
        }
}
    }
