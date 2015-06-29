using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Interaction
{
    class OpeningDoor : InteractiveGameObject
    {
        bool isTalking;

        public bool IsTalking
        {
            get { return isTalking; }
            set { isTalking = value; }
        }
        public OpeningDoor()
        {
            this.Information = "";
            isTalking = false;
        }
        public override void Interaction(Scene _scene)
        {
            _scene.PlayerObject.elonState.SetCurrentState(State.interact);

            _scene.PlayerObject.isOpening = true;
            Dialogues.DialoguesManager.IsOpening = true;
        }

        public override void AfterInteraction(Scene _scene)
        {
            isTalking = false;
        }
    }
}