using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.ControlPlayer
{
    public class ObjectDetectionManager
    {
        public Scene MyScene { get; set; }
        public bool Information { get; set; }
        public Interaction.InteractiveGameObject currentInteractiveObject = null;

        public ObjectDetectionManager(Scene _myScene)
        {
            MyScene = _myScene;
            Information = false;
        }

        public void CheckRay()
        {
            Ray pickRay = GetPickRay();
            Information = false;
            for (int i = 0; i < MyScene.GameObjects.Count; i++)
            {
                if (MyScene.GameObjects[i].Interactive == true)
                {
                    //Nullable<float> result2 = pickRay2.Intersects(MyScene.GameObjects[i].boundingBox);
                    Nullable<float> result = MyScene.GameObjects[i].boundingBox.Intersects(pickRay);
                    if (result.HasValue == true)
                    {
                        Information = true;
                        currentInteractiveObject = (Interaction.InteractiveGameObject)MyScene.GameObjects[i];
                    }
                }
            }
        }

        Ray GetPickRay()
        {
            Vector3 nearPoint = MyScene.PlayerObject.camera.position;
            Vector3 direction = MyScene.PlayerObject.camera.desiredTarget - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);
            return pickRay;
        }
    }
}
