using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SkinnedModel;
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
        public Vector3 Scale { get; set; }
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
        [XmlIgnore]
        public Box AAbox;
        [XmlIgnore]
        public Vector3 center;
        [XmlIgnore]
        public Plane plane;

        public GameObject()
        {
            //Rotation = new Vector3(-90, 0, 0);
            //Position = new Vector3(0, 0, 0);
            MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
            //MatrixView = Matrix.CreateLookAt(new Vector3(10, 10, 10), new Vector3(0, 0, 0), Vector3.UnitY);
            //MatrixProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);
            GameObjects = new List<GameObject>();
            AAbox = new Box();
        }
        public GameObject(Vector3 _position)
        {
            MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
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
        public void DrawModels(ContentManager _contentManager, Player _playerObject)
        {

            DrawSimpleModel(GameObjectModel, MatrixWorld, _playerObject.camera.viewMatrix, _playerObject.camera.projectionMatrix);
            if( GameObjects != null)
            {
                foreach(var elem in GameObjects)
                {
                    elem.DrawModels(_contentManager, _playerObject);
                }
            }
        }

        public void DrawAnimatedModels(ContentManager _contentManager, Player _playerObject, AnimationPlayer animationPlayer)
        {
            DrawAnimatedModel(GameObjectModel, animationPlayer, MatrixWorld, _playerObject.camera.viewMatrix, _playerObject.camera.projectionMatrix);
            if (GameObjects != null)
            {
                foreach (var elem in GameObjects)
                {
                    elem.DrawAnimatedModels(_contentManager, _playerObject, animationPlayer);
                }
            }
        }
        public void RefreshMatrix()
        {
            MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
        }
        public void ChangePosition(Vector3 _position)
        {
            Position += Vector3.Transform(_position, Matrix.CreateRotationY(MathHelper.ToRadians(90) + MathHelper.ToRadians(Rotation.Y)));
            MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
        }
        public void SetPosition(Vector3 _position)
        {
            Position = _position;
            MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
        }
        public void LookAt(GameObject _obj)
        {
            MatrixWorld = Matrix.CreateScale(Scale) *  Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateLookAt(Position,_obj.Position, Vector3.Up) * Matrix.CreateTranslation(Position);
        }
        public void ChangeRotation(Vector3 _rotation)
        {
            Rotation += Vector3.Transform(_rotation, Matrix.Identity);
            MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
        }
        public void SetRotation(Vector3 _rotation)
        {
            Rotation = _rotation;
            MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
        }
        public void setScale(Vector3 _scale)
        {
            Scale = _scale;
            MatrixWorld = Matrix.CreateScale(Scale);
        }
        public void Initialize()
        {
            boneTransformations = new Matrix[GameObjectModel.Bones.Count];
            this.GameObjectModel.CopyAbsoluteBoneTransformsTo(boneTransformations);
        }
        public float getDistance(GameObject _target)
        {
            return Vector3.Distance(_target.Position, Position);
        }
        public void createBoudingBox()
        {
            Initialize();
            Matrix tmp = Matrix.Identity;
            if(Rotation.Y !=0 && this.ObjectPath !="3D/ludzik/dude")
            {
                tmp = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(-Rotation.Y)) * Matrix.CreateTranslation(Position);
            }
            else
            {
                tmp = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
            }
            

            Vector3 meshMax = new Vector3(float.MinValue);
            Vector3 meshMin = new Vector3(float.MaxValue);
            Matrix meshTransform = new Matrix();
            foreach (ModelMesh mesh in GameObjectModel.Meshes)
            {
                meshTransform = boneTransformations[mesh.ParentBone.Index] * tmp;

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
            float x, y, z;
            x = Math.Abs(meshMin.X - meshMax.X) * Scale.X / Scale.X * 4;
            y = Math.Abs(meshMin.Y - meshMax.Y) * Scale.Y / Scale.Y * 4;
            z = Math.Abs(meshMin.Z - meshMax.Z) * Scale.Z / Scale.Z * 4;

            // boundingBox = new BoundingBox(new Vector3(-x/2,-y/2,-z/2),new Vector3(x/2,y/2,z/2));
            boundingBox = new BoundingBox(meshMin, meshMax);
        }
        public void GetCentre()
        {
            BoundingSphere sphere = new BoundingSphere();

            Matrix tmp = Matrix.Identity;
            tmp = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
            Matrix meshTransform = new Matrix();

            foreach (ModelMesh mesh in GameObjectModel.Meshes)
            {
                meshTransform = boneTransformations[mesh.ParentBone.Index] * tmp;


                sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
                sphere = sphere.Transform(meshTransform);

            }
            this.center = sphere.Center;
        }
        public void createPlane()
        {
            float dotProduct = 0;
            Vector3 normal = new Vector3(0, 0, 0);

            foreach (ModelMesh mesh in GameObjectModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {

                    int stride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;

                    VertexPositionNormalTexture[] vertexData = new VertexPositionNormalTexture[meshPart.NumVertices];
                    meshPart.VertexBuffer.GetData(meshPart.VertexOffset * stride, vertexData, 0, meshPart.NumVertices, stride);

                    Vector3 vecAB = vertexData[1].Position - vertexData[0].Position;
                    Vector3 vecAC = vertexData[2].Position - vertexData[0].Position;
                    
                    // Cross vecAB and vecAC
                    normal = Vector3.Cross(vecAB, vecAC);
                    normal.Normalize();

                    Vector3 tmp = vertexData[0].Position;
                    dotProduct = Vector3.Dot(-normal, tmp);

                }
            }
            plane = new Plane(normal, dotProduct);
        }

        private void DrawSimpleModel(Model model, Matrix world, Matrix view, Matrix projection, Texture2D newTexture = null)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = mesh.ParentBone.Transform * world;
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

        private void DrawAnimatedModel(Model currentModel, AnimationPlayer animationPlayer, Matrix world, Matrix view, Matrix projection, Texture2D newTexture = null)
        {
            Matrix[] bones = animationPlayer.GetSkinTransforms();

            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

                    effect.World = mesh.ParentBone.Transform * world;
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
        
    }
}


