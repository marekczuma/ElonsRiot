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
        public int ammo;
        public int ammoMax;
        private bool isMouseMovement;
        private float angle;
        public Vector3 oldPosition, newPosition;
        public bool showGun, showProgress, showCrosshair, showItem1, showItem2, showSkills;
        public PaloCharacter Palo { get; set; }
        public List<BoundingBox> boxes;
        public Vector3 nearPoint;
        public Vector3 farPoint;
        public Player()
        {
            nearPoint = new Vector3(0, 0, 0);
            farPoint = new Vector3(0, 0, 0);
            elonState = new CharacterState(State.idle);
            camera = new Camera();
            isMouseMovement = false;
            angle = 0;
            health = 100.0f;
            boxes = new List<BoundingBox>();
            gravity = -0.12f;
            ammo = 50;
            ammoMax = 50;
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
            ammo = 50;
            ammoMax = 50;
            showGun = false;
            showProgress = false;
            showCrosshair = false;
            showItem1 = false; 
            showItem2 = false;
            showSkills = false;
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
            else if (state.IsKeyDown(Keys.D1))
            {
                elonState.SetCurrentState(State.climb);
            }
            else if (state.IsKeyDown(Keys.D2))
            {
                elonState.SetCurrentState(State.idleShoot);
            }
            else if (state.IsKeyDown(Keys.D3))
            {
                elonState.SetCurrentState(State.walkShoot);
            }
            else if (state.IsKeyDown(Keys.D4))
            {
                elonState.SetCurrentState(State.push);
            }
            else if (state.IsKeyDown(Keys.D5))
            {
                elonState.SetCurrentState(State.interact);
            }
            else if (state.IsKeyDown(Keys.D6))
            {
                elonState.SetCurrentState(State.walk);
            }
            else if(! state.IsKeyDown(Keys.E))
            {
                elonState.SetCurrentState(State.idle);
            }
        }
        public void Movement(KeyboardState state, MouseState _oldMouseState)
        {
            
            oldPosition = Position;
            if (state.IsKeyDown(Keys.S))
            {
                ChangePosition(new Vector3(0, 0, elonState.VelocityBack));
              
            }
            else if (state.IsKeyDown(Keys.W))
            {
                ChangePosition(new Vector3(0, 0,-elonState.VelocityForward ));
             
            }
            if (state.IsKeyDown(Keys.A))
            {
                ChangePosition(new Vector3(-elonState.VelocitySide, 0,0 ));
               
            }
            else if (state.IsKeyDown(Keys.D))
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
            if (state.IsKeyDown(Keys.B))
            {
                Palo.health -= 1;
                if (Palo.health <= 0)
                    Palo.health = 0;
            }
            else if (state.IsKeyDown(Keys.N))
            {
                Palo.health += 1;
                if (Palo.health >= 100)
                    Palo.health = 100;
            }
        }
        public void ShowHUDElements(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.T))
            {
                showGun = true;
            }
            else if (state.IsKeyDown(Keys.Y))
            {
                showGun = false;
            }
            else if (state.IsKeyDown(Keys.NumPad1))
            {
                showCrosshair = true;
            }
            else if (state.IsKeyDown(Keys.NumPad2))
            {
                showCrosshair = false;
            }
            else if (state.IsKeyDown(Keys.NumPad3))
            {
                showProgress = true;
            }
            else if (state.IsKeyDown(Keys.NumPad4))
            {
                showProgress = false;
            }
            else if (state.IsKeyDown(Keys.NumPad5))
            {
                showItem1 = true;
                showItem2 = false;
            }
            else if (state.IsKeyDown(Keys.NumPad6))
            {
                showItem1 = false;
                showItem2 = true;
            }
            else if (state.IsKeyDown(Keys.NumPad7))
            {
                showItem1 = false;
                showItem2 = false;
            }
            else if (state.IsKeyDown(Keys.NumPad8))
            {
                showSkills = true;
            }
            else if (state.IsKeyDown(Keys.NumPad9))
            {
                showSkills = false;
            }
        }
        public void ChangeAmmo(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Z))
            {
                ammo -= 1;
                if (ammo <= 0)
                    ammo = 0;
            }
            else if (state.IsKeyDown(Keys.X))
            {
                ammo += 1;
                if (ammo >= 50)
                    ammo = 50;
            }
            if (state.IsKeyDown(Keys.C))
            {
                Palo.ammo -= 1;
                if (Palo.ammo <= 0)
                    Palo.ammo = 0;
            }
            else if (state.IsKeyDown(Keys.V))
            {
                Palo.ammo += 1;
                if (Palo.ammo >= 50)
                    Palo.ammo = 50;
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
            if(state.IsKeyDown(Keys.O))
            {
                Palo.RotateToBox(Palo.MoveBoxAI.Cube);
            }
        }


        //Do usunięcia
        public GameObject GetObjectByRay(Scene _scene, KeyboardState _state, string _name, float _distance)
        {
            Ray pickRay = GetPickRay(_scene);
            float selectedDistance = _distance;
            for (int i = 0; i < _scene.GameObjects.Count; i++)
            {
                if ((_scene.GameObjects[i].Interactive == true) && (_scene.GameObjects[i].Name == _name))
                {
                    Nullable<float> result = pickRay.Intersects(_scene.GameObjects[i].boundingBox);
                    if (result.HasValue == true)
                    {
                        if (result.Value < selectedDistance)
                        {
                            return _scene.GameObjects[i];
                        }
                    }
                }
            }
            return null;
        }
        //Do Usunięcia
        private Ray GetPickRay(Scene _scene)
        {
            Matrix world = Matrix.CreateTranslation(10, 0, 0);
             nearPoint = _scene.GraphicsDevice.Viewport.Unproject(_scene.PlayerObject.camera.position, _scene.PlayerObject.camera.projectionMatrix, _scene.PlayerObject.camera.viewMatrix, world);
             farPoint = _scene.GraphicsDevice.Viewport.Unproject(this.camera.target, this.camera.projectionMatrix, this.camera.viewMatrix, world);
            Vector3 temp = farPoint;
            temp.Y += 0.3f;
            farPoint = temp;
            temp = nearPoint;
            temp.Y += 0.3f;
            nearPoint = temp;
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);
            return pickRay;
        }
    }
}
