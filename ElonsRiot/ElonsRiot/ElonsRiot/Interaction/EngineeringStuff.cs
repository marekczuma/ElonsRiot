using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Interaction
{
    class EngineeringStuff : InteractiveGameObject
    {
        bool isTalking;

        public bool IsTalking
        {
            get { return isTalking; }
            set { isTalking = value; }
        }
        public EngineeringStuff()
        {
            this.Information = "Kliknij E, aby rozbroic system";
            isTalking = false;
        }
        public override void Interaction(Scene _scene)
        {
            _scene.PlayerObject.elonState.SetCurrentState(State.interact);

            _scene.PlayerObject.isHacking = true;
        }

        public override void AfterInteraction(Scene _scene)
        {
            isTalking = false;
        }
    }
}