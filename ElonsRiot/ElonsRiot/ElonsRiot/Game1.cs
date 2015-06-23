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
using ElonsRiot.Music;
using ElonsRiot.Particles;


namespace ElonsRiot
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //W³aœciwoœci
        Scene MyScene { get; set; }             //Scena. Dziêki niej ³adujemy wszystkie gameobjecty itd.
     //   DialoguesManager MyDialogues{get;set;}
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
        SpriteBatch spriteBatchHUD7;
        SpriteBatch spriteBatchString;
        SpriteBatch spriteBatchLearning;
        SpriteBatch spriteBatchVideo;
        bool isEnd = false;
        public bool isTalking = false;
        GameObject currentInteractiveObject;
        SpriteBatch sptiteBatchDialogues;
       // HUD myHUD;
        ParticleSystem explosionParticles;
        List<Projectile> projectiles = new List<Projectile>();
        TimeSpan timeToNextProjectile = TimeSpan.Zero;
        ParticleSystem bigExplosionParticles;
        ParticleSystem tinExplosionParticles;
        public float countdownTime;
        public float hackingCuntdownTime;
        public float bigExplosionTime;
        public float stringTime;

        Video intro;
        VideoPlayer player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            explosionParticles = new ExplosionParticleSystem(this, Content);
            bigExplosionParticles = new BigExplosionParticleSystem(this, Content);
            tinExplosionParticles = new TinExplosionParticleSystem(this, Content);
            Components.Add(explosionParticles);
            Components.Add(bigExplosionParticles);
            Components.Add(tinExplosionParticles);

            MyScene = new Scene(Content, GraphicsDevice);   //Dziêki temu mo¿emy korzystaæ z naszego contentu
         //   MyDialogues = new DialoguesManager();
            CurrentMouseState = Mouse.GetState();
            graphics.IsFullScreen = false;
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            graphics.PreferMultiSampling = true;
        }
        protected override void Initialize()
        {
            PhysicManager.setElements(graphics.GraphicsDevice);
            MusicManager.Initialize(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatchHUD = new SpriteBatch[3];
            spriteBatchHUD2 = new SpriteBatch[2];
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD[0] = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD[1] = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD[2] = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD2[0] = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD2[1] = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD3 = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD4 = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD5 = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD6 = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD7 = new SpriteBatch(GraphicsDevice);
            spriteBatchString = new SpriteBatch(GraphicsDevice);
            sptiteBatchDialogues = new SpriteBatch(GraphicsDevice);
            spriteBatchLearning = new SpriteBatch(GraphicsDevice);
            spriteBatchVideo = new SpriteBatch(GraphicsDevice);
            MyScene.LoadAllContent(graphics.GraphicsDevice);
            DialoguesManager.InitializeDialoguesManager();
            HUD.LoadHUD(MyScene.ContentManager, MyScene.PlayerObject.health);

            intro = Content.Load<Video>("Video/Intro");
            player = new VideoPlayer();
            player.Play(intro);
            countdownTime = 6;
            hackingCuntdownTime = 6;
            bigExplosionTime = 0.15f;
            stringTime = 0f;
           // MyRay.setReferences(GraphicsDevice, MyScene);

        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        protected override void Update(GameTime gameTime)
        {
            MyScene.time = gameTime;

            if (MyScene.PlayerObject.showShootExplosion)
            {
                UpdateProjectiles(gameTime, explosionParticles);
            }

            if (MyScene.PlayerObject.showBigExplosion)
            {
                UpdateProjectiles(gameTime, bigExplosionParticles);
            }
            if (MyScene.PlayerObject.showTinExplosion)
            {
                UpdateProjectiles(gameTime, tinExplosionParticles);
            }

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
            if (MyScene.PlayerObject.introEnd)
            {
                DialoguesManager.withLine(gameTime);
                DialoguesManager.checkStatements();
            }
            if (MyScene.PlayerObject.isBomb)
                Countdown(gameTime);
            if(MyScene.PlayerObject.isHacking)
                HackingCountdown(gameTime);
     		if(MyScene.PlayerObject.showTinExplosion)
                tinExplosionUpdate(gameTime);            //CheckRay(state);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            explosionParticles.SetCameraParameters(MyScene.PlayerObject.camera.viewMatrix, MyScene.PlayerObject.camera.projectionMatrix);
            bigExplosionParticles.SetCameraParameters(MyScene.PlayerObject.camera.viewMatrix, MyScene.PlayerObject.camera.projectionMatrix);
            tinExplosionParticles.SetCameraParameters(MyScene.PlayerObject.camera.viewMatrix, MyScene.PlayerObject.camera.projectionMatrix);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            MyScene.GraphicsDevice = GraphicsDevice;
            MyScene.DrawAllContent(graphics.GraphicsDevice, explosionParticles, bigExplosionParticles, tinExplosionParticles, gameTime);
            HUD.DrawHUD(spriteBatchHUD, MyScene.PlayerObject.health, MyScene.PaloObject.health, GraphicsDevice, MyScene, GraphicsDevice.Viewport.Width, countdownTime);
            HUD.DrawProgress(spriteBatchHUD, GraphicsDevice, MyScene, GraphicsDevice.Viewport.Width, hackingCuntdownTime);
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

            //if (MyScene.PlayerObject.showItem1 == true)
            //{
            //    HUD.DrawItem1(spriteBatchHUD5, GraphicsDevice);
            //}
            //if (MyScene.PlayerObject.showItem2 == true)
            //{
            //    HUD.DrawItem2(spriteBatchHUD6, GraphicsDevice);
            //}
            if (MyScene.PlayerObject.showSkills == true)
            {
                HUD.DrawSkills(spriteBatchHUD6, GraphicsDevice, MyScene.PaloObject);
            }
            if(MyScene.ObjectDetector.Information)
            {
                HUD.DrawStringForInformation(spriteBatchHUD4, MyScene.ObjectDetector.currentInteractiveObject.Information, GraphicsDevice);
            }

            if (MyScene.PlayerObject.introEnd)
            {
                if (DialoguesManager.IsCorrectRoom)
                {
                    HUD.DrawString(sptiteBatchDialogues,
                        DialoguesManager.Statements[DialoguesManager.AcctualStatementNumber].dialogLines.Line[DialoguesManager.AcctualLineOfStatementCounter], GraphicsDevice);
                }
                if (DialoguesManager.IsPressed)
                {
                    HUD.DrawString(sptiteBatchDialogues,
                           DialoguesManager.OnKeystatements[0].dialogLines.Line[DialoguesManager.ActualLineOnPress], GraphicsDevice);
                }
                if (DialoguesManager.IsLerning)
                {
                    HUD.DrawString(sptiteBatchDialogues,
                           DialoguesManager.LerningStatements[0].dialogLines.Line[DialoguesManager.ActualLineLerning], GraphicsDevice);
                }
                if(isEnd == true && stringTime <4)
                {
                    HUD.DrawString(sptiteBatchDialogues, "Elon:Ah...nie uda mi sie tego rozbroic!", this.GraphicsDevice);
                   // HUD.DrawString(sptiteBatchDialogues, "Palo:Przeanalizowalem budowe czujek.Mozemy je unieszkodliwic jesli zestrzelimy je jednoczesnie", this.GraphicsDevice);
                    isEnd = false;
                }
                
                if (MyScene.PaloObject.PaloLearningState == LearningState.Learning)
                {
                    HUD.DrawLearningIcon(spriteBatchLearning, GraphicsDevice);
                }
            }
            playIntro();

            base.Draw(gameTime);
        }

        public void CheckRay(KeyboardState _state)
        {
                Ray pickCameraRay = GetPickRayCamera();
                float selectedDistance = 1;
                for (int i = 0; i < MyScene.GameObjects.Count; i++)
                {
                    Nullable<float> result = pickCameraRay.Intersects(MyScene.GameObjects[i].boundingBox);
                    if (result.HasValue == true)
                    {
                        if (result.Value < selectedDistance)
                        {
                            selectedDistance = result.Value;
                            MyScene.PlayerObject.camera.offsetDistance.Z = -150;
                        }
                    }
                  //  else
                       // MyScene.PlayerObject.camera.offsetDistance.Z = 150;
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


        void UpdateProjectiles(GameTime gameTime, ParticleSystem explosion)
        {

            projectiles.Add(new Projectile(explosion, MyScene));

            int i = 0;

            while (i < projectiles.Count)
            {
                if (!projectiles[i].Update(gameTime))
                    projectiles.RemoveAt(i);
                else
                    i++;
            }
        }
        void HackingCountdown(GameTime gameTime)
        {
            if (hackingCuntdownTime > 0)
            {
                hackingCuntdownTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                //HUD.DrawString(spriteBatchHUD7, "Ah...nie uda mi sie tego rozbroic!", this.GraphicsDevice);
                isEnd = true;
                stringTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        void Countdown(GameTime gameTime)
        {
            if (countdownTime > 0)
            {
                countdownTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (bigExplosionTime > 0)
            {
                bigExplosionTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                MyScene.PlayerObject.showBigExplosion = true;
                MusicManager.PlaySound(2);
            }
            else
            {
                MyScene.PlayerObject.showBigExplosion = false;
            }
        }

        public void tinExplosionUpdate(GameTime gameTime)
        {
            if (MyScene.tinExplosionTime > 0)
            {
                MyScene.tinExplosionTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                MusicManager.PlaySound(2);
            }
            else
            {
                MyScene.PlayerObject.showTinExplosion = false;
            }
        }

        void playIntro()
        {
            Texture2D videoTexture = null;
            if (MyScene.PlayerObject.introEnd)
                player.Stop();

            if (player.State != MediaState.Stopped)
                videoTexture = player.GetTexture();
            else
                MyScene.PlayerObject.introEnd = true;

            if (videoTexture != null)
            {
                spriteBatchVideo.Begin();
                spriteBatchVideo.Draw(videoTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                spriteBatchVideo.End();
            }
        }
    }
}
