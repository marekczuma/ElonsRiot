using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Interaction
{
    class Sensore : InteractiveGameObject
    {
        public Sensore()
        {
            Information = "Kliknij E zeby zestrzelic czujki";
        }

        public override void Interaction(Scene _scene)
        {
            if(_scene.isSensore == false)
            {
                _scene.isSensore = true;
                _scene.PlayerObject.elonState.State = State.idleShoot;
            }
        }

        public override void AfterInteraction(Scene _scene)
        {
            this.Interactive = false;
        }
    }
}
