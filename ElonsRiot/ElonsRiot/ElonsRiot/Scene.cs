using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ElonsRiot
{
    public class Scene// : Microsoft.Xna.Framework.Game
    {
        public ContentManager ContentManager { get; set; }
        public List<GameObject> GameObjects { get; set; }
        public XMLScene XMLScene { get; set; }

        private Player PlayerObject;
        private BasicEffect basicEffect;
        private Physic physic;
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
            XMLScene = DeserializeFromXML();
            GameObjects = XMLScene.GameObjects;
            LoadElon();
            foreach (var elem in GameObjects)
            {
                if (!string.IsNullOrEmpty(elem.ObjectPath))
                    elem.GameObjectModel = ContentManager.Load<Model>(elem.ObjectPath);
                  //  elem.Initialize();
                   // elem.RefreshMatrix();
            }
            basicEffect = new BasicEffect(graphic);
        }
        public void DrawAllContent(GraphicsDevice graphic)
        {
             foreach(var elem in GameObjects)
              {
                 DrawSimpleModel(elem.GameObjectModel, elem.MatrixWorld, PlayerObject.camera.viewMatrix, PlayerObject.camera.projectionMatrix);
              
                  elem.RefreshMatrix();
              }
            foreach (GameObject gObj in this.GameObjects)
            {
              //  DrawModel(gObj);
                gObj.createBoudingBox();
                gObj.RefreshMatrix();
            }
            DrawBoudingBox(graphic);
            physic = new Physic(GameObjects);
            physic.DetectCollision(PlayerObject);
        }
        public void PlayerControll(KeyboardState _state, GameTime gameTime, MouseState _mouseState)
        {
            PlayerObject.SetState(_state);
            PlayerObject.Movement(_state, _mouseState);
            PlayerObject.CameraUpdate(gameTime);
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
        private void DrawSimpleModel(Model model, Matrix world, Matrix view, Matrix projection, Texture2D newTexture = null)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                    if (newTexture != null)
                    {
                        effect.Texture = newTexture;
                    }
                }

                mesh.Draw();
            }
        }
        private void ChangeXMLToStructure()
        {

        }
        private void LoadElon()
        {
            Player Elon = new Player();
            Elon.Name = "Elon";
            Elon.Scale = 1;
            Elon.Position = new Vector3(-150, 2, 0);
            Elon.Rotation = new Vector3(-90, 0, 0);
            Elon.ObjectPath = "3D/ludzik/elon";
            GameObjects.Add(Elon);
            PlayerObject = Elon;
        }
        public void DrawBoudingBox(GraphicsDevice graphic)
        {
            foreach (GameObject gameObj in this.GameObjects)
            {

                gameObj.boneTransformations = new Matrix[gameObj.GameObjectModel.Bones.Count];
                gameObj.GameObjectModel.CopyAbsoluteBoneTransformsTo(gameObj.boneTransformations);

                //draw bouding box 
                Vector3[] corners = gameObj.boundingBox.GetCorners();

                VertexPositionColor[] primitiveList = new VertexPositionColor[corners.Length];

                // Assign the 8 box vertices
                for (int i = 0; i < corners.Length; i++)
                {
                    primitiveList[i] = new VertexPositionColor(corners[i], Color.White);
                }


                basicEffect.World = gameObj.MatrixWorld;
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
      /*  public void DrawModel(GameObject gameObj)
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
        }*/
    }
}
