using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public enum FriendState { idle, walk, follow }
    public enum LearningState { idle, EngineeringLearning, ShootingLearning, UsingLearning }
    public class PaloCharacter : GameObject
    {
        public float distance { get; set; }
        public FriendState PaloState { get; set; }
        public LearningState PaloLearningState { get; set; }
        public Player Elon { get; set; }

        private float velocity;
        public PaloCharacter()
        {
            distance = 1.0f;
            PaloState = FriendState.idle;
            PaloLearningState = LearningState.idle;
            velocity = 0.05f;
        }

        public void WalkForward()
        {
            ChangePosition(new Microsoft.Xna.Framework.Vector3(velocity, 0, 0 ));
        }

        public void WalkToPlayer()
        {
            Vector3 toPlayer = Vector3.Normalize((Elon.Position - Position));
            Vector3 currentDirection = Vector3.Normalize(MatrixWorld.Forward);
            float angle = (float)Math.Atan2(Convert.ToDouble(toPlayer.X - currentDirection.X), Convert.ToDouble(toPlayer.Z - currentDirection.Z));
            //if (angle > 45)
            //{
            //    SetRotation(new Vector3( 0, 10, 0));
            //}
            ChangeRotation(new Vector3(0, angle, 0));

            if (getDistance(Elon) > 15)
            {
                WalkForward();
            }
        }

    }
}
