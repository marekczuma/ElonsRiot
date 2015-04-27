using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public enum FriendState { idle, walk, follow }
    public enum LearningState { idle, EngineeringLearning, ShootingLearning, UsingLearning }
    class PaloCharacter : GameObject
    {
        public float distance { get; set; }
        public FriendState PaloState { get; set; }
        public LearningState PaloLearningState { get; set; }

        private float velocity;
        public PaloCharacter()
        {
            distance = 1.0f;
            PaloState = FriendState.idle;
            PaloLearningState = LearningState.idle;
            velocity = 0.2f;
        }

        public void WalkForward()
        {
            ChangePosition(new Microsoft.Xna.Framework.Vector3(0, 0, velocity));
        }

    }
}
