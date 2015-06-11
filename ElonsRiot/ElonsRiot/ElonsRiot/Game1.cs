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
using ElonsRiot.BSPTree;
using ElonsRiot.Dialogues;

namespace ElonsRiot
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //W³aœciwoœci
        Scene MyScene { get; set; }             //Scena. Dziêki niej ³adujemy wszystkie gameobjecty itd.
        DialoguesManager MyDialogues{get;set;}
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
        SpriteBatch spriteBatchString;
        SpriteBatch spriteBatchLearning;
        bool isStatement = false;
        GameObject currentInteractiveObject;
        SpriteBatch sptiteBatchDialogues;
       // HUD myHUD;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            MyScene = new Scene(Content, GraphicsDevice);   //Dziêki temu mo¿emy korzystaæ z naszego contentu
            MyDialogues = new DialoguesManager();
            CurrentMouseState = Mouse.GetState();
            graphics.IsFullScreen = false;
        }
        protected override void Initialize()
        {
            PhysicManager.setElements(graphics.GraphicsDevice);
            CreateBSP.CreateLeafs();
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
            spriteBatchString = new SpriteBatch(GraphicsDevice);
            sptiteBatchDialogues = new SpriteBatch(GraphicsDevice);
            spriteBatchLearning = new SpriteBatch(GraphicsDevice);
            MyScene.LoadAllContent(graphics.GraphicsDevice);
            MyDialogues.InitializeDialoguesManager();
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
            MyScene.Update(MyScene.PlayerObject, gameTime, state);
            
            //MyRay.setReferences(GraphicsDevice, MyScene);
            //myHUD.DrawHUD(spriteBatchHUD);
            CreateBSP.checkPositionOfPlayer(MyScene.PlayerObject.Position);
            MyDialogues.withLine(gameTime);
            MyDialogues.checkStatements();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            MyScene.GraphicsDevice = GraphicsDevice;
            MyScene.DrawAllContent(graphics.GraphicsDevice);
            HUD.DrawHUD(spriteBatchHUD, MyScene.PlayerObject.health,MyScene.PaloObject.health, GraphicsDevice, MyScene, GraphicsDevice.Viewport.Width);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            
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
                Vector2 tmp = new Vector2(0, 0);
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
            if(MyScene.ObjectDetector.Information)
            {
                HUD.DrawStringForInformation(spriteBatchHUD4, MyScene.ObjectDetector.currentInteractiveObject.Information, GraphicsDevice);
            }
            if (MyDialogues.IsCorrectRoom)
            {
                    HUD.DrawString(sptiteBatchDialogues, 
                        MyDialogues.Statements[MyDialogues.AcctualStatementNumber].dialogLines.Line[MyDialogues.AcctualLineOfStatementCounter], GraphicsDevice);
            }
            if(MyScene.PaloObject.PaloLearningState == LearningState.Learning)
            {
                HUD.DrawLearningIcon(spriteBatchLearning, GraphicsDevice);
            }
            base.Draw(gameTime);
        }

        public void CheckRay(KeyboardState _state)
        {
            Ray pickRay2 = GetPickRay();
            isStatement = false;
            for (int i = 0; i < MyScene.GameObjects.Count; i++)
            {
                if (MyScene.GameObjects[i].Interactive == true)
                {
                    //Nullable<float> result2 = pickRay2.Intersects(MyScene.GameObjects[i].boundingBox);
                    Nullable<float> result2 = MyScene.GameObjects[i].boundingBox.Intersects(pickRay2);
                    if (result2.HasValue == true)
                    {
                        isStatement = true;
                        currentInteractiveObject = MyScene.GameObjects[i];
                    }
                }
            }
            if (_state.IsKeyDown(Keys.E))
            {
                Ray pickRay = GetPickRay();
                float selectedDistance = 100;// float.MaxValue;
                for (int i = 0; i < MyScene.GameObjects.Count; i++)
                {
                    if (MyScene.GameObjects[i].Interactive == true)
                    {
                        //Nullable<float> result = pickRay.Intersects(MyScene.GameObjects[i].boundingBox);
                        Nullable<float> result = MyScene.GameObjects[i].boundingBox.Intersects(pickRay);
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
            Vector3 nearPoint = MyScene.PlayerObject.camera.position;
            Vector3 direction = MyScene.PlayerObject.camera.desiredTarget - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);
            return pickRay;
        }

        Ray GetPickRayCamera()
        {
            //Matrix world = Matrix.CreateTranslation(0, 0, 0);
            //Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(MyScene.PlayerObject.camera.position, MyScene.PlayerObject.camera.projectionMatrix, MyScene.PlayerObject.camera.viewMatrix, world);
            //Vector3 farPoint = GraphicsDevice.Viewport.Unproject(MyScene.PlayerObject.Position, MyScene.PlayerObject.camera.projectionMatrix, MyScene.PlayerObject.camera.viewMatrix, world);
            //Vector3 direction = farPoint - nearPoint;

            Vector3 nearPoint = MyScene.PlayerObject.camera.position;
            Vector3 direction = MyScene.PlayerObject.camera.desiredTarget - nearPoint;
            direction.Normalize();
            //Ray pickRay = new Ray(nearPoint, direction);

           // direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction );
            return pickRay;
        }
    }
}
