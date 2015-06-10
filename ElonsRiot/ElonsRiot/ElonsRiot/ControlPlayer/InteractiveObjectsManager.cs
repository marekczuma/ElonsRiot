using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.ControlPlayer
{
    public class InteractiveObjectsManager
    {
        public Scene Scene { get; set; }
        public InteractiveObjectsManager(Scene _scene)
        {
            Scene = _scene;
        }

        public void ManageInteractiveObject(KeyboardState _state)
        {
            if(Scene.ObjectDetector.currentInteractiveObject != null)
            {
                if(_state.IsKeyDown(Keys.E))
                {
                    Scene.ObjectDetector.currentInteractiveObject.Interaction();
                }
            }
        }
    }
}
