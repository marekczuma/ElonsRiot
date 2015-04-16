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
        public Camera camera;

        private bool isMouseMovement;
        private float angle;
        public Player()
        {
            elonState = new CharacterState(State.idle);
            camera = new Camera();
            isMouseMovement = false;
            angle = 0;
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
        public void Movement(KeyboardState state, MouseState _oldMouseState)
        {
            if (state.IsKeyDown(Keys.W))
            {
                ChangePosition(new Vector3(elonState.VelocityForward, 0,0 ));
            }
            else if (state.IsKeyDown(Keys.S))
            {
                ChangePosition(new Vector3(-elonState.VelocityBack, 0,0 ));
            }
            if (state.IsKeyDown(Keys.D))
            {
                ChangePosition(new Vector3(0, 0, elonState.VelocitySide));
            }
            else if (state.IsKeyDown(Keys.A))
            {
                ChangePosition(new Vector3(0, 0, -elonState.VelocitySide));
            }
            //obracanie playera
            if (!(elonState.State == State.idle))
            {
                MouseState newState;
                newState = Mouse.GetState();
                int newMouseX = newState.X;
                int oldMouseX = _oldMouseState.X;
                float delta = 0;
                if ((oldMouseX - newMouseX) != 0)
                {
                    isMouseMovement = true;
                }
                if (isMouseMovement)
                {
                    delta = oldMouseX - newMouseX;
                }

                if (delta < 0)
                {
                    angle -= 6.1f;
                    //angle += delta / 10;
                }
                else if (delta > 0)
                {
                    angle += 6.1f;
                    //angle -= delta / 10;
                }
                ChangeRotation(new Vector3(0, angle, 0));
                _oldMouseState = newState;
                isMouseMovement = false;
                angle = 0;
            }
        }
        public void CameraUpdate(GameTime gameTime)
        {
            camera.Update(this.MatrixWorld, elonState, this.Rotation);
            camera.UpdateCamera(gameTime);

        }
    }
}
