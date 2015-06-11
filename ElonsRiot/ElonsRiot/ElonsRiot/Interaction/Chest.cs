using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Interaction
{
    class Chest : InteractiveGameObject
    {
        public Chest()
        {
            mass = 100;
            Information = "Kliknij E zeby przesunac skrzynie";
        }
        public override void Interaction(Scene _scene)
        {
            if (_scene.PaloObject.PaloLearningState == LearningState.Learning)
            {
                float timeInMS = _scene.time.ElapsedGameTime.Milliseconds;
                timeInMS /= 1000;
                _scene.PaloObject.LearningManager.Timer += timeInMS;
            }
            mass = 30;
            _scene.PlayerObject.elonState.State = State.push;
            _scene.PlayerObject.ChangePosition(new Vector3(0, 0, -0.1f));
        }

        public override void AfterInteraction(Scene _scene)
        {
            if(_scene.PaloObject.PaloLearningState == LearningState.Learning)
            {
                Random rnd = new Random();
                _scene.PaloObject.Skills.Using += _scene.PaloObject.LearningManager.Timer * rnd.Next(5,10);
                _scene.PaloObject.LearningManager.Timer = 0;
            }
            mass = 100;
        }
    }
}
