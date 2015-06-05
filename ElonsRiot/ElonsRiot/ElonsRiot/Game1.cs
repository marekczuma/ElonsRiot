using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace ElonsRiot
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //W³aœciwoœci
        Scene MyScene { get; set; }             //Scena. Dziêki niej ³adujemy wszystkie gameobjecty itd.
        KeyboardState state { get; set; }
        MouseState CurrentMouseState { get; set; }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch[] spriteBatchHUD;
        SpriteBatch[] spriteBatchHUD2;
        SpriteBatch spriteBatchHUD3;
        SpriteBatch spriteBatchHUD4;
        SpriteBatch spriteBatchHUD5;
        SpriteBatch spriteBatchHUD6;
       // HUD myHUD;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            MyScene = new Scene(Content, GraphicsDevice);   //Dziêki temu mo¿emy korzystaæ z naszego contentu
            CurrentMouseState = Mouse.GetState();
            //graphics.IsFullScreen = true;
         //   myHUD = new HUD();
        }
        protected override void Initialize()
        {
            PhysicManager.setElements(graphics.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatchHUD = new SpriteBatch[2];
            spriteBatchHUD2 = new SpriteBatch[2];
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD[0] = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD[1] = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD2[0] = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD2[1] = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD3 = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD4 = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD5 = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD6 = new SpriteBatch(GraphicsDevice);
            MyScene.LoadAllContent(graphics.GraphicsDevice);
            HUD.LoadHUD(MyScene.ContentManager, MyScene.PlayerObject.health);
           // MyRay.setReferences(GraphicsDevice, MyScene);

        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        protected override void Update(GameTime gameTime)
        {
            state = Keyboard.GetState();
            // Allows the game to exit
            if (state.IsKeyDown(Keys.Escape))
                this.Exit();

            MyScene.PlayerControll(state, gameTime, CurrentMouseState);
            CurrentMouseState = Mouse.GetState();
            MyScene.Update(MyScene.PlayerObject, gameTime);
            CheckRay(state);
            //MyRay.setReferences(GraphicsDevice, MyScene);
            //myHUD.DrawHUD(spriteBatchHUD);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            MyScene.GraphicsDevice = GraphicsDevice;
            MyScene.DrawAllContent(graphics.GraphicsDevice);
            HUD.DrawHUD(spriteBatchHUD, MyScene.PlayerObject.health,MyScene.PaloObject.health, GraphicsDevice, MyScene, GraphicsDevice.Viewport.Width);
            if(MyScene.PlayerObject.showGun == true)
            {
                HUD.DrawHUDGuns(spriteBatchHUD2, MyScene.PlayerObject.ammo, MyScene.PaloObject.ammo,MyScene.PlayerObject.ammoMax,
                GraphicsDevice,GraphicsDevice.Viewport.Width);
            }

            if(MyScene.PlayerObject.showProgress == true)
            {
                HUD.DrawProgress(spriteBatchHUD3, GraphicsDevice);
            }

            if (MyScene.PlayerObject.showCrosshair == true)
            {
                HUD.DrawCrosshair(spriteBatchHUD4, GraphicsDevice);
            }

            if (MyScene.PlayerObject.showItem1 == true)
            {
                HUD.DrawItem1(spriteBatchHUD5, GraphicsDevice);
            }
            if (MyScene.PlayerObject.showItem2 == true)
            {
                HUD.DrawItem2(spriteBatchHUD6, GraphicsDevice);
            }
            if (MyScene.PlayerObject.showSkills == true)
            {
                HUD.DrawSkills(spriteBatchHUD6, GraphicsDevice);
            }
            base.Draw(gameTime);
        }

        public void CheckRay(KeyboardState _state)
        {
            
            if (_state.IsKeyDown(Keys.E))
            {
                Ray pickRay = GetPickRay();
                float selectedDistance = float.MaxValue;
                for (int i = 0; i < MyScene.GameObjects.Count; i++)
                {
                    if (MyScene.GameObjects[i].Interactive == true)
                    {
                        Nullable<float> result = pickRay.Intersects(MyScene.GameObjects[i].boundingBox);
                        if (result.HasValue == true)
                        {
                            if (result.Value < selectedDistance)
                            {
                                
                               // Interactions interactionsClass = new Interactions(MyScene.GameObjects[i].interactionType, MyScene.GameObjects[i]);
                                MyScene.GameObjects[i].ChangePosition(new Vector3(0.05f, 0f, 0.0f));
                                //Interactions.Add(MyScene.GameObjects[i].interactionType);
                                //Interactions.CallInteraction(MyScene.GameObjects[i]);
                            }
                        }
                    }
                }
            }
            else
            {
                Ray pickCameraRay = GetPickRayCamera();
                float selectedDistance = 2;
                for (int i = 0; i < MyScene.GameObjects.Count; i++)
                {
                    Nullable<float> result = pickCameraRay.Intersects(MyScene.GameObjects[i].boundingBox);
                    if (result.HasValue == true)
                    {
                        if (result.Value < selectedDistance)
                        {
                            selectedDistance = result.Value;
                            MyScene.PlayerObject.camera.offsetDistance.Z = -70;
                        }
                    }
                  //  else
                       // MyScene.PlayerObject.camera.offsetDistance.Z = 150;
                }
            }
        }

        Ray GetPickRay()
        {
            Matrix world = Matrix.CreateTranslation(10, 0, 0);
            Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(MyScene.PlayerObject.camera.position, MyScene.PlayerObject.camera.projectionMatrix, MyScene.PlayerObject.camera.viewMatrix, world);
            Vector3 farPoint = GraphicsDevice.Viewport.Unproject(MyScene.PlayerObject.camera.target, MyScene.PlayerObject.camera.projectionMatrix, MyScene.PlayerObject.camera.viewMatrix, world);
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

        Ray GetPickRayCamera()
        {
            Matrix world = Matrix.CreateTranslation(0, 0, 0);
            Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(MyScene.PlayerObject.camera.position, MyScene.PlayerObject.camera.projectionMatrix, MyScene.PlayerObject.camera.viewMatrix, world);
            Vector3 farPoint = GraphicsDevice.Viewport.Unproject(MyScene.PlayerObject.Position, MyScene.PlayerObject.camera.projectionMatrix, MyScene.PlayerObject.camera.viewMatrix, world);
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);
            return pickRay;
        }
    }
}
