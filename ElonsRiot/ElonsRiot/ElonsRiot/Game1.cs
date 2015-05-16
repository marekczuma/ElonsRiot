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
        SpriteBatch spriteBatchHUD;
       // HUD myHUD;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            MyScene = new Scene(Content);   //Dziêki temu mo¿emy korzystaæ z naszego contentu
            CurrentMouseState = Mouse.GetState();
         //   myHUD = new HUD();
        }
        protected override void Initialize()
        {
            PhysicManager.setElements();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD = new SpriteBatch(GraphicsDevice);
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            state = Keyboard.GetState();
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
            MyScene.DrawAllContent(graphics.GraphicsDevice);
            HUD.DrawHUD(spriteBatchHUD, MyScene.PlayerObject.health, GraphicsDevice);
            
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
                                
                                Interactions interactionsClass = new Interactions(MyScene.GameObjects[i].interactionType, MyScene.GameObjects[i]);
                              //  MyScene.GameObjects[i].ChangePosition(new Vector3(0f, 0f, 0.2f));
                                interactionsClass.Add();
                                interactionsClass.CallInteraction();
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
                            MyScene.PlayerObject.camera.offsetDistance.Z = 20;
                        }
                    }
                }
            }
        }

        Ray GetPickRay()
        {
            Matrix world = Matrix.CreateTranslation(10, 0, 0);
            Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(MyScene.PlayerObject.camera.position, MyScene.PlayerObject.camera.projectionMatrix, MyScene.PlayerObject.camera.viewMatrix, world);
            Vector3 farPoint = GraphicsDevice.Viewport.Unproject(MyScene.PlayerObject.camera.target, MyScene.PlayerObject.camera.projectionMatrix, MyScene.PlayerObject.camera.viewMatrix, world);
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
