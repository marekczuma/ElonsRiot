using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    enum State { idle, walk, run, jump };
    class CharacterState
    {
        public float VelocityForward { get; set; }      //Do przodu
        public float VelocityBack { get; set; }         //Do tyłu
        public float VelocitySide { get; set; }         //W boki
        public State State { get; set; }

        public CharacterState(State _state)
        {
            SetCurrentState(_state);
        }

        public void SetCurrentState(State _state)
        {
            State = _state;
            if (_state == ElonsRiot.State.idle)
            {
                VelocityForward = 0;
                VelocitySide = 0;
                VelocityBack = 0;
            }
            else if (_state == ElonsRiot.State.walk)
            {
                VelocityForward = 0.2f;
                VelocitySide = 0.15f;
                VelocityBack = 0.15f;
            }
            else if (_state == ElonsRiot.State.run)
            {
                VelocityForward = 0.4f;
                VelocitySide = 0.25f;
                VelocityBack = 0.25f;
            }
        }
    }
}
