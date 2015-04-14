using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        
        public void LoadAllContent()
        {
            XMLScene = DeserializeFromXML();
            GameObjects = XMLScene.GameObjects;
            LoadElon();
            foreach (var elem in GameObjects)
            {
                elem.GameObjectModel = ContentManager.Load<Model>(elem.ObjectPath);
            }

        }

        public void DrawAllContent()
        {
            foreach(var elem in GameObjects)
            {
                DrawSimpleModel(elem.GameObjectModel, elem.MatrixWorld, elem.MatrixView, elem.MatrixProjection);
                elem.RefreshMatrix();
            }
        }
        public XMLScene DeserializeFromXML()
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
            Elon.Position = new Vector3(0, 2, 0);
            Elon.Rotation = new Vector3(-90, 0, 0);
            Elon.ObjectPath = "3D/ludzik/elon";
            GameObjects.Add(Elon);
        }
    }
}
