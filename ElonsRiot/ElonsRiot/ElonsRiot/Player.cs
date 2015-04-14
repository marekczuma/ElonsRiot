using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public class Player : GameObject
    {
        public CharacterState elonState {get; set;}    //STAN ELONA - TO JEST KLASA KUŹWA!
        public Player()
        {
            elonState = new CharacterState(State.idle);
        }

        public void SetState(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.LeftShift))
            {
                elonState.SetCurrentState(State.run);
            }
            else if ((state.IsKeyDown(Keys.W)) || (state.IsKeyDown(Keys.S)) || (state.IsKeyDown(Keys.A)) || (state.IsKeyDown(Keys.D)))
            {
                elonState.SetCurrentState(State.walk);
            }
            else
            {
                elonState.SetCurrentState(State.idle);
            }
        }
        public void Movement(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.W))
            {
                ChangePosition(new Vector3(0, 0, -elonState.VelocityForward));
            }
            else if (state.IsKeyDown(Keys.S))
            {
                ChangePosition(new Vector3(0, 0, elonState.VelocityBack));
            }
            if (state.IsKeyDown(Keys.D))
            {
                ChangePosition(new Vector3(elonState.VelocitySide, 0, 0));
            }
            else if (state.IsKeyDown(Keys.A))
            {
                ChangePosition(new Vector3(-elonState.VelocitySide, 0, 0));
            }
        }
    }
}
