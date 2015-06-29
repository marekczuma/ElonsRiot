using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Interaction
{
    class EndStuff : InteractiveGameObject
    {
        bool isTalking;

        public bool IsTalking
        {
            get { return isTalking; }
            set { isTalking = value; }
        }
        public EndStuff()
        {
            this.Information = "";
            isTalking = false;
        }
        public override void Interaction(Scene _scene)
        {
            _scene.PlayerObject.elonState.SetCurrentState(State.interact);
            _scene.PlayerObject.isHacking = true;
        }

        public override void AfterInteraction(Scene _scene)
        {
            Music.MusicManager.PlaySound(0);
            isTalking = false;
        }
    }
}
