using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SkinnedModel;
using ElonsRiot.BSPTree;
using ElonsRiot.Music;
using ElonsRiot.Particles;

namespace ElonsRiot
{
    public class Scene// : Microsoft.Xna.Framework.Game
    {
        public ContentManager ContentManager { get; set; }
        //Interactions
        public ControlPlayer.ObjectDetectionManager ObjectDetector { get; set; }
        public Interaction.InteractiveObjectsList InteractiveObjects { get; set; }
        public ControlPlayer.InteractiveObjectsManager InteractionsManager { get; set; }
        //Quests
        public Quests.QuestManager QuestManager { get; set; }
        //Shooting System
        public Shooting.ShootingManager ShootingManager { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public List<GameObject> GameObjects { get; set; }
        public List<Guard> NPCs { get; set; }
        public List<GameObject> VisibleGameObjects {get; set;}
        public List<GameObject> ActualGameOjects { get; set; }
        public XMLScene XMLScene { get; set; }
        public Player PlayerObject { get; set; }
        public PaloCharacter PaloObject { get; set; }
        public GameTime time { get; set; }
        private BasicEffect basicEffect;
        private List<GameObject> actualGameObjects;
        public Vector3 currentTinPos;
        public float tinExplosionTime;

        Guard Marian;
        Guard Zenon;

        public Effect effect;
        Vector3 lightPos;
        float lightPower;
        float ambientPower;
        Matrix lightViewProjection;
        RenderTarget2D renderTarget;
        Texture2D shadowMap;

        Vector3 mirrorPos;
        Effect mirrorEffect;
        Matrix reflect;
        Matrix mirrorWorld;
        VertexBuffer mirrorVertexBuffer;
        IndexBuffer mirrorIndices;

        int indexMarian;

        DepthStencilState addIfMirror = new DepthStencilState()
        {
            StencilEnable = true,
            StencilFunction = CompareFunction.Always,
            StencilPass = StencilOperation.Increment
        };

        DepthStencilState checkMirror = new DepthStencilState()
        {
            StencilEnable = true,
            StencilFunction = CompareFunction.Equal,
            ReferenceStencil = 1,
            StencilPass = StencilOperation.Keep
        };

        public Scene(ContentManager _contentManager, GraphicsDevice _graphicsDevice)
        {
            GameObjects = new List<GameObject>();
            ActualGameOjects = new List<GameObject>();
            NPCs = new List<Guard>();
            ContentManager = _contentManager;
            GraphicsDevice = _graphicsDevice;
            XMLScene = new XMLScene();
            VisibleGameObjects = new List<GameObject>();
            ObjectDetector = new ControlPlayer.ObjectDetectionManager(this);
            InteractiveObjects = new Interaction.InteractiveObjectsList(this);
            InteractionsManager = new ControlPlayer.InteractiveObjectsManager(this);
            ShootingManager = new Shooting.ShootingManager { Scene = this };
            actualGameObjects = new List<GameObject>();
            QuestManager = new Quests.QuestManager(this);
        }
        public Scene()
        {
            GameObjects = new List<GameObject>();
            XMLScene = new XMLScene();
        }

        public void LoadAllContent(GraphicsDevice graphic)
        {
          //  physic = new Physic();
           // playSpeed = 2.0f;

            mirrorPos = new Vector3(42, 2, 0);
            mirrorWorld = Matrix.CreateTranslation(mirrorPos);

            XMLScene = DeserializeFromXML();

            effect = ContentManager.Load<Effect>("Effects/LightEffect");
            mirrorEffect = ContentManager.Load<Effect>("Effects/MirrorEffect");

            PresentationParameters pp = graphic.PresentationParameters;
            renderTarget = new RenderTarget2D(graphic, pp.BackBufferWidth, pp.BackBufferHeight, false, graphic.DisplayMode.Format, DepthFormat.Depth24);

            reflect = Matrix.CreateScale(1, 1, -1);
            SetUpMirrorIndices(graphic);
            SetUpMirrorVertices(graphic, mirrorPos);

            GameObjects = XMLScene.GameObjects;
            LoadPalo();
            LoadElon();
            LoadGuards();
            InteractiveObjects.AddToScene();
            PaloObject.LearningManager.AddObjectToScene();
            BSPTree.CreateBSP.CreateLeafs(GameObjects);
            BSPTree.CreateBSP.checkPositionOfPlayer(PlayerObject.Position);
            Methods.setPlayer( PlayerObject,GameObjects);
            foreach (var elem in GameObjects)
            {
                if (!string.IsNullOrEmpty(elem.ObjectPath))
                {
                    elem.RotationVectorToQuaternion();
                    elem.LoadModels(ContentManager);

                    if(elem.Interactive == true)
                    {
                        elem.setInteractionType();
                    }

                    //elem.Initialize();
                    //elem.RefreshMatrix();
                }
            }

            foreach (GameObject obj in GameObjects)
            {
                if (obj.Name != "characterElon" && obj.Name != "characterPalo" && obj.Tag != "guard")
                    obj.LoadEffects(effect);
            }
            int indexElon = 0;
            int indexPalo = 0;
            indexMarian = 0;
            for (int i=0; i<GameObjects.Count; i++)
            {
                if (GameObjects[i].Name == "characterElon")
                    indexElon = i;
            }
            for (int i = 0; i < GameObjects.Count; i++)
            {
                if (GameObjects[i].Name == "characterPalo")
                    indexPalo = i;
            }
            for (int i = 0; i < GameObjects.Count; i++)
            {
                if (GameObjects[i].Name == "enemyMarian")
                    indexMarian = i;
            }

            basicEffect = new BasicEffect(graphic);
            PlayerObject.LoadAnimation();
            PaloObject.LoadAnimation();
            foreach (var npc in NPCs)
                npc.LoadAnimation();
            SetLightData();
            CreateBSP.CreateLeafs(GameObjects);
            PhysicManager.InitializePhysicManager(GameObjects, PlayerObject);
            tinExplosionTime = 0.1f;
        }
        public void DrawAllContent(GraphicsDevice graphic, ParticleSystem explosion, ParticleSystem bigExplosion, ParticleSystem tinExplosion, GameTime gameTime)
        {
            graphic.SetRenderTarget(renderTarget);
            //graphic.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            graphic.DepthStencilState = DepthStencilState.Default;
            graphic.RasterizerState = RasterizerState.CullCounterClockwise;

            foreach (var elem in GameObjects)
            {
                if (elem.Name != "characterElon" && elem.Name != "characterPalo" && elem.Tag != "guard" && elem.Name != "ceil" && elem.Name != "Kuleczka" && elem.Name != "gun" && elem.Name != "Bomba" && elem.Name != "gunPalo")                
                {
                    elem.DrawModels(ContentManager, PlayerObject, lightPos, lightPower, ambientPower, lightViewProjection, "ShadowMap", shadowMap, reflect, false);
                }

                
            }
            graphic.SetRenderTarget(null);
            shadowMap = (Texture2D)renderTarget;

            graphic.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

             foreach(var elem in VisibleGameObjects)
             {
                 if (elem.Name == "characterElon")
                     elem.DrawAnimatedModels(ContentManager, PlayerObject, PlayerObject.animationPlayer, reflect, false);
                 else if (elem.Name == "characterPalo")
                     elem.DrawAnimatedModels(ContentManager, PlayerObject, PaloObject.animationPlayer, reflect, false);
                 else if (elem.Name == "ceil")
                     elem.DrawModels(ContentManager, PlayerObject, lightPos, lightPower, ambientPower, lightViewProjection, "Simplest", shadowMap, reflect, false);
                 else if (elem.Name == "Kuleczka" || elem.Name == "Bomba")
                     elem.DrawNoEffectModels(ContentManager, PlayerObject, reflect, false);
                elem.RefreshMatrix();
             }

            foreach (var npc in NPCs)
            {
                if (!npc.isDead)
                    npc.DrawAnimatedModels(ContentManager, PlayerObject, npc.animationPlayer, reflect, false);
            }

            foreach(var elem in GameObjects)
            {
                if ((PlayerObject.elonState.State == State.idleShoot || PlayerObject.elonState.State == State.walkShoot) && elem.Name == "gun")
                    elem.DrawModels(ContentManager, PlayerObject, lightPos, lightPower, ambientPower, lightViewProjection, "ShadowedScene", shadowMap, reflect, false);
                
                if((PaloObject.PaloState == FriendState.shoot) && elem.Name == "gunPalo")
                    elem.DrawModels(ContentManager, PlayerObject, lightPos, lightPower, ambientPower, lightViewProjection, "ShadowedScene", shadowMap, reflect, false);
            }


             foreach (var elem in VisibleGameObjects)
             {
                 if (elem.Name != "characterElon" && elem.Name != "characterPalo" && elem.Tag != "guard" && elem.Name != "ceil" && elem.Name != "Kuleczka" && elem.Name != "gun" && elem.Name != "Bomba" && elem.Name != "gunPalo")                 
                 {
                     elem.DrawModels(ContentManager, PlayerObject, lightPos, lightPower, ambientPower, lightViewProjection, "ShadowedScene", shadowMap, reflect, false);
                 }

            
             }
            // gun.DrawModels(ContentManager, PlayerObject, lightPos, lightPower, ambientPower, lightViewProjection, "ShadowedScene", shadowMap, reflect, false);

             explosion.DrawParticle(gameTime);
             bigExplosion.DrawParticle(gameTime);
             tinExplosion.DrawParticle(gameTime);

            foreach (GameObject gObj in this.VisibleGameObjects)
            {
                gObj.RefreshMatrix();
               
               // DrawBoudingBoxes(graphic, gObj);
            }
            shadowMap = null;
       //    DrawBoudingBox(graphic);
            DrawRay(graphic);
        }
        public void PlayerControll(KeyboardState _state, GameTime gameTime, MouseState _mouseState)
        {
            PlayerObject.SetState(_state, gameTime);
            PlayerObject.Movement(_state, _mouseState);
            PlayerObject.CameraUpdate(gameTime);
            PlayerObject.ChangeHealth(_state);
            PlayerObject.ChangeAmmo(_state);
            PlayerObject.ShowHUDElements(_state);
            PlayerObject.SetPaloState(_state, this);
        }
        private XMLScene DeserializeFromXML()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(XMLScene));
            TextReader textReader = new StreamReader(@"../../../../ElonsRiotContent/XML/scena.xml");
            XMLScene tmpGO;
            tmpGO = (XMLScene)deserializer.Deserialize(textReader);
            textReader.Close();
            return tmpGO;
        }
        public void Update(Player player, GameTime gameTime, KeyboardState _state)
        {

            VisibleGameObjects.Clear();
            //GameObjects = BSPTree.CreateBSP.ListOfVisibleObj();
            Console.WriteLine(GameObjects.Count);
            CreateBSP.checkPositionOfPlayer(player.Position);
            actualGameObjects = GameObjects;//CreateBSP.ListOfVisibleObj();
         //   foreach(GameObject stuff in PhysicManager.Stuffs)
            foreach (GameObject obj in actualGameObjects)
            {
                //if(PlayerObject.camera.IsVisible(obj,PlayerObject.camera.frustum))
               // {
                 VisibleGameObjects.Add(obj);
                //}
            }
            if (PlayerObject.elonState.State == State.idleShoot || PlayerObject.elonState.State == State.walkShoot)
                PlayerObject.showGun = true;
            else
                PlayerObject.showGun = false;

          //  Debug.WriteLine(VisibleGameObjects.Count.ToString());
            PhysicManager.update(gameTime, GameObjects, PlayerObject);
            //physic.update(gameTime, GameObjects, PlayerObject);
            PaloControl();
            NPCControl();
            PaloObject.LearningManager.LearningUpdate();
            PlayerObject.AnimationUpdate(gameTime);
            PlayerObject.animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

            PaloObject.AnimationUpdate(gameTime);
            PaloObject.animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

            foreach (var npc in NPCs)
            {
                npc.AnimationUpdate(gameTime);
                npc.animationPlayer.Update(npc.elapsedTime, true, Matrix.Identity);
            }


            foreach(GameObject gobj in GameObjects)
            {
                if (gobj.Name == "gun")
                {
                    Vector3 gunPosition;
                    gobj.RotationQ = PlayerObject.RotationQ;
                    if (PlayerObject.elonState.State == State.walkShoot)
                    {
                        gunPosition = PlayerObject.Position;
                        gunPosition += Vector3.Transform(Vector3.Right * 1.2f, PlayerObject.RotationQ);
                        gobj.Position = gunPosition + 3.0f * Vector3.Transform(Vector3.Forward, PlayerObject.RotationQ) + Vector3.Up * 6.8f;
                    }
                    else if (PlayerObject.elonState.State == State.idleShoot)
                    {
                        gunPosition = PlayerObject.Position;
                        gunPosition += Vector3.Transform(Vector3.Right * 1f, PlayerObject.RotationQ);
                        gobj.Position = gunPosition + 3.5f * Vector3.Transform(Vector3.Forward, PlayerObject.RotationQ) + Vector3.Up * 6.8f;
                    }
                }
                if (gobj.Name == "gunPalo")
                {
                    Vector3 gunPosition;
                    gobj.RotationQ = PaloObject.RotationQ;
                    if (PaloObject.PaloState == FriendState.shoot)
                    {
                        gunPosition = PaloObject.Position;
                        gunPosition += Vector3.Transform(Vector3.Right * 2.15f, PaloObject.RotationQ);
                        gobj.Position = gunPosition + 5.0f * Vector3.Transform(Vector3.Forward, PaloObject.RotationQ) + Vector3.Up * 7.8f;
                    }
                }
                gobj.update();
            }

            //if (PlayerObject.showTinExplosion)
            //    tinExplosionUpdate(gameTime);

            ObjectDetector.CheckRay();
            InteractionsManager.ManageInteractiveObject(_state);
            time = gameTime;
            ShootingManager.BulletsMovement();
            QuestManager.UpdateQuests();
        }
        private void LoadElon()
        {
			Vector3 tmpPos = new Vector3(80, 10, -25);
            Vector3 tmpRot = new Vector3(0, 180, 0);
            Player Elon = new Player(tmpPos, tmpRot, this);
            Elon.Name = "characterElon";
            Elon.id = "ABCDEF";
            Elon.Scale = new Vector3(0.035f, 0.035f, 0.035f);
            //Elon.Position = new Vector3(-100, 4 , 13);
            //Elon.Rotation = new Vector3(0, 0, 0);
            Elon.ObjectPath = "3D/ludzik/elon-idle";
            Elon.id = "ABCDEF";
            Elon.Palo = PaloObject;
            Elon.Palo.Elon = Elon;
            Elon.mass = 70;
            Elon.Tag = "Player";
            Elon.GraphicsDevice = GraphicsDevice;
            Elon.Interactive = false;
            GameObjects.Add(Elon);
            PlayerObject = Elon;
        }
        private void LoadGuards()
        {
            Marian = new Guard();
            Marian.Name = "enemyMarian";
            Marian.id = "ABCDEF";
            Marian.Scale = new Vector3(0.4f, 0.4f, 0.4f);
            Marian.Position = new Vector3(90, 4, 35);
            Marian.Rotation = new Vector3(86, 0, 34);
            Marian.ObjectPath = "3D/ludzik/soldier_idle";
            Marian.Tag = "guard";
            Marian.Scene = this;
            Marian.oldPosition = new Vector3(90, 4, 35);
            Marian.newPosition = new Vector3(90, 4, 35);

            Zenon = new Guard();
            Zenon.Name = "enemyZenon";
            Zenon.id = "ABCDEF";
            Zenon.Scale = new Vector3(0.4f, 0.4f, 0.4f);
            Zenon.Position = new Vector3(80, 0, 34);
            Zenon.Rotation = new Vector3(0, 0, 0);
            Zenon.ObjectPath = "3D/ludzik/soldier_idle";
            Zenon.oldPosition = new Vector3(80, 0, 34);
            Zenon.newPosition = new Vector3(80, 0, 34);
            Zenon.Tag = "guard";
            Zenon.Scene = this;

            GameObjects.Add(Marian);
            GameObjects.Add(Zenon);
            NPCs.Add(Marian);
            NPCs.Add(Zenon);

        }
        private void LoadPalo()
        {
            PaloCharacter Palo = new PaloCharacter(this);
            Palo.Name = "characterPalo";
            Palo.id = "ABCDEF";
            Palo.Scale = new Vector3(32.0f, 32.0f, 32.0f);
            Palo.Position = new Vector3(110, 4, -30);
            Palo.oldPosition = new Vector3(110, 4, -30);
            Palo.newPosition = new Vector3(110, 4, -30); 
            Palo.Rotation = new Vector3(0, 0, 0);
            Palo.ObjectPath = "3D/ludzik/palo_walk";
            Palo.Scene = this;
            Palo.id = "ABCDEF";
            Palo.Tag = "Palo";
            Palo.Interactive = false;
            Palo.mass = 100;
            PaloObject = Palo;
            GameObjects.Add(Palo);
        }
        private void PaloControl()
        {
            if ((PaloObject.PaloState == FriendState.follow) || (PaloObject.PaloState == FriendState.idleFollow))
            {
                PaloObject.WalkToPlayer();
            }
            else if(PaloObject.PaloState == FriendState.decoy)
            {
                PaloObject.Decoy(this);
            }
            else if ((PaloObject.PaloState == FriendState.moveBox) || (PaloObject.PaloState == FriendState.moveToBox))
            {
                PaloObject.MoveBox();
            }
        }
        private void NPCControl()
        {
            foreach(var elem in NPCs)
            {
                Guard currGuard = (Guard)elem;
                if(currGuard.State == GuardState.chase)
                {
                    currGuard.Chase();
                }
            }
        }

        



        public void DrawBoudingBox(GraphicsDevice graphic)
        {
           foreach (GameObject gameObj in this.GameObjects)
            {
              
                    Vector3[] corners = gameObj.boundingBox.GetCorners();
                
                    VertexPositionColor[] primitiveList = new VertexPositionColor[corners.Length];

                    // Assign the 8 box vertices
                    for (int i = 0; i < corners.Length; i++)
                    {
                        primitiveList[i] = new VertexPositionColor(corners[i], Color.White);
                    }

                    basicEffect.World = Matrix.Identity;
                    basicEffect.View = PlayerObject.camera.viewMatrix;
                    basicEffect.Projection = PlayerObject.camera.projectionMatrix;
                    basicEffect.TextureEnabled = false;

                    // Draw the box with a LineList
                    foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        graphic.DrawUserIndexedPrimitives(
                            PrimitiveType.LineList, primitiveList, 0, 8,
                            gameObj.bBoxIndices, 0, 12);
                    }
            }
        }
        public void DrawRay(GraphicsDevice graphic)
        {
            short[] bBoxIndices ={0, 1,0};
            VertexPositionColor[] primitiveList = new VertexPositionColor[2];
            primitiveList[0] = new VertexPositionColor(PlayerObject.nearPoint, Color.White);
            primitiveList[1] = new VertexPositionColor(PlayerObject.farPoint, Color.White);
           
            basicEffect.World = Matrix.Identity;
            basicEffect.View = PlayerObject.camera.viewMatrix;
            basicEffect.Projection = PlayerObject.camera.projectionMatrix;
            basicEffect.TextureEnabled = false;

            // Draw the box with a LineList
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphic.DrawUserIndexedPrimitives(
                    PrimitiveType.LineList, primitiveList, 0, 2,
                    bBoxIndices, 0, 1);
            }
        }
        public void DrawBoudingBoxes(GraphicsDevice graphic,GameObject gameObj)
        {
            if (gameObj.boxes.Count != 0)
            {

                Vector3[] corners = gameObj.boxes[0].GetCorners();

                VertexPositionColor[] primitiveList = new VertexPositionColor[corners.Length];

                // Assign the 8 box vertices
                for (int i = 0; i < corners.Length; i++)
                {
                    primitiveList[i] = new VertexPositionColor(corners[i], Color.White);
                }

                basicEffect.World = Matrix.Identity;
                basicEffect.View = PlayerObject.camera.viewMatrix;
                basicEffect.Projection = PlayerObject.camera.projectionMatrix;
                basicEffect.TextureEnabled = false;

                // Draw the box with a LineList
                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphic.DrawUserIndexedPrimitives(
                        PrimitiveType.LineList, primitiveList, 0, 8,
                        PlayerObject.bBoxIndices, 0, 12);
                }
            }
        }
        public void DrawModel(GameObject gameObj)
        {

            foreach (ModelMesh mesh in gameObj.GameObjectModel.Meshes)
            {

                foreach (BasicEffect effect2 in mesh.Effects)
                {
                    effect2.World = Matrix.CreateScale(0.4f) *Matrix.CreateRotationX(MathHelper.ToRadians(-gameObj.Rotation.X))
                    * Matrix.CreateRotationZ(MathHelper.ToRadians(-gameObj.Rotation.Z)) * gameObj.boneTransformations[mesh.ParentBone.Index] * gameObj.MatrixWorld;
                    effect2.View = this.PlayerObject.camera.viewMatrix;
                    effect2.Projection = this.PlayerObject.camera.projectionMatrix;
                    effect2.EnableDefaultLighting();

                }
                mesh.Draw();

            }
        }

        private void SetLightData()
        {
            ambientPower = 0.2f;

            lightPos = new Vector3(100, 100, -250);
            lightPower = 1.0f;


            Matrix lightsView = Matrix.CreateLookAt(lightPos, new Vector3(lightPos.X, lightPos.Y - 80, lightPos.Z + 300), new Vector3(0, 1, 0));
            Matrix lightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1f, 70f, 500f);

            lightViewProjection = lightsView * lightsProjection;
        }

        private void SetUpMirrorVertices(GraphicsDevice graphic, Vector3 mirrorPos)
        {
            VertexPositionColor[] mirrorVertices = new VertexPositionColor[4];

            mirrorVertices[0].Position = new Vector3(mirrorPos.X - 10, mirrorPos.Y - 5, mirrorPos.Z);
            mirrorVertices[1].Position = new Vector3(mirrorPos.X + 10, mirrorPos.Y - 5, mirrorPos.Z);
            mirrorVertices[2].Position = new Vector3(mirrorPos.X + 10, mirrorPos.Y + 10, mirrorPos.Z);
            mirrorVertices[3].Position = new Vector3(mirrorPos.X - 10, mirrorPos.Y + 10, mirrorPos.Z);

            mirrorVertices[0].Color = Color.White;
            mirrorVertices[1].Color = Color.White;
            mirrorVertices[2].Color = Color.White;
            mirrorVertices[3].Color = Color.White;

            mirrorVertexBuffer = new VertexBuffer(graphic, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            mirrorVertexBuffer.SetData<VertexPositionColor>(mirrorVertices);
        }

        private void SetUpMirrorIndices(GraphicsDevice graphic)
        {
            UInt16[] indices = new UInt16[6];

            indices[0] = 0;
            indices[1] = 2;
            indices[2] = 3;
            indices[3] = 0;
            indices[4] = 1;
            indices[5] = 2;

            mirrorIndices = new IndexBuffer(graphic, IndexElementSize.SixteenBits, 6, BufferUsage.WriteOnly);
            mirrorIndices.SetData<UInt16>(indices);
        }

        public void tinExplosionUpdate(GameTime gameTime)
        {
            if (tinExplosionTime > 0)
            {
                tinExplosionTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                PlayerObject.showTinExplosion = false;
            }
        }
    }
}
