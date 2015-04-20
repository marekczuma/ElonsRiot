using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
         [XmlIgnore]
        public BoundingBox boundingBox;
        [XmlIgnore]
        public Matrix[] boneTransformations;
        [XmlIgnore]
        public short[] bBoxIndices ={
                0, 1, 1, 2, 2, 3, 3, 0, // Front edges
                4, 5, 5, 6, 6, 7, 7, 4, // Back edges
                0, 4, 1, 5, 2, 6, 3, 7 // Side edges connecting front and back
            };

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
        public void LoadModels(ContentManager _contentManager)
        {
            GameObjectModel = _contentManager.Load<Model>(ObjectPath);
            RefreshMatrix();
            if( GameObjects != null)
            {
                foreach(var elem in GameObjects)
                {
                    elem.LoadModels(_contentManager);
                }
            }
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
        public void Initialize()
        {
            boneTransformations = new Matrix[GameObjectModel.Bones.Count];
            this.GameObjectModel.CopyAbsoluteBoneTransformsTo(boneTransformations);
        }
        public void createBoudingBox()
        {
            boneTransformations = new Matrix[GameObjectModel.Bones.Count];
            this.GameObjectModel.CopyAbsoluteBoneTransformsTo(boneTransformations);


            Vector3 meshMax = new Vector3(float.MinValue);
            Vector3 meshMin = new Vector3(float.MaxValue);
            Matrix meshTransform = new Matrix();

            foreach (ModelMesh mesh in GameObjectModel.Meshes)
            {
                meshTransform = Matrix.CreateScale(0.4f) * Matrix.CreateRotationX(MathHelper.ToRadians(-Rotation.X))
                    * Matrix.CreateRotationZ(MathHelper.ToRadians(-Rotation.Z)) * boneTransformations[mesh.ParentBone.Index];
                
                foreach (ModelMeshPart part in mesh.MeshParts)
                {

                    // The stride is how big, in bytes, one vertex is in the vertex buffer
                    // We have to use this as we do not know the make up of the vertex
                    int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

                    VertexPositionNormalTexture[] vertexData = new VertexPositionNormalTexture[part.NumVertices];
                    part.VertexBuffer.GetData(part.VertexOffset * stride, vertexData, 0, part.NumVertices, stride);

                    // Find minimum and maximum xyz values for this mesh part
                    Vector3 vertPosition = new Vector3();

                    for (int i = 0; i < vertexData.Length; i++)
                    {
                        vertPosition = vertexData[i].Position;

                        // update our values from this vertex
                        meshMin = Vector3.Min(meshMin, vertPosition);
                        meshMax = Vector3.Max(meshMax, vertPosition);
                    }
                }

                // transform by mesh bone matrix
                meshMin = Vector3.Transform(meshMin, meshTransform);
                meshMax = Vector3.Transform(meshMax, meshTransform);

            }
            if (this.ObjectPath == "3D/sciana/miniWall")
            {
                meshMax.Y -= 4;
                meshMin.Y -= 4;
            }
            boundingBox = new BoundingBox(meshMin, meshMax);
            
        }
        
    }
}


