using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.ControlPlayer
{
    class ObjectDetectionManager
    {
        public Scene MyScene { get; set; }

        public void CheckRay(KeyboardState _state)
        {



            //Ray pickRay2 = GetPickRay();
            //isStatement = false;
            //for (int i = 0; i < MyScene.GameObjects.Count; i++)
            //{
            //    if (MyScene.GameObjects[i].Interactive == true)
            //    {
            //        //Nullable<float> result2 = pickRay2.Intersects(MyScene.GameObjects[i].boundingBox);
            //        Nullable<float> result2 = MyScene.GameObjects[i].boundingBox.Intersects(pickRay2);
            //        if (result2.HasValue == true)
            //        {
            //            isStatement = true;
            //            currentInteractiveObject = MyScene.GameObjects[i];
            //        }
            //    }
            //}
            //if (_state.IsKeyDown(Keys.E))
            //{
            //    Ray pickRay = GetPickRay();
            //    float selectedDistance = 100;// float.MaxValue;
            //    for (int i = 0; i < MyScene.GameObjects.Count; i++)
            //    {
            //        if (MyScene.GameObjects[i].Interactive == true)
            //        {
            //            //Nullable<float> result = pickRay.Intersects(MyScene.GameObjects[i].boundingBox);
            //            Nullable<float> result = MyScene.GameObjects[i].boundingBox.Intersects(pickRay);
            //            if (result.HasValue == true)
            //            {
            //                if (result.Value < selectedDistance)
            //                {

            //                    // Interactions interactionsClass = new Interactions(MyScene.GameObjects[i].interactionType, MyScene.GameObjects[i]);
            //                    MyScene.GameObjects[i].ChangePosition(new Vector3(0.05f, 0f, 0.0f));
            //                    //Interactions.Add(MyScene.GameObjects[i].interactionType);
            //                    //Interactions.CallInteraction(MyScene.GameObjects[i]);
            //                }
            //            }
            //        }
            //    }
            //}
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
