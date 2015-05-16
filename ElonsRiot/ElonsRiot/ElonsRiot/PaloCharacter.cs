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
        public List<Guard> Guards { get; set; }
        public DecoyAI DecoyGuards { get; set; }

        private float velocity;
        public PaloCharacter()
        {
            distance = 1.0f;
            PaloState = FriendState.idle;
            PaloLearningState = LearningState.idle;
            velocity = 0.12f;
            Guards = new List<Guard>();
        }

        public void WalkForward()
        {
            ChangePosition(new Microsoft.Xna.Framework.Vector3(0, 0, velocity));
        }
        private bool IsNear(Vector3 v1, Vector3 v2)
        {
            if((Math.Abs(v1.Z - v2.Z) <= 0.01) && (Math.Abs(v1.X - v2.X) <= 0.01))
            {
                return true;
            }
            return false;
        }
        public void WalkToPlayer()
        {
            WalkToTarget(Elon, velocity, 20);
        }

        public List<Guard> FindGuards(float _range, Scene _scene)
        {
            List<GameObject> tmpGuards = new List<GameObject>();
            List<Guard> guards = new List<Guard>();
            foreach(var elem in _scene.GameObjects)
            {
                if(elem.Tag == "guard")
                {
                    tmpGuards.Add(elem);
                }
            }
            foreach(var elem in tmpGuards)
            {
                if(getDistance(elem) <= _range)
                {
                    guards.Add((Guard)elem);
                }
            }
            return guards;
        }

        public void Call(List<Guard> _guards)
        {
            foreach(var elem in _guards)
            {
                elem.Target = this;
                elem.Chase();
            }
        }

        public void Decoy(Scene _scene)
        {
            if(DecoyGuards == null) //Jak nie mamy podczepionej AI to nic nie robimy
            {
                return;
            }
            PaloState = FriendState.walk;
            if(!DecoyGuards.BIncluded)
            {
                WalkToTarget(DecoyGuards.PointB, velocity, 1);
                if(getDistance(DecoyGuards.PointB) <= 5)
                {
                    Guards = FindGuards(30, _scene);
                    Call(Guards);
                    DecoyGuards.BIncluded = true;
                }
            }else if(!DecoyGuards.CIncluded)
            {
                WalkToTarget(DecoyGuards.PointC, velocity, 1);
                if(getDistance(DecoyGuards.PointC) <= 5)
                {
                    DecoyGuards.CIncluded = true;
                    PaloState = FriendState.idle;
                    DecoyGuards = null;
                }
            }        
        }
        

    }
}
