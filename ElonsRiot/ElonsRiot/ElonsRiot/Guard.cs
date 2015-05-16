using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public enum GuardState { idle, chase, attack };
    public class Guard : GameObject
    {
        public GameObject Target {get; set;}
        public GuardState State { get; set; }
        public float Velocity { get; set; }
        public Guard()
        {
            State = GuardState.idle;
            Velocity = 0.09f;
        }
        public void Chase()
        {
            State = GuardState.chase;
            WalkToTarget(Target, Velocity, 0.5f);
        }
    }
}
