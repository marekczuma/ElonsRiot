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

namespace ElonsRiot
{
    public class Scene// : Microsoft.Xna.Framework.Game
    {
        public ContentManager ContentManager { get; set; }
        public List<GameObject> GameObjects { get; set; }
        public XMLScene XMLScene { get; set; }
        public Player PlayerObject { get; set; }
        public PaloCharacter PaloObject { get; set; }
        private BasicEffect basicEffect;
       // private Physic physic;
        AnimationPlayer animationPlayer;
        AnimationPlayer animationPlayerPalo;
        public Scene(ContentManager _contentManager)
        {
            GameObjects = new List<GameObject>();
            ContentManager = _contentManager;
            XMLScene = new XMLScene();
           
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
            LoadElon();
            LoadPalo();
            Methods.setPlayer(PlayerObject,GameObjects);
            foreach (var elem in GameObjects)
            {
                if (!string.IsNullOrEmpty(elem.ObjectPath))
                {
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
            for (int i=0; i<GameObjects.Count; i++)
            {
                if (GameObjects[i].Name == "Elon")
                    indexElon = i;
            }
            for (int i = 0; i < GameObjects.Count; i++)
            {
                if (GameObjects[i].Name == "Palo")
                    indexPalo = i;
            }
            //Elon - DO UCYWILIZOWANIA!
            SkinningData skinningData = GameObjects[indexElon].GameObjectModel.Tag as SkinningData;
            animationPlayer = new AnimationPlayer(skinningData);
            AnimationClip clip = skinningData.AnimationClips["Take 001"];
            //Palo anim
            SkinningData skinningDataPalo = GameObjects[indexPalo].GameObjectModel.Tag as SkinningData;
            animationPlayerPalo = new AnimationPlayer(skinningDataPalo);
            AnimationClip clipPalo = skinningDataPalo.AnimationClips["Take 001"];

            animationPlayer.StartClip(clip);
            animationPlayerPalo.StartClip(clipPalo);
            basicEffect = new BasicEffect(graphic);
        }
        public void DrawAllContent(GraphicsDevice graphic)
        {
             foreach(var elem in GameObjects)
             {
                 if ((elem.Name == "Elon") || (elem.Name == "Palo"))
                     elem.DrawAnimatedModels(ContentManager, PlayerObject, animationPlayer);
                 else
                    elem.DrawModels(ContentManager, PlayerObject);               
                elem.RefreshMatrix();
             }
            foreach (GameObject gObj in this.GameObjects)
            {
              //  DrawModel(gObj);
             //   gObj.createBoudingBox();
                gObj.RefreshMatrix();
             /*   if (gObj.Name.Contains("stairs"))
                {
                    gObj.AAbox.drawPlane(basicEffect, graphic);
                }*/
            }
         //   DrawBoudingBox(graphic);
       //     DrawBoudingBoxes(graphic);

        }
        public void PlayerControll(KeyboardState _state, GameTime gameTime, MouseState _mouseState)
        {
            PlayerObject.SetState(_state);
            PlayerObject.Movement(_state, _mouseState);
            PlayerObject.CameraUpdate(gameTime);
            PlayerObject.ChangeHealth(_state);
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
            PhysicManager.update(gameTime, GameObjects, PlayerObject);
            //physic.update(gameTime, GameObjects, PlayerObject);
            PaloControl();
            animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
            foreach(GameObject gObj in GameObjects)
            {
                gObj.update();
            }
        }
        private void LoadElon()
        {
            Player Elon = new Player();
            Elon.Name = "Elon";
            Elon.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            Elon.Position = new Vector3(60,4, -40);
            Elon.Rotation = new Vector3(0, 0, 0);
            Elon.ObjectPath = "3D/ludzik/dude";
            Elon.Interactive = true;
            Elon.gravity = -0.2f;
            Elon.mass = 50; //kg
            GameObjects.Add(Elon);
            PlayerObject = Elon;
            //GameObject sciana = new GameObject();
            //sciana.Name = "Sciana2";
            //sciana.Scale = 1;
            //sciana.Position = new Vector3(0, 0, 20);
            //sciana.Rotation = new Vector3(0, 0, 0);
            //sciana.ObjectPath = "3D/sciana/sciana";
            //GameObjects.Add(sciana);

        }
        private void LoadPalo()
        {
            int indexElon = 0;
            for (int i = 0; i < GameObjects.Count; i++)
            {
                if (GameObjects[i].Name == "Elon")
                    indexElon = i;
            }
            PaloCharacter Palo = new PaloCharacter();
            Palo.Name = "Palo";
            Palo.Scale = new Vector3(0.15f, 0.15f, 0.15f);
            Palo.Position = new Vector3(70, 4, 0);
            Palo.Rotation = new Vector3(0, 0, 0);
            Palo.ObjectPath = "3D/ludzik/dude";
            Palo.Interactive = true;
            Palo.Elon = (Player)GameObjects[indexElon];
            PaloObject = Palo;
            GameObjects.Add(Palo);
        }
        private void PaloControl()
        {
            PaloObject.WalkToPlayer();
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
        public void DrawBoudingBoxes(GraphicsDevice graphic)
        {
          
                Vector3[] corners = PlayerObject.boxes[0].GetCorners();

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
