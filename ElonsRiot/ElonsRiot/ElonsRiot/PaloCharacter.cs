using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public enum FriendState { idle, walk, follow }
    public enum LearningState { idle, EngineeringLearning, ShootingLearning, UsingLearning }
    public enum WalkState { decoy, moveBox }
    public class PaloCharacter : GameObject
    {
        public float distance { get; set; }
        public FriendState PaloState { get; set; }
        public LearningState PaloLearningState { get; set; }
        public Player Elon { get; set; }
        public List<Guard> Guards { get; set; }
        public DecoyAI DecoyGuards { get; set; }
        public BoxMovementAI MoveBoxAI { get; set; }
        public WalkState Walk { get; set; }
        private float velocity;
        public float health;
        public PaloCharacter()
        {
            distance = 1.0f;
            PaloState = FriendState.idle;
            PaloLearningState = LearningState.idle;
            velocity = 0.12f;
            Guards = new List<Guard>();
            health = 100;
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
        
        //ALGORYTM PRZESUWANI SKRZYNKI
        //1.1 Wykrycie miejsca za skrzynką
        public Vector3 FindPlaceBehindObject(GameObject _object, Vector3 _targetPosition)
        {
            Vector3 goodDirection = (_targetPosition - _object.Position) * (-1);
            goodDirection.Normalize();
            goodDirection *= 6;
            Vector3 newPosition = _object.Position + goodDirection;
            return newPosition;
        }
        //1.2 Pójście do tego punktu
        public void StandBehindBox(Vector3 _behindBox)
        {
            GameObject tmpGO = new GameObject(_behindBox);
            WalkToTarget(tmpGO, velocity, 0.5f);
        }
        //2 - Obróć się w kierunku skrzynki
        public void RotateToBox(GameObject _targetBox)
        {
            Vector3 deltaVectorCopy = new Vector3(-_targetBox.Position.X, 0, -_targetBox.Position.Z);
            Matrix mat = Matrix.CreateLookAt(Position,
                                                Position + deltaVectorCopy,
                                                Vector3.Up);
            mat = Matrix.Transpose(mat);
            Quaternion q = Quaternion.Slerp(RotationQ,
                                            Quaternion.CreateFromRotationMatrix(mat),
                                            0.1f);
            RotationQ = q;
        }

        public void MoveBox()
        {
            if(MoveBoxAI == null)
            {
                return;
            }
            PaloState = FriendState.walk;
            if (!MoveBoxAI.AIncluded)
            {
                StandBehindBox(MoveBoxAI.PointA);
                GameObject tmpA = new GameObject(MoveBoxAI.PointA);
                if(getDistance(tmpA) <= 2)
                {
                    MoveBoxAI.AIncluded = true;
                }
            }else if(!MoveBoxAI.BIncluded)
            {
                //RotateToBox(MoveBoxAI.Cube);
                GameObject tmpB = new GameObject(MoveBoxAI.PointB);
                WalkToTarget(tmpB,velocity,2);
                if (getDistance(tmpB) <= 2)
                {
                    MoveBoxAI.BIncluded = true;
                    MoveBoxAI.Cube.mass = 500;
                }
            }else if(!MoveBoxAI.IsFinished)
            {
                GameObject tmpA = new GameObject(MoveBoxAI.PointA);
                WalkToTarget(tmpA, velocity, 2);
                if (getDistance(tmpA) <= 2)
                {
                   MoveBoxAI.IsFinished = true;
                   MoveBoxAI.Cube.mass = MoveBoxAI.CubeMass;
                }
            }else
            {
                PaloState = FriendState.idle;
            }
        }

        //Jak jest walk to jaką akcję ma wybrać
        public void ChooseAction(Scene _scene)
        {
            if(Walk == WalkState.decoy)
            {
                Decoy(_scene);
            }else if(Walk == WalkState.moveBox)
            {
                MoveBox();
            }
        }
    }
}
