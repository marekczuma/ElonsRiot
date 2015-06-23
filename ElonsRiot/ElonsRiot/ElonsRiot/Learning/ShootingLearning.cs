using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Learning
{
    class ShootingLearning
    {
        public Scene Scene { get; set; }
        public List<Tin> Tins {get; set;}
        public ShootingLearning(Scene _scene)
        {
            Scene = _scene;
            Tins = new List<Tin>();
        }

        public void FillTins()
        {

        }

        public void AddObjectsToScene()
        {
            foreach(var element in Tins)
            {
                Scene.GameObjects.Add(element);
            }
        }
    }
}
