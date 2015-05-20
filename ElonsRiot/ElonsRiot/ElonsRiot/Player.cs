﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public class Player : GameObject
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public CharacterState elonState {get; set;}    //STAN ELONA - TO JEST KLASA KUŹWA!
        public Camera camera;
        public float health;
        private bool isMouseMovement;
        private float angle;
        public Vector3 oldPosition, newPosition;
        public PaloCharacter Palo { get; set; }
        public List<BoundingBox> boxes;
        public Player()
        {
            elonState = new CharacterState(State.idle);
            camera = new Camera();
            isMouseMovement = false;
            angle = 0;
            health = 100.0f;
            boxes = new List<BoundingBox>();
            gravity = -0.12f;
        }

        public Player(Vector3 _position, Vector3 _rotation)
        {
            Position = _position;
            Rotation = _rotation;
            RotationQ = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(Rotation.Y));
            elonState = new CharacterState(State.idle);
            camera = new Camera();
            isMouseMovement = false;
            angle = 0;
            health = 100.0f;
            boxes = new List<BoundingBox>();
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
            else if(state.IsKeyDown(Keys.M))
            {
                PhysicManager.ClimbBox(this);
            }
            else
            {
                elonState.SetCurrentState(State.idle);
            }
        }
        public void Movement(KeyboardState state, MouseState _oldMouseState)
        {
            
            oldPosition = Position;
            if (state.IsKeyDown(Keys.W))
            {
                ChangePosition(new Vector3(0, 0, elonState.VelocityForward));
            }
            else if (state.IsKeyDown(Keys.S))
            {
                ChangePosition(new Vector3(0, 0,-elonState.VelocityBack ));
            }
            if (state.IsKeyDown(Keys.D))
            {
                ChangePosition(new Vector3(-elonState.VelocitySide, 0,0 ));
            }
            else if (state.IsKeyDown(Keys.A))
            {
                ChangePosition(new Vector3(elonState.VelocitySide, 0, 0 ));
            }
            
            //obracanie playera
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
                angle -= 0.08f;
                //angle += delta / 10;
            }
            else if (delta > 0)
            {
                angle += 0.08f;
                //angle -= delta / 10;
            }
            //ChangeRotation(new Vector3(0, angle, 0));
            RotateQuaternions(angle);
            _oldMouseState = newState;
            isMouseMovement = false;
            angle = 0;
            RefreshMatrix();
            newPosition = Position;
        }
        public void CameraUpdate(GameTime gameTime)
        {
            camera.Update(this.MatrixWorld, elonState, this.Rotation);
            camera.UpdateCamera(gameTime);

        }

        public void ChangeHealth(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.H))
            {
                health -= 1;
                if (health <= 0)
                    health = 0;
            }
            else if (state.IsKeyDown(Keys.J))
            {
                health += 1;
                if (health >= 100)
                    health = 100;
            }
        }
        public void SetPaloState(KeyboardState state, Scene _scene)
        {
            if (state.IsKeyDown(Keys.Space))
            {
                if (Palo.PaloState != FriendState.follow)
                {
                    Palo.PaloState = FriendState.follow;
                }
                else if (Palo.PaloState == FriendState.follow)
                {
                    Palo.PaloState = FriendState.idle;
                }
            }
            if(state.IsKeyDown(Keys.P))
            {
                if (Palo.PaloState != FriendState.walk)
                {
                    GameObject placeB = new GameObject();
                    GameObject placeC = new GameObject();
                    List<GameObject> guardPlaces = new List<GameObject>();
                    foreach (var elem in _scene.GameObjects)
                    {
                        if (elem.Tag == "guardPlace")
                        {
                            guardPlaces.Add(elem);
                        }
                        if (elem.Tag == "escapePlace")
                        {
                            placeC = elem;
                        }
                        
                    }
                    placeB = guardPlaces[0];
                    foreach(var elem in guardPlaces)
                    {
                        if(getDistance(elem) <= getDistance(placeB))
                        {
                            placeB = elem;
                        }
                    }
                    DecoyAI tmpDecoy = new DecoyAI(placeB, placeC);
                    Palo.DecoyGuards = tmpDecoy;
                    Palo.PaloState = FriendState.walk;
                    Palo.Walk = WalkState.decoy;
                }
            }
            if(state.IsKeyDown(Keys.L))
            {
                GameObject tmpBox = new GameObject();
                foreach(var elem in _scene.GameObjects)
                {
                    if(elem.Name == "boxForMovement")
                    {
                        tmpBox = elem;
                        break;
                    }
                }
                BoxMovementAI tmpAI = new BoxMovementAI(this.Position);
                tmpAI.PointA = Palo.FindPlaceBehindObject(tmpBox, this.Position);
                tmpAI.Cube = tmpBox;
                tmpAI.CubeMass = tmpBox.mass;
                Palo.MoveBoxAI = tmpAI;
                Palo.PaloState = FriendState.walk;
                Palo.Walk = WalkState.moveBox;
            }
            if(state.IsKeyDown(Keys.O))
            {
                Palo.RotateToBox(Palo.MoveBoxAI.Cube);
            }
        }

        public GameObject GetObjectByRay(Scene _scene, KeyboardState _state)
        {
            return new GameObject();
        }

        private Ray GetPickRay(Scene _scene)
        {
            Matrix world = Matrix.CreateTranslation(10, 0, 0);
            Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(this.camera.position, this.camera.projectionMatrix, this.camera.viewMatrix, world);
            Vector3 farPoint = GraphicsDevice.Viewport.Unproject(this.camera.target, this.camera.projectionMatrix, this.camera.viewMatrix, world);
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);
            return pickRay;
        }
    }
}
