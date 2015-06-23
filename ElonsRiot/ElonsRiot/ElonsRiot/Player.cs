using ElonsRiot.Dialogues;
using ElonsRiot.Music;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SkinnedModel;

namespace ElonsRiot
{
    public class Player : GameObject
    {
        public Equipment equipment;
        public GraphicsDevice GraphicsDevice { get; set; }
        public Scene Scene { get; set; }
        public CharacterState elonState {get; set;}    //STAN ELONA - TO JEST KLASA KUŹWA!
        public Camera camera;
        public float health;
        public int ammo;
        public int ammoMax;
        private bool isMouseMovement;
        public float angle;
        public Vector3 oldPosition, newPosition;
        public bool showGun, showProgress, showCrosshair, showItem1, showItem2, showSkills, showShootExplosion, showBigExplosion, showTinExplosion;
        public bool introEnd = false;
        public PaloCharacter Palo { get; set; }
        public List<BoundingBox> boxes;
        public Vector3 nearPoint;
        public Vector3 farPoint;
        KeyboardState oldState;
        public bool isBomb = false;
        public bool isHacking = false;
        public AnimationClip clip;
        public AnimationPlayer animationPlayer;
        public SkinningData skinningData;
        State previousState = State.idle;
        public float interactTime;

        private float timer = 0;

        public Player()
        {
            nearPoint = new Vector3(0, 0, 0);
            farPoint = new Vector3(0, 0, 0);
            elonState = new CharacterState(State.idle);
            camera = new Camera(this);
            isMouseMovement = false;
            angle = 0;
            health = 100.0f;
            boxes = new List<BoundingBox>();
            gravity = -0.12f;
            ammo = 50;
            ammoMax = 50;

        }

        public Player(Vector3 _position, Vector3 _rotation, Scene _scene)
        {
            equipment = new Equipment();
            Position = _position;
            Rotation = _rotation;
            RotationQ = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(Rotation.Y));
            elonState = new CharacterState(State.idle);
            camera = new Camera(this);
            isMouseMovement = false;
            angle = 0;
            health = 100.0f;
            ammo = 50;
            ammoMax = 50;
            interactTime = 2.9f;
            showGun = false;
            showProgress = false;
            showCrosshair = true;
            showItem1 = false; 
            showItem2 = false;
            showSkills = false;
            boxes = new List<BoundingBox>();
            Scene = _scene;
        }

        public void SetState(KeyboardState state, GameTime gameTime)
        {
            float timeInMS = Scene.time.ElapsedGameTime.Milliseconds;
            timer -= timeInMS;
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
                //PhysicManager.ClimbBox(this);
               // DialoguesManager.IsPressed = true;
                equipment.PressButton = true;
            }
            else if (state.IsKeyDown(Keys.D1))
            {
                elonState.SetCurrentState(State.climb);
            }
            else if (state.IsKeyDown(Keys.D2))
            {
                camera.SetOffsetDistance();
                elonState.SetCurrentState(State.idleShoot);
                if (state.IsKeyDown(Keys.G))
                {
                    Scene.ShootingManager.Shot(this, RotationQ);
                    MusicManager.PlaySound(1);

                    if (!oldState.IsKeyDown(Keys.G))
                    {
                        showShootExplosion = true;
                        ammo--;
                    }
                    else
                    {
                        showShootExplosion = false;
                    }
                }
            }
            else if (state.IsKeyDown(Keys.D3))
            {
                elonState.SetCurrentState(State.walkShoot);
                
                if (state.IsKeyDown(Keys.G))
                {
                    Scene.ShootingManager.Shot(this, RotationQ);
                    MusicManager.PlaySound(1);
                    if (!oldState.IsKeyDown(Keys.G))
                    {
                        ammo--;
                        showShootExplosion = true;
                    }
                    else
                    {
                        showShootExplosion = false;
                    }
                    
                }
            }
            else if (state.IsKeyDown(Keys.D4))
            {
                elonState.SetCurrentState(State.push);
            }
            else if (state.IsKeyDown(Keys.D5))
            {
                elonState.SetCurrentState(State.interact);
                if (isBomb == false)
                {
                    GameObject tmp = new GameObject { Name = "Bomba", ObjectPath = "3D/Placeholders/Bomba", Position = new Vector3(86, 5, -1f), Scale = new Vector3(1, 3, 1), id = "ABCDEF", Rotation = new Vector3(90,0,0) };
                    tmp.LoadModels(Scene.ContentManager);

                    Scene.GameObjects.Add(tmp);
                    isBomb = true;
                }
                else if(isHacking == false)
                {
                    isHacking = true;
                }

            }
            else if (state.IsKeyDown(Keys.D6))
            {
                if (!oldState.IsKeyDown(Keys.D6))
                    showShootExplosion = true;
                else
                {
                    showShootExplosion = false;
                }
            }
            else if (state.IsKeyDown(Keys.D3) && state.IsKeyDown(Keys.D6))
            {
                elonState.SetCurrentState(State.walkShoot);
                if (!oldState.IsKeyDown(Keys.D6))
                    showShootExplosion = true;
                else
                {
                    showShootExplosion = false;
                }
            }
            else if(state.IsKeyDown(Keys.L))
            {
                if (timer <= 0)
                {
                    if (Palo.PaloLearningState == LearningState.idle)
                    {
                        Palo.PaloLearningState = LearningState.Learning;
                        DialoguesManager.IsLerning = true;
                        DialoguesManager.mixDailogues();
                    }
                    else
                    {
                        Palo.PaloLearningState = LearningState.idle;
                        DialoguesManager.IsLerning = false;
                    }
                    timer = 500;
                }
                
            }
            else if(state.IsKeyDown(Keys.Back))
            {
                introEnd = true;
            }

            else if((!state.IsKeyDown(Keys.E)) && (elonState.State == State.interact))
            {
                if (interactTime > 0)
                {
                    interactTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (interactTime <= 0)
                        elonState.SetCurrentState(State.idle);
                }
            }
            else if (!state.IsKeyDown(Keys.E))
            {
                elonState.SetCurrentState(State.idle);
            }
            oldState = state;
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
                if(elonState.State == State.idleShoot)
                {
                    angle -= 0.005f;
                }else
                    angle -= 0.08f;
            }
            else if (delta > 0)
            {
                if (elonState.State == State.idleShoot)
                {
                    angle += 0.005f;
                }
                else
                    angle += 0.08f;
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
            camera.Update(this.MatrixWorld, elonState, this.Rotation, this);
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
            //else if (state.IsKeyDown(Keys.NumPad5))
            //{
            //    showLegend = true;
            //    showItem2 = false;
            //}
            //else if (state.IsKeyDown(Keys.NumPad6))
            //{
            //    showItem1 = false;
            //    showItem2 = true;
            //}
            //else if (state.IsKeyDown(Keys.NumPad7))
            //{
            //    showItem1 = false;
            //    showItem2 = false;
            //}
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
            float timeInMS = Scene.time.ElapsedGameTime.Milliseconds;
            timer -= timeInMS;
            if (state.IsKeyDown(Keys.Space))
            {
                if (timer <= 0)
                {
                    if (Palo.PaloState != FriendState.follow)
                    {
                        Palo.PaloState = FriendState.follow;
                    }
                    else if (Palo.PaloState == FriendState.follow)
                    {
                        Palo.PaloState = FriendState.idle;
                    }
                    timer = 500;
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
                    Palo.PaloState = FriendState.decoy;
                    //Palo.Walk = WalkState.decoy;
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

        public void LoadAnimation()
        {
            skinningData = GameObjectModel.Tag as SkinningData;
            animationPlayer = new AnimationPlayer(skinningData);
            clip = skinningData.AnimationClips["Take 001"];
            animationPlayer.StartClip(clip);
        }

        public void AnimationUpdate(GameTime gameTime)
        {
            if (elonState.State == State.run)
            {
                clip = skinningData.AnimationClips["Take 003"];
                if (previousState != State.run)
                    animationPlayer.StartClip(clip);
                previousState = State.run;
            }
            else if (elonState.State == State.walk)
            {
                clip = skinningData.AnimationClips["Take 002"];
                if (previousState != State.walk)
                    animationPlayer.StartClip(clip);
                previousState = State.walk;

            }
            else if (elonState.State == State.idle)
            {
                clip = skinningData.AnimationClips["Take 001"];
                if (previousState != State.idle)
                    animationPlayer.StartClip(clip);
                previousState = State.idle;
            }
            else if (elonState.State == State.walkShoot)
            {
                clip = skinningData.AnimationClips["Take 006"];
                if (previousState != State.walkShoot)
                    animationPlayer.StartClip(clip);
                previousState = State.walkShoot;
            }
            else if (elonState.State == State.idleShoot)
            {
                clip = skinningData.AnimationClips["Take 005"];
                if (previousState != State.idleShoot)
                    animationPlayer.StartClip(clip);
                previousState = State.idleShoot;
            }
            else if (elonState.State == State.climb)
            {
                clip = skinningData.AnimationClips["Take 004"];
                if (previousState != State.climb)
                    animationPlayer.StartClip(clip);
                previousState = State.climb;
            }
            else if (elonState.State == State.push)
            {
                clip = skinningData.AnimationClips["Take 007"];
                if (previousState != State.push)
                    animationPlayer.StartClip(clip);
                previousState = State.push;
            }
            else if (elonState.State == State.interact)
            {
                clip = skinningData.AnimationClips["Take 008"];
                if (previousState != State.interact)
                    animationPlayer.StartClip(clip);
                previousState = State.interact;

                
            }
        }
    }
}
