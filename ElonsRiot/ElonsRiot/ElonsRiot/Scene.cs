﻿using Microsoft.Xna.Framework;
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

namespace ElonsRiot
{
    public class Scene// : Microsoft.Xna.Framework.Game
    {
        public ContentManager ContentManager { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public List<GameObject> GameObjects { get; set; }
        public List<GameObject> NPCs { get; set; }
        public List<GameObject> VisibleGameObjects {get; set;}
        public XMLScene XMLScene { get; set; }
        public Player PlayerObject { get; set; }
        public PaloCharacter PaloObject { get; set; }
        private BasicEffect basicEffect;
       // private Physic physic;
        public AnimationPlayer animationPlayer;
        State previousState = State.idle;
        AnimationClip clip;
        public SkinningData skinningData;
        AnimationPlayer animationPlayerPalo;
        AnimationPlayer animationPlayerEnemy;

        public Scene(ContentManager _contentManager, GraphicsDevice _graphicsDevice)
        {
            GameObjects = new List<GameObject>();
            NPCs = new List<GameObject>();
            ContentManager = _contentManager;
            GraphicsDevice = _graphicsDevice;
            XMLScene = new XMLScene();
            VisibleGameObjects = new List<GameObject>();
           
        }
        public Scene()
        {
            GameObjects = new List<GameObject>();
            XMLScene = new XMLScene();
        }

        public void LoadAllContent(GraphicsDevice graphic)
        {
          //  physic = new Physic();
            XMLScene = DeserializeFromXML();
            GameObjects = XMLScene.GameObjects;
            LoadPalo();
            LoadElon();
            LoadGuards();
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
            int indexElon = 0;
            int indexPalo = 0;
            int indexMarian = 0;
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
            //Elon - inicjalizacja
            skinningData = GameObjects[indexElon].GameObjectModel.Tag as SkinningData;
            animationPlayer = new AnimationPlayer(skinningData);
            clip = skinningData.AnimationClips["Take 001"];
            animationPlayer.StartClip(clip);

            //Palo anim
            SkinningData skinningDataPalo = GameObjects[indexPalo].GameObjectModel.Tag as SkinningData;
            animationPlayerPalo = new AnimationPlayer(skinningDataPalo);
            AnimationClip clipPalo = skinningDataPalo.AnimationClips["Take 001"];

            SkinningData skinningDataEnemy = GameObjects[indexMarian].GameObjectModel.Tag as SkinningData;
            animationPlayerEnemy = new AnimationPlayer(skinningDataEnemy);
            AnimationClip clipEnemy = skinningDataEnemy.AnimationClips["Take 001"];

          //  animationPlayer.StartClip(clip);
            animationPlayerPalo.StartClip(clipPalo);
            animationPlayerEnemy.StartClip(clipEnemy);

            basicEffect = new BasicEffect(graphic);
        }
        public void DrawAllContent(GraphicsDevice graphic)
        {
             foreach(var elem in VisibleGameObjects)
             {
                 if (elem.Name == "characterElon")
                     elem.DrawAnimatedModels(ContentManager, PlayerObject, animationPlayer);
                 else if (elem.Name == "characterPalo")
                     elem.DrawAnimatedModels(ContentManager, PlayerObject, animationPlayerPalo);
                 else if (elem.Tag == "guard")
                     elem.DrawAnimatedModels(ContentManager, PlayerObject, animationPlayerEnemy);
                 else
                    elem.DrawModels(ContentManager, PlayerObject);               
                elem.RefreshMatrix();
             }
            foreach (GameObject gObj in this.VisibleGameObjects)
            {
              //  DrawModel(gObj);
             //   gObj.createBoudingBox();
                gObj.RefreshMatrix();
             //   DrawBoudingBoxes(graphic, gObj);
            }
            DrawBoudingBox(graphic);
            DrawRay(graphic);

        }
        public void PlayerControll(KeyboardState _state, GameTime gameTime, MouseState _mouseState)
        {
            PlayerObject.SetState(_state);
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
        public void Update(Player player, GameTime gameTime)
        {
            VisibleGameObjects.Clear();

            foreach (GameObject obj in GameObjects)
            {
                if (PlayerObject.camera.frustum.Contains(obj.boundingBox) != ContainmentType.Disjoint || obj.Name == "terrain" || obj.Name == "ceil")
                    VisibleGameObjects.Add(obj);
            }

            PhysicManager.update(gameTime, GameObjects, PlayerObject);
            //physic.update(gameTime, GameObjects, PlayerObject);
            PaloControl();
            NPCControl();
            ElonAnimationUpdate(gameTime.ElapsedGameTime);
            animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
            animationPlayerPalo.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
            animationPlayerEnemy.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

            foreach(GameObject gobj in GameObjects)
            {
                gobj.update();
            }
            
        }
        private void LoadElon()
        {
            Vector3 tmpPos = new Vector3(100, 4, -90);
            Vector3 tmpRot = new Vector3(0, 180, 0);
            Player Elon = new Player(tmpPos, tmpRot);
            Elon.Name = "characterElon";
            Elon.Scale = new Vector3(0.035f, 0.035f, 0.035f);
            //Elon.Position = new Vector3(-100, 4 , 13);
            //Elon.Rotation = new Vector3(0, 0, 0);
            Elon.ObjectPath = "3D/ludzik/elon-idle";
            Elon.Palo = PaloObject;
            Elon.Palo.Elon = Elon;
            Elon.mass = 70;
            Elon.Tag = "Player";
            Elon.GraphicsDevice = GraphicsDevice;
            GameObjects.Add(Elon);
            PlayerObject = Elon;
        }
        private void LoadGuards()
        {
            Guard Marian = new Guard();
            Marian.Name = "enemyMarian";
            Marian.Scale = new Vector3(0.09f, 0.09f, 0.09f);
            Marian.Position = new Vector3(90, 4, 35);
            Marian.Rotation = new Vector3(86, 0, 34);
            Marian.ObjectPath = "3D/ludzik/dude";
            Marian.Tag = "guard";
            Marian.oldPosition =
            Marian.oldPosition = new Vector3(90, 4, 35);
            Marian.newPosition = new Vector3(90, 4, 35);
            //Guard Zenon = new Guard();
            //Zenon.Name = "enemyZenon";
            //Zenon.Scale = new Vector3(0.09f, 0.09f, 0.09f);
            //Zenon.Position = new Vector3(86, 0, 34);
            //Zenon.Rotation = new Vector3(0, 0, 0);
            //Zenon.ObjectPath = "3D/ludzik/dude";
            //Zenon.oldPosition = new Vector3(86, 0, 34);
            //Zenon.newPosition = new Vector3(86, 0, 34);
            //Zenon.Tag = "guard";
            GameObjects.Add(Marian);
            //GameObjects.Add(Zenon);
            NPCs.Add(Marian);
            //NPCs.Add(Zenon);

        }
        private void LoadPalo()
        {
            PaloCharacter Palo = new PaloCharacter();
            Palo.Name = "characterPalo";
            Palo.Scale = new Vector3(0.32f, 0.32f, 0.32f);
            Palo.Position = new Vector3(110, 4, -30);
            Palo.oldPosition = new Vector3(110, 4, -30);
            Palo.newPosition = new Vector3(110, 4, -30); 
            Palo.Rotation = new Vector3(0, 0, 0);
            Palo.ObjectPath = "3D/ludzik/chod_2006";
            Palo.Tag = "Palo";
            Palo.Interactive = true;
            Palo.mass = 100;
            PaloObject = Palo;

            GameObjects.Add(Palo);
        }
        private void PaloControl()
        {
            if (PaloObject.PaloState == FriendState.follow)
            {
                PaloObject.WalkToPlayer();
            }else if(PaloObject.PaloState == FriendState.walk)
            {
                PaloObject.ChooseAction(this);
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

        private void ElonAnimationUpdate(TimeSpan time)
        {
            if (PlayerObject.elonState.State == State.run)
            {
                clip = skinningData.AnimationClips["Take 003"];
                if (previousState != State.run)
                    animationPlayer.StartClip(clip);
                previousState = State.run;
            }
            else if (PlayerObject.elonState.State == State.walk)
            {
                clip = skinningData.AnimationClips["Take 003"];
                if (previousState != State.walk)
                    animationPlayer.StartClip(clip);
                previousState = State.walk;
                
            }
            else if (PlayerObject.elonState.State == State.idle)
            {
                clip = skinningData.AnimationClips["Take 001"];
                if (previousState != State.idle)
                    animationPlayer.StartClip(clip);
                previousState = State.idle;
            }
            else if (PlayerObject.elonState.State == State.walkShoot)
            {
                clip = skinningData.AnimationClips["Take 006"];
                if (previousState != State.walkShoot)
                    animationPlayer.StartClip(clip);
                previousState = State.walkShoot;
            }
            else if (PlayerObject.elonState.State == State.idleShoot)
            {
                clip = skinningData.AnimationClips["Take 005"];
                if (previousState != State.idleShoot)
                    animationPlayer.StartClip(clip);
                previousState = State.idleShoot;
            }
            else if (PlayerObject.elonState.State == State.climb)
            {
                clip = skinningData.AnimationClips["Take 004"];
                if (previousState != State.climb)
                    animationPlayer.StartClip(clip);
                previousState = State.climb;
            }
            else if (PlayerObject.elonState.State == State.push)
            {
                clip = skinningData.AnimationClips["Take 007"];
                if (previousState != State.push)
                    animationPlayer.StartClip(clip);
                previousState = State.push;
            }
            else if (PlayerObject.elonState.State == State.interact)
            {
                clip = skinningData.AnimationClips["Take 008"];
                if (previousState != State.interact)
                    animationPlayer.StartClip(clip);
                previousState = State.interact;
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
    }
}
