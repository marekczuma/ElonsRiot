using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Interaction
{
    public class Stuff : InteractiveGameObject
    {
        private List<GameObject> stuffs;
        public Stuff()
        {
            this.Information = "Kliknij E, aby podniesc przedmiot";
            stuffs = new List<GameObject>();
        }
        public override void Interaction(Scene _scene)
        {
          
          _scene.PlayerObject.elonState.SetCurrentState(State.interact);
          stuffs.Add(this);
          _scene.GameObjects.Remove(this);
          _scene.InteractiveObjects.InteractiveObjects.Remove(this);
       
        }

        public override void AfterInteraction(Scene _scene)
        {

        }
    }
}
