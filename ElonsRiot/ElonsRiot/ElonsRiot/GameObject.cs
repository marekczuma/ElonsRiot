using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace ElonsRiot
{
    [Serializable]
    public class GameObject// :Microsoft.Xna.Framework.Game
    {
        //Wrzucane z XMLa
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlElement("Position")]
        public Vector3 Position { get; set; }
        [XmlElement("Rotation")]
        public Vector3 Rotation { get; set; }
        [XmlAttribute("Path")]
        public string ObjectPath { get; set; }
        [XmlElement("GameObject")]
        public List<GameObject> GameObjects { get; set; }
        [XmlElement("Scale")]
        public float Scale { get; set; }
        public Matrix MatrixWorld { get; set; }
        //public Matrix MatrixView { get; set; }
        //public Matrix MatrixProjection { get; set; }
        [XmlIgnore]
        public Model GameObjectModel { get; set; }

        public GameObject()
        {
            //Rotation = new Vector3(-90, 0, 0);
            //Position = new Vector3(0, 0, 0);
            MatrixWorld = Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateTranslation(Position) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateScale(Scale);
            //MatrixView = Matrix.CreateLookAt(new Vector3(10, 10, 10), new Vector3(0, 0, 0), Vector3.UnitY);
            //MatrixProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);
            GameObjects = new List<GameObject>();
        }
        public GameObject(Vector3 _position)
        {
            MatrixWorld = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateTranslation(Position) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateScale(Scale);
            GameObjects = new List<GameObject>();
        }
        public void RefreshMatrix()
        {
            MatrixWorld = Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position) * Matrix.CreateScale(Scale);
        }
        public void ChangePosition(Vector3 _position)
        {
            Position += Vector3.Transform(_position, Matrix.CreateRotationY(MathHelper.ToRadians(90) + MathHelper.ToRadians(Rotation.Y)));
            MatrixWorld = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateTranslation(Position) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z));
        }
        public void SetPosition(Vector3 _position)
        {
            Position = _position;
            MatrixWorld = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateTranslation(Position) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z));
        }
        public void ChangeRotation(Vector3 _rotation)
        {
            Rotation += Vector3.Transform(_rotation, Matrix.Identity);
            MatrixWorld = Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
        }
        public void SetRotation(Vector3 _rotation)
        {
            Rotation = _rotation;
            MatrixWorld = Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateTranslation(Position) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z));
        }
        public void setScale(float _scale)
        {
            Scale = _scale;
            MatrixWorld = Matrix.CreateScale(Scale);
        }
    }

}
